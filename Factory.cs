using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Ionic.Zip;


namespace FileManager
{
    class Factory
    {
        private static IEnumerable<EntityFile> GetFiles(EntityDirect dir)
        {
            return from item in dir.dirInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
                   where (Path.GetExtension(item.FullName) != ".zip" && Path.GetExtension(item.FullName) != ".rar")
                   select new EntityFile(item);
        }

        private static IEnumerable<EntityDirect> GetDirectories(EntityDirect dir)
        {
            return from item in dir.dirInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                   select new EntityDirect(item);
        }

        private static IEnumerable<EntityZip> GetZipFiles(EntityDirect dir)
        {
            return from item in dir.dirInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
                   where (Path.GetExtension(item.FullName) == ".zip" || Path.GetExtension(item.FullName) == ".rar")
                   select new EntityZip(item.FullName);
        }

        public static List<IEntity> GetItems(IEntity item)
        {
            switch (item.GetType())
            {
                case EntityType.DIRECTORY:
                    List<IEntity> res = new List<IEntity>();
                    res.AddRange(GetFiles((EntityDirect)item));
                    res.AddRange(GetDirectories((EntityDirect)item));
                    res.AddRange(GetZipFiles((EntityDirect)item));
                    return res;

                case EntityType.ZIPDIRECTORY:
                    return ((EntityZipDirect)item).items;

                case EntityType.ZIP:
                    return ((EntityZip)item).items;

                default:
                    List<IEntity> l = new List<IEntity>();
                    l.Add(item);
                    return l; 
            }
        }
    }
}
