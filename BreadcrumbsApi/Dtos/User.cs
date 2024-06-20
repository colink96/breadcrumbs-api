using BreadcrumbsApi.Dtos;

namespace BreadcrumbsApi;

public partial class User
{
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public int Userid { get; set; }

    public virtual ICollection<Crumb> Crumbs { get; set; } = new List<Crumb>();

    public string Password { get; set; } = string.Empty;
}
