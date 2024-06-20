namespace BreadcrumbsApi.Dtos
{
    public class LoginResult
    {
        public string? ApiKey { get; set; }
        public bool LoginSuccess { get; set; } = false;
        public string? Message { get; set; }
    }
}
