using System;
using System.Windows.Forms;
    
namespace Launcher
{
    internal class TransparentRichTextBox : RichTextBox
    {
        public TransparentRichTextBox()
        {
            base.ScrollBars = RichTextBoxScrollBars.None;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x20;
                return createParams;
            }
        }
    }
}

