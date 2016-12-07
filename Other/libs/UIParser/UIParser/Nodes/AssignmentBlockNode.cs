﻿/*This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at
http://mozilla.org/MPL/2.0/.

The Original Code is the UIParser library.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s):
*/

using System.Collections.Generic;
using Irony.Parsing;

namespace UIParser.Nodes
{
    public class AssignmentBlockNode : UINode
    {
        public List<AssignmentNode> AssignmentNodes = new List<AssignmentNode>();

        public override void Accept(IUIVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void InitChildren(ParseTreeNodeList nodes)
        {
            InitChildrenAsList(nodes);

            foreach (ParseTreeNode Node in nodes)
            {
                if(Node.ChildNodes.Count > 1)
                    AssignmentNodes.Add((AssignmentNode)Node.ChildNodes[0].AstNode);
            }

            AsString = "AssignmentBlock";
        }
    }
}
