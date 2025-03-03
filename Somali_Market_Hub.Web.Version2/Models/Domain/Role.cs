using Somali_Market_Hub.Web.Version2.Models.Domain;

namespace Somali_Market_Hub.Web.Version2.Models.Domain
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<UserAccount>? UserAccounts { get; set; }
    }
}
