using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Enigmaze.Core;

namespace Enigmaze.UI.Test;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Map Map { get; set; }
    private List<char> Path { get; set; }
    private TimeSpan ExecutionTime { get; set; }
    private List<(int, int)> VisitedNodes { get; set; }
    private List<List<int>> NumVisits { get; set; } // represents the number of visits a cell has
    private string FileName { get; set; }
    private bool IsGoBackChecked { get; set; } = false;
    private bool IsVisualizationRunning { get; set; } = false;
    private int FileOpened { get; set; } = 0;
    private int? AlgorithmUsed { get; set; } = null; // 1 if it's BFS, 2 if it's DFS
    private int VisitedNodeCount { get; set; }
    private int StepInterval { get; set; } = 100;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void File_Dialog(object sender, RoutedEventArgs e)
    {
        if (IsVisualizationRunning)
        {
            FileTextBlock.Text = "Wait for the visualization.";
            FileTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            return;
        }

        var fileDialog = new OpenFileDialog();
        fileDialog.Title = "Open Maze";
        fileDialog.DefaultExt = ".txt";
        fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

        bool? result = fileDialog.ShowDialog();

        if (result == true)
        {
            FileName = fileDialog.FileName;

            try
            {
                Map = FileParser.ParseFile(FileName);
                FileOpened = 1;

                // Valid file
                FileTextBlock.Text = "File opened successfully.";
                FileTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00CC00"));

                // Empty output texts
                RouteTextBlock.Text = "";
                VisitedNodesCountTextBlock.Text = "";
                RouteLengthTextBlock.Text = "";
                ExecutionTimeTextBlock.Text = "";

                InitializeGrid(sender, e);
            }
            catch (InvalidDataException)
            {
                // Invalid file
                FileTextBlock.Text = "Invalid text file selected.";
                FileTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            }
        }
    }

    private void InitializeGrid(object sender, RoutedEventArgs e)
    {
        string[] temp = new string[0];
        DataTable dataTable = new DataTable();

        for (int i = 0; i < Map.Cols; i++)
        {
            string columnName = "Column" + i;
            dataTable.Columns.Add(columnName, typeof(char));
        }

        for (int i = 0; i < Map.Rows; i++)
        {
            temp = new string[0];

            for (int j = 0; j < Map.Cols; j++)
            {
                temp = temp.Append("" + Map.Matrix[i][j]).ToArray();
            }

            dataTable.Rows.Add(temp);
        }

        DataView dv = new DataView(dataTable);
        MapGrid.ItemsSource = dv;

        int height = (int)MapGrid.Height / Map.Rows;
        foreach (DataRow row in dataTable.Rows)
        {
            MapGrid.RowHeight = height;
        }
    }

    private void BFSRadioButton_CLick(object sender, RoutedEventArgs e)
    {
        AlgorithmUsed = 1;
    }

    private void DFSRadioButton_CLick(object sender, RoutedEventArgs e)
    {
        AlgorithmUsed = 2;
    }

    private void GoBack_Checked(object sender, RoutedEventArgs e)
    {
        IsGoBackChecked = true;
    }

    private void GoBack_Unchecked(object sender, RoutedEventArgs e)
    {
        IsGoBackChecked = false;
    }

    private void Start(object sender, RoutedEventArgs e)
    {
        if (FileOpened == 0)
        {
            StartTextBlock.Text = "Open a maze first.";
            StartTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            return;
        }

        if (FileOpened == 2)
        {
            StartTextBlock.Text = "Open a new maze.";
            StartTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            return;
        }

        if (AlgorithmUsed == null)
        {
            StartTextBlock.Text = "Select an algorithm first.";
            StartTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            return;
        }

        StepInterval = (int)StepIntervalSlider.Value;

        // Initialize NumVisits
        NumVisits = new List<List<int>>();

        for (int i = 0; i < Map.Rows; i++)
        {
            var numVisitsRow = new List<int>();

            for (int j = 0; j < Map.Cols; j++)
            {
                numVisitsRow.Add(0);
            }

            NumVisits.Add(numVisitsRow);
        }

        if (AlgorithmUsed == 1)
        {
            StartTextBlock.Text = "";

            // BFS
            Stopwatch stopwatch = new Stopwatch();
            var bfs = new BreadthFirstSearch(FileParser.ParseFile(FileName));

            if (IsGoBackChecked)
            {
                stopwatch.Start();
                bfs.Run(Utils.createNewHasVisited(bfs.Map.Rows, bfs.Map.Cols), true);
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
                bfs.Run(Utils.createNewHasVisited(bfs.Map.Rows, bfs.Map.Cols));
                stopwatch.Stop();
            }

            // Set fields
            Path = bfs.Path;
            VisitedNodes = bfs.VisitedNodes;
            VisitedNodeCount = bfs.VisitedNodes.Count;
            ExecutionTime = stopwatch.Elapsed;

            VisualizePathFinding(sender, e);

            // Output
            RouteTextBlock.Text = string.Join("-", Path);
            VisitedNodesCountTextBlock.Text = VisitedNodeCount.ToString();
            RouteLengthTextBlock.Text = Path.Count.ToString();
            ExecutionTimeTextBlock.Text = ExecutionTime.TotalMilliseconds + " ms";

            FileOpened = 2;
        }
        else // AlgorithmUsed = 2
        {
            StartTextBlock.Text = "";

            // DFS
            Stopwatch stopwatch = new Stopwatch();
            var dfs = new DepthFirstSearch(FileParser.ParseFile(FileName));

            if (IsGoBackChecked)
            {
                stopwatch.Start();
                dfs.Run(new List<char>(), Utils.createNewHasVisited(dfs.Map.Rows, dfs.Map.Cols), true);
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
                dfs.Run(new List<char>(), Utils.createNewHasVisited(dfs.Map.Rows, dfs.Map.Cols));
                stopwatch.Stop();
            }

            // Set fields
            Path = dfs.Path;
            VisitedNodes = dfs.VisitedNodes;
            VisitedNodeCount = dfs.VisitedNodes.Count;
            ExecutionTime = stopwatch.Elapsed;

            VisualizePathFinding(sender, e);

            // Output
            RouteTextBlock.Text = string.Join("-", Path);
            VisitedNodesCountTextBlock.Text = VisitedNodeCount.ToString();
            RouteLengthTextBlock.Text = Path.Count.ToString();
            ExecutionTimeTextBlock.Text = ExecutionTime.TotalMilliseconds + " ms";

            FileOpened = 2;
        }
    }

    private async void VisualizePathFinding(object sender, RoutedEventArgs e)
    {
        IsVisualizationRunning = true;
        
        foreach (var tupleOfIndex in VisitedNodes)
        {
            DataGridRow row = (DataGridRow)MapGrid.ItemContainerGenerator.ContainerFromIndex(tupleOfIndex.Item1);
            DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(tupleOfIndex.Item2);

            int numVisits = NumVisits[tupleOfIndex.Item1][tupleOfIndex.Item2];

            double brightness = 1.0;

            if (numVisits > 0)
            {
                brightness -= numVisits * 0.1;
            }

            Color
                darkerColor =
                    ColorFromHsb(60, 1,
                        brightness); // create a new color with the same hue and saturation, but a darker brightness value

            cell.Foreground = new SolidColorBrush(darkerColor);
            cell.Background = new SolidColorBrush(darkerColor);

            NumVisits[tupleOfIndex.Item1][tupleOfIndex.Item2]++; // increments the number of visits the current cell has

            await Task.Delay(StepInterval);
        }

        IsVisualizationRunning = false;
    }

    private T GetVisualChild<T>(DependencyObject parent) where T : Visual
    {
        T child = default(T);
        int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < numVisuals; i++)
        {
            Visual visual = (Visual)VisualTreeHelper.GetChild(parent, i);
            child = visual as T;

            if (child == null)
            {
                child = GetVisualChild<T>(visual);

                if (child != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return child;
    }

    private Color ColorFromHsb(double hue, double saturation, double brightness)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);
        double p = brightness * (1 - saturation);
        double q = brightness * (1 - f * saturation);
        double t = brightness * (1 - (1 - f) * saturation);
        byte r, g, b;

        switch (hi)
        {
            case 0:
                r = (byte)(brightness * 255);
                g = (byte)(t * 255);
                b = (byte)(p * 255);
                break;
            case 1:
                r = (byte)(q * 255);
                g = (byte)(brightness * 255);
                b = (byte)(p * 255);
                break;
            case 2:
                r = (byte)(p * 255);
                g = (byte)(brightness * 255);
                b = (byte)(t * 255);
                break;
            case 3:
                r = (byte)(p * 255);
                g = (byte)(q * 255);
                b = (byte)(brightness * 255);
                break;
            case 4:
                r = (byte)(t * 255);
                g = (byte)(p * 255);
                b = (byte)(brightness * 255);
                break;
            default:
                r = (byte)(brightness * 255);
                g = (byte)(p * 255);
                b = (byte)(q * 255);
                break;
        }

        return Color.FromRgb(r, g, b);
    }
}