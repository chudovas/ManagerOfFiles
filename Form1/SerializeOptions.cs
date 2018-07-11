using System;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;


namespace FileManager
{
    [Serializable] class SerializeOptions
    {
        public Font font;
        public Image im;
        public string saveFile;
        public Color fontColor;
        public string passToCode;

        public string username, password;

        public SerializeOptions()
        {
            saveFile = "";
            username = "";
            password = "";
        }

        public void DeCoding()
        {
            username = Encoding.Default.GetString(EntityFunctions.Decrypt(Encoding.Default.GetBytes(username), passToCode));
            password = Encoding.Default.GetString(EntityFunctions.Decrypt(Encoding.Default.GetBytes(password), passToCode));
        }

        public void Coding()
        {
            username = Encoding.Default.GetString(EntityFunctions.Encrypt(Encoding.Default.GetBytes(username), passToCode));
            password = Encoding.Default.GetString(EntityFunctions.Encrypt(Encoding.Default.GetBytes(password), passToCode));
        }
    }
}
