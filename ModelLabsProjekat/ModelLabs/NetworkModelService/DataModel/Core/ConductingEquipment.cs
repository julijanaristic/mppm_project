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
                return (CompareHelper.CompareLists(obj.terminals, this.terminals));
            }
            return false;
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.CONDEQ_TERMINAL:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONDEQ_TERMINAL:
                    property.SetValue(terminals);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                default:
                    base.SetProperty(property);
                    break;
            }
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
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONDEQ:
                    terminals.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONDEQ:
                    if (terminals.Contains(globalId))
                        terminals.Remove(globalId);
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        public override bool IsReferenced => terminals.Count > 0;
	}
}
