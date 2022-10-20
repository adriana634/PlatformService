using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Protos;
using static PlatformService.Protos.PlatformService;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : PlatformServiceBase
    {
        private readonly IPlatformRepo platformRepo;
        private readonly IMapper mapper;

        public GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper)
        {
            this.platformRepo = platformRepo;
            this.mapper = mapper;
        }

        public override async Task<GetAllPlatformsResponse> GetAllPlatforms(GetAllPlatformsRequest request, ServerCallContext context)
        {
            var response = new GetAllPlatformsResponse();
            var platforms = await platformRepo.GetAllPlatforms();

            foreach (var platform in platforms)
            {
                response.Platform.Add(mapper.Map<Platform>(platform));
            }

            return response;
        }
    }
}
