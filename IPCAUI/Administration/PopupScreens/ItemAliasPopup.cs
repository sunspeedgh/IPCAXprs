﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPCAUI.Administration.PopupScreens
{
    public partial class ItemAliasPopup : Form
    {
        public ItemAliasPopup()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Loop through the grid and get the values
            
            //Administration.ItemMasterNew.objModel.BarCodes.Add()
        }
    }
}
