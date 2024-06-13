using randomization.library;
using randomization.tool.Services.interfaces;

namespace randomization.tool.Services;
public class ExperimentService : IExperimentService
{
    private Experiment _currentExperiment { get; set; }
    public ExperimentService()
    {
        _currentExperiment = new Experiment();
    }

    public void Start(List<ExperimentalUnit> experimentalUnits, List<short> blockSizes, List<Variable> activeVariable, List<Variable> allVariables)
    {
        _currentExperiment = new Experiment(
            experimentalUnits,
            blockSizes,
            activeVariable,
            allVariables,
            false); // TODO: Remove this experimentalUnitsHaveMarkers parameter
    }

    public int GetBlockSizeNumber()
        => _currentExperiment.BlockSizes.Count;
    public int GetAllExperimentalUnitsNumber()
        => _currentExperiment.AllExperimentalUnits.Count;

    public Run CreateRun()
        => _currentExperiment.CreateNewRun(true);

}
