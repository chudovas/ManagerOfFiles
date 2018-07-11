using System.Runtime.Serialization.Formatters.Binary;

namespace FileManager
{
    partial class Form1
    {
        SerializeOptions so;

        private void SaveOptions()
        {
            so.Coding();
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(EntityFunctions.CreateFileStream(so.saveFile), so);
            so.DeCoding();
        }

        private void LoadOptions()
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            so = (SerializeOptions)binFormatter.Deserialize(EntityFunctions.CreateFileStream(so.saveFile));

            so.DeCoding();
            ChangeOptions();
        }

        private void ChangeOptions()
        {
            listBoxOfDisks.Font = so.font;
            listViewOfFiles.Font = so.font;
            textBox1.Font = so.font;
            textBox2.Font = so.font;

            listBoxOfDisks.ForeColor = so.fontColor;
            listViewOfFiles.ForeColor = so.fontColor;
            textBox1.ForeColor = so.fontColor;
            textBox2.ForeColor = so.fontColor;

            this.BackgroundImage = so.im;
            userName = so.username;
            password = so.password;

            SaveOptions();
        }
    }
}
