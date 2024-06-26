﻿namespace randomization.library;
public class Experiment
{
    private bool hasChangedSinceLastSave = true;

    public Experiment(List<ExperimentalUnit> allExperimentalUnits, List<short> blockSizes, List<Variable> activeVariables, List<Variable> allVariables, bool experimentalUnitsHaveMarkers, List<List<short>> subgroupSizesOFEachBlock = null)
    {
        AllExperimentalUnits = allExperimentalUnits;
        BlockSizes = blockSizes;
        ActiveVariables = activeVariables;
        AllVariables = allVariables;
        ExperimentalUnitsHaveMarkers = experimentalUnitsHaveMarkers;

        if (subgroupSizesOFEachBlock != null)
            SubgroupSizesOfEachBlock = subgroupSizesOFEachBlock;
    }

    public Experiment()
    {
        //creates an empty new Experiment();
    }
    /// <summary>
    /// Creates a new Run and sets the OnRequest handlers.
    /// </summary>
    /// <returns>A new instance of Run with OnRequest handlers set.</returns>
    public Run CreateNewRun(bool checkForBlockSetUnicity = true)
    {
        Run run = new Run(checkForBlockSetUnicity);

        run.OnRequestVariables += new Func<List<Variable>>(delegate { return ActiveVariables; });
        run.OnRequestCreateSubgroups += new Func<bool>(delegate { return CreateSubgroups; });
        run.OnRequestExperimentalUnitsHaveMarker += new Func<bool>(delegate { return ExperimentalUnitsHaveMarkers; });
        run.OnRequestSubgroupSizesPerBlock += new Func<List<List<short>>>(delegate { return SubgroupSizesOfEachBlock; });
        run.OnRequestBlockSizes += new Func<List<short>>(delegate { return BlockSizes; });
        run.OnRequestAllExperimentalUnits += new Func<List<ExperimentalUnit>>(delegate { return AllExperimentalUnits; });
        run.OnRequestGroups += new Func<List<Group>>(delegate { return Groups; });

        return run;
    }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as Experiment);
    }

    public bool Equals(Experiment experiment)
    {
        // If Variable is null, return false.
        if (Object.ReferenceEquals(experiment, null))
            return false;

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != experiment.GetType())
            return false;

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.

        //First compare the two-dimensional lists SubgroupSizesOfEachBlock
        bool SubgroupSizesOfEachBlockAreEqual = (SubgroupSizesOfEachBlock.Count == experiment.SubgroupSizesOfEachBlock.Count); //check if counts are equal

        for (int i = 0; SubgroupSizesOfEachBlockAreEqual && i < SubgroupSizesOfEachBlock.Count; i++)
            SubgroupSizesOfEachBlockAreEqual = SubgroupSizesOfEachBlock[i].SequenceEqual(experiment.SubgroupSizesOfEachBlock[i]);

        return (AllExperimentalUnits.SequenceEqual(experiment.AllExperimentalUnits))
            && (BlockSizes.SequenceEqual(experiment.BlockSizes))
            && (ActiveVariables.SequenceEqual(experiment.ActiveVariables))
            && (AllVariables.SequenceEqual(experiment.AllVariables))
            && (Groups.SequenceEqual(experiment.Groups))
            && (CreateSubgroups == experiment.CreateSubgroups)
            && (ExperimentalUnitsHaveMarkers == experiment.ExperimentalUnitsHaveMarkers)
            && (SubgroupSizesOfEachBlockAreEqual)
            && (InputData == experiment.InputData);
    }

    public static bool operator ==(Experiment leftHandSide, Experiment rightHandSide)
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

    public static bool operator !=(Experiment leftHandSide, Experiment rightHandSide)
    {
        return !(leftHandSide == rightHandSide);
    }

    public Experiment Clone()
    {
        //clone relevant data
        List<ExperimentalUnit> CloneOfAllExperimentalUnits = AllExperimentalUnits.Select(ExperimentalUnit => ExperimentalUnit.Clone()).ToList();
        List<short> CloneOfBlockSizes = BlockSizes.Select(blockSize => blockSize).ToList();
        List<Variable> CloneOfAllVariables = AllVariables.Select(Variable => Variable.Clone()).ToList();
        List<Variable> CloneOfActiveVariables = ActiveVariables.Select(Variable => Variable.Clone()).ToList();
        List<List<short>> CloneOfSubgroupSizesOfEachBlock = SubgroupSizesOfEachBlock.Select(SubgroupSizesOfBlock => SubgroupSizesOfBlock.Select(subgroupSize => subgroupSize).ToList()).ToList();
        List<Group> CloneOfGroups = Groups.Select(group => group.Clone()).ToList();

        //create new experiment
        Experiment newExperiment = new Experiment(CloneOfAllExperimentalUnits, CloneOfBlockSizes, CloneOfActiveVariables, CloneOfAllVariables, ExperimentalUnitsHaveMarkers, CloneOfSubgroupSizesOfEachBlock)
        {
            //fill new experiment with clones of all remaining relevant data
            CreateSubgroups = CreateSubgroups,
            InputData = (string)InputData.Clone(),
            Groups = CloneOfGroups,
            SubgroupSizesAreDefinedViaFormControls = SubgroupSizesAreDefinedViaFormControls
        };

        return newExperiment;
    }

    public override string ToString()
    {
        string[] subgroupSizesOfEachBlock = new string[SubgroupSizesOfEachBlock.Count];

        for (int i = 0; i < SubgroupSizesOfEachBlock.Count; i++)
        {
            //fill array with joined subgroup sizes
            subgroupSizesOfEachBlock[i] = "{" + string.Join(";", SubgroupSizesOfEachBlock[i].Select(subgroupSizes => subgroupSizes.ToString()).ToArray()) + "}";
        }

        string ExperimentString = "Number of runs: " // + Runs.Count.ToString()
            + "\nExperimentalUnits: {" + string.Join(";", AllExperimentalUnits.Select(ExperimentalUnit => ExperimentalUnit.Name).ToArray()) + "}"
            + "\nBlock sizes: {" + string.Join(";", BlockSizes.Select(i => i.ToString()).ToArray()) + "}"
            + "\nVariables: " + string.Join(";", ActiveVariables.Select(Variable => Variable.Name).ToArray())
            + "\nCreate subgroups: " + CreateSubgroups.ToString()
            + "\nExperimentalUnits have markers: " + ExperimentalUnitsHaveMarkers.ToString()
            + "\nSubgroup sizes of each block: \n------\n" + string.Join("\n", subgroupSizesOfEachBlock) + "\n------"
            + "\nInput data: \n------\n" + InputData + "\n------";

        return ExperimentString;
    }


    /// <summary>
    /// Resets the properties that are responsible for determining if the Experiment has changed since last save.
    /// This function should always be called after deserializing an Experiment from a file.
    /// </summary>
    public void ResetExperimentHasChangedProperties()
    {
        HasChangedSinceLastSave = false;
        // RunCountAtLastSave = Runs.Count();
    }

    public List<Variable> AllVariables { get; set; } = new List<Variable>();

    public List<Variable> ActiveVariables { get; set; } = new List<Variable>(); //All variables except those that contain names or markers of experimental units

    public List<ExperimentalUnit> AllExperimentalUnits { get; set; } = new List<ExperimentalUnit>();

    public List<short> BlockSizes { get; set; } = new List<short>();

    public bool BlockSizesAreTooLarge { get { return BlockSizes.Sum(blockSize => blockSize) > AllExperimentalUnits.Count; } }

    /// <summary>
    /// The original string that is copied by the user in the InputDataGridView
    /// and contains all information of the ExperimentalUnits.
    /// </summary>
    public string InputData { get; set; } = string.Empty;

    public bool SubgroupSizesAreDefinedViaFormControls { get; set; } = true;

    public List<List<short>> SubgroupSizesOfEachBlock { get; set; } = new List<List<short>>();


    public bool CreateSubgroups { get; set; } = false;

    public bool ExperimentalUnitsHaveMarkers { get; set; } = false;

    public bool HasChangedSinceLastSave
    {
        get
        {
            // if (Runs.Count() > RunCountAtLastSave)
            // hasChangedSinceLastSave = true;

            return hasChangedSinceLastSave;
        }
        private set
        {
            hasChangedSinceLastSave = value;
        }
    }

    /// <summary>
    /// Contains the full file path to which the Experiment was last saved to.
    /// </summary>
    public string SaveFilePath { get; set; } = string.Empty;

    /// <summary>
    /// The number of runs at last save (i.e. serialization).
    /// This is used to track if experiment has changed since last save.
    /// </summary>
    private int RunCountAtLastSave { get; set; } = 0;

    public bool GroupNamesHaveBeenDefinedByUser { get { return Groups.Count != 0; } }

    public List<Group> Groups { get; set; } = new List<Group>();
}
