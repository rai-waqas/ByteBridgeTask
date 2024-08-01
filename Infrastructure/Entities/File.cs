using File = DataAccess.Entities.File;

namespace DataAccess.Entities
{
    public partial class File
    {
        public int Id { get; set; }
        public string Filename { get; set; } = null!;
        public byte[]? Filedata { get; set; }
        public int? ClientDetailsId { get; set; }

        public virtual Clientdetail? ClientDetails { get; set; }
    }
}
