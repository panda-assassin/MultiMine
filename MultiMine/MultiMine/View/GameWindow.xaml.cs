using MultiMine.Controller;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SharedClasses;
using MultiMine.Model;

namespace MultiMine
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window, GameBoardListener {
        private int size;
        private int percentageMines;

        private GameBoardManager manager;

        private RowDefinition[] rows;
        private ColumnDefinition[] columns;

        public GameWindow(int size)
        {
            InitializeComponent();
            this.size = size;
            this.percentageMines = 25; //hardcoded difficulty TODO: changable

            this.Width = (size + 2) * 32;
            this.Height = (size + 3) * 32;

            //Initializing gameBoard
            double mines = size * size * (percentageMines / 100.0);
            GameBoard gameBoard = new GameBoard(size, size, (int) mines);
            manager = new GameBoardManager(gameBoard, this);

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

            int counter = 0;
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


                    // Creating Tile
                    Tile tile = new Tile(counter, i, j);
                    counter++;
                    // Creating image
                    Image image = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri((manager.getGameBoard().Tiles[tile.iD].imagePath), UriKind.Relative);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    image.Source = bitmap;

                    // Creating button
                    Button button = new Button();
                    button.Height = 32;
                    button.Width = 32;
                    button.Content = image;
                    button.Tag = i + "+" + j;


                    // Setting button event listener
                    button.Click += new RoutedEventHandler(onButtonClick);
                    button.MouseRightButtonDown += new MouseButtonEventHandler(onRightButtonCLick);



                    // Adding button to Grid
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    mainGrid.Children.Add(button);

                }
            }
        }

        private void onButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string[] tag = button.Tag.ToString().Split("+".ToCharArray());
            int x = Convert.ToInt32(tag[0]);
            int y = Convert.ToInt32(tag[1]);

            manager.onLeftClick(y, x);
        }

        private void onRightButtonCLick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string[] tag = button.Tag.ToString().Split("+".ToCharArray());
            int x = Convert.ToInt32(tag[0]);
            int y = Convert.ToInt32(tag[1]);

            manager.onRightClick(y, x);
        }

        public void gameBoardUpdated()
        {
            loadGrid();
            textField.Text = "Mines: +" + manager.getGameBoard().MineCount;
            if (manager.getGameBoard().Status == GameStatus.Completed)
            {
                textField.Text = "Game Completed!";
            }
            if (manager.getGameBoard().Status == GameStatus.Failed)
            {
                textField.Text = "Game Failed!";
            }
        }
    }
}
