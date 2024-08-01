using DataAccess.Dtos;
namespace DataAccess.Dtos
{
    public class ClientDetailsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public int[] StateId { get; set; } = Array.Empty<int>();
        public DateTime Dob { get; set; }
        public DateTime ExpStart { get; set; }
        public DateTime ExpEnd { get; set; }
        public decimal PayValue { get; set; }
        public string PayType { get; set; } = null!;
        public char Gender { get; set; }
        public List<FileDto> Files { get; set; } = new List<FileDto>();
    }
}
