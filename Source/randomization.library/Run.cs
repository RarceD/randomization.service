namespace randomization.library;
public class Run
{
    public Run(bool checkForBlockSetUnicity)
    {
        this.CheckForUnicity = checkForBlockSetUnicity;
    }

    /// <summary>
    /// Creates a new BlockSet, randomly fills blocks with ExperimentsUnits and sets the OnRequest handlers.
    /// </summary>
    /// <param name="random">An instance of the Random class.</param>
    /// <returns>A new instance of BlockSet with randomly filled blocks and with OnRequest handlers set.</returns>
    public BlockSet CreateNewBlockSet(Random random)
    {
        BlockSet newBlockSet = new BlockSet();
        newBlockSet.OnRequestVariables += new Func<List<Variable>>(delegate { return RequestVariables(); });

        List<short> blockSizes = RequestBlockSizes();

        List<ExperimentalUnit> copyOfAllExperimentalUnits = new List<ExperimentalUnit>(RequestAllExperimentalUnits()); //copy ExperimentalUnits (by reference)

        //fill the newBlockSet with new blocks of ExperimentalUnits
        for (int i = 0; i < blockSizes.Count; i++)
        {
            BlockOfExperimentalUnits newBlockOfExperimentalUnits = new BlockOfExperimentalUnits();

            for (int j = 0; j < blockSizes[i]; j++)
            {
                int r = random.Next(0, copyOfAllExperimentalUnits.Count);
                newBlockOfExperimentalUnits.ExperimentalUnits.Add(copyOfAllExperimentalUnits[r]);
                copyOfAllExperimentalUnits.RemoveAt(r);
            }

            newBlockSet.BlocksOfExperimentalUnits.Add(newBlockOfExperimentalUnits);
            newBlockSet.Groups.Add(new Group((char)999, "Block " + (i + 1).ToString())); //until the user instructed the software to randomly allocate group names to blocks
                                                                                         //(i.e. the function RandomizeGroups is called within the selected BlockSet),
                                                                                         //the group names should be "Block 1", "Block 2" etc.
        }

        return (newBlockSet);
    }

    /// <summary>
    /// Requests the bool CreateSubgroups in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a bool CreateSubgroups.
    /// Each Run must to be able to request the bool CreateSubgroups from its parent.
    /// </summary>
    /// <returns>The bool CreateSubgroups in the current Run's parent Experiment.</returns>
    protected bool RequestCreateSubgroups()
    {
        if (OnRequestCreateSubgroups == null)
            throw new Exception("OnRequestCreateSubgroups handler is not assigned");

        return OnRequestCreateSubgroups();
    }

    /// <summary>
    /// Requests the bool ExperimentalUnitsHaveMarker in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a bool ExperimentalUnitsHaveMarker.
    /// Each Run must to be able to request the bool ExperimentalUnitsHaveMarker from its parent.
    /// </summary>
    /// <returns>The bool ExperimentalUnitsHaveMarker in the current Run's parent Experiment.</returns>
    protected bool RequestExperimentalUnitsHaveMarker()
    {
        if (OnRequestExperimentalUnitsHaveMarker == null)
            throw new Exception("OnRequestExperimentalUnitsHaveMarker handler is not assigned");

        return OnRequestExperimentalUnitsHaveMarker();
    }

    /// <summary>
    /// Requests the list of Variables in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a list of Variables.
    /// Each Run must to be able to request the list of Variables from its parent.
    /// </summary>
    /// <returns>The list of Variables in the current Run's parent Experiment.</returns>
    protected List<Variable> RequestVariables()
    {
        if (OnRequestVariables == null)
            throw new Exception("OnRequestVariables handler is not assigned");

        return OnRequestVariables();
    }

    /// <summary>
    /// Requests the list of ExperimentalUnits in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a list of ExperimentalUnits.
    /// Each Run must to be able to request the list of Variables from its parent.
    /// </summary>
    /// <returns>The list of Variables in the current Run's parent Experiment.</returns>
    protected List<ExperimentalUnit> RequestAllExperimentalUnits()
    {
        if (OnRequestAllExperimentalUnits == null)
            throw new Exception("OnRequestAllExperimentalUnits handler is not assigned");

        return OnRequestAllExperimentalUnits();
    }

    /// <summary>
    /// Requests the list of GroupNames in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a list of group names.
    /// Each Run must to be able to request the list of GroupNames from its parent.
    /// </summary>
    /// <returns>The list of Variables in the current Run's parent Experiment.</returns>
    protected List<Group> RequestGroups()
    {
        if (OnRequestGroups == null)
            throw new Exception("OnRequestGroups handler is not assigned");

        return OnRequestGroups();
    }

    /// <summary>
    /// Requests the two-dimensional list of shorts SubgroupSizesPerBlock in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a two-dimensional list of shorts SubgroupSizesPerBlock.
    /// Each Run must to be able to request the SubgroupSizesPerBlock from its parent.
    /// </summary>
    /// <returns>The two-dimensional list of shorts SubgroupSizesPerBlock in the current Run's parent Experiment.</returns>
    protected List<List<short>> RequestSubgroupSizesPerBlock()
    {
        if (OnRequestSubgroupSizesPerBlock == null)
            throw new Exception("OnRequestSubgroupSizesPerBlock handler is not assigned");

        return OnRequestSubgroupSizesPerBlock();
    }

