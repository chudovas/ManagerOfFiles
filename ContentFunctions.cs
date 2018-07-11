using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FileManager
{
    partial class FileManagerModel
    {
        public void DeleteItemsClick(List<IEntity> deleteItems)
        {
            foreach (IEntity el in deleteItems)
                el.Delete();
        }

        public void CopyItemsClick(List<IEntity> copyItems)
        {
            copyFiles = copyItems;
        }

        public void PastItemsClick()
        {
            foreach (var file in copyFiles)
                file.Copy(curDirectory);                             
        }

        public void RenameItemClick(KeyValuePair<IEntity, string> renameData)
        {
            if (EntityFunctions.Exists(curDirectory.GetFullName() + @"\" + renameData.Value))
            {
                MessageBox.Show("This file or directory is already exists!");
                return;
            }

            if (renameData.Key.GetType() == EntityType.DIRECTORY || renameData.Key.GetType() == EntityType.FILE || renameData.Key.GetType() == EntityType.ZIP)
                renameData.Key.Rename(curDirectory.GetFullName() + @"\" + renameData.Value);
            else
                renameData.Key.Rename(renameData.Value);
        }

        public void StatisticItemClick(IEntity selectedItem)
        {
            if (selectedItem != null && EntityFunctions.GetExtension(selectedItem) == ".txt")
            {
                string[] strLines; 
                if (selectedItem.GetType() == EntityType.ZIPFILE)
                    strLines = ((EntityZipFile)selectedItem).GetFileLines().ToArray();
                else
                    strLines = ((EntityFile)selectedItem).GetFileLines().ToArray();

                string str = "";
                foreach (string s in strLines)
                    str += s;

                var list = str.Split(new char[] { ' ', '\n', '\r', '.', ',', '!', '?', '-' }, StringSplitOptions.RemoveEmptyEntries);
                int numOfLines = (from w in str.Split('\n') select w).Count();

                var Counts = (from w in list
                              group w by w into g
                              select new { Name = g.Key, Count = g.Count() }).OrderByDescending(j => j.Count).ToList();

                string strInfo = "Num of words - " + list.Count() + "        " + "Num of lines - " + numOfLines + "       " + "Top 10 words -> ";
                for (int i = 0; i < Math.Min(Counts.Count(), 10); i++)
                {
                    strInfo += Counts[i].Name + " ";
                }
                Info info = new Info(strInfo);
                info.Show();
            }
        }
    }
}

