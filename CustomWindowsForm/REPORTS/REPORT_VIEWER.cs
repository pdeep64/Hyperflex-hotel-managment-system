using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CustomWindowsForm;
using CustomWindowsForm.FORMS;
using HYFLEX_HMS.CLASS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomWindowsForm
{
    public partial class REPORT_VIEWER : Form
    {
        public REPORT_VIEWER()
        {
            InitializeComponent();
        }

        private void rpt_customers_details_Load(object sender, EventArgs e)
        {
            try
            {
                String RES_NO = REPORT.RESERVATION_NO;
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(Application.StartupPath + "\\reports\\RECEIPT.rpt");
                ParameterFieldDefinitions crParameterFieldDefinitions;
                ParameterFieldDefinition crParameterFieldDefinition;
                ParameterValues crParameterValues = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
                crParameterDiscreteValue.Value = RES_NO;
                crParameterFieldDefinition = crParameterFieldDefinitions["RESERVATION_NO"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }

        }
    }
}
