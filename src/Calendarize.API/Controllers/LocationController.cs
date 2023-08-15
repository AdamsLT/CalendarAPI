using AutoMapper;
using Calendarize.Core.Dto;
using Calendarize.Core.Entities;
using Calendarize.Core.Services;
using Calendarize.API.Validation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.API.Controllers
{
    [ApiController]
    public class LocationController : BaseCalendarizeController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LocationController> _logger;
        private readonly LocationService _locationService;
        private readonly IValidator<LocationCreateDto> _createDtoValidator;

        public LocationController(
            IMapper mapper,
            ILogger<LocationController> logger,
            LocationService locationService,
            IValidator<LocationCreateDto> createDtoValidator)
        {
            _mapper = mapper;
            _logger = logger;
            _locationService = locationService;
            _createDtoValidator = createDtoValidator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync(string name, string city, string address)
        {
            var result = await _locationService.GetLocationsAsync(name, city, address);
            var resultDto = _mapper.Map<IEnumerable<LocationDto>>(result);
            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody]LocationCreateDto createDto)
        {
            var validationResult = await _createDtoValidator.ValidateAsync(createDto);
            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }

            var locationToCreate = _mapper.Map<Location>(createDto);
            var result = await _locationService.CreateLocationAsync(locationToCreate);
            var urlAction = Url.Action(nameof(GetAsync),new
            {
                id = result.Id
            });

            var resultDto = _mapper.Map<LocationDto>(result);
            return Created(urlAction, resultDto);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var result = await _locationService.GetLocationAsync(id);
            var resultDto = _mapper.Map<LocationDto>(result);
            return Ok(resultDto);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _locationService.DeleteLocationAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, LocationCreateDto locationDto)
        {
            var obj = _mapper.Map<Location>(locationDto);
            obj.Id = new ObjectId(id);

            var result = await _locationService.UpdateLocationAsync(obj);
            var resultDto = _mapper.Map<LocationDto>(result);
            return Ok(resultDto);
        }
    }
}
