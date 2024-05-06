using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("[controller]")]
public class NumberToWordsController : ControllerBase
{
    [HttpGet("{input}")]
    public ActionResult<string> ConvertToWords(string input)
    {
        // Validate the numerical input
        if (!double.TryParse(input, out double number))
        {
            return BadRequest("The input is invalid, please enter a valid number.");
        }

        // Check for negative numbers
        if (number < 0)
        {
            return BadRequest("Input must not be negative.");
        }

        // Checks if there are more than two decimal places
        double roundedNumber = Math.Round(number, 2);
        if ((number * 100) % 1 != 0) 
        {
            return BadRequest("Input has more than two decimal places. Please limit it to two.");
        }

        // Convert the number to words
        string words = NumberToWordsConverter.ConvertNumberToWords(roundedNumber);
        return Ok(words);
    }
}

public static class NumberToWordsConverter
{
    private static readonly string[] Units = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
    private static readonly string[] Teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
    private static readonly string[] Tens = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
    private static readonly string[] Thousands = { "", "Thousand", "Million", "Billion", "Trillion" };

    public static string ConvertNumberToWords(double number)
    {
        string[] parts = number.ToString("F2").Split('.');
        string dollars = parts[0];
        string cents = parts.Length > 1 ? parts[1] : "00";

        string dollarWords = ConvertWholeNumber(dollars);
        if (cents == "00")
        {
            return dollarWords;
        }
        else
        {
            string centWords = ConvertCents(cents);
            return $"{dollarWords} and {centWords}".Trim();
        }
    }

    private static string ConvertWholeNumber(string number)
    {
        string word = "";
        bool isZero = true;

        for (int i = 0; i < number.Length; i++)
            if (number[i] != '0')
                isZero = false;

        if (isZero)
            return "Zero Dollars";

        int numDigits = number.Length;
        int pos = 0;
        int place = (numDigits - 1) / 3;

        while (numDigits > 0)
        {
            int n = numDigits % 3 == 0 ? 3 : numDigits % 3;
            string numSection = number.Substring(pos, n);
            int numericValue = int.Parse(numSection);

            if (numericValue != 0)
            {
                string sectionWords = ConvertThreeOrLessDigits(numericValue);
                word += sectionWords + " " + Thousands[place] + " ";
            }

            numDigits -= n;
            pos += n;
            place--;
        }

        return word.Trim() + " Dollars";
    }

    private static string ConvertThreeOrLessDigits(int number)
    {
        string word = "";
        int num = number % 100;

        if (number > 99)
        {
            word += Units[number / 100] + " Hundred ";
            number %= 100;
        }

        if (number > 19)
        {
            word += Tens[number / 10] + " ";
            number %= 10;
        }

        if (number > 0)
            word += number < 10 ? Units[number] : Teens[number - 10];

        return word.Trim();
    }

    private static string ConvertCents(string cents)
    {
        string centWords = ConvertThreeOrLessDigits(int.Parse(cents));
        return centWords + " Cents";
    }
}
