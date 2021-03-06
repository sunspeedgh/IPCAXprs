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
    public partial class AccountList : Form
    {
        AccountMasterBL objaccbl = new AccountMasterBL();
        public AccountList()
        {
            InitializeComponent();
        }

        private void AccountList_Load(object sender, EventArgs e)
        {
            List<eSunSpeedDomain.AccountMasterModel> lstaccounts = objaccbl.GetListofAccount();
            dvgAccountList.DataSource = lstaccounts;
        }

        

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void dvgAccountList_DoubleClick(object sender, EventArgs e)
        {
            AccountMasterModel lstMasters;

            lstMasters = (AccountMasterModel)dvgAccountDetails.GetRow(dvgAccountDetails.FocusedRowHandle);
            string cellValue = lstMasters.AccountId.ToString();
        }

        private void dvgAccountList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar =='\r')
            {
                AccountMasterModel lstMasters;

                lstMasters = (AccountMasterModel)dvgAccountDetails.GetRow(dvgAccountDetails.FocusedRowHandle);
                Account.groupId = lstMasters.AccountId;

                this.Close();
            }            
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

        private void dvgAccountDetails_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            AccountMasterModel lstMasters;

            lstMasters = (AccountMasterModel)dvgAccountDetails.GetRow(dvgAccountDetails.FocusedRowHandle);
            Account.groupId = lstMasters.AccountId;

            this.Close();
        }
    }
}
