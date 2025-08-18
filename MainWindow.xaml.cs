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
    static double GRID_SIZE = 20.0;
    static double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
    static double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
    public static int cellHeightCount = (int)(screenHeight / GRID_SIZE) - 1;
    public static int cellWidthCount = (int)(screenWidth / GRID_SIZE) - 1 ;       
    public static Grid baseGrid = new Grid();
    public static Cell[,] currentCells = new Cell[cellHeightCount, cellWidthCount];
    public static Cell[,] nextCells = new Cell[cellHeightCount, cellWidthCount];
    public static string cellBrush = "Sand";

    ConfigWindow configWindow = new ConfigWindow();
    
    public MainWindow()
    {
        InitializeComponent();
	
        genGrid();
	
	configWindow.Show();

	mainLoop();
    }

    async void mainLoop()
    {
	while(true)
	{
	    foreach(Cell cell in currentCells)
	    {
		if(cell == null)
		    continue;

		cell.update();
	    }

	    if(Water.colorUpdateCounter < 2)
	        Water.colorUpdateCounter++;
	    else
		Water.colorUpdateCounter = 0;

	    for (int i = 0; i < cellHeightCount; i++)
	    {
    		for (int j = 0; j < cellWidthCount; j++)
    		{
        	    currentCells[i, j] = nextCells[i, j];
    		}
	    }
	    
	    await Task.Delay(30);
	}	
    }

    private void genCell(object sender, RoutedEventArgs e)
    {
        Rectangle rect = (Rectangle)sender;
	int row = Grid.GetRow(rect);
	int col = Grid.GetColumn(rect);
	Cell cell;

        if(System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed && nextCells[row, col] == null)
        {
	    switch(cellBrush)
	    {
	        case "Sand":
		    cell = new Sand(row, col);
		    break;
		case "Water":
		    cell = new Water(row, col);
		    break;
		default:
		    cell = null;
		    break;
	    }

	    nextCells[row, col] = cell;

	    if(cell == null)
		return;

	    baseGrid.Children.Add(nextCells[row, col].rect);
        }
    }

    public void genGrid()
    {
       	baseGrid.HorizontalAlignment = HorizontalAlignment.Center;
        baseGrid.VerticalAlignment = VerticalAlignment.Center;
        baseGrid.ShowGridLines = false;

        for (int i = 0; i < cellHeightCount; i++) {
            baseGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_SIZE) });
        }

        for (int i = 0; i < cellWidthCount; i++) {
            baseGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GRID_SIZE) });
            for (int j = 0; j < cellHeightCount; j++) {
                Rectangle rect = new Rectangle();
                rect.MouseEnter += genCell;
		rect.MouseLeftButtonDown += genCell;
                Grid.SetRow(rect, j);
                Grid.SetColumn(rect, i);
                rect.Fill = System.Windows.Media.Brushes.Silver;
                baseGrid.Children.Add(rect);
            }
        }

        this.Content = baseGrid;
    } 
}

public abstract class Cell
{
    public Rectangle rect = new Rectangle();
    public abstract void update();
    public int row, col;
}
