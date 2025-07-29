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
using System.Threading;

namespace Sand_Simulation_with_WPF;

public partial class MainWindow : Window
{
    static double GRID_SIZE = 40.0;
    static double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
    static double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
    public static int cellHeightCount = (int)(screenHeight / GRID_SIZE) - 1;
    public static int cellWidthCount = (int)(screenWidth / GRID_SIZE) - 1 ;       
    static Grid baseGrid = new Grid();
    static List<Cell> cells = new List<Cell>();


    public MainWindow()
    {
        InitializeComponent();

        genGrid();
	mainLoop();
    }

    async void mainLoop()
    {
	while(true)
	{
	    foreach(Cell cell in cells)
	    {
		cell.update();	
	    }

	    await Task.Delay(30);
	}	
    }

    private void genCell(object sender, RoutedEventArgs e)
    {
        Rectangle rect = (Rectangle)sender;

        if(System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed && !isCellAtGridPos(Grid.GetRow(rect), Grid.GetColumn(rect)))
        {
	    
	    Sand sand = new Sand(Grid.GetRow(rect), Grid.GetColumn(rect));
	    baseGrid.Children.Add(sand.rect);
	    cells.Add(sand);
        };
    }

    public void genGrid()
    {
       	baseGrid.HorizontalAlignment = HorizontalAlignment.Center;
        baseGrid.VerticalAlignment = VerticalAlignment.Center;
        baseGrid.ShowGridLines = true;

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
    
    static public bool isCellAtGridPos(int row, int col)
    {
	foreach(Cell cell in cells)
	{
	    if(Grid.GetRow(cell.rect) == row && Grid.GetColumn(cell.rect) == col)
		return true;
	}
	
	return false;
    }
}

public abstract class Cell
{
    public Rectangle rect = new Rectangle();
    public abstract void update();
}

public class Sand : Cell
{
    int row, col;

    public Sand(int row, int col)
    {
        Grid.SetRow(rect, row);
        Grid.SetColumn(rect, col);
        rect.Fill = System.Windows.Media.Brushes.Yellow;
    }

    public override void update()
    {
	row = Grid.GetRow(rect);
	col = Grid.GetColumn(rect);

	//Move down if nothing is under it
        if(!MainWindow.isCellAtGridPos(row+1, col) && MainWindow.cellHeightCount >= row+1)
	{
	    Grid.SetRow(rect, row+1);
	    return;
	}
	
	//Move right-down if nothing is there
	if(!MainWindow.isCellAtGridPos(row+1, col+1) && MainWindow.cellWidthCount >= col+1 && MainWindow.cellHeightCount >= row+1)
	{
	    Grid.SetRow(rect, row+1);
	    Grid.SetColumn(rect, col+1);
	    return;
	}

	//Move left-down if nothing is there
	if(!MainWindow.isCellAtGridPos(row+1, col-1) && 0  >= col-1 && MainWindow.cellHeightCount >= row+1)
	{
	    Grid.SetRow(rect, row+1);
	    Grid.SetColumn(rect, col-1);
	    return;
	}
    }
}
