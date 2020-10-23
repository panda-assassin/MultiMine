using MultiMine.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MultiMine.View {
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Window {

        List<string> dataSource;
        public Lobby()
        {
            InitializeComponent();

            Connector.GetInstance().requestRoomList();

            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => {

                while (true)
                {
                    if (Connector.GetInstance().GotRooms())
                    {
                        dataSource = Connector.GetInstance().rooms;
                        LobbyBox.DataContext = dataSource;
                        foreach (string room in dataSource) {
                            LobbyBox.Items.Add(room);
                            }
                        Connector.GetInstance().gotRooms = false;
                        break;
                    }
                }

            }));

        }

        private void CreateLobbyClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Connector.GetInstance().createRoom();

            // TODO: Request created room from server to know what room to show. 

            CreateRoomMultiPlayer multiPlayerRrom = new CreateRoomMultiPlayer();
            multiPlayerRrom.Closed += (s, args) => this.Close();
            multiPlayerRrom.Show();
        }

        private void JoinLobbyClick(object sender, RoutedEventArgs e)
        {
            

            Connector.GetInstance().getRoom(LobbyBox.SelectedItem.ToString());

            this.Hide();
            CreateRoomMultiPlayer multiPlayerRrom = new CreateRoomMultiPlayer();
            multiPlayerRrom.Closed += (s, args) => this.Close();
            multiPlayerRrom.Show();

            // TODO: Request created room from server to know what room to show. 

            //Connector.GetInstance().joinHost(LobbyBox.SelectedItem.ToString());
            //Application.Current.Dispatcher.BeginInvoke(
            //DispatcherPriority.Background,
            //new Action(() => {
            //    while (true)
            //    {
            //        if (Connector.GetInstance().GotGameBoard())
            //        {
            //            this.Hide();
            //            GameWindow gameWindow = new GameWindow(Connector.GetInstance().localGameBoard);
            //            gameWindow.Closed += (s, args) => this.Close();
            //            gameWindow.Show();
            //            break;
            //        }
            //    }
            //}));

        }
    }
}
