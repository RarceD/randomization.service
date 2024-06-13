using randomization.library;
using randomization.tool.Models;
using randomization.tool.Service.interfaces;
using randomization.tool.Services.interfaces;

namespace randomization.tool.Service;
public class RandomizationService : IRandomizationService
{
    private readonly IExperimentService _experimentService;
    private ILogger<RandomizationService> _logger;
    private readonly int DesiredUniqueSets;
    private readonly int? _randomSeed;
    private double TheoreticalUniqueBlockSets { get; set; }

    private List<short> BlockSizes;
    private Run CurrentRun { get; set; }
    private int RememberSets { get; set; }


    public RandomizationService(ILogger<RandomizationService> logger, IExperimentService experimentService, int? randomSeed = null)
    {
        _logger = logger;
        _experimentService = experimentService;
        _randomSeed = randomSeed;

        DesiredUniqueSets = 80000;
        RememberSets = 2000;
    }

    public RandomizationResults Randomize(List<ExperimentalUnit> units, int numberOfGroups, int sizeOfGroups)
    {
        StartExperiment(units, numberOfGroups, sizeOfGroups);

        Random random = GetRandomSeed();
        while (CurrentRun.UniqueSetsCreated < DesiredUniqueSets)
        {
            bool setIsUnique = true; //default is true
            BlockSet newSet = CurrentRun.CreateNewBlockSet(random);

            //check if newSet is unique, if desired
            if (CurrentRun.CheckForUnicity)
                setIsUnique = CurrentRun.BlockSetHashes.Add(newSet.StableHashCode); //setHashes is a SortedSet which will return false if an item is added, but already exists in the SortedSet

            if (setIsUnique)
            {
                CurrentRun.UniqueSetsCreated++;
                newSet.CalculateRank();

                //Always remember the newly created set, if the number of unique sets is smaller than the number of sets that will be remembered
                if (CurrentRun.BlockSets.Count < RememberSets)
                {
                    CurrentRun.BlockSets.Add(newSet);
                    CurrentRun.SortBlockSetsByRank();
                }
                else
                {
                    //else, add the newSet only if its rank is lower than the last (i.e. "worst") BlockSet
                    if (newSet.Rank < CurrentRun.BlockSets.Last().Rank)
                    {
                        CurrentRun.BlockSets.RemoveAt(RememberSets - 1);
                        CurrentRun.BlockSets.Add(newSet);
                        CurrentRun.SortBlockSetsByRank(); //finally, sort the BlockSets by rank
                    }
                }
            }
        }
        return GenerateResults();
    }

    private RandomizationResults GenerateResults()
    {
        int blockToSelect = 1;
        var bestBlocks = CurrentRun.BlockSets.OrderBy(i => i.Rank).Take(blockToSelect).ToList();
        var result = new RandomizationResults { UniqueSetsNumber = CalculateTheoreticalNumberOfUniqueSets() };

        int groupNumber = 1;
        foreach (BlockSet? block in bestBlocks)
        {
            block.CalculateRank();
            block.CalculateDescriptivesOfBlocks(true);
            var units = block.BlocksOfExperimentalUnits.Select(descriptive => descriptive.ExperimentalUnits).ToList();
            var descriptivesTest = block.BlocksOfExperimentalUnits.Select(descriptive => descriptive.Descriptives).ToList();
            for (int index = 0; index < units.Count; index++)
            {
                var unitList = units[index].Select(u => u.Name).ToList();
                var dValue = descriptivesTest[index].First();
                var stdDeviation = block.SDs.First();
                var sampleValues = units[index].Select(i => i.Values).Select(i => i.First()).ToList(); ;
                double pValue = Calc.CalculatePValue(sampleValues, dValue.Mean);
                result.Groups.Add(new()
                {
                    Name = $"Group {groupNumber++}",
                    SD = RoundValue(stdDeviation),
                    Values = unitList,
                    Rank = RoundValue(block.Rank),
                    Median = RoundValue(dValue.Median),
                    Mean = RoundValue(dValue.Mean),
                    Max = RoundValue(dValue.Max),
                    Min = RoundValue(dValue.Min),
                    pValue = RoundValue(pValue)
                });
            }
        }

        // Dirty sort only for one case:
        // should be deleted but ...
        try
        {
            foreach (var group in result.Groups)
                group.Values = group.Values.OrderBy(s => int.Parse(s.Substring(7))).ToList();
        }
        catch (Exception) { }

        return result;
    }
    private double RoundValue(double value)
        => Math.Round(value, 4, MidpointRounding.AwayFromZero);
    private double CalculateTheoreticalNumberOfUniqueSets()
    {
        double theoreticalUniqueBlocks = 0; //default value

        //calculates the theoretical number of unique combinations that can be created
        //with the total number of ExperimentalUnits, and the currently given block sizes.
        if (BlockSizes.Count != 0 && !BlockSizes.Contains(Settings.MissingBlockSize))
        {
            double result;
            double ExperimentalUnitsRemaining = (double)_experimentService.GetAllExperimentalUnitsNumber();
            List<double> uniqueBlocks = new List<double>();
            double uniqueCombinations;

            foreach (int blockSize in BlockSizes)
            {
                result = Calc.Factorial(ExperimentalUnitsRemaining);

                if (ExperimentalUnitsRemaining - blockSize != 0)
                    result /= Calc.Factorial(ExperimentalUnitsRemaining - blockSize);

                result /= Calc.Factorial(blockSize);
                ExperimentalUnitsRemaining -= blockSize;
                uniqueBlocks.Add(result);
            }

            uniqueCombinations = uniqueBlocks[0];

            for (int i = 1; i < uniqueBlocks.Count; i++)
                uniqueCombinations *= uniqueBlocks[i];

            uniqueCombinations /= Calc.Factorial(_experimentService.GetBlockSizeNumber());

            if (double.IsNaN(uniqueCombinations) || double.IsInfinity(uniqueCombinations))
                theoreticalUniqueBlocks = double.MaxValue;
            else if (uniqueCombinations > 0)
                theoreticalUniqueBlocks = Math.Round(uniqueCombinations, 0, MidpointRounding.AwayFromZero);
        }

        TheoreticalUniqueBlockSets = theoreticalUniqueBlocks;

        return theoreticalUniqueBlocks;
    }

    private List<short> GetBlockSize(int numberOfGroups, int sizeOfGroups)
        => Enumerable.Repeat((short)sizeOfGroups, numberOfGroups).ToList();

    private Variable GetGenericVariable() => new("Variable 1", 3, 1);

    private void StartExperiment(List<ExperimentalUnit> units, int numberOfGroups, int sizeOfGroups)
    {
        List<short> blockSizes = GetBlockSize(numberOfGroups, sizeOfGroups);
        List<Variable> activeVariable = new() { GetGenericVariable() };
        List<Variable> allVariables = new() { new("Variable Grid", 0, 1), GetGenericVariable() };
        BlockSizes = GetBlockSize(numberOfGroups, sizeOfGroups);
        _experimentService.Start(units, blockSizes, activeVariable, allVariables);
        CurrentRun = _experimentService.CreateRun();
    }

    // Do not create a new Random() within a loop, because that may cause the same seed to be re-used
    private Random GetRandomSeed()
        => _randomSeed.HasValue ? new Random(_randomSeed.Value) : new Random();
}
