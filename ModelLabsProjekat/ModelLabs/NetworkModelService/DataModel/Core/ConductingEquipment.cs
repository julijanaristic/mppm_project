using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
	public class ConductingEquipment : Equipment
	{
		private List<long> terminals = new List<long>();
		public ConductingEquipment(long globalId) : base(globalId) 
		{
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                ConductingEquipment obj = (ConductingEquipment)x;
                return terminals.SequenceEqual(obj.terminals);
            }
            return false;
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (terminals.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.CONDEQ_TERMINAL] = new List<long>(terminals);
			}

			base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.TERMINAL_CONDEQ)
			{
				terminals.Add(globalId);
			}
            else
            {
				base.AddReference(referenceId, globalId);
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.TERMINAL_CONDEQ)
            {
                terminals.Remove(globalId);
            }
            else
            {
                base.RemoveReference(referenceId, globalId);
            }
        }

        public override bool IsReferenced => terminals.Count > 0;
	}
}
