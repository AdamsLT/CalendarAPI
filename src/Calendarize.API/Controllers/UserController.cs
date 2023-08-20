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
    public class UserController : BaseCalendarizeController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        private readonly IValidator<UserCreateDto> _createDtoValidator;

        public UserController(
            IMapper mapper,
            ILogger<UserController> logger,
            UserService userService,
            IValidator<UserCreateDto> createDtoValidator)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _createDtoValidator = createDtoValidator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(string name, string lastname, string email, string phoneNumber)
        {
            var result = await _userService.GetUsersAsync(name, lastname, email, phoneNumber);
            var resultDto = _mapper.Map<IEnumerable<UserDto>>(result);
            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDto createDto)
        {
            var validationResult = await _createDtoValidator.ValidateAsync(createDto);
            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }

            var userToCreate = _mapper.Map<User>(createDto);
            var result = await _userService.CreateUserAsync(userToCreate);
            var urlAction = Url.Action(nameof(GetAsync),new
            {
                id = result.Id
            });

            var resultDto = _mapper.Map<UserDto>(result);
            return Created(urlAction, resultDto);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var result = await _userService.GetUserAsync(id);
            var resultDto = _mapper.Map<UserDto>(result);
            return Ok(resultDto);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, UserCreateDto userDto)
        {
            var obj = _mapper.Map<User>(userDto);
            obj.Id = new ObjectId(id);

            var result = await _userService.UpdateUserAsync(obj);
            var resultDto = _mapper.Map<UserDto>(result);
            return Ok(resultDto);
        }
    }
}
