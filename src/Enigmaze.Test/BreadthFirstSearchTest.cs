using Enigmaze.Core;

namespace Enigmaze.Test;

public class BreadthFirstSearchTest{
    [Test]
    public void Run_ReturnsCorrectPath(){
        // Arrange
        var expectedPath = new List<char>
        {
            'D', 'U', 'R', 'R', 'D'
        };
        
        // Act
        var breadthFirstSearch = new BreadthFirstSearch(FileParser.ParseFile("../../../../../test/maze1.txt"));
        breadthFirstSearch.Run(Utils.createNewHasVisited(breadthFirstSearch.Map.Rows, breadthFirstSearch.Map.Cols));
        
        // Assert
        CollectionAssert.AreEqual(breadthFirstSearch.Path, expectedPath);
    }

    [Test]
    public void Run_ReturnsCorrectPath_Complex(){
        // Arrange
        var expectedPath = new List<char>
        {
            'R', 'R', 'U', 'U', 'D', 'D', 'D', 'D', 'U', 'U',
            'R', 'R', 'U', 'U', 'D', 'D', 'D', 'D', 'U', 'U',
            'R', 'R', 'U', 'U', 'D', 'D', 'R', 'R', 'L', 'L',
            'D', 'D'
        };
        
        // Act
        var breadthFirstSearch = new BreadthFirstSearch(FileParser.ParseFile("../../../../../test/maze6.txt"));
        breadthFirstSearch.Run(Utils.createNewHasVisited(breadthFirstSearch.Map.Rows, breadthFirstSearch.Map.Cols));
        
        // Assert
        CollectionAssert.AreEqual(breadthFirstSearch.Path, expectedPath);
    }

    [Test]
    public void Run_ReturnsCorrectPath_Complex_GoBackToStart(){
        // Arrange
        var expectedPath = new List<char>
        {
            'R', 'R', 'U', 'U', 'D', 'D', 'D', 'D', 'U', 'U',
            'R', 'R', 'U', 'U', 'D', 'D', 'D', 'D', 'U', 'U',
            'R', 'R', 'U', 'U', 'D', 'D', 'R', 'R', 'L', 'L',
            'D', 'D', 'U', 'U', 'L', 'L', 'L', 'L', 'L', 'L'
        };
        
        // Act
        var breadthFirstSearch = new BreadthFirstSearch(FileParser.ParseFile("../../../../../test/maze6.txt"));
        breadthFirstSearch.Run(Utils.createNewHasVisited(breadthFirstSearch.Map.Rows, breadthFirstSearch.Map.Cols), true);
        
        // Assert
        CollectionAssert.AreEqual(breadthFirstSearch.Path, expectedPath);
    }
}