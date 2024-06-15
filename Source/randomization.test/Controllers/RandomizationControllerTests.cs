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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using randomization.library;
using randomization.tool.Controllers;
using randomization.tool.Controllers.Dto;
using randomization.tool.Models;
using randomization.tool.Service.interfaces;

namespace randomization.test.Controllers;

[TestFixture]
public class RandomizationControllerTests
{
    private RandomizationController _controller;
    private IRandomizationService _randomizationService;
    private ILogger<RandomizationController> _logger;


    [SetUp]
    public void Setup()
    {
        _randomizationService = Substitute.For<IRandomizationService>();
        _logger = Substitute.For<ILogger<RandomizationController>>();
        _controller = new RandomizationController(_logger, _randomizationService);
    }

    [Test]
    public async Task GetRandomization_ReturnsTimeoutErrorAsync()
    {
        int delaySeconds = 15;
        _randomizationService
            .Randomize(Arg.Any<List<ExperimentalUnit>>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns(await Task<RandomizationResults>.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                return new RandomizationResults() { Groups = new() { }, UniqueSetsNumber = 0 };
            }));

        var inputParams = new RandomizationInputDto()
        {
            AnimalMeasurements = new(),
            NumberOfGroups = 5,
            NumberOfAnimalsPerGroup = 10
        };

        var result = await _controller.GetRandomization(inputParams);

        var statusCodeResult = result.Result as StatusCodeResult;
        Assert.That(statusCodeResult, Is.Not.Null);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(408));
    }
    [Test]
    public void GetRandomization_ReturnsErrorBecauseNoAnimals()
    {
        var endpointInputData = new RandomizationInputDto()
        {
            AnimalMeasurements = null,
            NumberOfAnimalsPerGroup = 10,
            NumberOfGroups = 10
        };
        var result = _controller.GetRandomization(endpointInputData).Result;
        var statusCode = (result.Result as NotFoundObjectResult)?.StatusCode;
        Assert.That(statusCode, Is.EqualTo(404));
    }

    [Test]
    public void GetRandomization_ReturnsErrorBecauseNoNumberOfGroups()
    {
        var endpointInputData = new RandomizationInputDto()
        {
            AnimalMeasurements = new() { },
            NumberOfAnimalsPerGroup = 10,
            NumberOfGroups = 0
        };
        var result = _controller.GetRandomization(endpointInputData).Result;
        var statusCode = (result.Result as NotFoundObjectResult)?.StatusCode;
        Assert.That(statusCode, Is.EqualTo(404));
    }

    [Test]
    public void GetRandomization_ReturnsErrorBecauseNoNumberOfAnimalsPerGroups()
    {
        var endpointInputData = new RandomizationInputDto()
        {
            AnimalMeasurements = new() { },
            NumberOfAnimalsPerGroup = -1,
            NumberOfGroups = 2
        };
        var result = _controller.GetRandomization(endpointInputData).Result;
        var statusCode = (result.Result as NotFoundObjectResult)?.StatusCode;
        Assert.That(statusCode, Is.EqualTo(404));
    }
}
