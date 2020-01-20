using MultiMine.Model;
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

namespace MultiMine
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private int size;
        private int percentageMines;
        private GameBoard gameBoard;

        private RowDefinition[] rows;
        private ColumnDefinition[] columns;

        public GameWindow(int size)
        {
            InitializeComponent();
            this.size = size;
            this.percentageMines = 20; //hardcoded difficulty TODO: changable

            this.Width = (size + 2) * 32;
            this.Height = (size + 3) * 32;

            //Initializing gameBoard
            gameBoard = new GameBoard(size, size, size * size * (percentageMines / 100));

            //Initializing tileGrid
            
            mainGrid.Children.Clear();


            loadGrid();

        }

        private void loadGrid()
        {

            // Setting size Grid
            mainGrid.Height = size * 32;
            mainGrid.Width = size * 32;

            // Adding Rows and Colums to Grid.
            rows = new RowDefinition[size];
            columns = new ColumnDefinition[size];


            for (int i = 0; i < size; i++)
            {
                rows[i] = new RowDefinition();
                mainGrid.RowDefinitions.Add(rows[i]);
                // Setting Row height
                rows[i].Height = new GridLength(32);
                for (int j = 0; j < size; j++)
                {
                    columns[j] = new ColumnDefinition();
                    mainGrid.ColumnDefinitions.Add(columns[j]);
                    // Setting Collumn width
                    columns[j].Width = new GridLength(32);

                    // Creating image
                    Image image = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri((gameBoard.Tiles[i + j].imagePath), UriKind.Relative);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    image.Source = bitmap;

                    // Creating button
                    Button button = new Button();
                    button.Height = 32;
                    button.Width = 32;
                    button.Content = image;
                    
                    // Adding button to Grid
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    mainGrid.Children.Add(button);
                }
            }
        }
    }
}
