using System;
using System.Diagnostics; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MEC_Analysis.DAL;
namespace MEC_Analysis
{
    public partial class MDIParentForm : Form
    {

        MEC melting = new MEC();
        private int childFormNumber = 0;
        private String _logonUser = "管理员";
        private String _userShift;
        private String _currentShift;
        private String _host;

        private Boolean _canConnectDb = false;
        Int32 _Connect = 0;
        private Int32 Connect_num = 0;

        public MDIParentForm()
        {
            InitializeComponent();

        }

        public String LoginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
            }
        }

        private void MDIParentForm_Load(object sender, EventArgs e)
        {
            this.SetMenuEnabled(false);
        }

        private void menuIntroduce_Click(object sender, EventArgs e)
        {
            this.OpenIntroduceForm();
        }

        private void menuRelogin_Click(object sender, EventArgs e)
        {
            _logonUser = "无Host";
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }

            this.SetMenuEnabled(false);

            this.OpenLoginForm();
        }
       
        private static IntroduceForm myIntroduceForm;
        public void OpenIntroduceForm()
        {
            if (myIntroduceForm != null && !myIntroduceForm.IsDisposed)
            {
                myIntroduceForm.Close();
                myIntroduceForm.Dispose();
            }
            myIntroduceForm = new IntroduceForm();

            this.OpenWindow(myIntroduceForm, false, true);
        }
        private static SteelRouteForm mySteelRouteForm;
        public void OpenSteelRouteForm()
        {
            if (mySteelRouteForm != null && !mySteelRouteForm.IsDisposed)
            {
                mySteelRouteForm.Close();
                mySteelRouteForm.Dispose();
            }
            mySteelRouteForm = new SteelRouteForm();

            this.OpenWindow(mySteelRouteForm, false, true);
        }

        private static SteelYieldForm mySteelYieldForm;
        public void OpenSteelYieldForm()
        {
            if (mySteelYieldForm != null && !mySteelYieldForm.IsDisposed)
            {
                mySteelYieldForm.Close();
                mySteelYieldForm.Dispose();
            }
            mySteelYieldForm = new SteelYieldForm();

            this.OpenWindow(mySteelYieldForm, false, true);
        }
        private static BoundaryForm myBoundaryForm;
        public void OpenBoundaryForm()
        {
            if (myBoundaryForm != null && !myBoundaryForm.IsDisposed)
            {
                myBoundaryForm.Close();
                myBoundaryForm.Dispose();
            }
            myBoundaryForm = new BoundaryForm();

            this.OpenWindow(myBoundaryForm, false, true);
        }
        private static DataCalculateForm myDataCalculateForm;
        public void OpenDataCalculateForm()
        {
            if (myDataCalculateForm != null && !myDataCalculateForm.IsDisposed)
            {
                myDataCalculateForm.Close();
                myDataCalculateForm.Dispose();
            }
            myDataCalculateForm = new DataCalculateForm();

            this.OpenWindow(myDataCalculateForm, false, true);
        }
        private static UpstreamForm myUpstreamForm;
        public void OpenUpstreamForm()
        {
            if (myUpstreamForm != null && !myUpstreamForm.IsDisposed)
            {
                myUpstreamForm.Close();
                myUpstreamForm.Dispose();
            }
            myUpstreamForm = new UpstreamForm();

            this.OpenWindow(myUpstreamForm, false, true);
        }

        private static LoginForm myLoginForm;
        public void OpenLoginForm()
        {
            if (myLoginForm == null || myLoginForm.IsDisposed)
            {
                myLoginForm = new LoginForm();
                ++childFormNumber;
            }

            this.OpenWindow(myLoginForm, false, true);
        }

        public void ExitSystem()
        {
            Application.Exit();
        }
        private Form currentForm; 

        private void OpenWindow(Form myWindow, bool isDialog, bool isMinimized)
        {
            if (currentForm != null && !currentForm.IsDisposed) 
            {
                currentForm.Close();
                currentForm.Dispose();
            }

            myWindow.ShowInTaskbar = false;

            if (isDialog)
            {
                myWindow.ShowDialog();
                myWindow.Dispose();
            }
            else
            {
                if (isMinimized)
                {
                    myWindow.WindowState = FormWindowState.Maximized;
                }

                myWindow.MdiParent = this;

                myWindow.Activate();
                myWindow.Show();
                currentForm = myWindow; 
            }
        }
        private void MDIParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否要退出系统？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }

        }
        public void SetPrivilege(String Authority)
        {
            ToolStripMenuItem[] nowWindous = new ToolStripMenuItem[4] { menuFunctionForm, menuSteelForm, menuTheoDateForm, menuCalculateForm };
            for (Int32 i = 0; i < nowWindous.Length; i++)
            {
                if (Authority.Substring(i, 1) == "1")
                {
                    nowWindous[i].Enabled = true;
                }
                else
                {
                    nowWindous[i].Enabled = false;
                }
            }
        }
        public void SetMenuEnabled(Boolean menuEnabled)
        {
            menuFunctionForm.Enabled = menuEnabled;
            menuSteelForm.Enabled = menuEnabled;
            menuTheoDateForm.Enabled = menuEnabled;
            menuCalculateForm.Enabled = menuEnabled;
        }

        private void tmrConnect_Tick(object sender, EventArgs e)
        {
            try
            {
                Connect_num = Connect_num + 1;
                
                
                if (Connect_num >= 12)
                {
                    _Connect = 1;
                }
            }
            catch
            {
                _Connect = 0;
            }

            if (_Connect == 0)
            {
                if (Connect_num <= 12)
                {
                    pgbConnect.Value = Connect_num;
                }
            }
            else
            {

                this.OpenLoginForm();
                menuRelogin.Enabled = true;

                gbConnect.Visible = false;
                tmrConnect.Enabled = false;
            }
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            lbHost.Text = _logonUser.Trim();
        }

        public string _loginUser { get; set; }

        private static DataForm myDataFrom;

        private void menuRouteForm_Click(object sender, EventArgs e)
        {
            this.OpenSteelRouteForm();
        }

        private void menuSteelYieldForm_Click(object sender, EventArgs e)
        {
            this.OpenSteelYieldForm();
        }
        private void menuBoundaryForm_Click(object sender, EventArgs e)
        {
            this.OpenBoundaryForm();
        }
        private void menuCalculateForm_Click(object sender, EventArgs e)
        {
            this.OpenDataCalculateForm();
        }
        private void menuFunctionForm_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void gbConnect_Enter(object sender, EventArgs e)
        {

        }

        private void pgbConnect_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel_Click(object sender, EventArgs e)
        {

        }
        
        
        private void menuUpstreamForm_Click(object sender, EventArgs e)
        {
            this.OpenUpstreamForm();
        }
        
        private static EAFWindEnergyProcessForm EAFWindEnergyProcessForm;

        public void OpenEAFWindEnergyProcessForm()
        {
            if (EAFWindEnergyProcessForm != null && !EAFWindEnergyProcessForm.IsDisposed)
            {
                EAFWindEnergyProcessForm.Close();
                EAFWindEnergyProcessForm.Dispose();

            }
            EAFWindEnergyProcessForm = new EAFWindEnergyProcessForm();

            
            EAFWindEnergyProcessForm.FormBorderStyle = FormBorderStyle.None;

            
            EAFWindEnergyProcessForm.Bounds = Screen.PrimaryScreen.WorkingArea;

            this.OpenWindow(EAFWindEnergyProcessForm, false, true);
        }
        private void EAFTES_EAFWES_Click(object sender, EventArgs e)
        {
            this.OpenEAFWindEnergyProcessForm();
        }
        
        private static ProductionCostForm ProductionCostForm;
        public void OpenProductionCostForm()
        {
            if (ProductionCostForm != null && !ProductionCostForm.IsDisposed)
            {
                ProductionCostForm.Close();
                ProductionCostForm.Dispose();
            }
            ProductionCostForm = new ProductionCostForm();

            this.OpenWindow(ProductionCostForm, false, true);
        }
        private void ProductionCost_Click(object sender, EventArgs e)
        {
            this.OpenProductionCostForm();
        }
        
        private static RawMaterialsCostForm RawMaterialsCostForm;
        public void OpenRawMaterialsCostForm()
        {
            if (RawMaterialsCostForm != null && !RawMaterialsCostForm.IsDisposed)
            {
                RawMaterialsCostForm.Close();
                RawMaterialsCostForm.Dispose();
            }
            RawMaterialsCostForm = new RawMaterialsCostForm();

            this.OpenWindow(RawMaterialsCostForm, false, true);
        }
        private void RawMaterialsCost_Click(object sender, EventArgs e)
        {
            this.OpenRawMaterialsCostForm();
        }
        
        private static ElectricityPriceAnalysisForm ElectricityPriceAnalysisForm;
        public void OpenElectricityPriceAnalysisForm()
        {
            if (ElectricityPriceAnalysisForm != null && !ElectricityPriceAnalysisForm.IsDisposed)
            {
                ElectricityPriceAnalysisForm.Close();
                ElectricityPriceAnalysisForm.Dispose();
            }
            ElectricityPriceAnalysisForm = new ElectricityPriceAnalysisForm();

            this.OpenWindow(ElectricityPriceAnalysisForm, false, true);
        }
        private void ElectricityPriceAnalysis_Click(object sender, EventArgs e)
        {
            this.OpenElectricityPriceAnalysisForm();
        }
        
        private static CarbonTaxInfulenceForm CarbonTaxInfulenceForm;
        public void OpenCarbonTaxInfulenceForm()
        {
            if (CarbonTaxInfulenceForm != null && !CarbonTaxInfulenceForm.IsDisposed)
            {
                CarbonTaxInfulenceForm.Close();
                CarbonTaxInfulenceForm.Dispose();
            }
            CarbonTaxInfulenceForm = new CarbonTaxInfulenceForm();

            this.OpenWindow(CarbonTaxInfulenceForm, false, true);
        }
        private void CarbonTaxInfulence_Click(object sender, EventArgs e)
        {
            this.OpenCarbonTaxInfulenceForm();
        }
        
        private static CoefficientVariationForm CoefficientVariationForm;
        public void OpenCoefficientVariationForm()
        {
            if (CoefficientVariationForm != null && !CoefficientVariationForm.IsDisposed)
            {
                CoefficientVariationForm.Close();
                CoefficientVariationForm.Dispose();
            }
            CoefficientVariationForm = new CoefficientVariationForm();

            this.OpenWindow(CoefficientVariationForm, false, true);
        }
        private void CoefficientVariation_Click(object sender, EventArgs e)
        {
            this.OpenCoefficientVariationForm();
        }
        
        private static LowCarbonForm myLowCarbonForm;
        public void OpenLowCarbonForm()
        {
            if (myLowCarbonForm != null && !myLowCarbonForm.IsDisposed)
            {
                myLowCarbonForm.Close();
                myLowCarbonForm.Dispose();
            }
            myLowCarbonForm = new LowCarbonForm();

            this.OpenWindow(myLowCarbonForm, false, true);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        private void menuLowCarbonForm_Click(object sender, EventArgs e)
        {
            this.OpenLowCarbonForm();
        }
        
        private static WindEnergyDistributionMapForm WindEnergyDistributionMapForm;
        public void OpenWindEnergyDistributionMapForm()
        {
            if (WindEnergyDistributionMapForm != null && !WindEnergyDistributionMapForm.IsDisposed)
            {
                WindEnergyDistributionMapForm.Close();
                WindEnergyDistributionMapForm.Dispose();
            }
            WindEnergyDistributionMapForm = new WindEnergyDistributionMapForm();

            this.OpenWindow(WindEnergyDistributionMapForm, false, true);
        }
        private void WindEnergyDistributionMap_Click(object sender, EventArgs e)
        {
            this.OpenWindEnergyDistributionMapForm();
        }
        
        private static ProductionCarbonEfficiencyForm ProductionCarbonEfficiencyForm;
        public void OpenProductionCarbonEfficiencyForm()
        {
            if (ProductionCarbonEfficiencyForm != null && !ProductionCarbonEfficiencyForm.IsDisposed)
            {
                ProductionCarbonEfficiencyForm.Close();
                ProductionCarbonEfficiencyForm.Dispose();
            }
            ProductionCarbonEfficiencyForm = new ProductionCarbonEfficiencyForm();

            this.OpenWindow(ProductionCarbonEfficiencyForm, false, true);
        }
        private void ProductionCarbonEfficiency_Click(object sender, EventArgs e)
        {
            this.OpenProductionCarbonEfficiencyForm();
        }
        
        private static EconomicCarbonEfficiencyForm EconomicCarbonEfficiencyForm;
        public void OpenEconomicCarbonEfficiencyForm()
        {
            if (EconomicCarbonEfficiencyForm != null && !EconomicCarbonEfficiencyForm.IsDisposed)
            {
                EconomicCarbonEfficiencyForm.Close();
                EconomicCarbonEfficiencyForm.Dispose();
            }
            EconomicCarbonEfficiencyForm = new EconomicCarbonEfficiencyForm();

            this.OpenWindow(EconomicCarbonEfficiencyForm, false, true);
        }
        private void EconomicCarbonEfficiency_Click(object sender, EventArgs e)
        {
            this.OpenEconomicCarbonEfficiencyForm();
        }
        
        private static EnvironmentalCarbonEfficiencyForm EnvironmentalCarbonEfficiencyForm;
        public void OpenEnvironmentalCarbonEfficiencyForm()
        {
            if (EnvironmentalCarbonEfficiencyForm != null && !EnvironmentalCarbonEfficiencyForm.IsDisposed)
            {
                EnvironmentalCarbonEfficiencyForm.Close();
                EnvironmentalCarbonEfficiencyForm.Dispose();
            }
            EnvironmentalCarbonEfficiencyForm = new EnvironmentalCarbonEfficiencyForm();

            this.OpenWindow(EnvironmentalCarbonEfficiencyForm, false, true);
        }
        private void EnvironmentalCarbonEfficiency_Click(object sender, EventArgs e)
        {
            this.OpenEnvironmentalCarbonEfficiencyForm();
        }
        
        private static ElectricityInfluenceCarbonReductionForm ElectricityInfluenceCarbonReductionForm;
        public void OpenElectricityInfluenceCarbonReductionForm()
        {
            if (ElectricityInfluenceCarbonReductionForm != null && !ElectricityInfluenceCarbonReductionForm.IsDisposed)
            {
                ElectricityInfluenceCarbonReductionForm.Close();
                ElectricityInfluenceCarbonReductionForm.Dispose();
            }
            ElectricityInfluenceCarbonReductionForm = new ElectricityInfluenceCarbonReductionForm();

            this.OpenWindow(ElectricityInfluenceCarbonReductionForm, false, true);
        }
        private void ElectricityInfluenceCarbonReduction_Click(object sender, EventArgs e)
        {
            this.OpenElectricityInfluenceCarbonReductionForm();
        }
        
        
        private void ME_Sinter_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_SinterFix"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "烧结单元的物料-能源平衡表"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        private void ME_BF_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_BFFix"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "高炉炼铁单元的物料-能源平衡表"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        private void ME_Electrode_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_ElectrodeFix"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "石墨电极的物料能源平衡表"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        
        private static LCCFFunctionUnitForm LCCFFunctionUnitForm;
        public void OpenLCCFFunctionUnitForm()
        {
            if (LCCFFunctionUnitForm != null && !LCCFFunctionUnitForm.IsDisposed)
            {
                LCCFFunctionUnitForm.Close();
                LCCFFunctionUnitForm.Dispose();
            }
            LCCFFunctionUnitForm = new LCCFFunctionUnitForm();

            this.OpenWindow(LCCFFunctionUnitForm, false, true);
        }
        private void LCCFFunctionUnit_Click(object sender, EventArgs e)
        {
            this.OpenLCCFFunctionUnitForm();
        }
        
        private void ME_BF30_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_BF30"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "不同电力模式下30%铁水比高炉冶炼过程的碳足迹"); 
            this.OpenWindow(myDataFrom, false, true); 
        }

        private void ME_Sinter30_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_Sinter30"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "不同电力模式下30%铁水比烧结过程的碳足迹"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        private void ME_EAF30_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_EAF30"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "不同电力模式下30%铁水比电弧炉炼钢过程的碳足迹"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        
        private void ME_Sinter50_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_Sinter50"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "不同电力模式下50%铁水比烧结过程的碳足迹"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        private void EAF50_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_EAF50"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "不同电力模式下50%铁水比电弧炉炼钢过程的碳足迹"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        private void ME_BF50_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_BF50"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "不同电力模式下50%铁水比高炉冶炼过程的碳足迹"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
        
        private void ME_PC50_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_PC50"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "50%铁水比总成本"); 
            this.OpenWindow(myDataFrom, false, true); 
        }

        private void ME_PC30_Click(object sender, EventArgs e)
        {
            DataTable Table = melting.GetDataTable("ME_WES_PC30"); 
            if (myDataFrom != null && !myDataFrom.IsDisposed)
            {
                myDataFrom.Close();
                myDataFrom.Dispose();
            }
            myDataFrom = new DataForm(Table, "30%铁水比总成本"); 
            this.OpenWindow(myDataFrom, false, true); 
        }
    }
}
