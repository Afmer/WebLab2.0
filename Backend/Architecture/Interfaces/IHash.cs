namespace Weblab.Architecture.Interfaces;

public interface IHash
{
    public (string Hash, int Salt) GeneratePasswordHash(string password);
    public bool IsPasswordValid(string passwordToValidate, int salt, string correctPasswordHash);
}