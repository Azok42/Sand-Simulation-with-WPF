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

public partial class ConfigWindow : Window
{
    	public ConfigWindow()
    	{
        	InitializeComponent();
	}

	public void ChangeCellBrush(object sender, RoutedEventArgs e)
	{
		MainWindow.cellBrush = ((Button)sender).Content.ToString();
	}
}
