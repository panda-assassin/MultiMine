using MultiMineCode;
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

namespace MultiMine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Connector connector;

        public MainWindow()
        {
            InitializeComponent();
            //connector = Connector.GetInstance();
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            CreateRoomSinglePlayer singlePlayerRoom = new CreateRoomSinglePlayer();
            singlePlayerRoom.Closed += (s, args) => this.Close();
            singlePlayerRoom.Show();
            //TODO: Start create room_singleplayer
        }

        private void MultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Start create room_multiplayer
        }
    }
}
