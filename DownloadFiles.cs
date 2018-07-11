using System;
using System.Collections.Generic;
using System.Text;                                                                      
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace FileManager
{
    partial class FileManagerModel
    {
        public async void LoadFilesClick(string[] names)
        {
            if (curDirectory == null)
                return;

            var cts = new CancellationTokenSource();
            Wait wait = new Wait(cts);
            wait.Show();
            
            Dictionary<string, Task<string>> books = new Dictionary<string, Task<string>>();

            try
            {
                foreach (string name in names)
                {
                    string fileName = "";
                    HttpWebResponse fwr = null;
                    if (!makeFileRequest(name, ref fwr, ref fileName))
                    {
                        wait.Close();
                        return;
                    }
                    books.Add(fileName, readFileAsync(fwr, cts.Token));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
                wait.Close();
                return;
            }

            var progressIndicator = new Progress<int>(wait.ChangeProgress);
            int count = 0, totalCount = books.Count;

            try
            {
                foreach (var book in books)
                {
                    if (EntityFunctions.Exists(curDirectory.GetFullName() + "\\" + book.Key))
                    {
                        MessageBox.Show("This file is already exists!");
                        return;
                    }

                    EntityFileStream fstream = new EntityFileStream(curDirectory.GetFullName() + "\\" + book.Key);
                    string str = await book.Value;
                    ((IProgress<int>)progressIndicator).Report(count * 100 / totalCount);
                    byte[] array = Encoding.Default.GetBytes(str);
                    fstream.Write(array, 0, array.Length);
                    count++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
                wait.Close();
                return;
            }

            wait.Close();
        }

        private bool makeFileRequest(string fileUri, ref HttpWebResponse myFileWebResponse, ref string fileName)
        {
            bool requestOk = false;
            HttpWebRequest myFileWebRequest = null;

            try
            {
                Uri myUrl = new Uri(fileUri);

                myFileWebRequest = (HttpWebRequest)WebRequest.Create(myUrl);
                myFileWebResponse = (HttpWebResponse)myFileWebRequest.GetResponse();
                WebHeaderCollection whc = myFileWebResponse.Headers;
                fileName = whc[1].Split('"')[1];
                requestOk = true;
            }
            catch (UriFormatException ex)
            {
                if (myFileWebResponse != null)
                    myFileWebResponse.Close();
                MessageBox.Show("Input correct uri! " + ex.Message);
            }
            catch (WebException ex)
            {
                if (myFileWebResponse != null)
                    myFileWebResponse.Close();
                MessageBox.Show("Web exception! " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
            }

            return requestOk;
        }

        private string readFile(HttpWebResponse myFileWebResponse, CancellationToken cancellationToken)
        {
            string res = "";

            EntityStreamReader readStream = new EntityStreamReader(myFileWebResponse.GetResponseStream());

            char[] readBuffer = new char[256];
            int count = readStream.Read(readBuffer, 0, 256);

            try
            {
                while (count > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string str = new string(readBuffer, 0, count);
                    res += str;
                    count = readStream.Read(readBuffer, 0, 256);
                }

                readStream.Close();
                myFileWebResponse.Close();
            }
            catch (OperationCanceledException)
            {
                readStream.Close();
                myFileWebResponse.Close();
                return null;
            }
            catch (UriFormatException ex)
            {
                readStream.Close();
                myFileWebResponse.Close();
                MessageBox.Show("Input correct uri! " + ex.Message);
            }
            catch (WebException ex)
            {
                readStream.Close();
                myFileWebResponse.Close();
                MessageBox.Show("Web exception! " + ex.Message);
            }
            catch (Exception ex)
            {
                readStream.Close();
                myFileWebResponse.Close();
                MessageBox.Show("Error! " + ex.Message);
            }

            return res;
        }

        private Task<string> readFileAsync(HttpWebResponse myFileWebResponse, CancellationToken cancellationToken)
        {
            return Task.Run(() => readFile(myFileWebResponse, cancellationToken));
        }
    }
}
