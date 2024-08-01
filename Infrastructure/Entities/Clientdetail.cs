using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class Clientdetail
    {
        public Clientdetail()
        {
            Files = new HashSet<File>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ClientId { get; set; }
        public string StateId { get; set; } = null!;
        public DateTime Dob { get; set; }
        public DateTime ExpStart { get; set; }
        public DateTime ExpEnd { get; set; }
        public decimal PayValue { get; set; }
        public string PayType { get; set; } = null!;
        public char Gender { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<File> Files { get; set; }
    }
}
