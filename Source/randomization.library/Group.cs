namespace randomization.library;
public class Group
{
    public Group(char groupID, string groupName)
    {
        ID = groupID;
        Name = groupName;
    }

    public bool Equals(Group group)
    {
        // If Variable is null, return false.
        if (Object.ReferenceEquals(group, null))
            return false;

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != group.GetType())
            return false;

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (ID == group.ID);
    }

    public static bool operator ==(Group leftHandSide, Group rightHandSide)
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

    public static bool operator !=(Group leftHandSide, Group rightHandSide)
    {
        return !(leftHandSide == rightHandSide);
    }

    /// <summary>
    /// Clones an instance of Group.
    /// </summary>
    /// <returns>A clone of an instance of Group.</returns>
    public Group Clone()
    {
        // TODO: EDITED
        // return new Group((char)Global.GroupID, (string)Name.Clone());
        return new Group(ID, (string)Name.Clone());
    }

    /// <summary>
    /// Clones an instance of Group, but preserves the ID.
    /// </summary>
    /// <returns>A clone of an instance of Group.</returns>
    /// <example>Preserving the ID is for example needed when
    /// randomizing blocks to groups.</example>
    public Group CloneButPreserveID()
    {
        return new Group(ID, (string)Name.Clone());
    }

    public char ID { get; private set; } = new char();

    public string Name { get; set; } = string.Empty;

    public bool IsValid { get { return this.Name != string.Empty; } }
}
