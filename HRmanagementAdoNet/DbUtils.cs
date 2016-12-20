using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Xml;
using System.Configuration;
using System.Data;

namespace HRmanagement
{
    public static class DbUtils
    {
        //db provider
        static readonly DbProviderFactory dbProviderFactory = null;
        //db connection
        static DbConnection dbConn= null;

        static DbUtils()
        {
            dbProviderFactory = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");
        }

        // singleton , get db connection
        public static DbConnection GetConnection()
        {
            if (dbConn != null && dbConn.State != ConnectionState.Closed && dbConn.State != ConnectionState.Broken) return dbConn;
            

            string connectionString = ConfigurationManager.ConnectionStrings["orcl_hr"].ConnectionString;
            try
            {
                
                dbConn = dbProviderFactory.CreateConnection();
                dbConn.ConnectionString = connectionString + ";Connection Timeout=3";


                dbConn.Open();
                return dbConn;


            }catch(Exception ex)
            {
                
                System.Windows.Forms.MessageBox.Show("failed to connect to DB:" + connectionString);
                return null;
                //throw new Exception("failed to connect: " + connectionString);

            }
        }

        // close dbConn
        public static void DbClose()
        {
            if(dbConn != null)
            {
                try
                {
                    dbConn.Close();
                }catch(Exception ex)
                {
                    throw new Exception("fail to close");
                }
            }
        }







     
    }
}
