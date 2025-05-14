namespace Sefer.Backend.Stats.Api.Tests.DataStructures;

[TestClass]
public class HistogramTests
{
    [TestMethod]
    public void Constructor_CorrectBinning()
    {
        // Arrange
        var data = new Dictionary<long, long>();
        
        // Act
        var histogram = new Histogram(data, 4, 1, 13);
        
        // Assert
        histogram.Data.Count.Should().Be(4);
        histogram.Data[0].Interval.Should().Be(1);
        histogram.Data[1].Interval.Should().Be(5);
        histogram.Data[2].Interval.Should().Be(9);
        histogram.Data[3].Interval.Should().Be(13);
    }
    
    [TestMethod]
    public void Constructor_CorrectBinningExactFit()
    {
        // Arrange
        var data = new Dictionary<long, long>();
        
        // Act
        var histogram = new Histogram(data, 4, 0, 11);
        
        // Assert
        histogram.Data.Count.Should().Be(3);
        histogram.Data[0].Interval.Should().Be(0);
        histogram.Data[1].Interval.Should().Be(4);
        histogram.Data[2].Interval.Should().Be(8);
    }

    [TestMethod]
    public void Constructor_Populate()
    {
        // Arrange
        var data = new Dictionary<long, long>() {{2, 3}, {13, 11}};

        // Act
        var histogram = new Histogram(data, 4, 1, 13);
        
        // Assert
        histogram.Data.Count.Should().Be(4);
        histogram.Data[0].Quantity.Should().Be(3);
        histogram.Data[3].Quantity.Should().Be(11);
    }
    
    [TestMethod]
    public void Constructor_OutsideRange()
    {
        // Arrange
        var data = new Dictionary<long, long>() {{-1, 3}};

        // Act
        var histogram = new Histogram(data, 4, 1, 3);
        
        // Assert
        histogram.Data.Count.Should().Be(1);
        histogram.Data[0].Quantity.Should().Be(0);
    }
    
    [TestMethod]
    public void Constructor_NoEndValue()
    {
        // Arrange
        var data = new Dictionary<long, long>() {{2, 3}, {13, 11}};

        // Act
        var histogram = new Histogram(data, 4, 1);
        
        // Assert
        histogram.Data.Count.Should().Be(4);
        histogram.Data[0].Quantity.Should().Be(3);
        histogram.Data[3].Quantity.Should().Be(11);
        histogram.Max.Should().Be(11);
        histogram.Sum.Should().Be(14);
    }
}