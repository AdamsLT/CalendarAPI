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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.API.Controllers
{
    public class EventController : BaseCalendarizeController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<EventController> _logger;
        private readonly EventService _eventService;
        private readonly IValidator<EventCreateDto> _eventCreateDtoValidator;

        public EventController(
            IMapper mapper,
            ILogger<EventController> logger, 
            EventService eventService,
            IValidator<EventCreateDto> eventCreateDtoValidator)
        {
            _mapper = mapper;
            _logger = logger;
            _eventService = eventService;
            _eventCreateDtoValidator = eventCreateDtoValidator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(
            DateTime? startDate = null, DateTime? endDate = null, string? locationId = null, string? coachId = null)
        {
            var result = await _eventService.GetEventsAsync(startDate, endDate, locationId, coachId);
            var resultDto = _mapper.Map<IEnumerable<EventDto>>(result);
            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(EventCreateDto eventDto)
        {
            var validationResult = await _eventCreateDtoValidator.ValidateAsync(eventDto);
            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }

            var eventToCreate = _mapper.Map<Event>(eventDto);
            var result = await _eventService.CreateEventAsync(eventToCreate);
            var urlAction = Url.Action(nameof(GetAsync), new
            {
                result.Id
            });

            var resultDto = _mapper.Map<EventDto>(result);
            return Created(urlAction, resultDto);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var result = await _eventService.GetEventAsync(id);
            var resultDto = _mapper.Map<EventDto>(result);
            return Ok(resultDto);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _eventService.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, EventCreateDto eventDto)
        {
            var validationResult = await _eventCreateDtoValidator.ValidateAsync(eventDto);
            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }

            var obj = _mapper.Map<Event>(eventDto);
            obj.Id = new ObjectId(id);

            var result = await _eventService.UpdateEventAsync(obj);
            var resultDto = _mapper.Map<EventDto>(result);
            return Ok(resultDto);
        }
    }
}
