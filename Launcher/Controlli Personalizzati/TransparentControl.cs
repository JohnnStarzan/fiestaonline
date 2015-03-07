namespace CustomControl
{
    using System;
    using System.Windows.Forms;

    internal class TransparentControl : Control
    {
        public TransparentControl()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}

