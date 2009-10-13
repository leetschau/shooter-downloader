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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ShooterDownloader
{
    public partial class DownloadForm : Form, ILogger
    {
        private JobQueue _jobQueue = null;
        private delegate void SetLogTextCallback(string msg);
        private ILogger _fileLogger = null;
        private string _lastDir = String.Empty;
        private string _videoFileExt = String.Empty;
        private int _currentMaxJobs = 0;
        private delegate void EnableInputCallback();
        SettingsForm _settingsForm;

        public DownloadForm()
        {
            InitializeComponent();

            LogMan.Instance.RegisterLogger(this);

            Upgrade();

            _settingsForm = new SettingsForm();
            LoadSettings();
            
        }

        private void Upgrade()
        {
            if (Properties.Settings.Default.FirstRun == true)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
            }
        }

        #region ILogger Members

        public void Log(string message)
        {
            if (this.InvokeRequired)
            {
                SetLogTextCallback d = new SetLogTextCallback(SetLogText);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                SetLogText(message);
            }
        }

        public void Log(string message, params Object[] args)
        {
            string formatMessage = String.Format(message, args);
            if (this.InvokeRequired)
            {
                SetLogTextCallback d = new SetLogTextCallback(SetLogText);
                this.Invoke(d, new object[] { formatMessage });
            }
            else
            {
                SetLogText(formatMessage);
            }
        }

        void ILogger.Close()
        {
            //no need to release anything
        }

        #endregion

        //Log implementation
        private void SetLogText(string message)
        {
            toolDownloadMessage.Text = message;
        }

        private void LoadSettings()
        {
            if (_fileLogger == null && Properties.Settings.Default.EnableLog)
            {
                _fileLogger = new FileLogger();
                LogMan.Instance.RegisterLogger(_fileLogger);
            }
            else if (_fileLogger != null && !Properties.Settings.Default.EnableLog)
            {
                LogMan.Instance.RemoveLogger(_fileLogger);
                _fileLogger.Close();
                _fileLogger = null;
            }

            _lastDir = Properties.Settings.Default.LastDir;
            _videoFileExt = Properties.Settings.Default.VideoFileExt;

            int maxJobs = Properties.Settings.Default.MaxConcurrentJobs;
            if (maxJobs != _currentMaxJobs)
            {
                if (_jobQueue != null)
                {
                    _jobQueue.Close();
                }
                _jobQueue = new JobQueue(maxJobs);
                _jobQueue.AllDone +=new AllDoneHandler(AllDownloadComplete);
                _jobQueue.Start();
            }
        }

        private void UpdateLastDir(string lastDir)
        {
            Properties.Settings.Default.LastDir = lastDir;
            Properties.Settings.Default.Save();
            _lastDir = lastDir;
        }

        private void DownloadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _jobQueue.Close();
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            if (_lastDir != String.Empty && Directory.Exists(_lastDir))
            {
                folderBrowserDialog.SelectedPath = _lastDir;
            }
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                UpdateLastDir(folderBrowserDialog.SelectedPath);
                txtDir.Text = folderBrowserDialog.SelectedPath;
                

                //list files in the selected directory.
                DirectoryInfo dirInfo = new DirectoryInfo(txtDir.Text);
                FileInfo[] fileInfoList = dirInfo.GetFiles();
                dgvFileList.Rows.Clear();
                foreach (FileInfo fileInfo in fileInfoList)
                {
                    bool selected = false;
                    
                    if(_videoFileExt.Contains(fileInfo.Extension))
                    {
                        selected = true;
                    }

                    object[] newRow = { selected, fileInfo.Name, "", fileInfo.FullName };
                    int rowIdx = dgvFileList.Rows.Add(newRow);

                    if (selected)
                    {
                        //change the background color of video files
                        DataGridViewRow dataRow = dgvFileList.Rows[rowIdx];
                        dataRow.DefaultCellStyle.BackColor = Color.AntiqueWhite;
                    }
                }
            }
        }

        private void OnDownloadStatusUpdate(object sender, int progress)
        {
            int jobId = ((IJob)sender).JobId;
            if (progress == ShooterConst.Error)
            {
                dgvFileList.Rows[jobId].Cells["StatusColumn"].Value = Properties.Resources.ProgressError;
            }
            else if (progress == ShooterConst.NoSubFound)
            {
                dgvFileList.Rows[jobId].Cells["StatusColumn"].Value = Properties.Resources.ProgressNoSub;
            }
            else
            {
                dgvFileList.Rows[jobId].Cells["StatusColumn"].Value = progress.ToString() + "%";
            }
        }

        private void btnStartBatch_Click(object sender, EventArgs e)
        {
            bool shouldDisableInput = true;

            foreach (DataGridViewRow row in dgvFileList.Rows)
            {
                if ((bool)row.Cells["CheckBoxColumn"].Value == true)
                {
                    if (shouldDisableInput == true)
                    {
                        //disable input if there is at least one checked column.
                        DisableInput();
                        ClearDownloadDtatus();
                        shouldDisableInput = false;
                    }
                    string filePath = (string)row.Cells["FullPathColumn"].Value;

                    ShooterDownloadJob dlJob = new ShooterDownloadJob();
                    dlJob.VideoFilePath = filePath;
                    dlJob.JobId = row.Index;
                    dlJob.ProgressUpdate += new ProgressHandler(OnDownloadStatusUpdate);
                    _jobQueue.AddJob(dlJob);
                }
            }          
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvFileList.Rows)
            {
                row.Cells["CheckBoxColumn"].Value = true;
            }

        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvFileList.Rows)
            {
                row.Cells["CheckBoxColumn"].Value = false;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (_settingsForm.ShowDialog(this) == DialogResult.OK && _settingsForm.IsDirty)
            {
                LoadSettings();
            }
        }

        private void AllDownloadComplete()
        {
            LogMan.Instance.Log(Properties.Resources.InfoAllDownloadOk);
            
            if (this.InvokeRequired)
            {
                EnableInputCallback d = new EnableInputCallback(EnableInput);
                this.Invoke(d);
            }
            else
            {
                EnableInput();
            }
        }

        private void EnableInput()
        {
            btnSelectDir.Enabled = true;
            btnSettings.Enabled = true;
            dgvFileList.Columns["CheckBoxColumn"].ReadOnly = false;
            btnSelectAll.Enabled = true;
            btnSelectNone.Enabled = true;
            btnStartBatch.Enabled = true;
        }

        private void DisableInput()
        {
            btnSelectDir.Enabled = false;
            btnSettings.Enabled = false;
            dgvFileList.Columns["CheckBoxColumn"].ReadOnly = true;
            btnSelectAll.Enabled = false;
            btnSelectNone.Enabled = false;
            btnStartBatch.Enabled = false;
        }

        private void ClearDownloadDtatus()
        {
            foreach (DataGridViewRow row in dgvFileList.Rows)
            {
                row.Cells["StatusColumn"].Value = String.Empty;
            }
        }

    }
}
