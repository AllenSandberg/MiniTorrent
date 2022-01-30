using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient
{

    public class ClientConfigurations
    {
        public enum ConfigStatus
        {
            OK, ERR_USERNAME, ERR_PASSWORD, ERR_CLIENT_IP, ERR_CLIENT_PORTIN,
            ERR_CLIENT_PORTOUT, ERR_DOWNLOAD_PATH, ERR_SERVER_IP, ERR_SERVER_PORT
        };

        public static ConfigStatus CheckConfigFile()
        {
            if (Properties.Settings.Default.Username == null || Properties.Settings.Default.Username.Trim().Length == 0)
                return ConfigStatus.ERR_USERNAME;
            if (Properties.Settings.Default.Password == null || Properties.Settings.Default.Password.Trim().Length == 0)
                return ConfigStatus.ERR_PASSWORD;
            if (Properties.Settings.Default.DownloadsPath == null || Properties.Settings.Default.DownloadsPath.Trim().Length == 0)
                return ConfigStatus.ERR_DOWNLOAD_PATH;
            if (Properties.Settings.Default.ClientIP == null || Properties.Settings.Default.ClientIP.Trim().Length == 0)
                return ConfigStatus.ERR_CLIENT_IP;
            if (Properties.Settings.Default.ClientPortIn == null || Properties.Settings.Default.ClientPortIn.Trim().Length == 0)
                return ConfigStatus.ERR_CLIENT_PORTIN;



            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.DownloadsPath);
            if (!d.Exists)
                return ConfigStatus.ERR_DOWNLOAD_PATH;

            try
            {
                int portIn = Int32.Parse(Properties.Settings.Default.ClientPortIn);
                if (portIn > 65535 || portIn < 1)
                    return ConfigStatus.ERR_CLIENT_PORTIN;
            } catch
            {
                return ConfigStatus.ERR_CLIENT_PORTIN;
            }
            

            return ConfigStatus.OK;
        }
    }
}
