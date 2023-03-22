using System.Collections.Generic;

namespace Enigmaze.Core;

public class DepthFirstSearch
{
    // Auto properties
    public Map Map { get; private set; }
    public List<char> Path { get; private set; } = new ();  // Empty list as default value
    public List<(int, int)> VisitedNodes { get; private set; } = new (); // Default value of 0

    // Constructor
    public DepthFirstSearch(Map map)
    {
        Map = new Map(map.Matrix, map.StartingPoint, map.TreasureCount, map.Rows, map.Cols);
    }
    
    // Run the depth-first search algorithm for path-finding
    public void Run(List<char> tempPath, List<List<bool>> hasVisited, bool goBackToStart = false)
    {
        if (Path.Count > 0)
        {
            return;
        }

        // Assign variables
        int row = Map.StartingPoint.Item1;  // Current row
        int col = Map.StartingPoint.Item2;  // Current column
        
        // Add current index to visited nodes
        VisitedNodes.Add((row, col));
        
        // If a treasure is found
        if (Map.Matrix[row][col] == 'T')
        {
            Map.TreasureCount--;
            Map.Matrix[row][col] = 'R';
            
            // Reset visited nodes to false since we can revisit nodes
            for (int i = 0; i < hasVisited.Count; i++)
            {
                for (int j = 0; j < hasVisited[0].Count; j++)
                {
                    hasVisited[i][j] = false;
                }
            }
        }
        
        // If all treasure has been found
        if (Map.TreasureCount == 0)
        {
            if (goBackToStart)
            {
                (int, int) krustyKrab = Map.FindKrustyKrab();
                Map.Matrix[krustyKrab.Item1][krustyKrab.Item2] = 'T';
                Map.TreasureCount = 1;
                Run(tempPath, hasVisited);
            }
            else
            {
                Path = new List<char>(tempPath);
            }
            
            return;
        }
        
        hasVisited[row][col] = true;

        // DFS call stack
        if (Path.Count == 0)
        {
            if (row - 1 >= 0)
            {
                if (Map.Matrix[row - 1][col] != 'X' && !hasVisited[row - 1][col])
                {
                    tempPath.Add('U');
                    Map.StartingPoint = (row - 1, col);
                    Run(tempPath, hasVisited, goBackToStart);
                    tempPath.RemoveAt(tempPath.Count - 1);
                }
            }
            if (col + 1 < Map.Cols)
            {
                if (Map.Matrix[row][col + 1] != 'X' && !hasVisited[row][col + 1])
                {
                    tempPath.Add('R');
                    Map.StartingPoint = (row, col + 1);
                    Run(tempPath, hasVisited, goBackToStart);
                    tempPath.RemoveAt(tempPath.Count - 1);
                }
            }
            if (row + 1 < Map.Rows)
            {
                if (Map.Matrix[row + 1][col] != 'X' && !hasVisited[row + 1][col])
                {
                    tempPath.Add('D');
                    Map.StartingPoint = (row + 1, col);
                    Run(tempPath, hasVisited, goBackToStart);
                    tempPath.RemoveAt(tempPath.Count - 1);
                }
            }
            if (col - 1 >= 0)
            {
                if (Map.Matrix[row][col - 1] != 'X' && !hasVisited[row][col - 1])
                {
                    tempPath.Add('L');
                    Map.StartingPoint = (row, col - 1);
                    Run(tempPath, hasVisited, goBackToStart);
                    tempPath.RemoveAt(tempPath.Count - 1);
                }
            }
        }
    }

    public void clearAssets(){
        Path = new List<char>();
        VisitedNodes = new List<(int, int)>();
    }
}