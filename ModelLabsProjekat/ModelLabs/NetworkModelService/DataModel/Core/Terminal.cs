using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Terminal: IdentifiedObject
    {
        private bool connected;
        private PhaseCode phases;
        private int sequenceNumber;

        private long conductingEquipment = 0;
        private long connectivityNode = 0;

        public Terminal(long globalId)
            : base(globalId)
        {
        }

        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        public PhaseCode Phases
        {
            get { return phases; }
            set { phases = value; }
        }

        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                Terminal obj = (Terminal)x;
                return obj.connected == connected && obj.phases == phases && obj.sequenceNumber == sequenceNumber && obj.conductingEquipment == conductingEquipment && obj.connectivityNode == connectivityNode;
            }

            return false;
        }

        #region IAccess
        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.TERMINAL_CONNECTED:
                case ModelCode.TERMINAL_PHASES:
                case ModelCode.TERMINAL_SEQUENCE:
                case ModelCode.TERMINAL_CONDEQ:
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TERMINAL_CONNECTED:
                    property.SetValue(connected);
                    break;
                case ModelCode.TERMINAL_PHASES:
                    property.SetValue((short)phases);
                    break;
                case ModelCode.TERMINAL_SEQUENCE:
                    property.SetValue(sequenceNumber);
                    break;
                case ModelCode.TERMINAL_CONDEQ:
                    property.SetValue(conductingEquipment);
                    break;
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    property.SetValue(connectivityNode);
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
                case ModelCode.TERMINAL_CONNECTED:
                    connected = property.AsBool();
                    break;
                case ModelCode.TERMINAL_PHASES:
                    phases = (PhaseCode)property.AsEnum();
                    break;
                case ModelCode.TERMINAL_SEQUENCE:
                    sequenceNumber = property.AsInt();
                    break;
                case ModelCode.TERMINAL_CONDEQ:
                    conductingEquipment = property.AsReference();
                    break;
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    connectivityNode = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion

        #region IReference
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (conductingEquipment != 0 && (refType == TypeOfReference.Reference ||  refType == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_CONDEQ] = new List<long> { conductingEquipment };
            }

            if (connectivityNode != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_CONNECTIVITYNODE] = new List<long> { connectivityNode };
            }

            base.GetReferences(references, refType);
        }
        #endregion
    }
}
