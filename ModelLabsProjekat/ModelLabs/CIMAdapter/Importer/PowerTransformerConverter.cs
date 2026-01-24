namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
				}
			}
		}

		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
			}
		}

		public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);
			}
		}

		public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimConductingEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
			}
		}
        #endregion Populate ResourceDescription

        // fali conductor, rectifierInverter, 

        #region Conductor
		public static void PopulateConductorProperties(FTN.Conductor cimConductor, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimConductor != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimConductor, rd, importHelper, report);
			}
		}
        #endregion

        #region RectifierInverter
		public static void PopulateRectifierInverterProperties(FTN.RectifierInverter cimRectifierInverter, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRectifierInverter != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimRectifierInverter, rd, importHelper, report);
			}
		}
        #endregion

        #region ACLineSegment
        public static void PopulateACLineSegmentProperties(FTN.ACLineSegment cimACLineSegment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimACLineSegment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductorProperties(cimACLineSegment, rd, importHelper, report);
			}
		}
        #endregion

        #region Clamp
		public static void PopulateClampProperties(FTN.Clamp cimClamp, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimClamp != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimClamp, rd, importHelper, report);
				if (cimClamp.LengthFromTerminal1HasValue)
				{
					rd.AddProperty(new Property(ModelCode.CLAMP_LENGTHFROMTERMINAL, cimClamp.LengthFromTerminal1));
				}
				if (cimClamp.ACLineSegmentHasValue)
				{
					long gid = importHelper.GetMappedGID(cimClamp.ACLineSegment.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Clamp rdfID=\"").Append(cimClamp.ID).Append("\" - ACLineSegment rdfID=\"").Append(cimClamp.ACLineSegment.ID).Append("\" not mapped!");
					}
					else
					{
						rd.AddProperty(new Property(ModelCode.CLAMP_ACLINESEGMENT, gid));
					}
				}
			}
		}
        #endregion

        #region Terminal
		public static void PopulateTerminalProperties(FTN.Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimTerminal != null) && (rd != null))
			{
				PopulateIdentifiedObjectProperties(cimTerminal, rd);

				if (cimTerminal.SequenceNumberHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TERMINAL_SEQUENCE, cimTerminal.SequenceNumber));
				}

				if (cimTerminal.ConnectedHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTED, cimTerminal.Connected));
				}

				if (cimTerminal.PhasesHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TERMINAL_PHASES, (short)GetDMSPhaseCode(cimTerminal.Phases)));
				}

				if (cimTerminal.ConductingEquipmentHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Terminal rdfID=\"").Append(cimTerminal.ID).Append("\" - ConductingEquipment rdfID=\"").Append(cimTerminal.ConductingEquipment.ID).Append("\" not mapped!");
					}
					else
					{
						rd.AddProperty(new Property(ModelCode.TERMINAL_CONDEQ, gid));
					}
				}

				if (cimTerminal.ConnectivityNodeHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTerminal.ConnectivityNode.ID);
					if (gid < 0)
					{
                        report.Report.Append("WARNING: Terminal rdfID=\"").Append(cimTerminal.ID).Append("\" - ConnectivityNode rdfID=\"").Append(cimTerminal.ConnectivityNode.ID).Append("\" not mapped!");
                    }
					else
					{
						rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTIVITYNODE, gid));
					}
                }
			}
		}
        #endregion

        #region ConnectivityNode 
		public static void PopulateConnectivityNodeProperties(FTN.ConnectivityNode cimConnectivityNode, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimConnectivityNode != null) && (rd != null))
			{
				PopulateIdentifiedObjectProperties(cimConnectivityNode, rd);
			}
		}
        #endregion

        #region Enums convert
        public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
		{
			switch (phases)
			{
				case FTN.PhaseCode.A:
					return PhaseCode.A;
				case FTN.PhaseCode.AB:
					return PhaseCode.AB;
				case FTN.PhaseCode.ABC:
					return PhaseCode.ABC;
				case FTN.PhaseCode.ABCN:
					return PhaseCode.ABCN;
				case FTN.PhaseCode.ABN:
					return PhaseCode.ABN;
				case FTN.PhaseCode.AC:
					return PhaseCode.AC;
				case FTN.PhaseCode.ACN:
					return PhaseCode.ACN;
				case FTN.PhaseCode.AN:
					return PhaseCode.AN;
				case FTN.PhaseCode.B:
					return PhaseCode.B;
				case FTN.PhaseCode.BC:
					return PhaseCode.BC;
				case FTN.PhaseCode.BCN:
					return PhaseCode.BCN;
				case FTN.PhaseCode.BN:
					return PhaseCode.BN;
				case FTN.PhaseCode.C:
					return PhaseCode.C;
				case FTN.PhaseCode.CN:
					return PhaseCode.CN;
				case FTN.PhaseCode.N:
					return PhaseCode.N;
				case FTN.PhaseCode.s12N:
					return PhaseCode.ABN;
				case FTN.PhaseCode.s1N:
					return PhaseCode.AN;
				case FTN.PhaseCode.s2N:
					return PhaseCode.BN;
				default: return PhaseCode.Unknown;
			}
		}
		#endregion Enums convert
	}
}
