using Enigmaze.Core;
using NUnit.Framework;

namespace Enigmaze.Test
{
    public class FileParserTest
    {
        [Test]
        public void ParseFile_ValidFile_ReturnsExpectedOutput()
        {
            // Arrange
            var parser = new FileParser();
            var expectedMatrix = new char[,]
            {
                { 'K', 'R', 'R' },
                { 'T', 'X', 'T' },
                { 'R', 'X', 'R' }
            };

            var expectedTreasureCount = 2;
            var expectedRows = 3;
            var expectedCols = 3;
            var expectedStartingPosition = (0, 0);

            // Act
            var (matrix, treasureCount, rows, cols, (startingRow, startingCol)) =
                parser.ParseFile("../../../../../test/maze1.txt");

            // Assert
            Assert.That(matrix, Is.EqualTo(expectedMatrix));
            Assert.That(treasureCount, Is.EqualTo(expectedTreasureCount));
            Assert.That(rows, Is.EqualTo(expectedRows));
            Assert.That(cols, Is.EqualTo(expectedCols));
            Assert.That((startingRow, startingCol), Is.EqualTo(expectedStartingPosition));
        }
    }
}