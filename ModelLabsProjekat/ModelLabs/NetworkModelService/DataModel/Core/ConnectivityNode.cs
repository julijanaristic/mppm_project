using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class ConnectivityNode: IdentifiedObject
    {
        private List<long> terminals = new List<long>();
        public ConnectivityNode(long globalId)
            : base(globalId)
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
                ConnectivityNode obj = (ConnectivityNode)x;
                return CompareHelper.CompareLists(obj.terminals, this.terminals);
            }
            return false;
        }

        public override bool HasProperty(ModelCode property)
        {
            if (property == ModelCode.CONNECTIVITYNODE_TERMINAL)
                return true;
            return base.HasProperty(property);
        }

        public override void GetProperty(Property property)
        {
            if (property.Id == ModelCode.CONNECTIVITYNODE_TERMINAL)
                property.SetValue(terminals);
            else
                base.GetProperty(property);
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
                references[ModelCode.CONNECTIVITYNODE_TERMINAL] = new List<long>(terminals);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
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
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    terminals.Remove(globalId); 
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get { return terminals.Count != 0 || base.IsReferenced; }
        }
    }
}
