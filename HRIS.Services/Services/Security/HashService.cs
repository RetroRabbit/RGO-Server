using Org.BouncyCastle.Tls;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HRIS.Services.Services.Security;

public class HashService
{
    private readonly SaltService _saltService;

    public HashService(SaltService saltService)
    {
        _saltService = saltService;
    }

    //Method to hash ID number with salt
    public string HashIdNumber(string idNumber)
    {
        // Generate a random salt using the injected SaltService
        var salt = _saltService.GenerateSalt();

        using (var sha256 = SHA256.Create()) // Create a new SHA256 algorithm instance (disposable)
        {
            // Combine the salt with the byte representation of the ID number
            var saltedIdNumber = Combine(salt, Encoding.UTF8.GetBytes(idNumber));

            // Hash the combined data using SHA256
            var hashedBytes = sha256.ComputeHash(saltedIdNumber);

            // Combine the salt again with the hashed bytes for storage
            var saltPlusHash = Combine(salt, hashedBytes);

            // Convert the combined data (salt + hash) to Base64 string for storage
            return Convert.ToBase64String(saltPlusHash);
        }
    }

    //Method to verify Id Number with hashed value
    public bool VerifyIdNumber(string idNumber, string hashIdNumber)
    {
        // Decode the Base64 string representation of the hashed data (salt + hash)
        var saltPlusHash = Convert.FromBase64String(hashIdNumber);

        // Extract the salt from the combined data
        var salt = new byte[16];
        Array.Copy(saltPlusHash, 0, salt, 0, 16);

        // Extract the hashed bytes from the combined data
        var hash = new byte[saltPlusHash.Length - 16];
        Array.Copy(saltPlusHash, 16, hash, 0, hash.Length);

        using (var sha256 = SHA256.Create()) // Create a new SHA256 algorithm instance (disposable)
        {
            // Combine the extracted salt with the ID number for verification
            var saltedIdNumber = Combine(salt, Encoding.UTF8.GetBytes(idNumber));

            // Compute the hash of the combined data (salt + ID number)
            var computedHash = sha256.ComputeHash(saltedIdNumber);

            // Compare the computed hash with the extracted hash for verification
            return CompareByteArrays(hash, computedHash);
        }
    }

    //Helper method to combine byte arrays
    private byte[] Combine(byte[] first, byte[] second)
    {
        // Allocate a new byte array to hold the combined data
        var result = new byte[first.Length + second.Length];

        // Copy the first byte array into the combined result
        Buffer.BlockCopy(first, 0, result, 0, first.Length);

        // Copy the second byte array into the combined result, starting from the end of the first
        Buffer.BlockCopy(second, 0, result, first.Length, second.Length);

        // Return the combined byte array
        return result;
    }

    //Helper method to compare byte arrays
    private bool CompareByteArrays(byte[] array1, byte[] array2) 
    {
        // Check if the lengths of the arrays are equal (fast check)
        if (array1.Length != array2.Length)
        {
            return false;
        }

        // Loop through each byte and compare them
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                return false; // Mismatch found, not equal
            }
        }

        // If the loop completes without finding a mismatch, the arrays are equal
        return true;
    }
}
