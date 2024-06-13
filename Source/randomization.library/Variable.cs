namespace randomization.library;
public class Variable
{
    public Variable(string name, byte decimals, short inputDatagridViewColumnNumber)
    {
        this.Name = name;
        this.DecimalPlaces = Math.Min(decimals, byte.MaxValue);
        this.InputDataGridViewColumnNumber = inputDatagridViewColumnNumber;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as Variable);
    }

    public bool Equals(Variable Variable)
    {
        // If Variable is null, return false.
        if (Object.ReferenceEquals(Variable, null))
            return false;

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != Variable.GetType())
            return false;

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (Name == Variable.Name) && (InputDataGridViewColumnNumber == Variable.InputDataGridViewColumnNumber) && (DecimalPlaces == Variable.DecimalPlaces) && (Weight == Variable.Weight);
    }

    public static bool operator ==(Variable leftHandSide, Variable rightHandSide)
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

    public static bool operator !=(Variable leftHandSide, Variable rightHandSide)
    {
        return !(leftHandSide == rightHandSide);
    }

    /// <summary>
    /// Clones an instance of Variable.
    /// </summary>
    /// <returns>A clone of an instance of Variable.</returns>
    public Variable Clone()
    {
        Variable cloneOfVariable = new Variable((string)Name.Clone(), DecimalPlaces, InputDataGridViewColumnNumber)
        {
            Weight = Weight
        };

        return cloneOfVariable;
    }

    public string Name { get; set; } = "unnamed";

    /// <summary>
    /// The column number of InputDataGridView which contains values of the current Variable.
    /// </summary>
    public short InputDataGridViewColumnNumber { get; set; } = 0;

    public byte DecimalPlaces { get; set; } = 1;

    public double Weight { get; set; } = 1;
}
