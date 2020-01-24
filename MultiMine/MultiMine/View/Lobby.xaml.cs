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

namespace MultiMine.View {
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Window {
        public Lobby()
        {
            InitializeComponent();

            Connector.GetInstance().requestClientList();


            var dataSource = Connector.GetInstance().clients;

            
        }

        private void LobbyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
