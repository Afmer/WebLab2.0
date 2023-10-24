using System.Security.Cryptography;
using System.Text;
using Weblab.Architecture.Interfaces;

namespace Weblab.Modules.Services;

public class HashService : IHash
{
    private int GenerateSaltForPassword()
    {
        byte[] saltBytes = new byte[4];
        Random.Shared.NextBytes(saltBytes);
        return (((int)saltBytes[0]) << 24) + (((int)saltBytes[1]) << 16) + (((int)saltBytes[2]) << 8) + ((int)saltBytes[3]);
    }
    private string ComputePasswordHash(string password, int salt)
    {
        byte[] saltBytes = new byte[4];
        saltBytes[0] = (byte)(salt >> 24);
        saltBytes[1] = (byte)(salt >> 16);
        saltBytes[2] = (byte)(salt >> 8);
        saltBytes[3] = (byte)(salt);

        byte[] passwordBytes = UTF8Encoding.UTF8.GetBytes(password);

        byte[] preHashed = new byte[saltBytes.Length + passwordBytes.Length];
        System.Buffer.BlockCopy(passwordBytes, 0, preHashed, 0, passwordBytes.Length);
        System.Buffer.BlockCopy(saltBytes, 0, preHashed, passwordBytes.Length, saltBytes.Length);

        SHA1 sha1 = SHA1.Create();
        return BitConverter.ToString(sha1.ComputeHash(preHashed)).Replace("-", "").ToLower();
    }
    public (string Hash, int Salt) GeneratePasswordHash(string password)
    {
        int salt = GenerateSaltForPassword();
        string hash = ComputePasswordHash(password, salt);
        return (hash, salt);
    }
    public bool IsPasswordValid(string passwordToValidate, int salt, string correctPasswordHash)
    {
        byte[] hashedPassword = UTF8Encoding.UTF8.GetBytes(ComputePasswordHash(passwordToValidate, salt));

        return hashedPassword.SequenceEqual(UTF8Encoding.UTF8.GetBytes(correctPasswordHash));
    }
}