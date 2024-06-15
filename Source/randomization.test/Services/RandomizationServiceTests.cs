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

using Microsoft.Extensions.Logging;
using NSubstitute;
using randomization.library;
using randomization.tool.Models;
using randomization.tool.Service;
using randomization.tool.Service.interfaces;
using randomization.tool.Services;
using randomization.tool.Services.interfaces;

namespace randomization.test.Services;
[TestFixture]
public class RandomizationServiceTests
{
    private IRandomizationService _randomizationService;
    private IExperimentService _experimentService;
    private ILogger<RandomizationService> _logger;
    private List<ExperimentalUnit> inputData;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<RandomizationService>>();

        inputData = new List<ExperimentalUnit>()
        {
            new ExperimentalUnit('1', "25") { Name = "1", Values = new List<double>() { 48.418 } },
            new ExperimentalUnit('2', "25") { Name = "2", Values = new List<double>() { 155.575 } },
            new ExperimentalUnit('3', "25") { Name = "3", Values = new List<double>() { 202.589 } },
            new ExperimentalUnit('4', "25") { Name = "4", Values = new List<double>() { 209.722 } },
            new ExperimentalUnit('5', "25") { Name = "5", Values = new List<double>() { 143.072 } },
            new ExperimentalUnit('6', "25") { Name = "6", Values = new List<double>() { 142.551 } },
            new ExperimentalUnit('7', "25") { Name = "7", Values = new List<double>() { 120.129 } },
            new ExperimentalUnit('8', "25") { Name = "8", Values = new List<double>() { 132.731 } },
            new ExperimentalUnit('9', "25") { Name = "9", Values = new List<double>() { 150.73 } },
            new ExperimentalUnit('a', "25") { Name = "10", Values = new List<double>() { 128.751 } },
            new ExperimentalUnit('b', "25") { Name = "11", Values = new List<double>() { 156.262 } },
            new ExperimentalUnit('c', "25") { Name = "12", Values = new List<double>() { 111.043 } },
            new ExperimentalUnit('d', "25") { Name = "13", Values = new List<double>() { 123.106 } },
            new ExperimentalUnit('e', "25") { Name = "14", Values = new List<double>() { 218.81 } },
            new ExperimentalUnit('f', "25") { Name = "15", Values = new List<double>() { 169.495 } },
            new ExperimentalUnit('g', "25") { Name = "16", Values = new List<double>() { 267.33 } },
            new ExperimentalUnit('h', "25") { Name = "17", Values = new List<double>() { 285.26 } },
            new ExperimentalUnit('i', "25") { Name = "18", Values = new List<double>() { 172.654 } },
            new ExperimentalUnit('j', "25") { Name = "19", Values = new List<double>() { 100.829 } },
            new ExperimentalUnit('k', "25") { Name = "20", Values = new List<double>() { 169.287 } },
            new ExperimentalUnit('l', "25") { Name = "21", Values = new List<double>() { 142.159 } },
            new ExperimentalUnit('m', "25") { Name = "22", Values = new List<double>() { 116.201 } },
            new ExperimentalUnit('n', "25") { Name = "23", Values = new List<double>() { 243.525 } },
            new ExperimentalUnit('o', "25") { Name = "24", Values = new List<double>() { 166.298 } },
            new ExperimentalUnit('p', "25") { Name = "25", Values = new List<double>() { 148.613 } }
        };
        _experimentService = new ExperimentService();
        int randomSeed = 1;
        _randomizationService = new RandomizationService(_logger, _experimentService, randomSeed);
    }

    [Test]
    public void Randomize_ReturnsExpectedResultGroupOne()
    {
        var result = _randomizationService.Randomize(inputData, 2, 10);
        var expectedResult = new RandomizationResults()
        {
            Groups = new List<GroupResult>()
            {
                new GroupResult()
                {
                    Name = "Group 1",
                    Values = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" },
                    Rank = 0.0018,
                    Mean = 165.5988,
                    SD = 49.3569,
                    Min = 111.043,
                    Median = 146.901,
                    Max = 267.330
                },
                new()
            }
        };

        // Expected: 2 groups and 10 values in each group
        Assert.That(2, Is.EqualTo(result.Groups.Count));
        Assert.That(10, Is.EqualTo(result.Groups.First().Values.Count));

        // Same Rank , Mean, SD, Min, Median, Max for both groups
        var firstGroup = result.Groups.First();
        var expectedFirstGroup = result.Groups.First();
        Assert.That(firstGroup.Rank, Is.EqualTo(expectedFirstGroup.Rank));
        Assert.That(firstGroup.Median, Is.EqualTo(expectedFirstGroup.Median));
        Assert.That(firstGroup.Mean, Is.EqualTo(expectedFirstGroup.Mean));
        Assert.That(firstGroup.Max, Is.EqualTo(expectedFirstGroup.Max));
        Assert.That(firstGroup.SD, Is.EqualTo(expectedFirstGroup.SD));
        Assert.That(firstGroup.Min, Is.EqualTo(expectedFirstGroup.Min));
    }


    [Test]
    public void Randomize_ReturnsExpectedResultGroupTwo()
    {
        var result = _randomizationService.Randomize(inputData, 2, 10);
        var expectedResult = new RandomizationResults()
        {
            Groups = new List<GroupResult>()
            {
                new(),
                new() {
                    Name = "Group 2",
                    Values = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" },
                    Rank = 0.0018,
                    Mean = 165.6133,
                    SD = 49.3569,
                    Min = 100.829,
                    Median = 160.9365,
                    Max = 285.26
                }
            }
        };

        // Expected: 2 groups and 10 values in each group
        Assert.That(2, Is.EqualTo(result.Groups.Count));
        Assert.That(10, Is.EqualTo(result.Groups[1].Values.Count));

        // Same Rank , Mean, SD, Min, Median, Max for both groups
        var secondGroup = result.Groups[1];
        var expectedSecondGroup = result.Groups[1];
        Assert.That(secondGroup.Rank, Is.EqualTo(expectedSecondGroup.Rank));
        Assert.That(secondGroup.Mean, Is.EqualTo(expectedSecondGroup.Mean));
        Assert.That(secondGroup.Median, Is.EqualTo(expectedSecondGroup.Median));
        Assert.That(secondGroup.Max, Is.EqualTo(expectedSecondGroup.Max));
        Assert.That(secondGroup.SD, Is.EqualTo(expectedSecondGroup.SD));
        Assert.That(secondGroup.Min, Is.EqualTo(expectedSecondGroup.Min));
    }

    [Test]
    public void Randomize_ReturnsExpectedSampleGroupAnimals()
    {
        var result = _randomizationService.Randomize(inputData, 2, 10);
        var expectedResult = new RandomizationResults()
        {
            Groups = new List<GroupResult>()
            {
                new()
                {
                    Name = "Group 1",
                    Values = new List<string>() { "16", "13", "5", "6", "4", "15", "12", "9", "7", "14" },
                },
                new() {
                    Name = "Group 2",
                    Values = new List<string>() { "24", "18", "17", "10", "3", "8", "21", "19", "2", "20" },
                }
            }
        };
        var firstGroupItems = result.Groups[0].Values;
        var firstExpectedGroupItems = expectedResult.Groups[0].Values;
        var secondGroupItems = result.Groups[1].Values;
        var secondExpectedGroupItems = expectedResult.Groups[1].Values;
        Assert.That(firstExpectedGroupItems, Is.EquivalentTo(firstGroupItems));
        Assert.That(secondExpectedGroupItems, Is.EquivalentTo(secondGroupItems));
    }

    [Test]
    public void Randomize_ReturnsExpectedUniqueSetsNumber()
    {
        var result = _randomizationService.Randomize(inputData, 2, 10);
        var expectedResult = new RandomizationResults()
        {
            UniqueSetsNumber = 4908043140
        };
        Assert.That(expectedResult.UniqueSetsNumber, Is.EqualTo(result.UniqueSetsNumber));
    }

}
