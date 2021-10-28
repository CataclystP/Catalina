namespace Catalina.Database.Models
{
    public class Role
    {
        public ulong ID { get; set; }
        public ulong? userID { get; set; }
        public ulong? roleID { get; set; }
        public ulong? guildID { get; set; }
    }
}