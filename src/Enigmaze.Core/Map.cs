using System.Collections.Generic;

namespace Enigmaze.Core;

public class Map
{
    private List<List<char>> _matrix;
    private (int, int) _startingPoint;
    private int _treasureCount;
    private int _rows;
    private int _cols;

    // Constructor
    public Map(List<List<char>> matrix, (int x, int y) startingPoint, int treasureCount, int rows, int cols)
    {
        _matrix = matrix;
        _startingPoint = startingPoint;
        _treasureCount = treasureCount;
        _rows = rows;
        _cols = cols;
    }

    // Auto properties
    public List<List<char>> Matrix { get; set; }
    
    public (int, int) StartingPoint { get; set; }
    
    public int TreasureCount { get; set; }
    
    public int Rows { get; set; }
    
    public int Cols { get; set; }
}