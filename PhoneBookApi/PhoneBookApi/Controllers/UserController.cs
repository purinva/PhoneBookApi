using Microsoft.AspNetCore.Mvc;
using PhoneBookApi.Models;
using PhoneBookApi.Repositories;
using AutoMapper;
using PhoneBookApi.ModelsDto;
using PhoneBookApi.Services; // Подключаем RedisService
using System.Text.Json;

namespace PhoneBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly RedisService _redisService;
        private readonly KafkaService _kafkaService; // Добавляем зависимость для Kafka

        public UserController(IUserRepository userRepository,
            IMapper mapper, RedisService redisService, KafkaService kafkaService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _redisService = redisService;
            _kafkaService = kafkaService; // Инициализируем KafkaService
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, 
            int pageSize = 10)
        {
            string cacheKey = $"users_page_{page}_size_{pageSize}";

            try
            {
                // Проверяем кэш
                var cachedUsers = await _redisService.GetAsync(cacheKey);
                if (cachedUsers != null)
                {
                    var usersDto = JsonSerializer
                        .Deserialize<IEnumerable<UserDto>>(cachedUsers);
                    return Ok(usersDto);
                }

                // Данные не найдены в кэше — получаем из базы
                var users = await _userRepository.GetPaginatedAsync(
                    page, pageSize);
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

                // Сохраняем в кэше на 5 минут
                await _redisService.SetAsync(
                    cacheKey, 
                    JsonSerializer.Serialize(userDtos), 
                    TimeSpan.FromMinutes(5));

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string cacheKey = $"user_{id}";

            try
            {
                // Проверяем кэш
                var cachedUser = await _redisService.GetAsync(cacheKey);
                if (cachedUser != null)
                {
                    var cacheduserDto = JsonSerializer.Deserialize<UserDto>(cachedUser);
                    return Ok(cacheduserDto);
                }

                // Получаем из базы
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                var userDto = _mapper.Map<UserDto>(user);

                // Кэшируем на 10 минут
                await _redisService.SetAsync(cacheKey, JsonSerializer.Serialize(userDto), TimeSpan.FromMinutes(10));

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Нет необходимости присваивать ID, оно будет назначено автоматически
                await _userRepository.AddAsync(user);

                // Очистка кэша всех пользователей (списки)
                await _redisService.ClearByPatternAsync("users_page_*");

                var userDto = _mapper.Map<UserDto>(user);

                // Отправляем сообщение в Kafka о создании пользователя
                await _kafkaService.ProduceMessageAsync("user_updates", $"User created: {userDto.Name} {userDto.Surname}");

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (user == null || id != user.Id)
                return BadRequest("Invalid user data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingUser = await _userRepository.GetByIdAsync(id);
                if (existingUser == null)
                    return NotFound("User not found.");

                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                existingUser.PhoneNumber = user.PhoneNumber;

                await _userRepository.UpdateAsync(existingUser);

                // Удаляем кэш конкретного пользователя и список пользователей
                await _redisService.DeleteAsync($"user_{id}");
                await _redisService.ClearByPatternAsync("users_page_*");

                // Отправляем сообщение в Kafka о том, что пользователь обновлен
                await _kafkaService.ProduceMessageAsync("user_updates", $"User updated: {user.Name} {user.Surname}");

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                await _userRepository.DeleteAsync(id);

                // Удаляем кэш конкретного пользователя и список пользователей
                await _redisService.DeleteAsync($"user_{id}");
                await _redisService.ClearByPatternAsync("users_page_*");

                // Отправляем сообщение в Kafka о том, что пользователь удален
                await _kafkaService.ProduceMessageAsync("user_updates", $"User deleted: {user.Name} {user.Surname}");

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }
    }
}