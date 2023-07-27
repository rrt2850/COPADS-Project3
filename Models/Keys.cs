// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: Key.cs
* -------------------------------------------------------------------------------------------
* This file contains the Key class, which is used to store the public and private keys
*********************************************************************************************/

namespace Messenger.Models{
    public class PrivateKey{
        public List<string> email {get; set;}   // Email addresses of valid recievers
        public string key {get; set;}           // Base64 encoded private key  

        /// <summary>
        /// Constructor for the PrivateKey class
        /// </summary>
        /// <param name="email">A list of emails</param>
        /// <param name="key">The private key</param>
        public PrivateKey(List<string> email, string key){
            this.email = email;
            this.key = key;
        }
        
        /// <summary>
        /// Constructor for the PrivateKey class
        /// </summary>
        /// <param name="email">A single email</param>
        /// <param name="key">The private key</param>
        public PrivateKey(string email, string key){
            this.email = new List<string>();
            this.email.Add(email);
            this.key = key;
        }

        /// <summary>
        /// Constructor for the PrivateKey class
        /// </summary>
        /// <param name="key">The private key</param>
        public PrivateKey(string key){
            this.email = new List<string>();
            this.key = key;
        }

        /// <summary>
        /// Constructor for the PrivateKey class
        /// Creates an empty PrivateKey object
        /// </summary>
        public PrivateKey(){
            this.email = new List<string>();
            this.key = "";
        }
    }

    public class PublicKey{
        public string email {get; set;}         // Email address of the user
        public string key {get; set;}           // Base64 encoded public key

        /// <summary>
        /// Constructor for the PublicKey class
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="key">The public key</param>
        public PublicKey(string email, string key){
            this.email = email;
            this.key = key;
        }

        /// <summary>
        /// Constructor for the PublicKey class
        /// </summary>
        /// <param name="key">The public key</param>
        public PublicKey(string key){
            this.email = "";
            this.key = key;
        }

        /// <summary>
        /// Constructor for the PublicKey class
        /// Creates an empty PublicKey object
        /// </summary>
        public PublicKey(){
            this.email = "";
            this.key = "";
        }
    }
}