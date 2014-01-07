﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Orc.SolutionTool;

namespace SolutionChecker
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : UserControl, IChildView
    {
        ReportViewerViewmodel _viewmodel;
        public ReportViewer(ReportViewerViewmodel viewmodel)
        {
            InitializeComponent();
            _viewmodel = viewmodel;
            viewmodel.View = this;
            DataContext = viewmodel;
        }

        public void SetPayload(object data)
        {
            _viewmodel.SetPayload(data);

            //throw new NotImplementedException();
        }
    }
}
