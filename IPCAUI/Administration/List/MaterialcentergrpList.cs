﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eSunSpeed.BusinessLogic;
using eSunSpeedDomain;


namespace IPCAUI.Administration.List
{
    public partial class MaterialcentergrpList : Form
    {
        MaterialCentreGroupMaster objgroupbl = new MaterialCentreGroupMaster();
        public MaterialcentergrpList()
        {
            InitializeComponent();
        }

        private void MaterialcentergrpList_Load(object sender, EventArgs e)
        {
            List<eSunSpeedDomain.MaterialCentreGroupMasterModel> lstGroups = objgroupbl.GetAllMaterialGroups();
            dvgMCgrpList.DataSource = lstGroups;
        }


        private void dvgMCgrpList_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void dvgMCgrpDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void dvgMCgrpDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == '\r')
            {
                MaterialCentreGroupMasterModel lstMaterials;

                lstMaterials = (MaterialCentreGroupMasterModel)dvgMCgrpDetails.GetRow(dvgMCgrpDetails.FocusedRowHandle);
                Materialcentergroup.MCGId = lstMaterials.MCG_ID;

                this.Close();
            }
        }

        private void dvgMCgrpDetails_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            MaterialCentreGroupMasterModel lstMaterials;

            lstMaterials = (MaterialCentreGroupMasterModel)dvgMCgrpDetails.GetRow(dvgMCgrpDetails.FocusedRowHandle);
            Materialcentergroup.MCGId = lstMaterials.MCG_ID;

            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
