using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class Client
    {
        public Client()
        {
            Clientdetails = new HashSet<Clientdetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Clientdetail> Clientdetails { get; set; }
    }
}
