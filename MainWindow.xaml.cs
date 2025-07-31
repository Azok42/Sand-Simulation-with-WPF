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
    static double GRID_SIZE = 20.0;
    static double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
    static double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
    public static int cellHeightCount = (int)(screenHeight / GRID_SIZE) - 1;
    public static int cellWidthCount = (int)(screenWidth / GRID_SIZE) - 1 ;       
    public static Grid baseGrid = new Grid();
    public static Cell[,] currentCells = new Cell[cellHeightCount, cellWidthCount];
    public static Cell[,] nextCells = new Cell[cellHeightCount, cellWidthCount];

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
	    foreach(Cell cell in currentCells)
	    {
		if(cell == null)
		    continue;

		cell.update();
	    }
	   
	    for (int i = 0; i < cellHeightCount; i++)
	    {
    		for (int j = 0; j < cellWidthCount; j++)
    		{
        	    currentCells[i, j] = nextCells[i, j];
    		}
	    }
	    
	    await Task.Delay(33);
	}	
    }

    private void genCell(object sender, RoutedEventArgs e)
    {
        Rectangle rect = (Rectangle)sender;
	int row = Grid.GetRow(rect);
	int col = Grid.GetColumn(rect);
	
        if(System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed && nextCells[row, col] == null)
        {
	    
	    Sand sand = new Sand(row, col);
	    nextCells[row, col] = sand;
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

public class Sand : Cell
{
    static Random r = new Random();    

    public Sand(int row, int col)
    {
	this.row = row;
	this.col = col;
        Grid.SetRow(rect, row);
        Grid.SetColumn(rect, col);
        rect.Fill = new SolidColorBrush(Color.FromArgb(255, (Byte)r.Next(230, 250), (Byte)r.Next(190, 210), (Byte)r.Next(100, 110)));;
    }

    public override void update()
    {
	if(row+1 >= MainWindow.cellHeightCount)
	    return;

	//Move down if nothing is under it
        if(MainWindow.nextCells[row+1, col] == null)
	{
	    Grid.SetRow(rect, row+1);
	    MainWindow.currentCells[row, col] = null;
	    MainWindow.nextCells[row+1, col] = this;
	    MainWindow.nextCells[row, col] = null;
	    row++;
	    return;
	}
	
	//Move right-down if nothing is there
	if(col+1 < MainWindow.cellWidthCount && MainWindow.nextCells[row+1, col+1] == null)
	{
	    Grid.SetRow(rect, row+1);
	    Grid.SetColumn(rect, col+1);
	    MainWindow.currentCells[row, col] = null;
	    MainWindow.nextCells[row+1, col+1] = this;
	    MainWindow.nextCells[row, col] = null;
	    row++;
	    col++;
	    return;
	}

	//Move left-down if nothing is there
	if(col-1 >= 0 && MainWindow.nextCells[row+1, col-1] == null) 
	{
	    Grid.SetRow(rect, row+1);
	    Grid.SetColumn(rect, col-1);
	    MainWindow.currentCells[row, col] = null;
	    MainWindow.nextCells[row+1, col-1] = this;
	    MainWindow.nextCells[row, col] = null;
	    row++;
	    col--;
	    return;
	}
    }
}
