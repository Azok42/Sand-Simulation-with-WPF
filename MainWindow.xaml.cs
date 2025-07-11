using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sand_Simulation_with_WPF;

public partial class MainWindow : Window
{
    Grid baseGrid = new Grid();

    public MainWindow()
    {
        InitializeComponent();

        genGrid();
    }

    private void genCell(object sender, RoutedEventArgs e) {
        if(System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed)
        {
            Rectangle rect = sender as Rectangle;
            rect.Fill = System.Windows.Media.Brushes.Blue;
        };
    }

    private void genGrid() {
        double GRID_SIZE = 40.0;
        double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
        double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
        int cellHeightCount = (int)(screenHeight / GRID_SIZE);
        int cellWidthCount = (int)(screenWidth / GRID_SIZE);

        baseGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
        baseGrid.VerticalAlignment = VerticalAlignment.Stretch;
        baseGrid.ShowGridLines = true;

        for (int i = 0; i < cellHeightCount; i++) {
            baseGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_SIZE) });
        }

        for (int i = 0; i < cellWidthCount; i++) {
            baseGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GRID_SIZE) });
            for (int j = 0; j < cellHeightCount; j++) {
                Rectangle rect = new Rectangle();
                rect.MouseEnter += genCell;
                Grid.SetRow(rect, j);
                Grid.SetColumn(rect, i);
                rect.Fill = System.Windows.Media.Brushes.Transparent;
                baseGrid.Children.Add(rect);
            }
        }

        this.Content = baseGrid;
    }
}