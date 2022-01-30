using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceInterface
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWCFTorrentService" in both code and config file together.
    [ServiceContract]
    public interface IWCFTorrentService
    {
        [OperationContract]
        string ClientSignIn(string json);

        [OperationContract]
        string ClientSignOut(string json);

        [OperationContract]
        string FileRequest(string json);

        [OperationContract]
        string PublishFileInformation(string json);

        [OperationContract]
        string DeletePublishedFiles(string json);

        [OperationContract]
        string UserFileExists(string json);

        [OperationContract]
        string CountFileResources(string json);

        [OperationContract]
        string RequestFileOwnerDetails(string json);

        [OperationContract]
        string CheckIPPortConflict(string json);
    }
}
