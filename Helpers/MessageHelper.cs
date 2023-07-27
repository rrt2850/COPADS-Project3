// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: MessageHelper.cs
* -------------------------------------------------------------------------------------------
* All of the functions related to messages are contained in this file. This includes 
* encrypting messages, decrypting messages, loading messages from the disk, and saving
* messages to the disk.
*********************************************************************************************/

using Messenger.Models;

namespace Messenger.Helpers{
    public static class MessageHelper{

        /// <summary>
        /// Encrypts a plaintext message using the provided public key
        /// </summary>
        /// <param name="plainText">The plaintext message to encrypt</param>
        /// <param name="publicKey">The public key to encrypt the message with</param>
        /// <returns>The Base64 ciphertext of the encrypted message</returns>
        public static string EncryptMessage(string plainText, PublicKey publicKey){
            return null;
        }

        /// <summary>
        /// Decrypts a ciphertext message using the provided private key
        /// </summary>
        /// <param name="cipherText">The ciphertext message to decrypt</param>
        /// <param name="privateKey">The private key to decrypt the message with</param>
        /// <returns>The plaintext message</returns>
        public static string DecryptMessage(string cipherText, PrivateKey privateKey){
            return null;
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