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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.API.Controllers
{
    public class RegistrationController : BaseCalendarizeController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrationController> _logger;
        private readonly RegistrationService _registrationService;
        private readonly IValidator<RegistrationCreateDto> _createDtoValidator;

        public RegistrationController(
            IMapper mapper,
            ILogger<RegistrationController> logger,
            RegistrationService registrationService,
            IValidator<RegistrationCreateDto> createDtoValidator)
        {
            _mapper = mapper;
            _logger = logger;
            _registrationService = registrationService;
            _createDtoValidator = createDtoValidator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RegistrationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncs(string userId = null, string eventId = null)
        {
            var result = await _registrationService.GetRegistrationsAsync(userId, eventId);
            var resultDto = _mapper.Map<IEnumerable<RegistrationDto>>(result);
            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegistrationCreateDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody]RegistrationCreateDto createDto)
        {
            var validationResult = await _createDtoValidator.ValidateAsync(createDto);
            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }

            var registrationToCreate = _mapper.Map<Registration>(createDto);
            await _registrationService.CreateRegistrationAsync(registrationToCreate);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _registrationService.DeleteRegistrationAsync(id);
            return NoContent();
        }
    }
}
