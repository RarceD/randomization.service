namespace randomization.library;
public class Descriptives
{
    private int decimals = 2;
    public Descriptives(int numberOfDecimalsForRounding)
    {
        Decimals = numberOfDecimalsForRounding;
    }

    public void Clear()
    {
        Mean = Settings.MissingValue;
        SD = Settings.MissingValue;
        Min = Settings.MissingValue;
        Median = Settings.MissingValue;
        Max = Settings.MissingValue;
        CV = Settings.MissingValue;
    }

    public double Mean { get; set; } = Settings.MissingValue;
    public double RoundedMean { get => Math.Round(Mean, Decimals, MidpointRounding.AwayFromZero); }
    public double SD { get; set; } = Settings.MissingValue;
    public double RoundedSD { get => Math.Round(SD, Decimals, MidpointRounding.AwayFromZero); }
    public double Min { get; set; } = Settings.MissingValue;
    public double RoundedMin { get => Math.Round(Min, Decimals, MidpointRounding.AwayFromZero); }
    public double Median { get; set; } = Settings.MissingValue;
    public double RoundedMedian { get => Math.Round(Median, Decimals, MidpointRounding.AwayFromZero); }
    public double Max { get; set; } = Settings.MissingValue;
    public double RoundedMax { get => Math.Round(Max, Decimals, MidpointRounding.AwayFromZero); }
    public double CV { get; set; } = Settings.MissingValue;
    public int Decimals { get => decimals; private set => decimals = value; }

    public double? this[string attributeName]
    {
        //this allows for calling a value by its name
        get { return this.GetType().GetProperty(attributeName).GetValue(this, null) as double?; }
        set { this.GetType().GetProperty(attributeName).SetValue(this, value, null); }
    }
}
