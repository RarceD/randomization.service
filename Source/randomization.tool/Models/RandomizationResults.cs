namespace randomization.tool.Models;
public class RandomizationResults
{
    public RandomizationResults()
    {
        Groups = new();
    }
    public double UniqueSetsNumber { get; set; }
    public List<GroupResult> Groups { get; set; }
}
