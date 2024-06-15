//    Randomization Service
//    Copyright(C) 2024 Rubén Arce
//    and individual contributors.
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program. If not, see <https://www.gnu.org/licenses/>.
//
//
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
