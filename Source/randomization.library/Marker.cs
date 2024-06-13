namespace randomization.library;
public class Marker

{
    public Marker(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The number of ExperimentalUnits that contain the current marker
    /// and that are not yet divided into subgroups during BlockOfExperimentalUnits.DivideExperimentalUnitsIntoSubgroups().
    /// </summary>
    public int ExperimentalUnitsContainingCurrentMarkerNotYetDividedIntoSubgroups { get; set; } = 1;
}

