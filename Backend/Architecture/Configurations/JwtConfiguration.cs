namespace Weblab.Architecture.Configurations;

public class JwtConfiguration
{
    public string Key {get; set;} = null!;
    public string Algorithm {get; set;} = null!;
    public string Issuer {get; set;} = null!;
    public string Audience {get; set;} = null!;
}