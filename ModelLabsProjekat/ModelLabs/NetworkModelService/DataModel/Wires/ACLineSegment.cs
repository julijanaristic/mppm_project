using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ACLineSegment: Conductor
    {
        private List<long> clamps = new List<long>();
        public ACLineSegment(long globalId)
            : base(globalId)
        {
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (clamps.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.ACLINESEGMENT_CLAMP] = new List<long>(clamps);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.CLAMP_ACLINESEGMENT)
            {
                clamps.Add(globalId);
            }
            else
            {
                base.AddReference(referenceId, globalId);
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.CLAMP_ACLINESEGMENT)
            {
                clamps.Remove(globalId);
            }
            else
            {
                base.RemoveReference(referenceId, globalId);
            }
        }

        public override bool IsReferenced => clamps.Count > 0;
    }
}
