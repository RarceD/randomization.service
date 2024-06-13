using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using randomization.tool.Controllers.Dto;
using randomization.tool.Models;
using randomization.library;
using randomization.tool.Service.interfaces;

namespace randomization.tool.Controllers;

[ApiController]
[Route("[controller]")]
public class RandomizationController : ControllerBase
{
    private readonly ILogger<RandomizationController> _logger;
    private readonly IRandomizationService _randomizationService;
    private readonly IMapper _mapper;
    private const int TIMEOUT_SECONDS = 10;

    public RandomizationController(ILogger<RandomizationController> logger, IRandomizationService randomizationService)
    {
        _logger = logger;
        _randomizationService = randomizationService;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RandomizationResults, RandomizationOutputDto>();
            cfg.CreateMap<GroupResult, GroupResultsDto>();
        }
        );
        _mapper = config.CreateMapper();
    }

    [HttpPost(Name = "GetRandomizationGroups")]
    public async Task<ActionResult<RandomizationOutputDto>> GetRandomization(RandomizationInputDto inputParams)
    {
        #region Validations

        if (inputParams.AnimalMeasurements is null)
            return NotFound("Not animal measurements provided!");
        else if (inputParams.NumberOfGroups <= 0)
            return NotFound("Number of groups must be greater than 0!");
        else if (inputParams.NumberOfAnimalsPerGroup <= 0)
            return NotFound("Number of animals per group must be greater than 0!");

        #endregion

        Task<RandomizationResults> randomizationTask = GetRandomizationTask(inputParams);

        var delayTask = Task.Delay(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
        var completedTask = await Task.WhenAny(randomizationTask, delayTask);

        if (completedTask == randomizationTask)
        {
            RandomizationResults results = await randomizationTask;
            var endpointOutput = _mapper.Map<RandomizationOutputDto>(results);
            return Ok(endpointOutput);
        }
        else
        {
            return StatusCode(408, "The operation timed out.");
        }
    }

    private Task<RandomizationResults> GetRandomizationTask(RandomizationInputDto inputParams)
    {
        int idGenerator = 0;
        string numberAnimals = inputParams.AnimalMeasurements.Count.ToString();
        var experimentalUnits = inputParams.AnimalMeasurements.Select(x =>
        {
            char id = (char)(idGenerator++);
            return new ExperimentalUnit(id, numberAnimals) { Name = x.AnimalID, Values = new() { x.Value } };
        }).ToList();
        int groupNumber = inputParams.NumberOfGroups;
        int sizeOfGroups = inputParams.NumberOfAnimalsPerGroup;
        var operationTask = Task.Run(() => _randomizationService.Randomize(experimentalUnits, groupNumber, sizeOfGroups));
        return operationTask;
    }
}
