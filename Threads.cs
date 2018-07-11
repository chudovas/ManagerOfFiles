using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FileManager
{
    partial class FileManagerModel
    { 
        public void FindElClick(string find)
        {
            find = find.Replace("?", ".");
            find = find.Replace("*", ".*");

            Wait wait = new Wait();
            Thread.Sleep(100);
            wait.Show();

            List<IEntity> finds = curDirectory.GetItemOfAllFiles().ToList().FindAll((IEntity itemName) =>
            {
                Match m = Regex.Match(itemName.GetName(), find);
                return m.Success;
            });

            ResOfFindData resOf = new ResOfFindData();
            foreach (IEntity file in finds)
                resOf.AddListEl(file.GetName() + " (" + file.GetFullName() + ")");

            wait.Close();
            resOf.CopyOllInListView();
            resOf.Show();
        }

        public void ThreadsFindClick(IEntity selectedItem)
        {
            var cts = new CancellationTokenSource();
            Wait wait = new Wait(cts);
            wait.Show();

            ResOfFindData resOf = new ResOfFindData();
            IParallelFind find = new ThreadsFind();

            List<string> res = find.FindData(EntityFunctions.GetItem(curDirectory, selectedItem).GetItemOfAllFiles(), cts, new Progress<int>(wait.ChangeProgress));
            foreach (var el in res)
                resOf.AddListEl(el);

            wait.Close();
            resOf.CopyOllInListView();
            resOf.Show();
        }

        public void ParallelsForEachFindClick(IEntity selectedItem)
        {
            var cts = new CancellationTokenSource();
            Wait wait = new Wait(cts);
            wait.Show();

            ResOfFindData resOf = new ResOfFindData();
            IParallelFind find = new ParallelForEachFind();
            IEntity item = EntityFunctions.GetItem(curDirectory, selectedItem);

            List<string> res = find.FindData(item.GetItemOfAllFiles(), cts, new Progress<int>(wait.ChangeProgress));
            foreach (var el in res)
                resOf.AddListEl(el);

            wait.Close();
            resOf.CopyOllInListView();
            resOf.Show();
        }

        public void TaskFindClick(IEntity selectedItem)
        {
            ResOfFindData resOf = new ResOfFindData();
            var cts = new CancellationTokenSource();
            Wait wait = new Wait(cts);
            wait.Show();

            IParallelFind find = new TaskFind();
            IEntity item = EntityFunctions.GetItem(curDirectory, selectedItem);

            List<string> res = find.FindData(item.GetItemOfAllFiles(), cts, new Progress<int>(wait.ChangeProgress));
            foreach (var el in res)
                resOf.AddListEl(el);

            wait.Close();
            resOf.CopyOllInListView();
            resOf.Show();
        }

        public async void AsyncAwaitFindClick(IEntity selectedItem)
        {
            var cts = new CancellationTokenSource();

            ResOfFindData resOf = new ResOfFindData();
            Wait wait = new Wait(cts);
            wait.Show();

            Finder find = new Finder(new AsyncAwaitFind());

            List<string> res = await find.Do(curDirectory, selectedItem, cts, new Progress<int>(wait.ChangeProgress));
            foreach (var el in res)
                resOf.AddListEl(el);

            wait.Close();
            resOf.CopyOllInListView();
            resOf.ShowDialog();
        }
    }
}
