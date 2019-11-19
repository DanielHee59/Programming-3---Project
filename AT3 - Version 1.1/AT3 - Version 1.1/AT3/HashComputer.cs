using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AT_4
{
    //SHA256CryptoServiceProvider class to generate the hash.
    public class HashComputer
    {
        public string GetPasswordHashAndSalt(string message)
        {
            //Lets us use SHA256 algorithm to generate the hash from this salted password
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = Utility.GetBytes(message);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            //return the hash string to the caller
            return Utility.GetString(resultBytes);
        }
    }
}
