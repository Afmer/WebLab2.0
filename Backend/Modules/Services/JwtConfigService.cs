using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Weblab.Architecture.Configurations;
using Weblab.Architecture.Interfaces;

namespace Weblab.Modules.Services;

public class JwtConfigService : IJwtConfig
{
    private JwtConfiguration _configuration;
    public JwtConfigService(IOptions<JwtConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }
    public string Issuer => new string(_configuration.Issuer);

    public string Audience => new string(_configuration.Audience);

    public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

    public string Algorithm => new string(_configuration.Algorithm);
}