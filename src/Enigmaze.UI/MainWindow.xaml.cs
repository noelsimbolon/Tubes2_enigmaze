using Enigmaze.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigmaze.UI;

public partial class MainWindow : Window
{
    // Attributes
    private int ALGORITHM = 0;      // 1 = BFS       2 = DFS
    private int TSP = 0;            // OddNumber = On    EvenNumber = Off
    private string mazePath = "";
    private List<char> finalPath;
    private List<(int, int)> finalVisitedNodes;
    private int nodesCount;
    private TimeSpan execTime;      // miliseconds
    private int waitTime;

    // Map Mazemap = (List<List<char>>, (0, 0), 0, 0, 0);
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

    private void Exit_Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    public static DataTable FromDataTableTxt(List<List<char>> matr)
    {
        DataTable result;
        result = FromDataTable(matr);
        return result;
    }

    private static DataTable FromDataTable(List<List<char>> matr)
    {
        DataTable dt = new DataTable();
        AddColumnToTable(matr, ref dt);
        AddRowToTable(matr, ref dt);
        return dt;
    }

    private static void AddColumnToTable(List<List<char>> matr, ref DataTable dt)
    {
        List<char> kolom = matr[0];                 // collect data per column dipisah spasi
        for (int i = 0; i < kolom.Count; i++)       // setiap kolom diiterasi
        {
            DataColumn dc = new DataColumn("", typeof(char)); // ini yang "" itu judul tiap kolom 
            dt.Columns.Add(dc);
        }
    }

    private static void AddRowToTable(List<List<char>> matr, ref DataTable dt)
    {
        for (int i = 0; i < matr.Count; i++)
        {
            List<char> values = matr[i];
            DataRow dr = dt.NewRow();
            for (int j = 0; j < values.Count; j++)
            {
                dr[j] = values[j];
            }
            dt.Rows.Add(dr);
        }
    }

    private void File_Dialog_Button_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Title = "INSERT MAZE";
        fileDialog.Filter = "Textfiles|*.txt|All Files|*.*";
        fileDialog.DefaultExt = ".txt";  
        Nullable<bool> dialogOk = fileDialog.ShowDialog();

        if (dialogOk == true)
        {
            PathFileName.Text = System.IO.Path.GetFileName(fileDialog.FileName);
            mazePath = fileDialog.FileName;
            Map Mazemap = new Map();
            Mazemap = FileParser.ParseFile(mazePath);
            string[] temp = new string[0];
            DataTable dt = new DataTable();
            for (int i = 0; i < Mazemap.Cols; i++)
            {
                string columnName = "Column" + i;
                dt.Columns.Add(columnName, typeof(char));
            }
            for (int i = 0; i < Mazemap.Rows; i++)
            {
                temp = new string[0];
                for (int j = 0; j < Mazemap.Cols; j++)
                {
                    temp = temp.Append( "" + (Mazemap.Matrix[i][j])).ToArray();
                }
                dt.Rows.Add(temp);
            }
            DataView dv = new DataView(dt);
            dtGridView.ItemsSource = dv;

            int height = (int)dtGridView.Height / Mazemap.Rows;
            foreach (DataRow row in dt.Rows)
            {
                dtGridView.RowHeight = height;
            }
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
        if (TSP != 0)
        {
            TSP = 0; return;
        }
        TSP = 1; return;
    }

    private void Algo_DFS()
    {
        Stopwatch time = new Stopwatch();
        var dfs = new DepthFirstSearch(FileParser.ParseFile(mazePath));
        time.Start();
        dfs.Run(new List<char>(),
            Utils.createNewHasVisited(dfs.Map.Rows, dfs.Map.Cols));
        time.Stop();
        finalPath = dfs.Path;
        finalVisitedNodes = dfs.VisitedNodes;
        nodesCount = dfs.VisitedNodes.Count;
        execTime = time.Elapsed;

        // Output
        textBoxRoute.Text = string.Join("-", finalPath);
        textBoxNodes.Text = nodesCount.ToString();
        textBoxSteps.Text = finalVisitedNodes.Count.ToString();
        textBoxExecTime.Text = execTime.TotalMilliseconds.ToString() + " ms";

    }

    private void Algo_BFS()
    {
        Stopwatch time = new Stopwatch();
        var bfs = new BreadthFirstSearch(FileParser.ParseFile(mazePath));
        time.Start();
        bfs.Run(Utils.createNewHasVisited(bfs.Map.Rows, bfs.Map.Cols));
        time.Stop();
        finalPath = bfs.Path;
        finalVisitedNodes = bfs.VisitedNodes;
        nodesCount = bfs.VisitedNodes.Count;
        execTime = time.Elapsed;

        // Output
        textBoxRoute.Text = string.Join("-", finalPath);
        textBoxNodes.Text = nodesCount.ToString();
        textBoxSteps.Text = finalPath.Count.ToString();
        textBoxExecTime.Text = execTime.TotalMilliseconds.ToString() + " ms";
    }

    private void Algo_TSP()
    {
        if (ALGORITHM == 1)
        {
            Stopwatch time = new Stopwatch();
            var bfs = new DepthFirstSearch(FileParser.ParseFile(mazePath));
            time.Start();
            bfs.Run(new List<char>(),
                Utils.createNewHasVisited(bfs.Map.Rows, bfs.Map.Cols),true);
            time.Stop();
            finalPath = bfs.Path;
            finalVisitedNodes = bfs.VisitedNodes;
            nodesCount = bfs.VisitedNodes.Count;
            execTime = time.Elapsed;

            // Output
            textBoxRoute.Text = string.Join("-", finalPath);
            textBoxNodes.Text = nodesCount.ToString();
            textBoxSteps.Text = finalPath.Count.ToString();
            textBoxExecTime.Text = execTime.TotalMilliseconds.ToString() + " ms";
        }
        else if (ALGORITHM == 2)
        {
            Stopwatch time = new Stopwatch();
            var dfs = new DepthFirstSearch(FileParser.ParseFile(mazePath));
            time.Start();
            dfs.Run(new List<char>(),
                Utils.createNewHasVisited(dfs.Map.Rows, dfs.Map.Cols),true);
            finalPath = dfs.Path;
            finalVisitedNodes = dfs.VisitedNodes;
            nodesCount = dfs.VisitedNodes.Count;
            time.Stop();
            execTime = time.Elapsed;

            // Output
            textBoxRoute.Text = string.Join("-", finalPath);
            textBoxNodes.Text = nodesCount.ToString();
            textBoxSteps.Text = finalVisitedNodes.Count.ToString();
            textBoxExecTime.Text = execTime.TotalMilliseconds.ToString() + " ms";
        }
    }

    public void changeColorMaze(int i, int j, bool isVisited, int sleepTime)
    {
        if (isVisited)
        {
            if (sleepTime == 0) { }
        }
    }


    private void Start_Button_Click(object sender, RoutedEventArgs e)
    {
        if (TSP == 0)
        {
            if (ALGORITHM == 1)
            {
                Algo_BFS();
            }
            else if (ALGORITHM == 2)
            {
                Algo_DFS();
            }
        } else
        {
            Algo_TSP();
        }

        // Visualize
        //private List<(int, int)> finalVisitedNodes;
        if (ALGORITHM == 1)
        {

        }


}

    private void slValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        waitTime = (int)slValue.Value;
    }
}