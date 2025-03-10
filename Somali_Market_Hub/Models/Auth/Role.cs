namespace Somali_Market_Hub.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<UserAccount>? UserAccounts { get; set; }
    }
}
