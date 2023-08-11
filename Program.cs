// Robert Tetreault (rrt2850@g.rit.edu)

/*******************************************************************************************
* File: Program.cs
* -----------------------------------------------------------------------------------------
* This file contains the main entry point for the application and some miscellaneous stuff
* TODO - rewrite this description when the program is finished and I know what
* "miscellaneous stuff" I added
********************************************************************************************/
using Messenger.Models;
using Messenger.Helpers;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Numerics;

class Program{
    private readonly ServerHelper serverHelper;
    private readonly KeyHelper keyHelper;

    public Program(){
        serverHelper = new ServerHelper();
        keyHelper = new KeyHelper();
    }

    bool validOption(string option){
        return option == "keyGen" || option == "getKey" || option == "sendKey" || option == "sendMsg" || option == "getMsg";
    }

    /// <summary>
    /// Handles the command-line arguments and calls the appropriate methods.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public async Task Run(string[] args){

        if (args.Length == 0 || !validOption(args[0])){
            Console.WriteLine("Usage: dotnet run <option> <other arguments>");
            Console.WriteLine("Options:");
            Console.WriteLine("\tkeyGen <keysize>           - Generates a public/private key pair");
            Console.WriteLine("\tgetKey <email>             - Gets the public key for the specified email address from the server");
            Console.WriteLine("\tsendKey <email>            - Sends the public key to the server and sets the email address as a valid receiver");
            Console.WriteLine("\tsendMsg <email> <message>  - Sends the specified message to the specified email address");
            Console.WriteLine("\tgetMsg <email>             - Get a message from the server");
            return;
        }

        string option = args[0];

        switch (option){
            case "keyGen":
                if (args.Length < 2){
                    Console.WriteLine("Usage: dotnet run keyGen <keysize>");
                    return;
                }

                int keySize = Int32.Parse(args[1]);
                keyHelper.GenerateKeys(keySize);
                break;

            case "getKey":
                if (args.Length < 2){
                    Console.WriteLine("Usage: dotnet run getKey <email>");
                    return;
                }

                string email = args[1];
                string response = await serverHelper.GetKey(email);
                Console.WriteLine(response);
                break;

            case "sendKey":
                if (args.Length < 2){
                    Console.WriteLine("Usage: dotnet run sendKey <email>");
                    return;
                }

                email = args[1];
                response = await serverHelper.SendKey(email);
                Console.WriteLine(response);
                break;
            case "sendMsg":
                if (args.Length < 3){
                    Console.WriteLine("Usage: dotnet run sendMsg <email> <message>");
                    return;
                }

                email = args[1];
                string message = args[2];
                response = await serverHelper.SendMessage(email, message);
                Console.WriteLine(response);
                break;
            case "getMsg":
                if (args.Length < 2){
                    Console.WriteLine("Usage: dotnet run getMsg <email>");
                    return;
                }

                email = args[1];
                response = await serverHelper.GetMessage(email);
                Console.WriteLine(response);
                break;
        }
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    static async Task Main(string[] args){
        try{
            await new Program().Run(args);
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
        }
    }
}
