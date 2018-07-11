using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace FileManager
{
    public interface IParallelFind
    {
        List<string> FindData(IEnumerable<IEntity> fileNames, 
            CancellationTokenSource cts, IProgress<int> progressChanger);
    }

    public interface IParallelFindAsync
    {
        Task<List<string>> FindDataAsync(IEnumerable<IEntity> fileNames,
            CancellationTokenSource cts, IProgress<int> progressChanger);
    }

    class ThreadsFind : IParallelFind
    {
        public class ThreadFindData
        {
            private List<IEntity> nameOfFiles;
            public HashSet<string> res;
            private Thread thread;
            public Thread Thread { get { return thread; } }

            public ThreadFindData(List<IEntity> namesF, CancellationTokenSource cts = null)
            {
                nameOfFiles = namesF;
                thread = new Thread(new ThreadStart(delegate() { ThreadFunc(cts); }));
                res = new HashSet<string>();
            }

            public void Begin()
            {
                thread.Start();
            }

            public void End()
            {
                thread.Join();
            }

            private void ThreadFunc(CancellationTokenSource cts)
            {
                foreach (var file in nameOfFiles)
                {
                    IVisitor findPersData = new FindPersDataVisit();
                    file.Accept(findPersData);

                    foreach (string el in file.PersData)
                        res.Add(el);
                }
            }
        }

        public List<string> FindData(IEnumerable<IEntity> fileNames,
            CancellationTokenSource cts = null, IProgress<int> progressChanger = null)
        {
            List<string> res = new List<string>();

            try
            {
                ThreadFindData[] threads = new ThreadFindData[Environment.ProcessorCount];

                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    List<IEntity> str = new List<IEntity>();
                    for (int j = i * fileNames.Count() / Environment.ProcessorCount; j < (i + 1) * fileNames.Count() / Environment.ProcessorCount; j++)
                        str.Add(fileNames.ToArray()[j]);
                    threads[i] = new ThreadFindData(str);
                    threads[i].Begin();
                    if (cts != null)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        progressChanger.Report(i * 100 / Environment.ProcessorCount);
                    }
                }

                for (int i = 0; i < Environment.ProcessorCount; i++)
                    threads[i].End();

                for (int i = 0; i < Environment.ProcessorCount; i++)
                    foreach (string str in threads[i].res)
                        res.Add(str);
            }
            catch (OperationCanceledException)
            {
                return new List<string>();
            }
            
            return res;
        }
    }

    class ParallelForEachFind : IParallelFind
    {
        public List<string> FindData(IEnumerable<IEntity> fileNames,
            CancellationTokenSource cts = null, IProgress<int> progressChanger = null)
        {
            List<string> res = new List<string>();
            Object lockThis = new Object();
            
            try
            {
                Parallel.ForEach(fileNames, file =>
                {
                    IVisitor findPersData = new FindPersDataVisit();
                    file.Accept(findPersData);

                    lock (lockThis)
                    {
                        foreach (var el in file.PersData)
                            res.Add(el);
                    }
                    if (cts != null)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        progressChanger.Report(50);
                    }
                });
            }
            catch (OperationCanceledException)
            {
                return new List<string>();
            }
            

            return res;
        }
    }

    class TaskFind : IParallelFind
    {
        public List<string> FindData(IEnumerable<IEntity> fileNames,
            CancellationTokenSource cts = null, IProgress<int> progressChanger = null)
        {
            List<string> res = new List<string>();
            try
            {
                List<Task<List<string>>> tasks = new List<Task<List<string>>>();

                int count = 0, totalCount = fileNames.Count();
                foreach (var file in fileNames)
                {
                    Task<List<string>> findTask = Task.Run(() => {
                        IVisitor findPersData = new FindPersDataVisit();
                        file.Accept(findPersData);
                        return file.PersData;
                    });
                    tasks.Add(findTask);
                }

                foreach (var t in tasks)
                {
                    List<string> result = t.Result;
                    cts.Token.ThrowIfCancellationRequested();
                    progressChanger.Report(count++ * 100 / totalCount);
                    foreach (string str in result)
                        res.Add(str);
                }
            }
            catch (OperationCanceledException)
            {
                return new List<string>();
            }
            
            return res;
        }
    }

    class AsyncAwaitFind : IParallelFindAsync
    {
        private Task<List<string>> findPersonDataAsync(IEntity file, CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                IVisitor findPersData = new FindPersDataVisit();
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();
                file.Accept(findPersData);
                return file.PersData;
            });
        }

        public async Task<List<string>> FindDataAsync(IEnumerable<IEntity> fileNames, CancellationTokenSource cts, 
            IProgress<int> progressChanger)
        {
            List<Task<List<string>>> resultsTask = new List<Task<List<string>>>();
            List<string> results = new List<string>();

            try
            {
                foreach (var file in fileNames)
                    resultsTask.Add(findPersonDataAsync(file, cts.Token));
            }
            catch (ArgumentNullException)
            {
            }

            try
            {
                int count = 0, totalCount = resultsTask.Count;

                foreach (var res in resultsTask)
                {
                    results.AddRange(await res);
                    cts.Token.ThrowIfCancellationRequested();
                    progressChanger.Report(count++ * 100 / totalCount);
                }
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
            }

            return results;            
        }
    }

    public class Finder
    {
        private IParallelFind find;
        private IParallelFindAsync asyncFind;

        public Finder(IParallelFind strategy)
        {
            find = strategy;
            asyncFind = null;
        }

        public Finder(IParallelFindAsync strategy)
        {
            find = null;
            asyncFind = strategy;
        }

        public void SetStrategy(IParallelFind strategy)
        {
            find = strategy;
            asyncFind = null;
        }

        public void SetStrategy(IParallelFindAsync strategy)
        {
            find = null;
            asyncFind = strategy;
        }

        public async Task<List<string>> Do(IEntity curDirect, IEntity selectedItem, 
            CancellationTokenSource cts = null, Progress<int> progressChanger = null)
        {
            IEntity item = EntityFunctions.GetItem(curDirect, selectedItem);
            List<string> result;

            if (find == null)
                result = await asyncFind.FindDataAsync(item.GetItemOfAllFiles(), cts, progressChanger);
            else
                result = find.FindData(item.GetItemOfAllFiles(), cts, progressChanger);

            return result.Distinct().ToList();
        }
    }
}
