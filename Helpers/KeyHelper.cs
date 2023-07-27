// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: KeyHelper.cs
* -------------------------------------------------------------------------------------------
* All of the functions that deal with keys are in this file. This includes generating keys,
* loading keys from the disk, saving keys to the disk, encoding keys, decoding keys, and
* converting keys to byte arrays.
*********************************************************************************************/

using Messenger.Models;

namespace Messenger.Helpers{
    public static class KeyHelper{

        /// <summary>
        /// Generates a RSA public and private key
        /// </summary>
        /// <param name="keySize">The size of the key to generate</param>
        /// <returns>A tuple containing the public and private keys</returns>
        public static Tuple<string, string> GenerateKey(int keySize){
            return null;
        }

        /// <summary>
        /// Loads a key from the disk
        /// </summary>
        /// <param name="filename">The name of the file to load the key from</param>
        /// <returns>The key loaded from the file</returns>
        public static Key LoadKey(string filename){
            return null;
        }

        /// <summary>
        /// Saves a key to the disk
        /// </summary>
        /// <param name="filename">The name of the file to save the key to</param>
        /// <param name="key">The key to save</param>
        public static void SaveKey(string filename, Key key){

        }

        /// <summary>
        /// Converts the key into Base64 encoding
        /// </summary>
        /// <param name="key">The key to encode</param>
        /// <returns>The key encoded in Base64</returns>
        public static string EncodeKey(Key key){
            return null;
        }

        /// <summary>
        /// Decodes a Base64 encoded key into a Key object
        /// </summary>
        /// <param name="key">The Base64 encoded key</param>
        /// <returns>The decoded key</returns>
        public static Key DecodeKey(string key){
            return null;
        }

        /// <summary>
        /// Converts a key into a byte array
        /// </summary>
        /// <param name="key">The key to convert</param>
        /// <returns>The key as a byte array</returns>
        public static byte[] ToByteArray(string key){
            return null;
        }
    }
}
