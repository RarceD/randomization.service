namespace randomization.tool.Controllers.Dto;
public class RandomizationInputDto
{
    public int NumberOfGroups { get; set; }
    public int NumberOfAnimalsPerGroup { get; set; }
    public List<ExperimentalUnitDto> AnimalMeasurements { get; set; }
}
