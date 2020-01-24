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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultiMine {
    /// <summary>
    /// Interaction logic for CreateRoomSinglePlayer.xaml
    /// </summary>
    public partial class CreateRoomMultiPlayer : Window {
        public CreateRoomMultiPlayer()
        {
            InitializeComponent();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Connector.GetInstance().sendChatMessage(chat.Text);
            chatview.Items.Add(chat.Text);
        }
    }
}
