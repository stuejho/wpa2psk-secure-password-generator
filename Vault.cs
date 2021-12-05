using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace wpa2psk_secure_password_generator
{
    public static class Vault
    {

        // This methodd will encrypt the current plaintext using the given key and produce a ciphertext that 
        // attached to an IV
        //Source: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-6.0
        public static byte[] EncryptStringToBytes_AesCBC(string plainText, byte[] Key)
        {
            byte[] encrypted;
            byte[] iv;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = Key;
                aesAlg.GenerateIV();
                iv = aesAlg.IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            var ivPlusCipher = new byte[iv.Length + encrypted.Length];
            Array.Copy(iv, 0, ivPlusCipher, 0, iv.Length);
            Array.Copy(encrypted, 0, ivPlusCipher, iv.Length, encrypted.Length);
            return ivPlusCipher;
        }

        // This method will decrypt the given iv + ciphertext into plaintext using the provided key
        // //Source: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-6.0
        public static string DecryptStringFromBytes_AesCBC(byte[] ivPlusCipher, byte[] Key)
        {
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = Key;

                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = new byte[ivPlusCipher.Length - iv.Length];

                Array.Copy(ivPlusCipher, iv, iv.Length);
                Array.Copy(ivPlusCipher, iv.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        // This method will generate a key of 16 bytes from a giving password string
        // I am hashing the giving password using sha512 then extracting the last 16 bytes as the key
        public static byte[] GeneratePrivateKey(string password)
        {
            byte[] privateKey = new byte[16];
            var algo = SHA512.Create();
            var temp = algo.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < 16; i++)
                privateKey[i] = temp[i + 47];
            return privateKey;


        }

        // This function will generate a wifi password of length 63
        // Source: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        public static string GenerateWifiPassword()
        {
            var chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int size = 63;
            StringBuilder tempString = new StringBuilder(size);
            byte[] data = new byte[size];
            using (var crypto = RandomNumberGenerator.Create())
                crypto.GetBytes(data);
            foreach (byte b in data)
                tempString.Append(chars[b % (chars.Length)]);
            return tempString.ToString();
        }

        // This function is used to generate wifi passwords of varying lengths is used for benchmarking
        public static string GenerateWifiPassword(int size)
        {
            var chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder tempString = new StringBuilder(size);
            byte[] data = new byte[size];
            using (var crypto = RandomNumberGenerator.Create())
                crypto.GetBytes(data);
            foreach (byte b in data)
                tempString.Append(chars[b % (chars.Length)]);
            return tempString.ToString();
        }
    }
}
