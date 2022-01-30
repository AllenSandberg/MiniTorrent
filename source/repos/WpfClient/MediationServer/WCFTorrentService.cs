using Newtonsoft.Json;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TorrentDataAccessLayer;

namespace MediationServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCFTorrentService" in both code and config file together.
    public class WCFTorrentService : IWCFTorrentService
    {


        public string ClientSignIn(string json)
        {
            Console.WriteLine(GetTimeString() + ": ClientSignIn(" + json + ")");

            User user = null;
            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["Username"] != null && parameters["Password"] != null
                && parameters["PortIn"] != null && parameters["PortOut"] != null && parameters["IP"] != null
                && Convert.ToInt32(parameters["PortOut"]) != 0 && Convert.ToInt32(parameters["PortIn"]) != 0)
            {
                string username = parameters["Username"];
                string password = parameters["Password"];
                int portIn = Convert.ToInt32(parameters["PortIn"]);
                int portOut = Convert.ToInt32(parameters["PortOut"]);
                string IP = parameters["IP"];

                // Prevent the user from connecting from multiple clients at the same time
                if (DataAccessLayer.IsConnected(username))
                    return JsonConvert.SerializeObject(new
                    {
                        SignedIn = false,
                        Error = "This user is already conneced to the torrent system"
                    });

                user = DataAccessLayer.LogIn(username, password, portIn, portOut, IP);
            }

            if (user != null)
                return JsonConvert.SerializeObject(new { SignedIn = true, UserId = user.Id });
            else
                return JsonConvert.SerializeObject(new { SignedIn = false, Error = "Wrong username/password" });
        }

        public string ClientSignOut(string json)
        {
            Console.WriteLine(GetTimeString() + ": ClientSignOut(" + json + ")");

            bool singedOut = false;

            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["Username"] != null && parameters["Password"] != null)
            {
                string username = parameters["Username"];
                string password = parameters["Password"];
                singedOut = DataAccessLayer.LogOut(username, password) != null;
            }

            return JsonConvert.SerializeObject(new { SignedOut = singedOut });
        }

        public string CountFileResources(string json)
        {
            dynamic parameters = JsonConvert.DeserializeObject(json);

            if (parameters != null && parameters["Filename"] != null)
            {
                string filename = parameters["Filename"];

                return JsonConvert.SerializeObject(new { Count = DataAccessLayer.FileResourcesCount(filename) });
            }

            return JsonConvert.SerializeObject(new { Count = 0 });
        }

        public string DeletePublishedFiles(string json)
        {
            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && Convert.ToInt32(parameters["UserId"]) != 0)
            {
                int userId = parameters["UserId"];
                DataAccessLayer.DeleteFileByUserId(userId);
                return "";
            }
            return "";
        }

        public string FileRequest(string json)
        {
            Console.WriteLine(GetTimeString() + ": PublishFileInformation(" + json + ")");

            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["Filename"] != null)
            {
                string filename = parameters["Filename"];
                List<File> files = DataAccessLayer.GetFilesByFilename(true, filename);
                if (files != null)
                {
                    return JsonConvert.SerializeObject(new { files });
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public string PublishFileInformation(string json)
        {
            Console.WriteLine(GetTimeString() + ": PublishFileInformation(" + json + ")");

            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["UserId"] != null && parameters["FileId"] != null
                && parameters["FileName"] != null && parameters["FileSize"] != null
                && Convert.ToInt32(parameters["UserId"]) != 0)
            {
                string fileSize = parameters["FileSize"];
                string fileId = parameters["FileId"];
                string fileName = parameters["FileName"];
                int userId = parameters["UserId"];
                return JsonConvert.SerializeObject(DataAccessLayer.InsertNewFile(fileId, fileName,
                    fileSize, userId));
            }
            return null;
        }

        public string RequestFileOwnerDetails(string json)
        {
            Console.WriteLine(GetTimeString() + ": RequestFileOwnerDetails(" + json + ")");

            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["FileId"] != null)
            {
                string fileId = parameters["FileId"];
                User user = DataAccessLayer.GetUserByFileId(fileId);
                if (user != null)
                    return JsonConvert.SerializeObject(user);
            }
            return null;
        }

        public string UserFileExists(string json)
        {
            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["UserId"] != null && parameters["FileId"] != null && Convert.ToInt32(parameters["UserId"]) != 0)
            {
                string fileId = parameters["FileId"];
                int userId = parameters["UserId"];
                if (DataAccessLayer.UserFileExists(fileId, userId) != null)
                    return JsonConvert.SerializeObject(new { exists = true });
            }
            return JsonConvert.SerializeObject(new { exists = false });
        }

        public string CheckIPPortConflict(string json)
        {
            Console.WriteLine(GetTimeString() + ": CheckIPPortConflict(" + json + ")");

            dynamic parameters = JsonConvert.DeserializeObject(json);
            if (parameters != null && parameters["ip"] != null && parameters["portIn"] != null && Convert.ToInt32(parameters["portIn"]) != 0)
            {
                string ip = parameters["ip"];
                int portIn = parameters["portIn"];
                return JsonConvert.SerializeObject(new { conflict = DataAccessLayer.CheckIPPortConflict(ip, portIn) });
            }
            return JsonConvert.SerializeObject(new { conflict = false });
        }

        private string GetTimeString()
        {
            return "[" + DateTime.Now + "]";
        }
    }
}
