using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;

namespace randomization.library;
static public class Calc
{
    /// <summary>
    /// Finds the median of a list of doubles.
    /// </summary>
    /// <param name="list">The list of doubles of which the median is desired.</param>
    /// <returns>A double containing the median of a given list of doubles.</returns>
    public static double Median(List<double> list)
    {
        List<double> sortedList = new List<double>(list);
        sortedList.Sort();

        double median = 0; //default is zero

        if (sortedList.Count % 2 != 0)
        {
            //list.count = odd
            median = sortedList[sortedList.Count / 2];
        }
        else
        {
            //list.count = even
            double topIndex = (sortedList.Count / 2) - 1; //minus 1 because we are working with indexes later
            int top = (int)Math.Ceiling(topIndex + 0.5);

            double bottomIndex = sortedList.Count / 2 - 1;
            int bottom = (int)Math.Floor(bottomIndex + 0.5);

            median = (sortedList[top] + sortedList[bottom]) / 2;
        }

        return median;
    }

    /// <summary>
    /// Calculates the factorial of a long.
    /// </summary>
    /// <param name="Number">The long of which the factorial is desired.</param>
    /// <returns>A Long containing the factorial of a given long.</returns>
    public static double Factorial(double Number)
    {
        //although slightly counter-intuitive, the factorial must be calculated using doubles
        //and not, for example, long values. Using long values will return wrong results.
        double result = Number;

        for (double i = Number - 1; i >= 1; i--)
            result *= i;

        return result;
    }

    /// <summary>
    /// Determines the number of decimal places of a given double.
    /// </summary>
    /// <param name="inputValue">A double of which to determine the number of decimal places.</param>
    /// <returns>The number of decimal places of the given double.</returns>
    public static int GetDecimalPlaces(double inputValue)
    {
        int decimalPlaces = 0; //default value
        string[] splittedInputValue = inputValue.ToString().Split(new[] { Settings.NumberDecimalSeparator }, StringSplitOptions.None);

        if (splittedInputValue.Length > 1)
            decimalPlaces = splittedInputValue[1].Length;

        return decimalPlaces;
    }

    public static double CalculatePValue(List<double> data, double expectedMean)
    {
        // Calculate the sample mean
        double sampleMean = Statistics.Mean(data);

        // Calculate the standard deviation
        double standardDeviation = Statistics.StandardDeviation(data);

        // Calculate the t-statistic
        double tStatistic = (sampleMean - expectedMean) / (standardDeviation / Math.Sqrt(data.Count));

        // Calculate the p-value
        double pValue = 2 * (1 - StudentT.CDF(0, 1, data.Count - 1, Math.Abs(tStatistic)));

        return pValue;
    }

}
