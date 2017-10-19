using MillProApp.Data.Models;

namespace MillProApp.UserAdmin.Models
{
    public class RoleViewModel
    {
        public RoleViewModel(ApplicationRole role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }

    }
}