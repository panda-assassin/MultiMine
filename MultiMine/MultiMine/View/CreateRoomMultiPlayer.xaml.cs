using MultiMine.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MultiMine {
    /// <summary>
    /// Interaction logic for CreateRoomSinglePlayer.xaml
    /// </summary>
    public partial class CreateRoomMultiPlayer : Window , ChatListener {
        public CreateRoomMultiPlayer()
        {
            InitializeComponent();
            ChatManager.GetInstance().setListener(this);
            if (ChatManager.GetInstance().getChat() != null)
            {
                foreach (string message in ChatManager.GetInstance().getChat())
                {
                    chatview.Items.Add(message);
                }
            }
            
        }


        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            //TODO StartGame
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Closed += (s, args) => this.Close();
            mainWindow.Show();
        }

        private void PlayerCountBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chat_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Connector.GetInstance().sendChatMessage(chat.Text);
                chat.Clear();
            }
        }

        public void chatUpdated(string message)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => {
                chatview.Items.Add(message);
            }));
            
        }
    }
}
