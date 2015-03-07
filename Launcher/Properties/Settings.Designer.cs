using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Launcher.Properties
{
    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings)SettingsBase.Synchronized(new Settings()));

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("False")]
        public bool alphaLogin
        {
            get
            {
                return (bool)this["alphaLogin"];
            }
            set
            {
                this["alphaLogin"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool alphaPatches
        {
            get
            {
                return (bool)this["alphaPatches"];
            }
            set
            {
                this["alphaPatches"] = value;
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }
    }
}
