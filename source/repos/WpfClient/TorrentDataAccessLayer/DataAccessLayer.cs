using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentDataAccessLayer
{
    public class DataAccessLayer
    {
        #region Insert Statements 
        public static User Register(string username, string password)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username);
                if (result.Count() == 0)
                {
                    User newUser = new User()
                    {
                        Username = username,
                        Password = password,
                        Enabled = true,
                    };
                    db.Users.InsertOnSubmit(newUser);
                    db.SubmitChanges();
                    return newUser;
                }
            }
            return null;
        }

        public static int FileResourcesCount(string filename)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                return (from user in db.Users
                             from userFile in db.UserFiles
                             from file in db.Files
                             where user.Connected && user.Id == userFile.UserId && userFile.FileId.CompareTo(file.Id) == 0 && file.FileName.CompareTo(filename) == 0
                             select user).Count();
            }
        }

        public static User GetOnlineUserByFileId(string fileId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                List<User> users = (from userFile in db.UserFiles
                                    from user in db.Users
                                    where user.Id == userFile.Id && userFile.FileId.CompareTo(fileId) == 0 && user.Connected
                                    select user).ToList();
                if (users != null && users.Count > 0)
                    return users.First();
            }
            return null;
        }

        public static File InsertNewFile(string fileId, string fileName, string fileSize, int userId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Files.Where(row => row.Id.CompareTo(fileId) == 0);
                if (result.Count() == 0)
                {
                    File newfile = new File()
                    {
                        Id = fileId,
                        FileName = fileName,
                        FileSize = fileSize,
                        DateAdded = DateTime.Now,
                    };

                    // Insert file
                    db.Files.InsertOnSubmit(newfile);
                    db.SubmitChanges();

                    // Insert userfile (many-to-many relationship)
                    InsertNewUserFile(userId, fileId);

                    return newfile;
                }
                else
                {
                    if (db.UserFiles.Where(row => row.FileId.CompareTo(fileId) == 0 && row.UserId == userId).Count() == 0)
                    {
                        File existingFile = result.First();
                        InsertNewUserFile(userId, existingFile.Id);
                        return existingFile;
                    }
                    else return null;
                }
            }
            //return null;
        }

        public static void DisconnectAllUsers()
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                foreach (User user in db.Users)
                    user.Connected = false;
                db.SubmitChanges();
            }
        }

        public static UserFile InsertNewUserFile(int userId, string fileId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.UserFiles.Where(row => row.UserId == userId && row.FileId.CompareTo(fileId) == 0);
                if (result.Count() == 0)
                {
                    UserFile newUserFile = new UserFile()
                    {
                        UserId = userId,
                        FileId = fileId,
                    };
                    db.UserFiles.InsertOnSubmit(newUserFile);
                    db.SubmitChanges();
                    return newUserFile;
                }
            }
            return null;
        }
        #endregion

        #region Delete Statements
        public static bool DeleteUser(string username)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username);
                if (result.Count() == 1)
                {
                    db.Users.DeleteOnSubmit(result.First());
                    db.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteFile(string fileId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Files.Where(row => row.Id == fileId);
                if (result.Count() == 1)
                {
                    db.Files.DeleteOnSubmit(result.First());
                    db.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        public static void DeleteFileByUserId(int userId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = (from file in db.Files
                              from userFile in db.UserFiles
                              where userFile.UserId == userId
                              select userFile);
                foreach (var userFile in result)
                {
                    db.UserFiles.DeleteOnSubmit(userFile);
                }

                var r = (from file in db.Files
                         where !db.UserFiles.Any(userFile => userFile.FileId.CompareTo(file.Id) == 0)
                         select file);

                foreach (var file in r)
                {
                    db.Files.DeleteOnSubmit(file);
                }

                db.SubmitChanges();
                return;
            }
        }
        #endregion

        #region User Queries
        public static User LogIn(string username, string password, int portIn, int portOut, string IP)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username && row.Password == password);
                if (result.Count() == 1)
                {
                    result.First().PortIn = portIn;
                    result.First().PortOut = portOut;
                    result.First().IP = IP;
                    result.First().Connected = true;
                    db.SubmitChanges();
                    return result.First();
                }
            }
            return null;
        }

        public static User LogOut(string username, string password)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username && row.Password == password);
                if (result.Count() == 1)
                {
                    result.First().Connected = false;
                    db.SubmitChanges();
                    return result.First();
                }
            }
            return null;
        }

        public static User Enable(string username, bool enabled)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username);
                if (result.Count() == 1)
                {
                    result.First().Enabled = enabled;
                    db.SubmitChanges();
                    return result.First();
                }
            }
            return null;
        }

        public static bool IsEnabled(string username)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username);
                if (result.Count() == 1)
                {
                    return result.First().Enabled;
                }
            }
            return false;
        }

        public static bool IsConnected(string username)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.Users.Where(row => row.Username == username);
                if (result.Count() == 1)
                {
                    return result.First().Connected;
                }
            }
            return false;
        }

        public static List<User> GetAllUsers(bool getConnectedUsersOnly)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                if (getConnectedUsersOnly)
                    return db.Users.Where(row => row.Connected).ToList();
                else
                    return db.Users.ToList();
            }
        }

        public static int GetUsersCount(bool countConnectedUsersOnly)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                if (countConnectedUsersOnly)
                    return db.Users.Count(row => row.Connected);
                else
                    return db.Users.Count();
            }
        }

        public static UserFile UserFileExists(string fileId, int userId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                var result = db.UserFiles.Where(userFile => userFile.FileId.CompareTo(fileId) == 0 && userFile.UserId == userId);
                if (result.Count() > 0)
                    return result.First();
            }
            return null; // if not found
        }

        public static bool CheckIPPortConflict(string ip, int portIn)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                List<User> files = db.Users.Where(row => row.IP.CompareTo(ip) == 0 && row.PortIn ==portIn && row.Connected).ToList();
                if (files.Count > 0)
                    return true;
                else return false;
            }
        }
        #endregion

        #region File Queries
        public static List<File> GetFilesByFilename(bool fromConnectedUsersOnly, string filename = "")
        {
            List<File> filesList = new List<File>();
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                if (filename != null)
                {
                    if (filename.Trim().CompareTo("") == 0 || filename.Trim().CompareTo("*") == 0)
                    {
                        if (fromConnectedUsersOnly)
                            filesList = (from file in db.Files
                                         from user in db.Users
                                         from userFile in db.UserFiles
                                         where userFile.UserId == user.Id && userFile.FileId.CompareTo(file.Id) == 0 && user.Connected
                                         select file).Distinct().ToList();
                        else
                            filesList = (from file in db.Files
                                         from user in db.Users
                                         from userFile in db.UserFiles
                                         where userFile.UserId == user.Id && userFile.FileId.CompareTo(file.Id) == 0
                                         select file).Distinct().ToList();
                    }
                    else
                    {
                        if (fromConnectedUsersOnly)
                            filesList = (from file in db.Files
                                         from user in db.Users
                                         from userFile in db.UserFiles
                                         where userFile.UserId == user.Id && userFile.FileId.CompareTo(file.Id) == 0 && user.Connected
                                         && SqlMethods.Like(file.FileName.ToLower(), "%" + filename.ToLower() + "%")
                                         select file).Distinct().ToList();
                        else
                            filesList = (from file in db.Files
                                         from user in db.Users
                                         from userFile in db.UserFiles
                                         where userFile.UserId == user.Id && userFile.FileId == file.Id
                                         && SqlMethods.Like(file.FileName.ToLower(), "%" + filename.ToLower() + "%")
                                         select file).Distinct().ToList();
                    }
                }
            }

            return filesList;
        }

        public static User GetUserByFileId(string fileId)
        {
            if (fileId != null)
            {
                using (TorrentDBDataContext db = new TorrentDBDataContext())
                {
                    List<User> userList = (from user in db.Users
                                           from userFile in db.UserFiles
                                           where userFile.UserId == user.Id && userFile.FileId.CompareTo(fileId) == 0 && user.Connected
                                           select user).Distinct().ToList();
                    if (userList.Count > 0)
                        return userList.First();
                }
            }
            return null;
        }

        public static File FileExists(string fileId)
        {
            using (TorrentDBDataContext db = new TorrentDBDataContext())
            {
                List<File> files = db.Files.Where(row => row.Id.CompareTo(fileId) == 0).ToList();
                if (files.Count == 1)
                    return files.First();
                else return null;
            }
        }
        #endregion
    }
}
