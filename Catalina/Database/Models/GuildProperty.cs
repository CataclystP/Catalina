using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Catalina.Database.Models
{
    public class GuildProperty
    {
        public ulong ID { get; set; }
        public string AdminRoleIDsSerialised { get; set; }

        [NotMapped]
        public ulong[] AdminRoleIDs
        {
            get => !string.IsNullOrEmpty(AdminRoleIDsSerialised) ? AdminRoleIDsSerialised.Split(',').Select(x => Convert.ToUInt64(x)).ToArray() : null;
            set => AdminRoleIDsSerialised = string.Join(',', value);
        }
        public ulong? DefaultRole { get; set; }
    }
}
