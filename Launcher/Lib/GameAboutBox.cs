using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Launcher
{
    internal class GameAboutBox : Form
    {
        private IContainer components;
        private Label label1;
        private Label labelVersion;
        private Button okButton;
        private PictureBox pictureBox1;
        private ListView VersionsList;

        public GameAboutBox()
        {
            this.InitializeComponent();
            this.Text = string.Format("About {0}", "Game Launcher");
            this.labelVersion.Text = string.Format("Game CSharpLauncher Version {0}", this.AssemblyVersion);
            this.VersionsList.View = View.Details;
            this.VersionsList.Columns.Add("Game", 0x97, HorizontalAlignment.Left);
            this.VersionsList.Columns.Add("Version", 0x98, HorizontalAlignment.Left);
            List<string> list = Launcher.GameLauncher._ConfigManager.getInstalledGames();
            for (int i = 0; i < list.Count; i++)
            {
                GameConfig gameConfig = GameLauncher._ConfigManager.GetGameConfig(list[i]);
                this.VersionsList.Items.Add("Game", gameConfig.strGameName, 0);
                this.VersionsList.Items[i].SubItems.Add(gameConfig.iVersion.ToString());
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
            this.labelVersion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.okButton = new System.Windows.Forms.Button();
            this.VersionsList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(113, 126);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(206, 13);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Outspark SharpLauncher Version 1.0.0.31";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(408, 111);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(171, 295);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click_1);
            // 
            // VersionsList
            // 
            this.VersionsList.Location = new System.Drawing.Point(59, 187);
            this.VersionsList.Name = "VersionsList";
            this.VersionsList.Size = new System.Drawing.Size(307, 97);
            this.VersionsList.TabIndex = 4;
            this.VersionsList.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(162, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Giochi Installati";
            // 
            // GameAboutBox
            // 
            this.ClientSize = new System.Drawing.Size(432, 328);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VersionsList);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelVersion);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameAboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Informazioni";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void okButton_Click_1(object sender, EventArgs e)
        {
            base.Close();
        }

        public string AssemblyCompany
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute) customAttributes[0]).Company;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute) customAttributes[0]).Product;
            }
        }

        public string AssemblyTitle
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (customAttributes.Length > 0)
                {
                    AssemblyTitleAttribute attribute = (AssemblyTitleAttribute) customAttributes[0];
                    if (attribute.Title != "")
                    {
                        return attribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}

