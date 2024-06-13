namespace randomization.library;
public class ExperimentalUnit
{
    public ExperimentalUnit(char numberOfExperimentalUnit, string nameOfExperimentalUnit)
    {
        ID = numberOfExperimentalUnit;
        Name = nameOfExperimentalUnit;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as ExperimentalUnit);
    }

    public bool Equals(ExperimentalUnit experimentalUnit)
    {
        // If Variable is null, return false.
        if (Object.ReferenceEquals(experimentalUnit, null))
            return false;

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != experimentalUnit.GetType())
            return false;

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (Name == experimentalUnit.Name) && (Values.SequenceEqual(experimentalUnit.Values)) && (Marker.Name == experimentalUnit.Marker.Name);
    }

    public static bool operator ==(ExperimentalUnit leftHandSide, ExperimentalUnit rightHandSide)
    {
        // Check for null on left side.
        if (Object.ReferenceEquals(leftHandSide, null))
        {
            if (Object.ReferenceEquals(rightHandSide, null))
                return true; // null == null = true.

            // Only the left side is null.
            return false;
        }

        // Equals handles case of null on right side.
        return leftHandSide.Equals(rightHandSide);
    }

    public static bool operator !=(ExperimentalUnit leftHandSide, ExperimentalUnit rightHandSide)
    {
        return !(leftHandSide == rightHandSide);
    }

    /// <summary>
    /// Clones an instance of ExperimentalUnit.
    /// </summary>
    /// <returns>A clone of an instance of ExperimentalUnit.</returns>
    public ExperimentalUnit Clone()
    {
        ExperimentalUnit cloneOfExperimentalUnit = new ExperimentalUnit(this.ID, this.Name)
        {
            Values = Values.Select(value => value).ToList(),
            Marker = new Marker(Marker.Name),
            CanBePlacedInASubgroup = CanBePlacedInASubgroup
        };

        return cloneOfExperimentalUnit;
    }

    public char ID { get; private set; } = new char();

    public string Name { get; set; } = "unnamed";

    public List<double> Values { get; set; } = new List<double>();

    public Marker Marker { get; set; } = new Marker(string.Empty);

    public bool CanBePlacedInASubgroup { get; set; } = true;

    public string Category { get; set; } = string.Empty;
}
