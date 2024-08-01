
namespace DataAccess.Dtos
{
    public class FileDto
    {
        public int Id { get; set; }
        public string? Filename { get; set; }
        public byte[]? Filedata { get; set; }
        public int ClientDetailsId { get; set; }
    }

}
