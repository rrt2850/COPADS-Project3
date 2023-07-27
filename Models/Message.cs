// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: Message.cs
* -------------------------------------------------------------------------------------------
* This file contains the Message class, it is used to store messages that are sent between
* users.
*********************************************************************************************/

namespace Messenger.Models{
    public class Message{
        public string Email {get; set;}         // Email address of the sender of the message
        public string CipherText {get; set;}    // Base64 encoded ciphertext of the message

        /// <summary>
        /// Constructor for the Message class
        /// </summary>
        /// <param name="email">The email address of the sender of the message</param>
        /// <param name="cipherText">A base64 encoded ciphertext of the message</param>
        public Message(string email, string cipherText){
            Email = email;
            CipherText = cipherText;
        }
    }
}