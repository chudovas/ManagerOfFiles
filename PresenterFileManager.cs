using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;

namespace FileManager
{
    class PresenterFileManager
    {
        private IView view;
        private FileManagerModel model;
        private Timer timer;

        public PresenterFileManager(IView newView)
        {
            view = newView;
            model = new FileManagerModel();

            timer = new Timer(1500);
            timer.Elapsed += (sender, e) => UpdateView();
            timer.Start();

            view.GetBackClick += new EventHandler(GetBack);
            view.DiskClick += new EventHandler<EntityDriveInfo>(DiskClick);
            view.ElementClick += new EventHandler<IEntity>(ElementClick);

            view.EncryptElementClick += new EventHandler<IEntity>(EncryptClick);
            view.DecryptElementClick += new EventHandler<IEntity>(DecryptClick);
            view.ArchiveElementClick += new EventHandler<List<IEntity>>(ArchiveFile);
            view.ExtractArchiveElementClick += new EventHandler<IEntity>(ExtractArchiveFile);

            view.LoadElementClick += new EventHandler<string[]>(LoadFiles);

            view.FindPersDataForEachClick += new EventHandler<IEntity>(ParallelForEachFindClick);
            view.FindPersDataAsicnClick += new EventHandler<IEntity>(AsyncAwaitFindClick);
            view.FindPersDataTaskClick += new EventHandler<IEntity>(TaskFindClick);
            view.FindPersDataThreadClick += new EventHandler<IEntity>(ThreadFindClick);

            view.FindElementClick += new EventHandler<string>(FindElClick);
            view.StatisticOfElementClick += new EventHandler<IEntity>(StatisticElClick);
            view.CopyElementClick += new EventHandler<List<IEntity>>(CopyElClick);
            view.PastElementClick += new EventHandler(PastClick);
            view.RenameElementClick += new EventHandler<KeyValuePair<IEntity, string>>(RenameElClick);
            view.DeleteElementClick += new EventHandler<List<IEntity>>(DeleteElClick);

            view.CreateFileClick += new EventHandler<string>(CreateFileClick);
            view.CreateDirectoryClick += new EventHandler<string>(CreateDirClick);
        }

        private void UpdateView()
        {
            model.UpdateDirectory();

            if (model.curDirectory != null)
            {
                List<IEntity> elements = model.curDirectory.GetItems().ToList();
                elements.Sort((a, b) => a.GetFullName().CompareTo(b.GetFullName()));
                model.oldElements.Sort((a, b) => a.GetFullName().CompareTo(b.GetFullName()));

                if (elements.Count == model.oldElements.Count)
                {
                    bool isEquals = true;
                    for (int i = 0; i < elements.Count; i++)
                        if (elements[i].GetFullName() != model.oldElements[i].GetFullName())
                        {
                            isEquals = false;
                            break;
                        }

                    if (!isEquals)
                    {
                        view.ViewDirect(model.curDirectory.GetItems(), model.curDirectory.GetFullName());
                        model.oldElements = model.curDirectory.GetItems().ToList();
                    }
                }
                else
                {
                    view.ViewDirect(model.curDirectory.GetItems(), model.curDirectory.GetFullName());
                    model.oldElements = model.curDirectory.GetItems().ToList();
                }
            }
        }

        private void GetBack(object sender, EventArgs e)
        {
            model.GetBack();
            UpdateView();
        }

        private void DiskClick(object sender, EntityDriveInfo e)
        {
            model.ClickOnDisc(e);
            UpdateView();
        }

        private void ElementClick(object sender, IEntity e)
        {
            model.ClickOnElement(e);
            UpdateView();
        }

        private void EncryptClick(object sender, IEntity e)
        {
            model.EncryptClick(e);
            UpdateView();
        }

        private void DecryptClick(object sender, IEntity e)
        {
            model.DecryptClick(e);
            UpdateView();
        }

        private void LoadFiles(object sender, string[] e)
        {
            model.LoadFilesClick(e);
            UpdateView();
        }

        private void ArchiveFile(object sender, List<IEntity> e)
        {
            model.ArchiveClick(e);
            UpdateView();
        }

        private void ExtractArchiveFile(object sender, IEntity e)
        {
            model.ExtractArchiveClick(e);
            UpdateView();
        }

        private void ThreadFindClick(object sender, IEntity e)
        {
            model.ThreadsFindClick(e);
            UpdateView();
        }

        private void ParallelForEachFindClick(object sender, IEntity e)
        {
            model.ParallelsForEachFindClick(e);
            UpdateView();
        }

        private void TaskFindClick(object sender, IEntity e)
        {
            model.TaskFindClick(e);
            UpdateView();
        }

        private void AsyncAwaitFindClick(object sender, IEntity e)
        {
            model.AsyncAwaitFindClick(e);
            UpdateView();
        }

        private void FindElClick(object sender, string e)
        {
            model.FindElClick(e);
            UpdateView();
        }

        private void StatisticElClick(object sender, IEntity e)
        {
            model.StatisticItemClick(e);
            UpdateView();
        }

        private void CopyElClick(object sender, List<IEntity> e)
        {
            model.CopyItemsClick(e);
            UpdateView();
        }

        private void PastClick(object sender, EventArgs e)
        {
            model.PastItemsClick();
            UpdateView();
        }

        private void DeleteElClick(object sender, List<IEntity> e)
        {
            model.DeleteItemsClick(e);
            UpdateView();
        }

        private void RenameElClick(object sender, KeyValuePair<IEntity, string> e)
        {
            model.RenameItemClick(e);
            UpdateView();
        }

        private void CreateFileClick(object sender, string e)
        {
            model.CreateFileClick(e);
            UpdateView();
        }

        private void CreateDirClick(object sender, string e)
        {
            model.CreateDirClick(e);
            UpdateView();
        }
    }
}
