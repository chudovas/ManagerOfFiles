using System;                  
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace FileManager
{
    public interface IVisitor
    {
        void Visit(EntityFile info);
        void Visit(EntityDirect info);
        void Visit(EntityZip info);
        void Visit(EntityZipFile info);
        void Visit(EntityZipDirect info);
    }

    public class FindPersDataVisit : IVisitor
    {
        static private string[] PersData = new string[]
        {
            @"[.\-_a-z0-9]+@([a-z0-9][\-a-z0-9]+\.)+[a-z]{2,6}",            // email
            @"(\+7|8)(-|\s)?\d{3}(-|\s)?\d{3}(\s|-)?\d{2}(-|\s)?\d{2}",     // phone
            @"\d{4}\s\d{6}",                                                // pasport
            @"\d?\d?\d\.\d?\d?\d\.\d?\d?\d\.\d?\d?\d",                      // IP
        };

        public void Visit(EntityFile info)
        {
            info.PersData = new List<string>();

            try
            {
                IEnumerable<string> strings = info.GetFileLines();
                foreach (string str in strings)
                    foreach (var reg in PersData)
                    {
                        MatchCollection mc = Regex.Matches(str, reg);
                        foreach (Match m in mc)
                            info.PersData.Add(m.Value + " in " + info.GetFullName() + " file.");
                    }
            }
            catch (Exception)
            {
            }
        }

        public void Visit(EntityDirect info)
        {
            return;
        }

        public void Visit(EntityZip info)
        {
            return;
        }

        public void Visit(EntityZipFile info)
        {
            info.PersData = new List<string>();

            try
            {
                IEnumerable<string> strings = info.GetFileLines();
                foreach (string str in strings)
                    foreach (var reg in PersData)
                    {
                        MatchCollection mc = Regex.Matches(str, reg);
                        foreach (Match m in mc)
                            info.PersData.Add(m.Value + " in " + info.GetFullName() + " file.");
                    }
            }
            catch (Exception)
            {
            }
        }

        public void Visit(EntityZipDirect info)
        {
            return;
        }
    }

    public class EncryptVisit : IVisitor
    {
        public void Visit(IEntity entity)
        {
            switch (entity.GetType())
            {
                case EntityType.DIRECTORY:
                    Visit((EntityDirect)entity);
                    break;

                case EntityType.FILE:
                    Visit((EntityFile)entity);
                    break;

                case EntityType.ZIP:
                    Visit((EntityZip)entity);
                    break;

                case EntityType.ZIPDIRECTORY:
                    Visit((EntityZipDirect)entity);
                    break;

                case EntityType.ZIPFILE:
                    Visit((EntityZipFile)entity);
                    break;
            }
        }

        public void Visit(EntityFile info)
        {
            EntityFunctions.EncryptEntityFile(info, info.cryptKey);
        }

        public void Visit(EntityDirect info)
        {
            List<IEntity> allFiles = info.GetItems().ToList();
            foreach (IEntity it in allFiles)
                Visit(it);
        }                   

        public void Visit(EntityZip info)
        {
            throw new NotImplementedException();
        }

        public void Visit(EntityZipFile info)
        {
            throw new NotImplementedException();
        }

        public void Visit(EntityZipDirect info)
        {
            throw new NotImplementedException();
        }
    }

    public class DecryptVisit : IVisitor
    {
        public void Visit(IEntity entity)
        {
            switch (entity.GetType())
            {
                case EntityType.DIRECTORY:
                    Visit((EntityDirect)entity);
                    break;

                case EntityType.FILE:
                    Visit((EntityFile)entity);
                    break;

                case EntityType.ZIP:
                    Visit((EntityZip)entity);
                    break;

                case EntityType.ZIPDIRECTORY:
                    Visit((EntityZipDirect)entity);
                    break;

                case EntityType.ZIPFILE:
                    Visit((EntityZipFile)entity);
                    break;
            }
        }

        public void Visit(EntityFile info)
        {
            EntityFunctions.DecryptEntityFile(info, info.cryptKey);
        }

        public void Visit(EntityDirect info)
        {
            List<IEntity> allFiles = info.GetItems().ToList();
            foreach (IEntity it in allFiles)
                Visit(it);
        }

        public void Visit(EntityZip info)
        {
            throw new NotImplementedException();
        }

        public void Visit(EntityZipFile info)
        {
            throw new NotImplementedException();
        }

        public void Visit(EntityZipDirect info)
        {
            throw new NotImplementedException();
        }
    }
}
