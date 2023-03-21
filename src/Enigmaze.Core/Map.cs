using System.Collections.Generic;

namespace Enigmaze.Core;

public class Map
{
    // Auto properties
    public List<List<char>> Matrix { get; set; }
    public (int, int) StartingPoint { get; set; }
    public int TreasureCount { get; set; }
    public int Rows { get; set; }
    public int Cols { get; set; }

    // Constructor
    public Map(List<List<char>> matrix, (int x, int y) startingPoint, int treasureCount, int rows, int cols)
    {
        Matrix = matrix;
        StartingPoint = startingPoint;
        TreasureCount = treasureCount;
        Rows = rows;
        Cols = cols;
    }

    // Method to find the index of matrix that contains the character 'K'
    public (int, int) FindKrustyKrab()
    {
        // Initialize starting point
        int startingRow = 0;
        int startingCol = 0;
        
        // Determine the starting point
        bool shouldBreak = false;
        for (int i = 0; i < Matrix.Count && !shouldBreak; i++)
        {
            for (int j = 0; j < Matrix[0].Count && !shouldBreak; j++)
            {
                if (Matrix[i][j] == 'K')
                {
                    startingRow = i;
                    startingCol = j;
                    shouldBreak = true;
                }
            }
        }

        return (startingRow, startingCol);
    }
}