    /// <summary>
    /// Requests the list of shorts BlockSizes in the current Run's parent Experiment.
    /// An Experiment contains a list of Runs and a list of shorts BlockSizes.
    /// Each Run must to be able to request the BlockSizes from its parent.
    /// </summary>
    /// <returns>The list of shorts BlockSizes in the current Run's parent Experiment.</returns>
    protected List<short> RequestBlockSizes()
    {
        if (OnRequestBlockSizes == null)
            throw new Exception("OnRequestBlockSizes handler is not assigned");

        return OnRequestBlockSizes();
    }

    public Func<bool> OnRequestCreateSubgroups;

    public Func<bool> OnRequestExperimentalUnitsHaveMarker;

    public Func<List<Variable>> OnRequestVariables;

    public Func<List<ExperimentalUnit>> OnRequestAllExperimentalUnits;

    public Func<List<Group>> OnRequestGroups;

    public Func<List<List<short>>> OnRequestSubgroupSizesPerBlock;

    public Func<List<short>> OnRequestBlockSizes;

    /// <summary>
    /// Divides all BlockSets in the current Run into subgroups.
    /// </summary>
    public void DivideAllBlockSetsIntoSubgroups()
    {
        //divides the groups of all sets into subgroups, either completely randomly or semi-randomly (based on markers)
        if (RequestCreateSubgroups())
        {
            Random random = new Random();

            foreach (BlockSet BlockSet in BlockSets)
                BlockSet.DivideBlocksIntoSubgroups(RequestSubgroupSizesPerBlock(), RequestExperimentalUnitsHaveMarker(), random);
        }
    }

    /// <summary>
    /// Randomly assigns BlocksOfExperimentalUnits of the given BlockSet to the GroupNames.
    /// </summary>
    public void RandomizeBlocksOfSet(int blockSetIndex)
    {
        Random random = new Random();
        BlockSets[blockSetIndex].RandomlyAssignBlockSetsToGroups(random, RequestGroups());
    }

    /// <summary>
    /// Calculates the descriptives (the mean and standard deviation, and optionally the min, median and max)
    /// in each BlockOfExperimentalUnits in the desired BlockSet.
    /// </summary>
    /// <param name="blockSetIndex">The index of the BlockSet in which to calculate the descriptives.</param>
    /// <param name="calculateAllDescriptives">Optional bool which must be true if it is desired to calculate the min,
    /// median and max besides the mean and standard deviation of each BlockOfExperimentalUnits. Default is false.</param>
    public void CalculateDescriptivesOfBlocksInBlockSet(int blockSetIndex, bool calculateAllDescriptives = false)
    {
        BlockSets[blockSetIndex].CalculateDescriptivesOfBlocks(calculateAllDescriptives);
    }

    /// <summary>
    /// Calculates the Rank in the desired BlockSet.
    /// </summary>
    /// <param name="blockSetIndex">The index of the BlockSet in which to calculate the Rank</param>
    public void CalculateSetRank(int blockSetIndex)
    {
        BlockSets[blockSetIndex].CalculateRank();
    }

    /// <summary>
    /// Sorts all BlockSets in the current Run by their Rank.
    /// </summary>
    public void SortBlockSetsByRank()
    {
        BlockSets = BlockSets.OrderBy(blockSet => blockSet.Rank).ToList();
    }

    /// <summary>
    /// Sorts block sets by rank and sets BlockSet.SortNumber based on the ordered index.
    /// </summary>
    public void NumberBlockSetsByRank()
    {
        SortBlockSetsByRank();

        for (int i = 0; i < BlockSets.Count; i++)
            BlockSets[i].SortNumber = i + 1;
    }

    public bool CheckForUnicity { get; set; } = true;

    /// <summary>
    /// The total number of Sets that have been considered when BlockSets were created
    /// This value therefore also includes any Sets that were omitted because they were not unique.
    /// </summary>
    public long TotalNumberOfSetsConsidered { get; set; } = 0;

    /// <summary>
    /// The total number of unique Sets that have been considered when BlockSets were created
    /// </summary>
    public long UniqueSetsCreated { get; set; } = 0;

    /// <summary>
    /// A list of BlockSets that should contain the best x number of Sets, based on their .Rank
    /// </summary>
    public List<BlockSet> BlockSets { get; set; } = new List<BlockSet>();

    public TimeSpan TotalTimeElapsedForCreatingBlockSets { get; set; } = new TimeSpan();

    private TimeSpan TimeRemaining { get; set; } = new TimeSpan();

    public List<TimeSpan> CalculatedTimesRemaining { get; set; } = new List<TimeSpan>();

    public SortedSet<long> BlockSetHashes { get; set; } = new SortedSet<long>();
}
