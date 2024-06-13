using randomization.library;
using randomization.tool.Models;

namespace randomization.tool.Service.interfaces;
public interface IRandomizationService
{
    RandomizationResults Randomize(List<ExperimentalUnit> units, int numberOfGroups, int sizeOfGroups);
}