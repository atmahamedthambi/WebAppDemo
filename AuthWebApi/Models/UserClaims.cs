namespace AuthWebApi.Models
{
    public class UserClaims
    {
        public string Email { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    //User claims are nothing but key value pair collections. User can have multiple claims
}
