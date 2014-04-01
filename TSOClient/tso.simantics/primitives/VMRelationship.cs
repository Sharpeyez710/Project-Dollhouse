﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSO.Simantics.engine;
using TSO.Files.utils;
using TSO.Simantics.engine.utils;
using TSO.Simantics.engine.scopes;

namespace TSO.Simantics.primitives
{
    public class VMRelationship : VMPrimitiveHandler
    {
        public override VMPrimitiveExitCode Execute(VMStackFrame context)
        {
            var operand = context.GetCurrentOperand<VMRelationshipOperand>();

            VMEntity obj1;
            VMEntity obj2;

            switch (operand.Mode)
            {
                case 0: //from me to stack object
                    obj1 = context.Caller;
                    obj2 = context.StackObject;
                    break;
                case 1: //from stack object to me
                    obj1 = context.StackObject;
                    obj2 = context.Caller;
                    break;
                case 2: //from stack object to object in local
                    obj1 = context.StackObject;
                    obj2 = context.VM.GetObjectById((short)context.Locals[operand.Local]);
                    break;
                case 3: //from object in local to stack object
                    obj1 = context.VM.GetObjectById((short)context.Locals[operand.Local]);
                    obj2 = context.StackObject;
                    break;
                default:
                    throw new Exception("Invalid relationship type!");
            }

            var rels = obj1.MeToObject;
            if (operand.Set)
            { //todo, special system for server persistent avatars and pets
                var value = VMMemory.GetVariable(context, (VMVariableScope)operand.VarScope, operand.VarData);

                var targId = (ushort)obj2.ObjectID;
                if (!rels.ContainsKey(targId))
                {
                    if (operand.FailIfTooSmall) return VMPrimitiveExitCode.GOTO_FALSE;
                    else rels.Add(targId, new Dictionary<short, short>());
                }
                if (!rels[targId].ContainsKey(operand.RelVar))
                {
                    if (operand.FailIfTooSmall) return VMPrimitiveExitCode.GOTO_FALSE;
                    else rels[targId].Add(operand.RelVar, value);
                }
                else rels[targId][operand.RelVar] = value;
            }
            else
            {
                var targId = (ushort)obj2.ObjectID;
                if (rels.ContainsKey(targId))
                {
                    if (rels[targId].ContainsKey(operand.RelVar))
                        VMMemory.SetVariable(context, (VMVariableScope)operand.VarScope, operand.VarData, rels[targId][operand.RelVar]);
                    else return VMPrimitiveExitCode.GOTO_FALSE;
                }
                else return VMPrimitiveExitCode.GOTO_FALSE;
            }

            return VMPrimitiveExitCode.GOTO_TRUE;
        }
    }

    public class VMRelationshipOperand : VMPrimitiveOperand
    {
        public byte RelVar;
        public byte Mode;
        public byte Flags;
        public byte Local;
        public ushort VarScope;
        public ushort VarData;

        #region VMPrimitiveOperand Members
        public void Read(byte[] bytes)
        {
            using (var io = IoBuffer.FromBytes(bytes, ByteOrder.LITTLE_ENDIAN))
            {
                RelVar = io.ReadByte();
                Mode = io.ReadByte();
                Flags = io.ReadByte();
                Local = io.ReadByte();
                VarScope = io.ReadUInt16();
                VarData = io.ReadUInt16();
            }
        }
        #endregion

        public bool UseNeighbor
        {
            get { return (Flags & 1) == 1; }
        }

        public bool FailIfTooSmall
        {
            get { return (Flags & 2) == 2; }
        }

        public bool Set
        { 
            get { return (Flags & 4) == 4; }
        }
    }
}
