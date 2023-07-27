// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: Key.cs
* -------------------------------------------------------------------------------------------
* This file contains the Key class, which is used to store the public and private keys of a
* user (identified by their email address)
*********************************************************************************************/

namespace Messenger.Models{
    public class Key{
        // TODO - make it so that there's only one key? that doesn't make sense to me but the
        //        example JSON only has one key displayed. I'm going to look into this more
        //        later. This is just a draft right now so it's not that serious yet.
        public string Email {get; set;}         // Email address of the owner of the keys
        public string PublicKey {get; set;}     // Base64 encoded public key
        public string PrivateKey {get; set;}    // Base64 encoded private key

        /// <summary>
        /// Constructor for the Key class
        /// </summary>
        /// <param name="email">The email address of the owner of the keys</param>
        /// <param name="publicKey">A base64 encoded public key</param>
        /// <param name="privateKey">A base64 encoded private key</param>
        public Key(string email, string publicKey, string privateKey){
            Email = email;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}