using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Collections.Generic;
using System.Linq;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ACLineSegment: Conductor
    {
        private List<long> clamps = new List<long>();
        public ACLineSegment(long globalId)
            : base(globalId)
        {
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ACLINESEGMENT_CLAMP:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLINESEGMENT_CLAMP:
                    property.SetValue(clamps);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                ACLineSegment obj = (ACLineSegment)x;
                return clamps.SequenceEqual(obj.clamps);
            }
            return false;
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
            switch (referenceId)
            {
                case ModelCode.CLAMP_ACLINESEGMENT:
                    clamps.Add(globalId); 
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
                case ModelCode.CLAMP_ACLINESEGMENT:
                    clamps.Remove(globalId);
                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        public override bool IsReferenced => clamps.Count > 0;
    }
}
