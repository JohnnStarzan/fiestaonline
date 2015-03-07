using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher
{
    
    public class Server : Form
    {
        private IContainer components = null;

        public Server(string[] args)
        {
            this.InitializeComponent();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Server));
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            //base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x10d);
            //base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "Server";
            this.Text = "Game Patcher";
            base.ResumeLayout(false);
        }
    }
}

