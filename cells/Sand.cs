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
