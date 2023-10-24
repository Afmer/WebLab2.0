using Microsoft.IdentityModel.Tokens;

namespace Weblab.Architecture.Interfaces;

public interface IJwtConfig
{
    public string Issuer {get;}
    public string Audience {get;}
    public SymmetricSecurityKey SecurityKey {get;}
    public string Algorithm {get;}
}