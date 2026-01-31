using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Clamp: ConductingEquipment
    {
        private float lengthFromTerminal;
        private long acLineSegment = 0;
        public Clamp(long globalId)
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
                Clamp obj = (Clamp)x;
                return obj.lengthFromTerminal == lengthFromTerminal && obj.acLineSegment == acLineSegment;
            }

            return false;
        }
        public float LengthFromTerminal
        {
            get { return lengthFromTerminal; }
            set { lengthFromTerminal = value; }
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.CLAMP_LENGTHFROMTERMINAL:
                case ModelCode.CLAMP_ACLINESEGMENT:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CLAMP_LENGTHFROMTERMINAL:
                    property.SetValue(lengthFromTerminal);
                    break;
                case ModelCode.CLAMP_ACLINESEGMENT:
                    property.SetValue(acLineSegment);
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
                case ModelCode.CLAMP_LENGTHFROMTERMINAL:
                    lengthFromTerminal = property.AsFloat();
                    break;
                case ModelCode.CLAMP_ACLINESEGMENT:
                    acLineSegment = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (acLineSegment != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.CLAMP_ACLINESEGMENT] = new List<long> { acLineSegment };
            }

            base.GetReferences(references, refType);
        }

        public override bool IsReferenced => base.IsReferenced;
    }
}
