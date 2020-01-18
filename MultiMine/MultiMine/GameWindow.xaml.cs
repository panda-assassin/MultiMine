using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MultiMine.Properties;


namespace MultiMine
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            CreateWPFGrid(10, 10);
        }

        public void CreateWPFGrid(int width, int height)
        {
            Grid grid = new Grid();
            // grid.Width = 400;
            // grid.Height = 400;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;


            grid.Background = new SolidColorBrush(Colors.LightSteelBlue);

            grid.ShowGridLines = true;

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(60);
                grid.ColumnDefinitions.Add(gridCol1);
            }

            for (int i = 0; i < height; i++)
            {
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(60);
                grid.RowDefinitions.Add(gridRow1);
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Button button = new Button();
                    
                }

            }



            this.Content = grid;
        }
    }
}
