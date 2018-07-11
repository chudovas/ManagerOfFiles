using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.IO;
using Ionic.Zip;

namespace FileManager
{
    public enum EntityType
    {
        FILE, DIRECTORY, ZIP, ZIPDIRECTORY, ZIPFILE
    }

    class EntityFunctions 
    {
        public static bool Exists(string FileOrDir)
        {
            return (File.Exists(FileOrDir) || Directory.Exists(FileOrDir));
        }

        public static bool CopyDir(string FromDir, string ToDir)
        {
            try
            {
                Directory.CreateDirectory(ToDir);
            }
            catch
            {
                return false;
            }

            bool flag = true;
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                try
                {
                    File.Copy(s1, s2);
                }
                catch
                {
                    flag = false;
                }
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                if (CopyDir(s, ToDir + "\\" + Path.GetFileName(s)) == false)
                    flag = false;
            }

            return flag;
        }

        public static bool Copy(IEntity copyFile, IEntity ToDir)
        {
            bool flag = true;

            if (IsFile(copyFile))
            {
                int k = 1;
                while (IsFile(ToDir + @"\" + Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(copyFile.GetFullName())))
                    k++;
                try
                {
                    File.Copy(copyFile.GetFullName(), ToDir + @"\" +
                        Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(copyFile.GetFullName()));
                }
                catch
                {
                    flag = false;
                }
            }
            else
            {
                int k = 1;
                while (Directory.Exists(ToDir + @"\" + Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(copyFile.GetFullName())))
                    k++;

                if (CopyDir(copyFile.GetFullName(), ToDir + @"\" + Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(copyFile.GetFullName())) == false)
                    flag = false;
            }

            return flag;
        }

        public static bool Copy(List<IEntity> FilesOrDir, IEntity ToDir)
        {
            if (FilesOrDir.Count == 0)
                return false;

            bool flag = true;
            foreach (var copyFile in FilesOrDir)
            {
                if (IsFile(copyFile))
                {
                    int k = 1;
                    while (IsFile(ToDir.GetFullName() + @"\" + Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                            Path.GetExtension(copyFile.GetFullName()))) 
                        k++;
                    try
                    {
                        File.Copy(copyFile.GetFullName(), ToDir.GetFullName() + @"\" +
                            Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                            Path.GetExtension(copyFile.GetFullName()));
                    }
                    catch
                    {
                        flag = false;
                    }
                }
                else{
                    int k = 1;
                    while (Directory.Exists(ToDir + @"\" + Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                            Path.GetExtension(copyFile.GetFullName())))
                        k++;

                    if (CopyDir(copyFile.GetFullName(), ToDir + @"\" + Path.GetFileNameWithoutExtension(copyFile.GetFullName()) + "(" + k + ")" +
                            Path.GetExtension(copyFile.GetFullName())) == false)
                        flag = false;
                }
            }

            return flag;
        }

        public static bool IsFile(string fileDirName)
        {
            return File.Exists(fileDirName);
        }

        public static IEntity GetItem(IEntity curDir, IEntity selectDir)
        {
            return selectDir != null ? selectDir : curDir;
        }

        public static bool IsFile(IEntity item)
        {
            return File.Exists(item.GetFullName());
        }

        public static FileStream CreateFileStream(string fileName)
        {
            return new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }

        public static byte[] Encrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        private static void EncryptFile(string fileName, string password)
        {
            byte[] data;
            byte[] encryptData;

            using (FileStream fstream = File.OpenRead(fileName))
            {
                data = new byte[fstream.Length];
                fstream.Read(data, 0, data.Length);
                encryptData = Encrypt(data, password);
            }

            File.Delete(fileName);
            using (FileStream outputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                outputFile.Write(encryptData, 0, encryptData.Length);
            }
        }

        public static byte[] Decrypt(byte[] data, string password)
        {
            CryptoStream cs = InternalDecrypt(data, password);
            BinaryReader br = new BinaryReader(cs);
            return br.ReadBytes(data.Length);
        }

        private static void DecryptFile(string fileName, string password)
        {
            byte[] data;
            byte[] decryptData;

            using (FileStream fstream = File.OpenRead(fileName))
            {
                data = new byte[fstream.Length];
                fstream.Read(data, 0, data.Length);
                decryptData = Decrypt(data, password);
            }

            File.Delete(fileName);
            using (FileStream outputFile = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                outputFile.Write(decryptData, 0, decryptData.Length);
            }
        }

        private static CryptoStream InternalDecrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }

        public static void EncryptEntityFile(EntityFile eFile, string password)
        {
            EncryptFile(eFile.GetFullName(), password);
        }

        public static void DecryptEntityFile(EntityFile eFile, string password)
        {
            DecryptFile(eFile.GetFullName(), password);
        }

        public static EntityDriveInfo[] GetDrivers()
        {
            return (from dr in DriveInfo.GetDrives() select new EntityDriveInfo(dr)).ToArray();
        }

        public static string GetExtension(IEntity ent)
        {
            return Path.GetExtension(ent.GetFullName());
        }
    }

    public class EntityDriveInfo
    {
        DriveInfo di;

        public EntityDriveInfo(DriveInfo newDi)
        {
            di = newDi;
        }

        public string Name()
        {
            return di.Name;
        }

        public override string ToString()
        {
            return di.Name;
        }
    }

    public class EntityStreamReader
    {
        StreamReader st;

        public EntityStreamReader(Stream stream)
        {
            st = new StreamReader(stream);
        }

        public int Read(char[] buff, int index, int count)
        {
            return st.Read(buff, index, count);
        }

        public void Close()
        {
            st.Close();
        }
    }

    public class EntityFileStream
    {
        private FileStream fs;

        public EntityFileStream(string fileName)
        {
            fs = new FileStream(fileName, FileMode.OpenOrCreate);
        }

        public EntityFileStream(FileStream newFs)
        {
            fs = newFs;
        }
                                                                                                
        public void Write(byte[] buff, int index, int count)
        {
            fs.Write(buff, index, count);
        }
    }

    public interface IEntity
    {
        string cryptKey { get; set; }
        List<string> PersData { get; set; }

        string GetName();
        string GetFullName();
        void Accept(IVisitor visitor);

        bool Delete();
        bool Copy(IEntity ToDir);
        bool Rename(string newName);

        EntityType GetType();

        IEntity GetParent();
        IEnumerable<IEntity> GetItems();
        IEnumerable<IEntity> GetItemOfAllFiles();

        void CreateFile(string fileName);
        void CreateDirect(string directoryName);
    }

    public class EntityFile : IEntity
    {
        private FileInfo fileInfo = null;

        public string cryptKey { get; set; }
        public List<string> PersData { get; set; }

        public EntityFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                fileInfo = new FileInfo(fileName);
                return;
            }

            throw new ArgumentException("Error name of file.");
        }

        public EntityFile(FileInfo info)
        {
            fileInfo = info;
        }

        public string GetFullName()
        {
            return fileInfo.FullName;
        }

        public string GetName()
        {
            return fileInfo.Name;
        }

        public string GetExtension()
        {
            return Path.GetExtension(GetFullName());
        }

        public IEnumerable<string> GetFileLines()
        {
            return File.ReadLines(GetFullName());
        }

        public IEnumerable<IEntity> GetItems()
        {
            return Factory.GetItems(this);
        }

        public IEnumerable<IEntity> GetItemOfAllFiles()
        {
            yield return this;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEntity GetParent()
        {
            return new EntityDirect(fileInfo.Directory);
        }

        EntityType IEntity.GetType()
        {
            return EntityType.FILE;
        }

        public bool Delete()
        {
            try
            {
                File.Delete(GetFullName());
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Copy(IEntity ToDir)
        {
            if (ToDir.GetType() == EntityType.DIRECTORY)
            {
                int k = 1;
                while (File.Exists(ToDir.GetFullName() + @"\" + Path.GetFileNameWithoutExtension(GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(GetFullName())))
                    k++;
                try
                {
                    File.Copy(GetFullName(), ToDir.GetFullName() + @"\" +
                        Path.GetFileNameWithoutExtension(GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(GetFullName()));
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIPDIRECTORY)
            {
                ZipEntry e = ((EntityZipDirect)ToDir).ZipParent.AddFileInZip(this);
                e.FileName = ToDir.GetFullName() + '/' + GetName();
                ((EntityZipDirect)ToDir).ZipParent.SaveZip();
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIP)
            {
                ((EntityZip)ToDir).AddFileInZip(this);
                return true;
            }

            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                File.Move(GetFullName(), newName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void CreateDirect(string directoryName)
        {
            throw new NotImplementedException();
        }
    }

    public class EntityDirect : IEntity
    {
        public DirectoryInfo dirInfo = null;

        public string cryptKey { get; set; }
        public List<string> PersData { get; set; }

        public EntityDirect(string dirName)
        {
            if (Directory.Exists(dirName))
            {
                dirInfo = new DirectoryInfo(dirName);
                return;
            }

            throw new ArgumentException("Error name of directory.");
        }

        public EntityDirect(DirectoryInfo info)
        {
            dirInfo = info;
        }

        public string GetFullName()
        {
            return dirInfo.FullName;
        }

        public string GetName()
        {
            return dirInfo.Name;
        }

        public IEnumerable<IEntity> GetItems()
        {
            return Factory.GetItems(this);
        }

        public IEnumerable<IEntity> GetItemOfAllFiles()
        {
            List<IEntity> res = new List<IEntity>();

            var it = GetItems();
            foreach (var el in it)
                res.AddRange(el.GetItemOfAllFiles());

            return res;
        }
     
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEntity GetParent()
        {
            if (dirInfo.Parent == null)
                return this;
            return new EntityDirect(dirInfo.Parent);
        }

        EntityType IEntity.GetType()
        {
            return EntityType.DIRECTORY;
        }

        public bool Delete()
        {
            try
            {
                Directory.Delete(GetFullName(), true);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Copy(IEntity ToDir)
        {
            if (ToDir.GetType() == EntityType.DIRECTORY)
            {
                int k = 1;
                while (Directory.Exists(ToDir + @"\" + Path.GetFileNameWithoutExtension(GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(GetFullName())))
                    k++;

                return EntityFunctions.CopyDir(GetFullName(), ToDir + @"\" + Path.GetFileNameWithoutExtension(GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(GetFullName()));
            }
            else if (ToDir.GetType() == EntityType.ZIPDIRECTORY)
            {
                ZipEntry e = ((EntityZipDirect)ToDir).ZipParent.AddDirectoryInZip(this);
                foreach (var item in ((EntityZipDirect)ToDir).ZipParent.GetItemOfAllFiles())
                    if (item.GetFullName().StartsWith(e.FileName))
                        item.Rename(ToDir.GetFullName() + '/' + item.GetFullName());
                e.FileName = ToDir.GetFullName() + '/' + GetName();
                ((EntityZipDirect)ToDir).ZipParent.SaveZip();
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIP)
            {
                ((EntityZip)ToDir).AddDirectoryInZip(this);
                return true;
            }

            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                Directory.Move(GetFullName(), newName);
                return true;
            }                                       
            catch
            {
                return false;
            }
        }

        public void CreateFile(string fileName)
        {
            File.Create(GetFullName() + "\\" + fileName);
        }

        public void CreateDirect(string directoryName)
        {
            Directory.CreateDirectory(GetFullName() + "\\" + directoryName);
        }
    }

    public class EntityZip : IEntity
    {
        private ZipFile zipFile;
        private IEntity parent;

        public List<IEntity> items;

        public string cryptKey { get; set; }
        public List<string> PersData { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void SaveZip()
        {
            lock (zipFile)
            {
                zipFile.Save();
            }
        }

        public bool DeleteEntity(ZipEntry entry)
        {
            lock (zipFile)
            {
                zipFile.RemoveEntry(entry);
                zipFile.Save();
            }

            return true;
        }

        public ZipEntry AddFileInZip(IEntity addItem)
        {
            ZipEntry e;

            lock (zipFile)
            {
                e = zipFile.AddItem(addItem.GetFullName());
                zipFile.Save();
            }
            return e;
        }

        public ZipEntry AddDirectoryInZip(IEntity addItem)
        {
            ZipEntry e;
            lock (zipFile)
            {
                e = zipFile.AddDirectory(addItem.GetFullName(), addItem.GetFullName().Split('\\').Last());
                zipFile.Save();
            }
            return e;
        }

        public EntityZip(string fullName)
        {
            zipFile = ZipFile.Read(fullName);
            lock (zipFile)
            {
                parent = new EntityDirect(Path.GetDirectoryName(fullName));

                List<ZipEntry> elements = zipFile.ToList();
                elements.Sort((a, b) => a.FileName.CompareTo(b.FileName));

                items = new List<IEntity>();

                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].IsDirectory)
                    {
                        int j = 1;
                        List<ZipEntry> directoryElements = new List<ZipEntry>();
                        while (i + j < elements.Count && elements[i + j].FileName.StartsWith(elements[i].FileName))
                            directoryElements.Add(elements[i + j++]);

                        items.Add(new EntityZipDirect(elements[i], directoryElements, this, this));
                        i = i + j - 1;
                    }
                    else
                    {
                        items.Add(new EntityZipFile(elements[i], this));
                    }
                }
                zipFile.Save();
            }
        }

        public string GetFullName()
        {
            return zipFile.Name;
        }
        
        public IEnumerable<IEntity> GetItems()
        {
            return Factory.GetItems(this);
        }

        public IEnumerable<IEntity> GetItemOfAllFiles()
        {
            List<IEntity> res = new List<IEntity>();

            foreach (var it in items)
                res.AddRange(it.GetItemOfAllFiles());

            return res;
        }

        public string GetName()
        {
            return Path.GetFileName(zipFile.Name);
        }

        public IEntity GetParent()
        {
            return parent;
        }

        EntityType IEntity.GetType()
        {
            return EntityType.ZIP;
        }

        public ZipEntry CreateEmptyFile(string fileName)
        {
            lock (zipFile)
            {
                ZipEntry e = zipFile.AddEntry(fileName, "");
                zipFile.Save();
                return e;
            }
        }

        public ZipEntry CreateEmptyDir(string dirName)
        {
            lock (zipFile)
            {
                Directory.CreateDirectory(dirName);
                var e =  zipFile.AddDirectory(Path.GetFullPath(dirName), dirName);
                Directory.Delete(dirName);
                zipFile.Save();
                return e;
            }
        }

        public bool Delete()
        {
            try
            {
                File.Delete(GetFullName());
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Copy(IEntity ToDir)
        {
            if (ToDir.GetType() == EntityType.DIRECTORY)
            {
                int k = 1;
                while (File.Exists(ToDir.GetFullName() + @"\" + Path.GetFileNameWithoutExtension(GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(GetFullName())))
                    k++;
                try
                {
                    File.Copy(GetFullName(), ToDir.GetFullName() + @"\" +
                        Path.GetFileNameWithoutExtension(GetFullName()) + "(" + k + ")" +
                        Path.GetExtension(GetFullName()));
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIPDIRECTORY)
            {
                ZipEntry e = ((EntityZipDirect)ToDir).ZipParent.AddFileInZip(this);
                e.FileName = ToDir.GetFullName() + '/' + GetName();
                ((EntityZipDirect)ToDir).ZipParent.SaveZip();
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIP)
            {
                ((EntityZip)ToDir).AddFileInZip(this);
                return true;
            }

            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                File.Move(GetFullName(), newName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateFile(string fileName)
        {
            lock (zipFile)
            {
                zipFile.AddEntry(fileName, "");
                zipFile.Save();
            }
        }

        public void CreateDirect(string directoryName)
        {
            lock (zipFile)
            {
                Directory.CreateDirectory(directoryName);
                zipFile.AddDirectory(Path.GetFullPath(directoryName), directoryName);
                Directory.Delete(directoryName);
                zipFile.Save();
            }
        }

        public IEntity GetItemByPath(string path)
        {
            foreach (var item in GetItems())
            {
                if (item.GetFullName() == path)
                    return item;
                if (item.GetType() == EntityType.ZIPDIRECTORY && ((EntityZipDirect)item).GetItemByPath(path) != null)
                    ((EntityZipDirect)item).GetItemByPath(path);
            }
            return null;
        }
    }

    public class EntityZipDirect : IEntity
    {
        ZipEntry entry;
        private IEntity parent;
        public EntityZip ZipParent;

        public List<IEntity> items;

        public string cryptKey { get; set; }
        public List<string> PersData { get; set; }

        public EntityZipDirect(ZipEntry newEntry, List<ZipEntry> newFiles, IEntity newParent, EntityZip newZipParent)
        {
            items = new List<IEntity>();
            parent = newParent;
            entry = newEntry;
            ZipParent = newZipParent;

            for (int i = 0; i < newFiles.Count; i++)
            {
                if (newFiles[i].IsDirectory)
                {
                    int j = 1;
                    List<ZipEntry> directoryElements = new List<ZipEntry>();
                    while (i + j < newFiles.Count && newFiles[i + j].FileName.StartsWith(newFiles[i].FileName))
                        directoryElements.Add(newFiles[i + j++]);

                    items.Add(new EntityZipDirect(newFiles[i], directoryElements, this, ZipParent));
                    i = i + j - 1;
                }
                else
                {
                    items.Add(new EntityZipFile(newFiles[i], ZipParent));
                }
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Extract(string path)
        {
            entry.Extract(path);
            foreach (var el in items)
                if (el.GetType() == EntityType.ZIPDIRECTORY)
                    ((EntityZipDirect)el).Extract(path);
                else
                    ((EntityZipFile)el).Extract(path);
        }

        public bool Delete()
        {
            bool isOk = true;

            foreach (var it in items)
                isOk &= it.Delete();

            isOk &= ZipParent.DeleteEntity(entry);
            return isOk;
        }

        public string GetFullName()
        {
            return entry.FileName;
        }

        public IEnumerable<IEntity> GetItems()
        {
            return Factory.GetItems(this);
        }

        public IEnumerable<IEntity> GetItemOfAllFiles()
        {
            List<IEntity> res = new List<IEntity>();

            foreach (var it in items)
                res.AddRange(it.GetItemOfAllFiles());

            return res;
        }

        public string GetName()
        {
            var a = GetFullName().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return a[a.Length - 1];
        }

        public IEntity GetParent()
        {
            return parent;
        }

        EntityType IEntity.GetType()
        {
            return EntityType.ZIPDIRECTORY;
        }

        public bool Copy(IEntity ToDir)
        {
            if (ToDir.GetType() == EntityType.DIRECTORY)
            {
                Extract(ToDir.GetFullName());
            }
            else if (ToDir.GetType() == EntityType.ZIPDIRECTORY)
            {
                ZipEntry e = ((EntityZipDirect)ToDir).ZipParent.AddFileInZip(this);
                e.FileName = ToDir.GetFullName() + '/' + GetName();
                ((EntityZipDirect)ToDir).ZipParent.SaveZip();
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIP)
            {
                ((EntityZip)ToDir).AddFileInZip(this);
                return true;
            }

            return false;
        }

        public bool RenameFullPath(string newName)
        {
            foreach (var el in items)
            {
                if (el.GetType() == EntityType.ZIPDIRECTORY)
                    ((EntityZipDirect)el).RenameFullPath(newName + '/' + el.GetName());
                else
                    ((EntityZipFile)el).RenameFullPath(newName + '/' + el.GetName());
            }
            entry.FileName = newName;
            return true;
        }

        public bool Rename(string newName)
        {
            var a = GetFullName().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var b = a[a.Length - 1].Length + 1;

            string str = GetFullName().Remove(GetFullName().Length - b, b);

            string fullPathRenames;
            if (str == "")
                fullPathRenames = newName;
            else
                fullPathRenames = str + '/' + newName;

            foreach (var el in items)
            {
                if (el.GetType() == EntityType.ZIPDIRECTORY)
                    ((EntityZipDirect)el).RenameFullPath(newName + '/' + el.GetName());
                else
                    ((EntityZipFile)el).RenameFullPath(newName + '/' + el.GetName());
            }

            entry.FileName = fullPathRenames;
            ZipParent.SaveZip();
            return true;
        }

        public void CreateFile(string fileName)
        {
            ZipEntry entry = ZipParent.CreateEmptyFile(fileName);
            entry.FileName = GetFullName() + "/" + fileName;
            ZipParent.SaveZip();
        }

        public void CreateDirect(string directoryName)
        {
            ZipEntry entry = ZipParent.CreateEmptyDir(directoryName);
            entry.FileName = GetFullName() + "/" + directoryName;
            ZipParent.SaveZip();
        }

        public IEntity GetItemByPath(string path)
        {
            foreach (var item in GetItems())
            {
                if (item.GetFullName() == path)
                    return item;
                if (item.GetType() == EntityType.ZIPDIRECTORY && ((EntityZipDirect)item).GetItemByPath(path) != null)
                    ((EntityZipDirect)item).GetItemByPath(path);
            }
            return null;
        }
    }

    public class EntityZipFile : IEntity
    {
        ZipEntry entry;
        public EntityZip ZipParent;

        public string cryptKey { get; set; }
        public List<string> PersData { get; set; }

        public EntityZipFile(ZipEntry newEntry, EntityZip newZipParent)
        {
            entry = newEntry;
            ZipParent = newZipParent;
        }

        public bool Delete()
        {
            return ZipParent.DeleteEntity(entry);
        }

        public void Extract(string path)
        {
            entry.Extract(path);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetFullName()
        {
            return entry.FileName;
        }

        public IEnumerable<IEntity> GetItems()
        {
            return Factory.GetItems(this);
        }

        public IEnumerable<IEntity> GetItemOfAllFiles()
        {
            yield return this;
        }

        public string GetName()
        {
            return Path.GetFileName(entry.FileName);
        }

        public IEnumerable<string> GetFileLines()
        {
            MemoryStream ms = new MemoryStream();

            lock (ZipParent)
                entry.Extract(ms);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            var res = text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return res;
        }

        public IEntity GetParent()
        {
            return null;
        }

        EntityType IEntity.GetType()
        {
            return EntityType.ZIPFILE;
        }

        public bool Copy(IEntity ToDir)
        {
            if (ToDir.GetType() == EntityType.DIRECTORY)
            {
                entry.Extract(ToDir.GetFullName());
            }
            else if (ToDir.GetType() == EntityType.ZIPDIRECTORY)
            {
                ZipEntry e = ((EntityZipDirect)ToDir).ZipParent.AddFileInZip(this);
                e.FileName = ToDir.GetFullName() + '/' + GetName();
                ((EntityZipDirect)ToDir).ZipParent.SaveZip();
                return true;
            }
            else if (ToDir.GetType() == EntityType.ZIP)
            {
                ((EntityZip)ToDir).AddFileInZip(this);
                return true;
            }

            return false;
        }

        public bool RenameFullPath(string newName)
        {
            entry.FileName = newName;
            return true;
        }

        public bool Rename(string newName)
        {
            entry.FileName = Path.GetDirectoryName(entry.FileName) + @"\" + newName;
            ZipParent.SaveZip();
            return true;
        }

        public void CreateFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void CreateDirect(string directoryName)
        {
            throw new NotImplementedException();
        }
    }
}
