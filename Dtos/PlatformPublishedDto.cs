namespace PlatformService.Dtos
{
    public class PlatformPublishedDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Event { get; set; } = default!;
    }
}