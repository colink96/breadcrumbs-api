namespace BreadcrumbsApi
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int Expiration { get; set; }
    }
}
