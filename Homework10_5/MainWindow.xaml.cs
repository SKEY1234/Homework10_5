using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Homework10_5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramMessageClient client;

        public MainWindow()
        {
            InitializeComponent();

            client = new TelegramMessageClient(this);

            logList.ItemsSource = client.BotMessageLog;

            checkBoxLog.IsChecked = true;
            client.saveLog = true;
        }
        private void btnMsgSendClick(object sender, RoutedEventArgs e)
        {
            client.SendMessage(textBoxMsgSend.Text, TargetSend.Text);
            textBoxMsgSend.Text = "Текст сообщения";
        }

        private void textBoxMsgSend_MouseEnter(object sender, MouseEventArgs e)
        {
            textBoxMsgSend.Text = String.Empty;
        }

        private void textBoxMsgSend_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!textBoxMsgSend.IsFocused)
                textBoxMsgSend.Text = "Текст сообщения";
        }

        private void checkBoxLog_Checked(object sender, RoutedEventArgs e)
        {
            client.saveLog = true;
        }

        private void checkBoxLog_Unchecked(object sender, RoutedEventArgs e)
        {
            client.saveLog = false;
        }
    }
}
