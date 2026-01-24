using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
			ImportACLineSegments();
			ImportRectifierInverter();
			ImportClamp();
			ImportTerminal();
			ImportConnectivityNode();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

        #region Import


        private void ImportACLineSegments()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.ACLineSegment");

            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.ACLineSegment cim = pair.Value as FTN.ACLineSegment;

				ResourceDescription rd = CreateACLineSegmentResourceDescription(cim);

				if (rd != null)
				{
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                }
            }
        }

		private ResourceDescription CreateACLineSegmentResourceDescription(FTN.ACLineSegment cim)
		{
			if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ACLINESEGMENT, importHelper.CheckOutIndexForDMSType(DMSType.ACLINESEGMENT));

            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            PowerTransformerConverter.PopulateACLineSegmentProperties(cim, rd, importHelper, report);
			
			return rd;
        }

		private void ImportRectifierInverter()
		{
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.RectifierInverter");

            if (cimObjects == null) return;

			foreach (var pair in cimObjects)
			{
				FTN.RectifierInverter cim = pair.Value as FTN.RectifierInverter;

				ResourceDescription rd = CreateRectifierInverterResourceDescription(cim);

				if (rd != null)
				{
					delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
				}
			}
        }

		private ResourceDescription CreateRectifierInverterResourceDescription(FTN.RectifierInverter cim)
		{
            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.RECTIFIERINVERTER, importHelper.CheckOutIndexForDMSType(DMSType.RECTIFIERINVERTER));

            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            PowerTransformerConverter.PopulateRectifierInverterProperties(cim, rd, importHelper, report);
			return rd;
        }

		private void ImportClamp()
		{
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.Clamp");

            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.Clamp cim = pair.Value as FTN.Clamp;

				ResourceDescription rd = CreateClampResourceDescription(cim);

                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                }
            }
        }

		private ResourceDescription CreateClampResourceDescription(FTN.Clamp cim)
		{
            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CLAMP, importHelper.CheckOutIndexForDMSType(DMSType.CLAMP));

            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            PowerTransformerConverter.PopulateClampProperties(cim, rd, importHelper, report);
			return rd;
        }

		private void ImportTerminal()
		{
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.Terminal");

            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.Terminal cim = pair.Value as FTN.Terminal;

				ResourceDescription rd = CreateTerminalResourceDescription(cim);

                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                }
            }
        }

		private ResourceDescription CreateTerminalResourceDescription(FTN.Terminal cim)
		{
            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));

            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            PowerTransformerConverter.PopulateTerminalProperties(cim, rd, importHelper, report);
			return rd;
        }

		private void ImportConnectivityNode()
		{
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.ConnectivityNode");

            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.ConnectivityNode cim = pair.Value as FTN.ConnectivityNode;

				ResourceDescription rd = CreateConnectivityNodeResourceDescription(cim);

				if (rd != null)
				{
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                }
            }
        }

		private ResourceDescription CreateConnectivityNodeResourceDescription(FTN.ConnectivityNode cim)
		{
            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTIVITYNODE, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTIVITYNODE));

            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            PowerTransformerConverter.PopulateConnectivityNodeProperties(cim, rd, importHelper, report);
			return rd;
        }

        #endregion Import
    }
}

