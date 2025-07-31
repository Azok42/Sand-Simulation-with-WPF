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

public class Water : Cell
{
	static Random r = new Random();
	bool movingRight = true;

	public Water(int row, int col)
	{
		this.row = row;
		this.col = col;
		Grid.SetRow(rect, row);
		Grid.SetColumn(rect, col);
		rect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, (Byte)r.Next(200, 255)));
	}
	public override void update()
	{
		if(row+1 < MainWindow.cellHeightCount)
		{
			if(MainWindow.nextCells[row+1, col] == null)
			{
				Grid.SetRow(rect, row+1);
				MainWindow.nextCells[row+1, col] = this;
				MainWindow.nextCells[row, col] = null;
				MainWindow.currentCells[row, col] = null;
				row++;
				return;
			}
	
			if(col+1 < MainWindow.cellWidthCount && MainWindow.nextCells[row+1, col+1] == null)
			{
		 		Grid.SetRow(rect, row+1);
		 		Grid.SetColumn(rect, col+1);
				MainWindow.nextCells[row+1, col+1] = this;
		  		MainWindow.nextCells[row, col] = null;
		  		MainWindow.currentCells[row, col] = null;
		  		row++;
		  		col++;
		  		return;
			}
	
			if(col-1 >= 0 && MainWindow.nextCells[row+1, col-1] == null) 
			{
		  		Grid.SetRow(rect, row+1);
		  		Grid.SetColumn(rect, col-1);
				MainWindow.nextCells[row+1, col-1] = this;
		  		MainWindow.nextCells[row, col] = null;
		  		MainWindow.currentCells[row, col] = null;
		  		row++;
		  		col--;
		  		return;
			}
		}
		
		if(movingRight && col+1 < MainWindow.cellWidthCount && MainWindow.nextCells[row, col+1] == null)
		{
			Grid.SetColumn(rect, col+1);
			MainWindow.nextCells[row, col+1] = this;
			MainWindow.nextCells[row, col] = null;
			MainWindow.currentCells[row, col] = null;
			col++;
			return;
		}
		else 
		{
			movingRight = false;
			return;
		}

		if(!movingRight && col-1 >= 0 && MainWindow.nextCells[row, col-1] == null)
		{
			Grid.SetColumn(rect, col-1);
			MainWindow.nextCells[row, col-1] = this;
			MainWindow.nextCells[row, col] = null;
			MainWindow.currentCells[row, col] = null;
			col--;
			return;
		}
		else 
		{
			movingRight = true;
			return;
		}
	}
}
