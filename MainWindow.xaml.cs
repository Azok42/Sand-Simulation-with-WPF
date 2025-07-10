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

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        double GRID_SIZE = 15.0;
        double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
        double screenHeight = System.Windows.SystemParameters.WorkArea.Height;


        Grid baseGrid = new Grid();
        baseGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
        baseGrid.VerticalAlignment = VerticalAlignment.Stretch;
        baseGrid.ShowGridLines = true;

        RowDefinition[] rowDefinition = new RowDefinition[(int)(screenHeight / GRID_SIZE)];
        for (int i = 0; i < (int)(screenHeight / GRID_SIZE); i++) {
            rowDefinition[i] = new RowDefinition();
            rowDefinition[i].Height = new GridLength(GRID_SIZE);
            baseGrid.RowDefinitions.Add(rowDefinition[i]);
        }

        ColumnDefinition[] columnDefinition = new ColumnDefinition[(int)(screenWidth / GRID_SIZE)];
        for (int i = 0; i < (int)(screenWidth / GRID_SIZE); i++) {
            columnDefinition[i] = new ColumnDefinition();
            columnDefinition[i].Width = new GridLength(GRID_SIZE);
            baseGrid.ColumnDefinitions.Add(columnDefinition[i]);
        }

        this.Content = baseGrid;
    }
}