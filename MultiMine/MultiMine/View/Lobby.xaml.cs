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

            Connector.GetInstance().requestClientList();

            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => {

                while (true)
                {
                    if (Connector.GetInstance().GotClients())
                    {
                        dataSource = Connector.GetInstance().clients;
                        LobbyBox.DataContext = dataSource;
                        foreach (string client in dataSource) {
                            LobbyBox.Items.Add(client);
                            }
                        Connector.GetInstance().gotClients = false;
                        break;
                    }
                }

            }));

        }

        private void CreateLobbyClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CreateRoomMultiPlayer multiPlayerRrom = new CreateRoomMultiPlayer();
            multiPlayerRrom.Closed += (s, args) => this.Close();
            multiPlayerRrom.Show();
        }

        private void JoinLobbyClick(object sender, RoutedEventArgs e)
        {
            //add
        }
    }
}
