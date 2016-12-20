using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Common;
using System.Xml;
using System.Configuration;

namespace HRmanagement
{
    public partial class FormDB : Form
    {
        string connectionString;
        public FormDB()
        {
            InitializeComponent();
        }

        private void FormDB_Activated(object sender, EventArgs e)
        {
            ReadConnectionString();
        }


        private static void UpdateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
             
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");

        }

        public void updateConnectionString(string connStr)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["orcl_hr"].ConnectionString = connStr;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static void AddAndSaveOneConnectionStringSettings(
       System.Configuration.Configuration configuration,
       System.Configuration.ConnectionStringSettings connectionStringSettings)
        {

            
            // You cannot add to ConfigurationManager.ConnectionStrings using
            // ConfigurationManager.ConnectionStrings.Add
            // (connectionStringSettings) -- This fails.

            // But you can add to the configuration section and refresh the ConfigurationManager.

            // Get the connection strings section; Even if it is in another file.
            ConnectionStringsSection connectionStringsSection = configuration.ConnectionStrings;

            // Add the new element to the section.
            connectionStringsSection.ConnectionStrings.Add(connectionStringSettings);

            // Save the configuration file.
            configuration.Save(ConfigurationSaveMode.Minimal);

            // This is needed. Otherwise the connection string does not show up in
            // ConfigurationManager
            ConfigurationManager.RefreshSection("connectionStrings");
        }


        private void ReadConnectionString()
        {
            

                var cs = ConfigurationManager.ConnectionStrings["orcl_hr"].ConnectionString;
                connectionString= cs;
                Console.WriteLine("conn ========"+cs);

            char[] separators = { '/',':','=',';' };
            //"user id=hr;password=hr;data source=//192.168.124.128:1521/ORCL"
            string[] connStrs = connectionString.Split(separators);
            txtUsername.Text = connStrs[1];
            txtPassword.Text = connStrs[3];
            txtHost.Text = connStrs[7];
            txtPort.Text = connStrs[8];
            txtService.Text = connStrs[9];

            // string configfile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; 
            //var appSettings = ConfigurationManager.AppSettings;
            // var appSettings  = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


        }





        private void btnTest_Click(object sender, EventArgs e)
        {

            string strConn = String.Format("user id={0};password={1};data source=//{2}:{3}/{4}",txtUsername.Text, txtPassword.Text, txtHost.Text, txtPort.Text, txtService.Text);
            Console.WriteLine(strConn);

            DbConnection dbConn = null;
            DbProviderFactory dpf = null;
            string msg = "";
            try {
                dpf = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");

                dbConn = dpf.CreateConnection();
                dbConn.ConnectionString = strConn + ";Connection Timeout=3";
                dbConn.Open();
                msg = "sucessfully connected !";
                connectionString = strConn;

            }
            catch (Exception ex )
            {
                msg = ex.Message;
            }finally
            {
                if(dbConn != null)
                {
                    dbConn.Close();
                }
                lblMsg.Text = msg;
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            updateConnectionString(connectionString);
            lblMsg.Text = "connection string saved: " + connectionString;
        }
    }
}
