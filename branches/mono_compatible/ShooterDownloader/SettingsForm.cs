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

            for (int i = ShooterConst.MinHttpTimeout; i <= ShooterConst.MaxHttpTimeout; 
                i += ShooterConst.HttpTimeoutIncrement)
            {
                cbHttpTimeout.Items.Add(i);
            }

            UpdateShellExtButton();
            
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            //Load settings and check value format.

            //1 <= concurrentNum <= ShooterConst.MaxConcurrentJobs
            int concurrentNum = 
                Util.GetGetBoundedValue(Properties.Settings.Default.MaxConcurrentJobs, 
                    1, ShooterConst.MaxConcurrentJobs);
            SelectCbItemByValue(cbConcurrenctNum, concurrentNum);
            
            int maxHttpTimeout =
                Util.GetGetBoundedValue(Properties.Settings.Default.HttpTimeout,
                    ShooterConst.MinHttpTimeout, ShooterConst.MaxHttpTimeout);
            maxHttpTimeout = maxHttpTimeout - (maxHttpTimeout % 10);
            SelectCbItemByValue(cbHttpTimeout, maxHttpTimeout);

            txtVideoFileExt.Text = Properties.Settings.Default.VideoFileExt;

            chkEnableLog.Checked = Properties.Settings.Default.EnableLog;

            

            if (concurrentNum != Properties.Settings.Default.MaxConcurrentJobs)
                _formIsDirty = true;
            else if (maxHttpTimeout != Properties.Settings.Default.HttpTimeout)
                _formIsDirty = true;
            else
                _formIsDirty = false;

            if (Util.IsWindows)
            {
                chkEnableConvert.Checked = 
                    Properties.Settings.Default.AutoChsToChtConversion;
                Util.AddShieldToButton(btnEnableShellExt);
            }
            else
            {
                chkEnableConvert.Checked = false;
                chkEnableConvert.Enabled = false;
                toolTip.SetToolTip(
                    chkEnableConvert, Properties.Resources.InfoDisabledInThisOs);
                btnEnableShellExt.Enabled = false;
                toolTip.SetToolTip(
                    btnEnableShellExt, Properties.Resources.InfoDisabledInThisOs);
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
                string subkey = String.Format("CLSID\\{0}", Properties.Settings.Default.ShellExtClsid);
                try
                {
                    if (hkcr.OpenSubKey(subkey, false) == null)
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
                hkcr.Close();
                return true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if (_formIsDirty)
            {
                Properties.Settings.Default.MaxConcurrentJobs = 
                    GetCbSelectedValueInt(cbConcurrenctNum, 1);
                Properties.Settings.Default.VideoFileExt = txtVideoFileExt.Text;
                Properties.Settings.Default.EnableLog = chkEnableLog.Checked;
                Properties.Settings.Default.AutoChsToChtConversion = chkEnableConvert.Checked;
                Properties.Settings.Default.HttpTimeout = 
                    GetCbSelectedValueInt(cbHttpTimeout, ShooterConst.MaxHttpTimeout);
                Properties.Settings.Default.Save();
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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

        private void cbHttpTimeout_SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnEnableShellExt_Click(object sender, EventArgs e)
        {
            string shellExtPath;

            if (Util.Is64BitOS)
            {
                shellExtPath =
                    String.Format("{0}\\{1}",
                    Application.StartupPath,
                    Properties.Settings.Default.ShellExtFileNameX64);
            }
            else
            {
                shellExtPath =
                    String.Format("{0}\\{1}",
                    Application.StartupPath,
                    Properties.Settings.Default.ShellExtFileName);
            }
            
            if (!IsShellExtRegistered)
            {
                if (Util.RegisterDll(shellExtPath))
                {
                    MessageBox.Show(
                        Properties.Resources.InfoEnableShellExtOk,
                        Properties.Resources.InfoTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        Properties.Resources.ErrEnableShellExt,
                        Properties.Resources.InfoTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (Util.UnregisterDll(shellExtPath))
                {
                    MessageBox.Show(
                        Properties.Resources.InfoDisableShellExtOk,
                        Properties.Resources.InfoTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        Properties.Resources.ErrDisableShellExt,
                        Properties.Resources.InfoTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            UpdateShellExtButton();
        }

        private void UpdateShellExtButton()
        {
            if (IsShellExtRegistered)
            {
                btnEnableShellExt.Text = Properties.Resources.UiDisableShellExt;
            }
            else
            {
                btnEnableShellExt.Text = Properties.Resources.UiEnableShellExt;
            }
        }

        private static void SelectCbItemByValue(ComboBox cb, object value)
        {
            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (cb.Items[i].ToString() == value.ToString())
                {
                    cb.SelectedIndex = i;
                    return;
                }
            }
        }

        private static int GetCbSelectedValueInt(ComboBox cb, int defaultValue)
        {
            int value;
            try
            {
                value = Int32.Parse(cb.Text);
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }

    }
}
