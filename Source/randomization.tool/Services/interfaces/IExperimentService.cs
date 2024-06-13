using randomization.library;

namespace randomization.tool.Services.interfaces
{
    public interface IExperimentService
    {
        int GetBlockSizeNumber();
        int GetAllExperimentalUnitsNumber();
        void Start(List<ExperimentalUnit> experimentalUnits, List<short> blockSizes, List<Variable> activeVariable, List<Variable> allVariables);
        Run CreateRun();
    }
}