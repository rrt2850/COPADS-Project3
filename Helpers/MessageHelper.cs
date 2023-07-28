// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: MessageHelper.cs
* -------------------------------------------------------------------------------------------
* All of the functions related to messages are contained in this file. This includes 
* encrypting messages, decrypting messages, loading messages from the disk, and saving
* messages to the disk.
*********************************************************************************************/

using Messenger.Models;
using System;
using System.Numerics;
using System.Text;
using System.Security.Cryptography;

namespace Messenger.Helpers{
    public static class MessageHelper{

        /// <summary>
        /// Encrypts a plaintext message using the provided public key
        /// </summary>
        /// <param name="publicKey">The public key to encrypt the message with</param>
        /// <param name="plainText">The plaintext message to encrypt</param>
        /// <returns>The Base64 ciphertext of the encrypted message</returns>
        public static string? EncryptMessage(PublicKey publicKey, string plaintext){
            try{
                // Make sure the public key is not null
                if (publicKey == null){
                    throw new ArgumentNullException("publicKey", "Public key must not be null.");
                }
                
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);          // Convert the plaintext message to a byte array
                BigInteger plaintextBigInteger = new BigInteger(plaintextBytes);    // Load the byte array into a big integer

                // Get the RSA parameters from the public key
                // Item1 = E, Item2 = N
                Tuple<BigInteger, BigInteger> rsaParameters = KeyHelper.DeconstructKey(publicKey.key);    

                BigInteger E = rsaParameters.Item1; // The public exponent
                BigInteger N = rsaParameters.Item2; // The modulus
                BigInteger ciphertextBigInteger = BigInteger.ModPow(plaintextBigInteger, E, N); // Encrypt the message
                
                byte[] ciphertextByteArray = ciphertextBigInteger.ToByteArray();        //  Convert the results big integer to a byte array
                string base64Ciphertext = Convert.ToBase64String(ciphertextByteArray);  //  Base64 encode the byte array

                return base64Ciphertext;
            }
            catch (ArgumentNullException ane){
                Console.WriteLine($"Argument Null Exception: {ane.Message}");
                return null;
            }
            catch (ArgumentException ae){
                Console.WriteLine($"Argument Exception: {ae.Message}");
                return null;
            }
            catch (CryptographicException ce){
                Console.WriteLine($"Cryptographic Exception: {ce.Message}");
                return null;
            }
        }


        /// <summary>
        /// Decrypts a ciphertext message using the provided private key
        /// </summary>
        /// <param name="ciphertext">The ciphertext message to decrypt</param>
        /// <param name="privateKey">The private key to decrypt the message with</param>
        /// <returns>The plaintext message</returns>
        public static string? DecryptMessage(string ciphertext, PrivateKey privateKey){
            try{
                // Make sure the private key is not null
                if (privateKey == null){
                    throw new ArgumentNullException("publicKey", "Public key must not be null.");
                }
                
                byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);          // Convert the ciphertext message to a byte array
                BigInteger ciphertextBigInteger = new BigInteger(ciphertextBytes);      // Load the byte array into a big integer

                // Get the RSA parameters from the public key
                // Item1 = E, Item2 = N
                Tuple<BigInteger, BigInteger> rsaParameters = KeyHelper.DeconstructKey(privateKey.key);    

                BigInteger D = rsaParameters.Item1; // The private exponent
                BigInteger N = rsaParameters.Item2; // The modulus
                BigInteger plaintextBigInteger = BigInteger.ModPow(ciphertextBigInteger, D, N); // Encrypt the message
                
                byte[] plaintextByteArray = plaintextBigInteger.ToByteArray();        //  Convert the results big integer to a byte array
                string result = Encoding.UTF8.GetString(plaintextByteArray);          //  Convert the byte array to a string

                return result;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine($"Argument Null Exception: {ane.Message}");
                return null;
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"Argument Exception: {ae.Message}");
                return null;
            }
            catch (CryptographicException ce)
            {
                Console.WriteLine($"Cryptographic Exception: {ce.Message}");
                return null;
            }
        }

        /// <summary>
        /// Loads a message from the disk
        /// </summary>
        /// <param name="filename">The name of the file to load the message from</param>
        /// <returns>The message loaded from the file</returns>
        public static Message LoadMessage(string filename){
            return null;
        }

        /// <summary>
        /// Saves a message to the disk
        /// </summary>
        /// <param name="filename">The name of the file to save the message to</param>
        /// <param name="message">The message to save</param>
        public static void SaveMessage(string filename, Message message){
            return;
        }
    }
}