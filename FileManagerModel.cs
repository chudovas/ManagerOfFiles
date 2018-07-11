using System;
using System.Collections.Generic;
using Ionic.Zip;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    partial class FileManagerModel
    {
        public IEntity curDirectory;

        public EntityDriveInfo[] drivers;
        public List<IEntity> copyFiles;
        public List<IEntity> oldElements;

        public FileManagerModel()
        {
            drivers = EntityFunctions.GetDrivers();
            curDirectory = null;
            copyFiles = new List<IEntity>();
            oldElements = new List<IEntity>();
        }

        public void GetBack()
        {
            if (curDirectory != null)
                curDirectory = curDirectory.GetParent();
        }

        public void ClickOnDisc(EntityDriveInfo selectedItem)
        {
            curDirectory = new EntityDirect(selectedItem.Name());
        }

        public void ClickOnElement(IEntity element)
        {
            if (element.GetType() == EntityType.FILE || element.GetType() == EntityType.ZIPFILE)
                return;

            curDirectory = element;
        }

        public void EncryptClick(IEntity element)
        {
            IVisitor visit = new EncryptVisit();
            element.Accept(visit);
        }

        public void DecryptClick(IEntity element)
        {
            IVisitor visit = new DecryptVisit();
            element.Accept(visit);
        }

        public void ArchiveClick(List<IEntity> elements)
        {
            using (ZipFile zip = new ZipFile())
            {
                foreach (IEntity el in elements)
                    zip.AddItem(el.GetFullName());

                int n = 0;
                while (EntityFunctions.Exists(curDirectory.GetFullName() + "\\Archive(" + n + ").zip"))
                    n++;
                zip.Save(curDirectory.GetFullName() + "\\Archive(" + n + ").zip");
            }
        }

        public void ExtractArchiveClick(IEntity item)
        {
            if (item.GetType() == EntityType.ZIP)
            {
                using (ZipFile zip1 = ZipFile.Read(item.GetFullName()))
                {
                    foreach (ZipEntry entry in zip1)
                    {
                        entry.Extract(curDirectory.GetFullName(), ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
        }

        public void CreateFileClick(string fileName)
        {
            curDirectory.CreateFile(fileName);
        }

        public void CreateDirClick(string dirName)
        {
            curDirectory.CreateDirect(dirName);
        }

        public void UpdateDirectory()
        {
            if (curDirectory == null)
                return;
            if (curDirectory.GetType() == EntityType.ZIP)
                curDirectory = new EntityZip(curDirectory.GetFullName());
            else if (curDirectory.GetType() == EntityType.ZIPDIRECTORY)
            {
                string path = curDirectory.GetFullName();
                EntityZip parent = ((EntityZipDirect)curDirectory).ZipParent;
                parent = new EntityZip(parent.GetFullName());
                curDirectory = parent.GetItemByPath(path);
            }
        }
    }
}
