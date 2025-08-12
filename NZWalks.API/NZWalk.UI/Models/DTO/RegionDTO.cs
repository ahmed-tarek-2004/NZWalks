namespace NZWalk.UI.Models.DTO
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? RegionImageUrl { get; set; }
    }
}
