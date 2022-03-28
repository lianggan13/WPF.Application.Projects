namespace YunDa.ASIS.Server.Services.JWT
{
    public interface IJWTAuthorizeService
    {
        string GetToken(string UserName, string password);
    }

    public class JWTTokenOptions
    {
        public string Audience
        {
            get;
            set;
        }
        public string SecurityKey
        {
            get;
            set;
        }

        public string Issuer
        {
            get;
            set;
        }
    }
}
