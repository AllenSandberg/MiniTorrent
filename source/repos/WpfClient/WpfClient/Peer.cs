using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServiceInterface;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace WpfClient
{
    public class Peer
    {
        //ChannelFactory<IWCFTorrentService> channelFactory;
        private IWCFTorrentService proxy;
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserIP { get; set; }
        public int PortIn { get; set; }
        public int PortOut { get; set; }

        public Peer()
        {
            proxy = new ChannelFactory<IWCFTorrentService>("TorrentServiceEndpoint").CreateChannel();
            UserId = -1;
        }

        public int CountFileResources(string filename)
        {
            string json = JsonConvert.SerializeObject(new
            {
                Filename = filename,
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.CountFileResources(json));
            if (response != null && response["Count"] != null)
            {
                int count = response["Count"];
                return count;
            }
            return 0;
        }

        public int SignIn() // returns userID, -1 on error
        {
            string signInJsonString = JsonConvert.SerializeObject(new
            {
                Username = Properties.Settings.Default.Username,
                Password = Properties.Settings.Default.Password,
                PortIn = Properties.Settings.Default.ClientPortIn,
                PortOut = Properties.Settings.Default.ClientPortOut,
                IP = Properties.Settings.Default.ClientIP,
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.ClientSignIn(signInJsonString));
            if (response != null && response["SignedIn"] != null && response["UserId"] != null)
            {
                UserId = response["UserId"];
                UserName = Properties.Settings.Default.Username;
                UserPassword = Properties.Settings.Default.Password;
                PortIn = Int32.Parse(Properties.Settings.Default.ClientPortIn);
                PortOut = Int32.Parse(Properties.Settings.Default.ClientPortOut);
                UserIP = Properties.Settings.Default.ClientIP;
                return UserId;
            }
            else
                return -1;
        }

        public int SignOut() // returns userID, -1 on error
        {
            string signInJsonString = JsonConvert.SerializeObject(new
            {
                Username = Properties.Settings.Default.Username,
                Password = Properties.Settings.Default.Password,
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.ClientSignOut(signInJsonString));
            if (response != null && response["SignedOut"] != null && response["UserId"] != null)
            {
                UserId = response["UserId"];
                return UserId;
            }
            else
                return -1;
        }

        public void PublishFileInformation(System.IO.FileInfo fileInfo)
        {
            string fileId = FileManager.CalculateFileId(fileInfo.FullName);
            string filePublishJsonString = JsonConvert.SerializeObject(new
            {
                UserId = this.UserId,
                FileName = fileInfo.Name,
                FileId = fileId,
                FileSize = fileInfo.Length.ToString(),
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.PublishFileInformation(filePublishJsonString));
        }

        public void ClearPublishedFiles()
        {
            string deletePublishJsonString = JsonConvert.SerializeObject(new
            {
                UserId = this.UserId,
            });

            JsonConvert.DeserializeObject(proxy.DeletePublishedFiles(deletePublishJsonString));
        }

        public bool UserFileExists(string fileId)
        {
            string userFileExistsJsonString = JsonConvert.SerializeObject(new
            {
                UserId = this.UserId,
                FileId = fileId,
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.UserFileExists(userFileExistsJsonString));
            if (response != null && response["exists"] != null)
            {
                bool exists = response["exists"];
                return exists;
            }
            return false;
        }

        public bool CheckIPPortConflict()
        {
            string json = JsonConvert.SerializeObject(new
            {
                ip = Properties.Settings.Default.ClientIP,
                portIn = Int32.Parse(Properties.Settings.Default.ClientPortIn),
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.CheckIPPortConflict(json));
            if (response != null && response["conflict"] != null)
            {
                bool conflict = response["conflict"];
                return conflict;
            }
            return false;
        }

        public UserDetails GetFileOwnerDetails(string fileId)
        {
            string json = JsonConvert.SerializeObject(new
            {
                FileId = fileId,
            });

            dynamic response = JsonConvert.DeserializeObject(proxy.RequestFileOwnerDetails(json));
            if (response != null)
            {
                string Name = response["Username"];
                string IP = response["IP"];
                int PortIn = response["PortIn"];
                int PortOut = response["PortOut"];
                return new UserDetails
                {
                    IP = IP,
                    Name = Name,
                    PortIn = PortIn,
                    PortOut = PortOut,
                };
            }
            return null;
        }

        public List<FileDetails> FileRequest(string fileName)
        {
            string fileRequestJsonString = JsonConvert.SerializeObject(new
            {
                Filename = fileName,
            });
            List<FileDetails> fileDetailsList = new List<FileDetails>();
            string responseJson = proxy.FileRequest(fileRequestJsonString);
            if (responseJson != null)
            {
                dynamic response = JsonConvert.DeserializeObject(responseJson);


                if (fileDetailsList != null && response["files"] != null)
                    foreach (var v in response["files"])
                    {
                        FileDetails fileDetails = new FileDetails();
                        if (v["FileName"] != null)
                            fileDetails.Filename = v["FileName"];
                        if (v["FileSize"] != null)
                        {
                            string size = v["FileSize"].ToString();
                            fileDetails.FileSize = long.Parse(size);
                        }
                        if (v["DateAdded"] != null)
                            fileDetails.PublishDate = v["DateAdded"];
                        if (v["Id"] != null)
                            fileDetails.FileId = v["Id"];
                        fileDetailsList.Add(fileDetails);
                    }
            }
            return fileDetailsList;
        }
    }
}
