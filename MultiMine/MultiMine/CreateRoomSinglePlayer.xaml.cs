﻿using System;
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
    public partial class CreateRoomSinglePlayer : Window {
        public CreateRoomSinglePlayer()
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
    }
}