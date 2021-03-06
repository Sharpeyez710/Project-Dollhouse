﻿/*This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at
http://mozilla.org/MPL/2.0/.

The Original Code is the Gonzo library.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s):
*/

using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Gonzo.Elements;
using Microsoft.Xna.Framework.Graphics;
using log4net;

namespace Gonzo.Dialogs
{
    public class LoginDialog : UIDialog, IDisposable
    {
        private UILabel m_LblTitle, m_LblUsername, m_LblPassword;
        private UITextEdit m_TxtUsername, m_TxtPassword;
        private UIButton m_BtnLogin, m_BtnExit;

        private CaretSeparatedText m_Cst;

        private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public LoginDialog(UIScreen Screen, Vector2 Pos) : base(Screen, Pos, false, true, false)
        {
            m_Font = m_Screen.Font11px;

            m_Cst = StringManager.StrTable(209);

            Vector2 RelativePosition = new Vector2(60, 0);
            m_LblTitle = new UILabel(m_Cst[1], 1, Pos + RelativePosition, m_Font.MeasureString(m_Cst[1]),
                m_Screen.StandardTxtColor, 11, m_Screen, UIParser.Nodes.TextAlignment.Center_Center);
            RelativePosition = new Vector2(20, 50);
            m_LblUsername = new UILabel(m_Cst[4], 2, Pos + RelativePosition, m_Font.MeasureString(m_Cst[4]), 
                m_Screen.StandardTxtColor, 9, m_Screen, UIParser.Nodes.TextAlignment.Center_Center);
            RelativePosition = new Vector2(20, 110);
            m_LblPassword = new UILabel(m_Cst[5], 3, Pos + RelativePosition, m_Font.MeasureString(m_Cst[4]), 
                m_Screen.StandardTxtColor, 9, m_Screen, UIParser.Nodes.TextAlignment.Center_Center);

            RelativePosition = new Vector2(20, 85);
            m_TxtUsername = new UITextEdit("TxtUsername", 4, true, false, Pos + RelativePosition, 
                new Vector2(250, 25), 10, m_Screen, "", this);
            RegistrableUIElements.Add(m_TxtUsername.Name, m_TxtUsername);
            RelativePosition = new Vector2(20, 145);
            m_TxtPassword = new UITextEdit("TxtPassword", 5, true, false, Pos + RelativePosition, 
                new Vector2(250, 25), 10, m_Screen, "", this);
            RegistrableUIElements.Add(m_TxtPassword.Name, m_TxtPassword);

            RelativePosition = new Vector2(120, 170);
            m_BtnLogin = new UIButton("BtnLogin", Pos + RelativePosition, m_Screen, null, m_Cst[2], 9);
            RelativePosition = new Vector2(200, 170);
            m_BtnExit = new UIButton("BtnExit", Pos + RelativePosition, m_Screen, null, m_Cst[3], 9);

            SetSize((int)((m_Font.MeasureString(m_Cst[1]).X + 40) * Resolution.getVirtualAspectRatio()), 
                (int)(175 * Resolution.getVirtualAspectRatio()));
        }

        public override void Update(InputHelper Helper, GameTime GTime)
        {
            base.Update(Helper, GTime);

            if(Visible)
            {
                if(m_DoDrag)
                {
                    Vector2 OffsetFromMouse = new Vector2(60, 0);
                    m_LblTitle.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;
                    OffsetFromMouse = new Vector2(20, 50);
                    m_LblUsername.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;
                    OffsetFromMouse = new Vector2(20, 110);
                    m_LblPassword.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;

                    OffsetFromMouse = new Vector2(20, 85);
                    //m_TxtUsername.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;
                    m_TxtUsername.CursorPosition = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;
                    OffsetFromMouse = new Vector2(20, 145);
                    //m_TxtPassword.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;
                    m_TxtPassword.CursorPosition = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;

                    OffsetFromMouse = new Vector2(120, 170);
                    m_BtnLogin.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;

                    OffsetFromMouse = new Vector2(200, 170);
                    m_BtnExit.Position = (Helper.MousePosition + OffsetFromMouse) - m_DragOffset;
                }

                m_TxtUsername.Update(Helper, GTime);
                m_TxtPassword.Update(Helper, GTime);

                m_BtnLogin.Update(Helper, GTime);
                m_BtnExit.Update(Helper, GTime);
            }
        }

        public override void Draw(SpriteBatch SBatch, float? LayerDepth)
        {
            float Depth;
            if (LayerDepth != null)
                Depth = (float)LayerDepth;
            else
                Depth = 0.10f;

            m_LblTitle.Draw(SBatch, Depth + 0.1f);
            m_LblUsername.Draw(SBatch, Depth + 0.1f);
            m_LblPassword.Draw(SBatch, Depth + 0.1f);

            m_TxtUsername.Draw(SBatch, Depth + 0.1f);
            m_TxtPassword.Draw(SBatch, Depth + 0.1f);

            m_BtnLogin.Draw(SBatch, Depth + 0.1f);
            m_BtnExit.Draw(SBatch, Depth + 0.1f);

            base.Draw(SBatch, LayerDepth);
        }

        ~LoginDialog()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the resources used by this LoginDialog instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes of the resources used by this LoginDialog instance.
        /// <param name="Disposed">Was this resource disposed explicitly?</param>
        /// </summary>
        protected virtual void Dispose(bool Disposed)
        {
            if (Disposed)
            {
                if (m_TxtUsername != null)
                    m_TxtUsername.Dispose();
                if (m_TxtPassword != null)
                    m_TxtPassword.Dispose();

                // Prevent the finalizer from calling ~LoginDialog, since the object is already disposed at this point.
                GC.SuppressFinalize(this);
            }
            else
                m_Logger.Error("LoginDialog not explicitly disposed!");
        }
    }
}
