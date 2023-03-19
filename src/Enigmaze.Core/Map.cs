using System.Collections.Generic;

namespace Enigmaze.Core;

public class Map
{
    private readonly List<List<char>> _matrix;
    private readonly (int, int) _startingPoint;
    private readonly int _treasureCount;
    private readonly int _rows;
    private readonly int _cols;

    public Map(List<List<char>> matrix, (int, int) startingPoint, int treasureCount, int rows, int cols)
    {
        _matrix = matrix;
        _startingPoint = startingPoint;
        _treasureCount = treasureCount;
        _rows = rows;
        _cols = cols;
    }

    public List<List<char>> GetMatrix()
    {
        return _matrix;
    }

    public (int x, int y) GetStartingPoint()
    {
        return _startingPoint;
    }

    public int GetTreasureCount()
    {
        return _treasureCount;
    }

    public int GetRows()
    {
        return _rows;
    }

    public int GetCols()
    {
        return _cols;
    }
}