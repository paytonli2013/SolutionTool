﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Orc.SolutionTool
{
    /// <summary>
    /// Interaction logic for ChildView.xaml
    /// </summary>
    public partial class ChildView : UserControl,IMessageService
    {
        private IWindowHost host;
        private string viewName;
        private string title;
        private Action<CloseResult> onClosed;

        private ChildView()
        {
            InitializeComponent();
        }

        public ChildView(IWindowHost host, string viewName, string title, Action<CloseResult> onClosed) :this()
        {
            // TODO: Complete member initialization
            this.host = host;
            this.viewName = viewName;
            this.title = title;
            this.onClosed = onClosed;
        }

        public void SetContent(object content)
        {
            this.content.Content = content;
        }

        public void Show(string message)
        {
            MessageBox.Show(message);
            //throw new NotImplementedException();
        }


        public void Confirm(string message,Action<MessageBoxResult> onConfirmed)
        {
            var result = MessageBox.Show(message,"alert", MessageBoxButton.YesNo);

            var result2 = (MessageBoxResult)((int)result);

            if (onConfirmed != null)
            {
                onConfirmed.Invoke(result2);
            }
            //throw new NotImplementedException();
        }
    }
}
