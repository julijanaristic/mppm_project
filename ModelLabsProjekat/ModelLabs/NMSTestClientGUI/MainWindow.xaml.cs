using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelventDMS.Services.NetworkModelService.TestClient.Tests;
using static System.Net.Mime.MediaTypeNames;

namespace NMSTestClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestGda testGda;
        public MainWindow()
        {
            InitializeComponent();
            testGda = new TestGda();
        }

        private void BtnGetValues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TbGetValuesGid.Text))
                    throw new Exception("Select GID first.");

                long gid = ParseGlobalId(TbGetValuesGid.Text);
                var rd = testGda.GetValues(gid);

                if (rd == null)
                    throw new Exception("GetValues returned null.");

                var sb = new StringBuilder();

                var nameProp = rd.GetProperty(ModelCode.IDOBJ_NAME);
                string nameValue = nameProp?.AsString() ?? "<no name>";
                sb.AppendLine($"NAME: {nameValue}");
                sb.AppendLine();

                sb.AppendLine("PROPERTIES:");
                foreach (var p in rd.Properties)
                    sb.AppendLine($"{p.Id} = {FormatPropertyValue(p)}");

                ShowResult(RtbGetValues, sb.ToString());
            }
            catch (Exception ex) {
                ShowResult(RtbGetValues, "ERROR: " + ex.Message);
            }
        }
        private void BtnGetExtent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ModelCode mc;
                ModelCodeHelper.GetModelCodeFromString(TbExtentModelCode.Text, out mc);
                var ids = testGda.GetExtentValues(mc);
                ShowResult(RtbExtentValues, ExtentToString(ids));
            }
            catch (Exception ex)
            {
                ShowResult(RtbExtentValues, "ERROR: " + ex.Message);
            }
        }

        private void BtnGetRelated_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long gid = ParseGlobalId(TbSourceGid.Text);
                ModelCode prop, type;
                ModelCodeHelper.GetModelCodeFromString(TbAssociationProperty.Text, out prop);
                ModelCodeHelper.GetModelCodeFromString(TbAssociationType.Text, out type);

                Association a = new Association
                {
                    PropertyId = prop,
                    Type = type
                };

                var ids = testGda.GetRelatedValues(gid, a);
                ShowResult(RtbRelatedValues, ExtentToString(ids));
            }
            catch (Exception ex)
            {
                ShowResult(RtbRelatedValues, "ERROR: " + ex.Message);
            }
        }

        private void BtnGetClamps_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long gid = ParseGlobalId(TbAclineGid.Text);
                var ids = testGda.GetClampsForACLineSegment(gid);
                ShowResult(RtbClamps, ExtentToString(ids));
            }
            catch (Exception ex)
            {
                ShowResult(RtbClamps, "ERROR: " + ex.Message);
            }
        }

        private void BtnClampMinLength_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long gid = ParseGlobalId(TbAclineGid.Text);
                long minClamp = testGda.GetClampWithMinLength(gid);
                var rd = testGda.GetValues(minClamp);
                var sb = new StringBuilder();
                foreach (var p in rd.Properties)
                    sb.AppendLine($"{p.Id,-40} = {p.GetValue()}");
                ShowResult(RtbClamps, "MIN CLAMP:\n" + sb.ToString());
            }
            catch (Exception ex)
            {
                ShowResult(RtbClamps, "ERROR: " + ex.Message);
            }
        }

        private void BtnGetTerminals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long gid = ParseGlobalId(TbCondEqGid.Text);
                var ids = testGda.GetTerminalsForConductingEquipment(gid);
                ShowResult(RtbTerminals, ExtentToString(ids));
            }
            catch (Exception ex)
            {
                ShowResult(RtbTerminals, "ERROR: " + ex.Message);
            }
        }

        private void BtnDisconnectedTerminals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ids = testGda.GetDisconnectedTerminals();
                ShowResult(RtbTerminals, ExtentToString(ids));
            }
            catch (Exception ex)
            {
                ShowResult(RtbTerminals, "ERROR: " + ex.Message);
            }
        }

        private long ParseGlobalId(string text)
        {
            if (text.StartsWith("0x")) return Convert.ToInt64(text.Substring(2), 16);
            return Convert.ToInt64(text);
        }

        private void ShowResult(RichTextBox rtb, string text)
        {
            rtb.Document.Blocks.Clear();
            rtb.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private string ExtentToString(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
                return "No data";
            var sb = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
                sb.AppendLine($"{i + 1,3}. 0x{ids[i]:X16}");

            return sb.ToString();
        }

        private string FormatPropertyValue(Property p)
        {
            // ako je vektor referenci (ili bilo koji Int64 vektor)
            if (p.Type == PropertyType.ReferenceVector ||
                p.Type == PropertyType.Int64Vector)
            {
                var list = p.AsReferences();   // ili (List<long>)p.GetValue()
                return "[" + string.Join(", ", list.Select(id => $"0x{id:X16}")) + "]";
            }
            return p.GetValue()?.ToString() ?? "<empty>";
        }

    }
}
