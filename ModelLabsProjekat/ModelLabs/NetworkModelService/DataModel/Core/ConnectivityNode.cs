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
            if (referenceId == ModelCode.TERMINAL_CONNECTIVITYNODE)
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
            if (referenceId == ModelCode.TERMINAL_CONNECTIVITYNODE)
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
