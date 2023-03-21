using Enigmaze.Core;

namespace Enigmaze.Test;

public class DepthFirstSearchTest
{
    [Test]
    public void Run_ReturnsCorrectPath()
    {
        // Arrange
        var expectedPath = new List<char>
        {
            'R', 'R', 'D', 'U', 'L', 'L', 'D'
        };
        
        // Act
        var depthFirstSearch = new DepthFirstSearch(FileParser.ParseFile("../../../../../test/maze1.txt"));
        depthFirstSearch.Run(new List<char>(),
            Utils.createNewHasVisited(depthFirstSearch.Map.Rows, depthFirstSearch.Map.Cols));
        
        // Assert
        CollectionAssert.AreEqual(depthFirstSearch.Path, expectedPath);
    }

    [Test]
    public void Run_ReturnsCorrectPath_Complex()
    {
        // Arrange
        var expectedPath = new List<char>
        {
            'R', 'R', 'U', 'U', 'D', 'D',
            'R', 'R', 'U', 'U', 'D', 'D',
            'R', 'R', 'U', 'U', 'D', 'D',
            'R', 'R', 'L', 'L', 'D', 'D',
            'U', 'U', 'L', 'L', 'D', 'D',
            'U', 'U', 'L', 'L', 'D', 'D'
        };
        
        // Act
        var depthFirstSearch = new DepthFirstSearch(FileParser.ParseFile("../../../../../test/maze6.txt"));
        depthFirstSearch.Run(new List<char>(),
            Utils.createNewHasVisited(depthFirstSearch.Map.Rows, depthFirstSearch.Map.Cols));
        
        // Assert
        CollectionAssert.AreEqual(depthFirstSearch.Path, expectedPath);
    }

    [Test]
    public void Run_ReturnsCorrectPath_Complex_GoBackToStart()
    {
        // Arrange
        var expectedPath = new List<char>
        {
            'R', 'R', 'U', 'U', 'D', 'D',
            'R', 'R', 'U', 'U', 'D', 'D',
            'R', 'R', 'U', 'U', 'D', 'D',
            'R', 'R', 'L', 'L', 'D', 'D',
            'U', 'U', 'L', 'L', 'D', 'D',
            'U', 'U', 'L', 'L', 'D', 'D',
            'U', 'U', 'L', 'L'
        };
        
        // Act
        var depthFirstSearch = new DepthFirstSearch(FileParser.ParseFile("../../../../../test/maze6.txt"));
        depthFirstSearch.Run(new List<char>(),
            Utils.createNewHasVisited(depthFirstSearch.Map.Rows, depthFirstSearch.Map.Cols), true);
        
        // Assert
        CollectionAssert.AreEqual(depthFirstSearch.Path, expectedPath);
    }
}