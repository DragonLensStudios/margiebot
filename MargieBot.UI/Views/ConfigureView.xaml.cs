using System;
using System.Windows.Controls;
using MargieBot.UI.ViewModels;

namespace MargieBot.UI.Views
{
    public partial class ConfigureView : UserControl
    {

        public ConfigureView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RememberKeyCheck.IsChecked = Properties.Settings.Default.RememberKey;

            if (Properties.Settings.Default.RememberKey)
            {
                SlackKeyText.Text = Properties.Settings.Default.SlackKey;
            }

        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }
    }
}