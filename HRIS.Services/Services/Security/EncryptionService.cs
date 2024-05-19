using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HRIS.Services.Services.Security;

public class EncryptionService
{
    // The encryption key used for encryption and decryption (should be kept secret)
    private readonly byte[] _key; // Encryption key
    // The initialization vector (IV) used for encryption (improves security)
    private readonly byte[] _iv; // Initialization vector

    /// <summary>
    /// Initializes a new instance of the EncryptionService class.
    /// </summary>
    /// <param name="key">The encryption key to be used (must be 16, 24, or 32 bytes).</param>
    /// <param name="iv">The initialization vector (IV) to be used (must be 16 bytes).</param>
    /// <exception cref="ArgumentException">Throws an exception if the key or IV lengths are invalid.</exception>
    public EncryptionService(byte[] key, byte[] iv)
    {
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
        {
            throw new ArgumentException("Invalid key size. Key must be 16, 24, or 32 bytes.");
        }

        if (iv.Length != 16)
        {
            throw new ArgumentException("Invalid IV size. IV must be 16 bytes.");
        }
        _key = key;
        _iv = iv;
    }

    /// <summary>
    /// Encrypts a plain text string using the AES algorithm with the configured key and IV.
    /// </summary>
    /// <param name="plainText">The plain text string to be encrypted.</param>
    /// <returns>The encrypted text as a Base64 encoded string.</returns>
    public string Encrypt(string plainText)
    {
        using (var aes = Aes.Create()) // Creates a new AES algorithm instance (disposable)
        {
            aes.Key = _key;
            aes.IV = _iv;
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV); // Creates an encryptor object

            using (var ms = new MemoryStream()) // Creates a memory stream to hold the encrypted data
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) // Encrypts data into the memory stream
                using (var sw = new StreamWriter(cs)) // Writes the plain text to the encrypted stream
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray()); // Converts the encrypted data to a Base64 string
            }
        }
    }

    /// <summary>
    /// Decrypts a cipher text string (Base64 encoded) using the AES algorithm with the configured key and IV.
    /// </summary>
    /// <param name="cipherText">The cipher text string (Base64 encoded) to be decrypted.</param>
    /// <returns>The decrypted plain text string.</returns>
    public string Decrypt(string cipherText)
    {
        using (var aes = Aes.Create()) // Creates a new AES algorithm instance (disposable)
        {
            aes.Key = _key;
            aes.IV = _iv;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV); // Creates a decryptor object

            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText))) // Creates a memory stream from the Base64 encoded data
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) // Decrypts data from the memory stream
                using (var sr = new StreamReader(cs)) // Reads the decrypted data as a string
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
