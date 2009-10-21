/*
 *   Shooter Subtitle Downloader: Automatic Subtitle Downloader for the http://shooter.cn.
 *   Copyright (C) 2009  John Fung
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Affero General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Affero General Public License for more details.
 *
 *   You should have received a copy of the GNU Affero General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace ShooterDownloader
{
    public partial class SettingsForm : Form
    {
        private bool _formIsDirty = false;

        public SettingsForm()
        {
            InitializeComponent();

            lblTitleVersion.Text = String.Format("{0} {1}", 
                Properties.Resources.InfoTitle, 
                Application.ProductVersion.ToString());

            for (int i = 1; i <= ShooterConst.MaxConcurrentJobs; i++)
            {
                cbConcurrenctNum.Items.Add(i);
            }
        }

        //Indicate at least one of the setting
        public bool IsDirty
        {
            get { return _formIsDirty; }
        }

        public bool IsShellExtRegistered
        {
            get
            {
                RegistryKey hkcr = Registry.ClassesRoot;
                string subkey = String.Format("CLSID\\{0}", ShooterConst.ShellExtClsid);
                try
                {
                    if (hkcr.OpenSubKey(subkey, false) == null)
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Properties.Settings.Default.MaxConcurrentJobs = cbConcurrenctNum.SelectedIndex + 1;
            Properties.Settings.Default.VideoFileExt = txtVideoFileExt.Text;
            Properties.Settings.Default.EnableLog = chkEnableLog.Checked;
            Properties.Settings.Default.AutoChsToChtConversion = chkEnableConvert.Checked;
            Properties.Settings.Default.Save();

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {          
            //1 <= concurrentNum <= ShooterConst.MaxConcurrentJobs
            int concurrentNum = Math.Min(Properties.Settings.Default.MaxConcurrentJobs, ShooterConst.MaxConcurrentJobs);
            concurrentNum = Math.Max(1, concurrentNum);
            cbConcurrenctNum.SelectedIndex = concurrentNum - 1;

            txtVideoFileExt.Text = Properties.Settings.Default.VideoFileExt;

            chkEnableLog.Checked = Properties.Settings.Default.EnableLog;

            chkEnableConvert.Checked = Properties.Settings.Default.AutoChsToChtConversion;

            _formIsDirty = false;
        }

        private void btnOpenLogFolder_Click(object sender, EventArgs e)
        {
            string logDir = FileLogger.LogDirectory;
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            Process.Start(logDir);
        }

        private void cbConcurrenctNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            _formIsDirty = true;
        }

        private void txtVideoFileExt_TextChanged(object sender, EventArgs e)
        {
            _formIsDirty = true;
        }

        private void chkEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            _formIsDirty = true;
        }

        private void chkEnableConvert_CheckedChanged(object sender, EventArgs e)
        {
            _formIsDirty = true;
        }
        
    }
}
