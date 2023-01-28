using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public class Utils
    {
        public static bool FormsIsOpen(string name)
        {
            // check if window is already opened
            var OpenForms = Application.OpenForms.Cast<Form>();
            var IsOpen = OpenForms.Any(x => x.Name == name);
            return IsOpen;
        }

        public static string HashPassword(string password)
        {
            SHA256 sha = SHA256.Create();

            // convert the input string to byte  array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            // create a new Stringbuilder to collect the bytes
            // and create a string
            StringBuilder sBuilder = new StringBuilder();

            // loop through each byte of the hashed data
            // and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            //var hashed_password = sBuilder.ToString();

            return sBuilder.ToString();
        }

        public static string DefaultPassword()
        {
            SHA256 sha = SHA256.Create();

            // convert the input string to byte  array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes("pass123"));

            // create a new Stringbuilder to collect the bytes
            // and create a string
            StringBuilder sBuilder = new StringBuilder();

            // loop through each byte of the hashed data
            // and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            //var hashed_password = sBuilder.ToString();

            return sBuilder.ToString();
        }
    }
}
