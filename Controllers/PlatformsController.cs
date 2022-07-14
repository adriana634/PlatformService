using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo platformRepo;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;

        public PlatformsController(
            IPlatformRepo platformRepo, 
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            this.platformRepo = platformRepo;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            IEnumerable<Platform> platforms = this.platformRepo.GetAllPlatforms();

            IEnumerable<PlatformReadDto> result = this.mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Platform? platform = this.platformRepo.GetPlatformById(id);
            if (platform == null) return NotFound();

            PlatformReadDto result = this.mapper.Map<PlatformReadDto>(platform);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Platform platformModel = this.mapper.Map<Platform>(platformCreateDto);

            this.platformRepo.CreatePlatform(platformModel);
            this.platformRepo.SaveChanges();

            PlatformReadDto platformReadDto = this.mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await this.commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}