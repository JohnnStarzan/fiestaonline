namespace Launcher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Settings : Form
    {
        private CheckBox AutoCheckAndApplyUpdates;
        private Button CancelButton;
        private CheckBox CheckAndApplyUpdates;
        private IContainer components;
        private Label label2;
        private CheckBox RunInTaskBar;
        private Button Save;

        public Settings()
        {
            this.InitializeComponent();
            switch (GameLauncher._ConfigManager.PrimeConfiguration.iAutoPatch)
            {
                case -1:
                    this.AutoCheckAndApplyUpdates.Checked = true;
                    GameLauncher._ConfigManager.PrimeConfiguration.iAutoPatch = 1;
                    break;

                case 0:
                    this.AutoCheckAndApplyUpdates.Checked = false;
                    break;

                case 1:
                    this.AutoCheckAndApplyUpdates.Checked = true;
                    break;
            }
            switch (GameLauncher._ConfigManager.PrimeConfiguration.iRunInTaskBar)
            {
                case -1:
                    this.RunInTaskBar.Checked = true;
                    GameLauncher._ConfigManager.PrimeConfiguration.iRunInTaskBar = 1;
                    return;

                case 0:
                    this.RunInTaskBar.Checked = false;
                    return;

                case 1:
                    this.RunInTaskBar.Checked = true;
                    return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.RunInTaskBar = new System.Windows.Forms.CheckBox();
            this.AutoCheckAndApplyUpdates = new System.Windows.Forms.CheckBox();
            this.Save = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RunInTaskBar
            // 
            this.RunInTaskBar.AutoSize = true;
            this.RunInTaskBar.Location = new System.Drawing.Point(73, 62);
            this.RunInTaskBar.Name = "RunInTaskBar";
            this.RunInTaskBar.Size = new System.Drawing.Size(127, 17);
            this.RunInTaskBar.TabIndex = 1;
            this.RunInTaskBar.Text = "Minimizza il processo.\r\n";
            this.RunInTaskBar.UseVisualStyleBackColor = true;
            // 
            // AutoCheckAndApplyUpdates
            // 
            this.AutoCheckAndApplyUpdates.AutoSize = true;
            this.AutoCheckAndApplyUpdates.Location = new System.Drawing.Point(73, 26);
            this.AutoCheckAndApplyUpdates.Name = "AutoCheckAndApplyUpdates";
            this.AutoCheckAndApplyUpdates.Size = new System.Drawing.Size(126, 17);
            this.AutoCheckAndApplyUpdates.TabIndex = 3;
            this.AutoCheckAndApplyUpdates.Text = "Controllo Automatico.\r\n";
            this.AutoCheckAndApplyUpdates.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(109, 124);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(68, 32);
            this.Save.TabIndex = 4;
            this.Save.Text = "Salva";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.OnSave);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(284, 127);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 29);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancella";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(314, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Nota: se il launcher non è attivo, le patch non verranno aggiunte.";
            // 
            // Settings
            // 
            this.ClientSize = new System.Drawing.Size(508, 168);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.AutoCheckAndApplyUpdates);
            this.Controls.Add(this.RunInTaskBar);
            this.Name = "Settings";
            this.Text = "Impostazioni";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void OnCancel(object sender, EventArgs e)
        {
            base.Close();
        }

        private void OnCheckForUpdatesClicked(object sender, EventArgs e)
        {
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (this.AutoCheckAndApplyUpdates.Checked)
            {
                GameLauncher._ConfigManager.PrimeConfiguration.iAutoPatch = 1;
            }
            else
            {
                GameLauncher._ConfigManager.PrimeConfiguration.iAutoPatch = 0;
            }
            if (this.RunInTaskBar.Checked)
            {
                GameLauncher._ConfigManager.PrimeConfiguration.iRunInTaskBar = 1;
            }
            else
            {
                GameLauncher._ConfigManager.PrimeConfiguration.iRunInTaskBar = 0;
            }
            base.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
        }
    }
}

