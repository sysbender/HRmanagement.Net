using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;


namespace HRmanagement
{
    class DepartmentADO
    {
        public OracleDataAdapter objDataAdapter { get; set; }
        public DataSet objDataSet { get; set; }
        public OracleConnection dbConn { get; set; }


        public DepartmentADO()
        {
            dbConn = (OracleConnection)DbUtils.GetConnection();
            objDataAdapter = new OracleDataAdapter();
            objDataSet = new DataSet();
            InitDaoCommands();

        }



        public String GetNextDepartmentId()
        {
            return (new OracleCommand("select DEPARTMENTS_SEQ.nextVal from Dual ", dbConn).ExecuteScalar()).ToString();
        }


        private void InitDaoCommands()
        {
            /* delete command */
            string deleteSql = @"DELETE FROM Departments WHERE Department_id = :p_department_id";
            OracleCommand deleteCmd = new OracleCommand(deleteSql, dbConn);
            deleteCmd.BindByName = true;

            // add parameter - name, type, size, column
            deleteCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 6, "department_id");
            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.DeleteCommand = deleteCmd;


            /* insert command */
            string insertSql = @"insert into departments  
                    (department_id, department_name, manager_id, location_id) 
                    values(:p_department_id, :p_department_name, 
                    :p_manager_id, :p_location_id)";

            // returning employee_id into :p_employee_id";
            OracleCommand insertCmd = new OracleCommand(insertSql, dbConn);
            insertCmd.BindByName = true;
            // add parameter - name, type, size, column
            insertCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 10, "department_id");
            insertCmd.Parameters.Add(":p_department_name", OracleDbType.Varchar2, 20, "department_name");
            insertCmd.Parameters.Add(":p_manager_id", OracleDbType.Int32, 10, "manager_id");
            insertCmd.Parameters.Add(":p_location_id", OracleDbType.Int32, 6, "location_id");

            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.InsertCommand = insertCmd;

            objDataAdapter.RowUpdated += new OracleRowUpdatedEventHandler(myRowUpdatedHandler);


            /* update command */
            string updateSql  = @"update departments 
                set department_name = :p_department_name, 
                manager_id = :p_manager_id, 
                location_id = :p_location_id 
                where department_id = :p_department_id";
            OracleCommand updateCmd = new OracleCommand(updateSql, dbConn);
            updateCmd.BindByName = true;
            // add parameter - name, type, size, column
            updateCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 6, "department_id");
            updateCmd.Parameters.Add(":p_department_name", OracleDbType.Varchar2, 20, "department_name");
            updateCmd.Parameters.Add(":p_manager_id", OracleDbType.Int32, 10, "manager_id");
            updateCmd.Parameters.Add(":p_location_id", OracleDbType.Int32, 6, "location_id");
            objDataAdapter.UpdateCommand = updateCmd;



            // select command 
            try
            {

                //table departments
                objDataAdapter.SelectCommand = new OracleCommand(@"select department_id, 
                    department_name, manager_id, location_id from departments", dbConn);

                objDataAdapter.SelectCommand.BindByName = true;
                objDataSet.Tables.Add("Departments");
                objDataAdapter.Fill(objDataSet.Tables["Departments"]);


            }
            catch (Exception ex)
            {
                Console.WriteLine("============ error: data adapter department===========");
                Console.WriteLine(ex);
            }


            // table - employees
            objDataAdapter.SelectCommand = new OracleCommand("select first_name ||' ' || last_name name, employee_id from employees", dbConn);
            objDataSet.Tables.Add("Employees");
            objDataAdapter.Fill(objDataSet.Tables["Employees"]);
            // table - locations

            objDataAdapter.SelectCommand = new OracleCommand("select street_address || ', ' || city address , location_id from locations", dbConn);
            objDataSet.Tables.Add("Locations");
            objDataAdapter.Fill(objDataSet.Tables["Locations"]);

        }


        private void myRowUpdatedHandler(object sender, OracleRowUpdatedEventArgs e)
        {
            Console.WriteLine("********************on row updated *******************");
            // If this is an insert, then skip this row. 
            if (e.StatementType == StatementType.Insert)
            {
                Console.WriteLine("********************on row updated : insert*******************");
                // e.Status = UpdateStatus.SkipCurrentRow;
                // objDataAdapter.InsertCommand.Parameters["employee_id"].Value = (new OracleCommand("select EMPLOYEES_SEQ.nextVal from Dual ", dbConn).ExecuteScalar());



            }
        }

    }


}
