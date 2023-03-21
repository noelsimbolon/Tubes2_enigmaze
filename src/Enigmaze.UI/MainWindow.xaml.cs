using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigmaze.UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    // Global Variable
    int ALGORITHM = 0; // 1 = BFS       2 = DFS
    int TSP = 0; // OddNumber = On    EvenNumber = Off
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void Exit_Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void File_Dialog_Button_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Title = "Open Maze";
        fileDialog.Filter = "Textfiles|*.txt|All Files|*.*";
        fileDialog.DefaultExt = ".txt";
        Nullable<bool> dialogOk = fileDialog.ShowDialog();

        if (dialogOk == true)
        {
            // PROSES

        }
    }

    private void RadioButton1_Checked(object sender, RoutedEventArgs e)
    {
        ALGORITHM = 1;
    }

    private void RadioButton2_Checked(object sender, RoutedEventArgs e)
    {
        ALGORITHM = 2;
    }

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        TSP += 1;
    }

    private void DisplayRoute(object sender, RoutedEventArgs e)
    {
        textBoxRoute.Text = "A-B-C-D-E";
    }

    private void DisplayNodes(object sender, RoutedEventArgs e)
    {
        int hasil = 10;
        textBoxNodes.Text = hasil.ToString();
    }

    private void DisplaySteps(object sender, RoutedEventArgs e)
    {
        int hasil = 10;
        textBoxSteps.Text = hasil.ToString();
    }

    private void DisplayExecutionTime(object sender, RoutedEventArgs e)
    {
        int hasil = 10;
        textBoxExecTime.Text = hasil.ToString();
    }
}