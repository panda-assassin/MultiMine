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

    public enum Difficulty {
        Easy, Normal, Hard, Brutal
    }
    /// <summary>
    /// Interaction logic for CreateRoomSinglePlayer.xaml
    /// </summary>
    public partial class CreateRoomSinglePlayer : Window {
        public CreateRoomSinglePlayer()
        {
            InitializeComponent();
        }



        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            int percentageMines = (DifficultyBox.SelectedIndex) * 10 + 10;
            GameWindow gameWindow = new GameWindow((int)slValue.Value, percentageMines);
            gameWindow.Closed += (s, args) => this.Close();
            gameWindow.Show();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Closed += (s, args) => this.Close();
            mainWindow.Show();
        }
    }
}
