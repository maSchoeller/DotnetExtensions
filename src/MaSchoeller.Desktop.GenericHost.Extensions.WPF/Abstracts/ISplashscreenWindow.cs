﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts
{
    public interface ISplashscreenWindow
    {
        public string Header { get; set; }

        public string ReportMessage { get; set; }

        public bool IsBusy { get; set; }

        public byte Progress { get; set; }

        void CloseWindow();
    }
}
