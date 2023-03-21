using System.Collections.Generic;

namespace Enigmaze.Core;

public class Utils
{
    public static List<List<bool>> createNewHasVisited(int rows, int cols)
    {
        var visitedNodes = new List<List<bool>>();
        
        for (int i = 0; i < rows; i++)
        {
            var visitedNodesRow = new List<bool>();
            
            for (int j = 0; j < cols; j++)
            {
                visitedNodesRow.Add(false);
            }
            
            visitedNodes.Add(visitedNodesRow);
        }

        return visitedNodes;
    }
}