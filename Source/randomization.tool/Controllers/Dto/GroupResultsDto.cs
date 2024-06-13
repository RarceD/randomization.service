namespace randomization.tool.Controllers.Dto;
public class GroupResultsDto
{
    public string Name { get; set; }
    public List<string> Values { get; set; }
    public double Rank { get; set; }
    public double Mean { get; set; }
    public double SD { get; set; }
    public double Min { get; set; }
    public double Median { get; set; }
    public double Max { get; set; }
    public double pValue { get; set; }
}
