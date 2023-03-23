using System.Collections.Generic;

namespace Enigmaze.Core;

public class BreadthFirstSearch
{
    public Map Map { get; }
    public List<char> Path { get; } = new ();
    public List<(int, int)> VisitedNodes { get; } = new ();
    public List<List<(int, int)>> Predecessors { get; } = new ();

    public BreadthFirstSearch(Map map)
    {
        Map = new Map(map.Matrix, map.StartingPoint, map.TreasureCount, map.Rows, map.Cols);
        for (int i = 0; i < Map.Rows; i++)
        {
            Predecessors.Add(new List<(int, int)>());
            for (int j = 0; j < Map.Cols; j++)
            {
                Predecessors[i].Add((-1, -1));
            }
        }
    }

    private void SetPath((int, int) startingPoint)
    {
        var temp = new List<char>();
        int row = startingPoint.Item1;
        int col = startingPoint.Item2;
        
        while (Predecessors[row][col] != (-1, -1))
        {
            if (row - Predecessors[row][col].Item1 == 1){
                temp.Add('D');
            }
            else if (row - Predecessors[row][col].Item1 == -1){
                temp.Add('U');
            }
            else if (col - Predecessors[row][col].Item2 == 1){
                temp.Add('R');
            }
            else if (col - Predecessors[row][col].Item2 == -1){
                temp.Add('L');
            }

            (row, col) = Predecessors[row][col];
        }
        
        while (temp.Count != 0)
        {
            Path.Add(temp[^1]);
            temp.RemoveAt(temp.Count - 1);
        }
    }

    public void Run(List<List<bool>> hasVisited, bool goBackToStart = false)
    {
        // There's no treasure left to find
        if (Map.TreasureCount == 0)
        {
            return;
        }

        // Queue for BFS
        var queue = new Queue<(int, int)>();
        queue.Enqueue(Map.StartingPoint);
        Predecessors[Map.StartingPoint.Item1][Map.StartingPoint.Item2] = (-1, -1);

        while (queue.Count != 0)
        {
            (int, int) current = queue.Dequeue();
            int row = current.Item1;
            int col = current.Item2;
            VisitedNodes.Add((row, col));
            hasVisited[row][col] = true;

            if (Map.Matrix[row][col] == 'T')
            {
                Map.TreasureCount--;
                Map.Matrix[row][col] = 'R';
                SetPath((row, col));
                Map.StartingPoint = (row, col);
                
                // Reset visited nodes to false since we can revisit nodes
                for (int i = 0; i < hasVisited.Count; i++)
                {
                    for (int j = 0; j < hasVisited[0].Count; j++)
                    {
                        hasVisited[i][j] = false;
                    }
                }

                break;
            }

            if (row - 1 >=0)
            {
                if (!hasVisited[row - 1][col] && Map.Matrix[row - 1][col] != 'X')
                {
                    queue.Enqueue((row - 1, col));
                    Predecessors[row - 1][col] = (row, col);
                }
            }

            if (col + 1 < Map.Cols)
            {
                if (!hasVisited[row][col + 1] && Map.Matrix[row][col + 1] != 'X')
                {
                    queue.Enqueue((row, col + 1));
                    Predecessors[row][col + 1] = (row, col);
                }
            }

            if (row + 1 < Map.Rows)
            {
                if (!hasVisited[row + 1][col] && Map.Matrix[row + 1][col] != 'X')
                {
                    queue.Enqueue((row + 1, col));
                    Predecessors[row + 1][col] = (row, col);
                }
            }

            if (col - 1 >= 0)
            {
                if (!hasVisited[row][col - 1] && Map.Matrix[row][col - 1] != 'X')
                {
                    queue.Enqueue((row, col - 1));
                    Predecessors[row][col - 1] = (row, col);
                }
            }
        }

        if (Map.TreasureCount == 0)
        {
            if (goBackToStart)
            {
                (int, int) krustyKrab = Map.FindKrustyKrab();
                Map.Matrix[krustyKrab.Item1][krustyKrab.Item2] = 'T';
                Map.TreasureCount++;
                
                for (int i = 0; i < Map.Rows; i++)
                {
                    for (int j = 0; j < Map.Cols; j++)
                    {
                        hasVisited[i][j] = false;
                    }
                }

                Run(hasVisited);
            }
        }
        
        if (Map.TreasureCount > 0)
        {
            Run(hasVisited, goBackToStart);
        }
    }
}