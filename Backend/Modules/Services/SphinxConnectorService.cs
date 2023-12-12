using MySqlConnector;
using Weblab.Architecture.Configurations;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Interfaces;

namespace Weblab.Modules.Services;

public class SphinxConnectorService : ISphinxConnector
{
    private MySqlConnection _connection;
    public SphinxConnectorService(IConfiguration configuration)
    {
        var connectionSettings = configuration.GetSection(SettingsNames.SphinxSettings).Get<SphinxConfiguration>();
        if(connectionSettings != null)
        {
            var connectionString = $"Server={connectionSettings.Server};Port={connectionSettings.Port};"
            + $"Character Set={connectionSettings.CharacterSet};ConnectionReset=false";
            _connection = new MySqlConnection(connectionString);
        }
        else throw new Exception("Sphinx settings is null");
    }

    public List<object[]> GetData(string command)
    {
        var result = new List<object[]>();
        _connection.Open();
        using var mySqlCommand = new MySqlCommand(command, _connection);
        using var reader = mySqlCommand.ExecuteReader();
        while(reader.Read())
        {
            var tempEntry = new object[reader.FieldCount];
            reader.GetValues(tempEntry);
            result.Add(tempEntry);
        }
        _connection.Close();
        return result;
    }
}