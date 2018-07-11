using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    interface IView                
    {
        void ViewDirect(IEnumerable<IEntity> items, string fullPath);

        event EventHandler GetBackClick;
        event EventHandler<EntityDriveInfo> DiskClick;
        event EventHandler<IEntity> ElementClick;

        event EventHandler<List<IEntity>> CopyElementClick;
        event EventHandler<List<IEntity>> DeleteElementClick;
        event EventHandler<KeyValuePair<IEntity, string>> RenameElementClick;
        event EventHandler PastElementClick;

        event EventHandler<List<IEntity>> ArchiveElementClick;
        event EventHandler<IEntity> ExtractArchiveElementClick;
        event EventHandler<IEntity> EncryptElementClick;
        event EventHandler<IEntity> DecryptElementClick;

        event EventHandler<IEntity> FindPersDataThreadClick;
        event EventHandler<IEntity> FindPersDataForEachClick;
        event EventHandler<IEntity> FindPersDataTaskClick;
        event EventHandler<IEntity> FindPersDataAsicnClick;

        event EventHandler<string[]> LoadElementClick;
        event EventHandler<string> FindElementClick;
        event EventHandler<IEntity> StatisticOfElementClick;

        event EventHandler<string> CreateFileClick;
        event EventHandler<string> CreateDirectoryClick;
    }
}
