using System;
using System.Windows;

namespace Ticketer.GUI
{
    public partial class MyWindow : Window
    {
        private Main main;

        public MyWindow(Main main)
        {
            this.main = main;
            InitializeComponent();
        }

        public void ClearConversation()
        {
            Dispatcher.Invoke(() => conversation.Text = "");
        }

        public void SetRepertoire(string rep)
        {
            Dispatcher.Invoke(() => repertoire.Text = rep);
        }

        public void AddTextToConversation(string text)
        {
            Dispatcher.Invoke(() => conversation.Text += text + "\n");
        }

        public void ShowStopButton()
        {
            Dispatcher.Invoke(() => stopButton.Visibility = Visibility.Visible);
        }

        public void HideStopButton()
        {
            Dispatcher.Invoke(() => stopButton.Visibility = Visibility.Hidden);
        }

        public void EnableStartButton()
        {
            Dispatcher.Invoke(() => startButton.IsEnabled = true);
        }

        public void DisableStartButton()
        {
            Dispatcher.Invoke(() => startButton.IsEnabled = false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            main.StartMainConversation();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            main.InterruptConversation();
        }
    }
}
