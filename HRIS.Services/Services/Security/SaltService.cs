using System.Security.Cryptography;

namespace HRIS.Services.Services.Security;

public class SaltService
{
    // Method to generate a random salt
    public byte[] GenerateSalt(int size = 16)
    {
        // Creates a byte array to hold the salt with the specified size (default 16 bytes)
        var salt = new byte[size];
        // Uses a 'using' statement for proper resource management
        using (var rng = RandomNumberGenerator.Create())
        {
            // Generates cryptographically secure random bytes and fills the salt array
            rng.GetBytes(salt);
        }
        // Returns the generated salt (byte array)
        return salt;
    }
}