
using DataAccess.Dtos;
using Microsoft.AspNetCore.Http;
using File = DataAccess.Entities.File;
namespace DataAccess.Dtos
{
    public class AddClientDetailsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int ClientId { get; set; }
        public string StateId { get; set; } = null!;
        public DateTime Dob { get; set; }
        public DateTime ExpStart { get; set; }
        public DateTime ExpEnd { get; set; }
        public decimal PayValue { get; set; }
        public string PayType { get; set; } = null!;
        public char Gender { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
