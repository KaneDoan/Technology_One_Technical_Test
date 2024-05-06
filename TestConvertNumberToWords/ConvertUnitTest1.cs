using Xunit;
using Microsoft.AspNetCore.Mvc;
 // Adjust this using statement to match your actual namespace

public class NumberToWordsControllerTests
{
    private readonly NumberToWordsController _controller;

    public NumberToWordsControllerTests()
    {
        // Setup the controller
        _controller = new NumberToWordsController();
    }

    [Theory]
    [InlineData("hello")]
    [InlineData("")]
    public void ConvertToWords_ReturnsBadRequest_ForInvalidInput(string input)
    {
        // Act
        var result = _controller.ConvertToWords(input);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.Equal($"The input is invalid, please enter a valid number.", badRequestResult.Value);
    }


    [Theory]
    [InlineData("-1")]
    [InlineData("-12.1")]
    [InlineData("-123.11")]
    public void ConvertToWords_ReturnsBadRequest_ForNegativeNumbers(string input)
    {
        // Act
        var result = _controller.ConvertToWords(input);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.Equal("Input must not be negative.", badRequestResult.Value);
    }

    [Theory]
    [InlineData("10.123")]
    [InlineData("0.123")]
    public void ConvertToWords_ReturnsBadRequest_ForTooManyDecimalPlaces(string input)
    {
        // Act
        var result = _controller.ConvertToWords(input);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.Equal("Input has more than two decimal places. Please limit it to two.",badRequestResult.Value);
    }

    [Theory]
    [InlineData("5", "Five Dollars")]
    [InlineData("12.1", "Twelve Dollars and Ten Cents")]
    [InlineData("42.41", "Forty Two Dollars and Forty One Cents")]
    [InlineData("100.50", "One Hundred Dollars and Fifty Cents")]
    [InlineData("0.1", "Zero Dollars and Ten Cents")]
    [InlineData("0.06", "Zero Dollars and Six Cents")]
    [InlineData("10000.25", "Ten Thousand Dollars and Twenty Five Cents")]
    [InlineData("2134567.41", "Two Million One Hundred Thirty Four Thousand Five Hundred Sixty Seven Dollars and Forty One Cents")]
    [InlineData("1232134567.2", "One Billion Two Hundred Thirty Two Million One Hundred Thirty Four Thousand Five Hundred Sixty Seven Dollars and Twenty Cents")]
    [InlineData("5123002134567.41", "Five Trillion One Hundred Twenty Three Billion Two Million One Hundred Thirty Four Thousand Five Hundred Sixty Seven Dollars and Forty One Cents")]
    public void ConvertToWords_ReturnsOk_ForValidInput(string input, string expected)
    {
        // Act
        var result = _controller.ConvertToWords(input);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.Equal(expected, okResult.Value);
    }
}