using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.ComponentModel;

using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace HRmanagement
{
    /*
     * this class hold all the data access object for employee
     */
    class EmployeeADO
    {
        public OracleDataAdapter objDataAdapter { get; set; }
        public DataSet objDataSet { get; set; }
        public OracleConnection dbConn { get; set; }

        public EmployeeADO()
        {
            dbConn = (OracleConnection)DbUtils.GetConnection();
            objDataAdapter = new OracleDataAdapter();
            objDataSet = new DataSet();
            InitDaoSelectCommand();
            AddDebug();
        }

        private void AddDebug()
        {
            objDataAdapter.RowUpdating += ObjDataAdapter_RowUpdating;
            
        }

        public string GetNextEmployeeId()
        {
           return (new OracleCommand("select EMPLOYEES_SEQ.nextVal from Dual ", dbConn).ExecuteScalar()).ToString();
        }

        private void ObjDataAdapter_RowUpdating(object sender, OracleRowUpdatingEventArgs e)
        {
            if (e.Command != null)

            {

                Console.WriteLine("nRow Updating…--------------------------");
                Console.WriteLine(e.Command);
                Console.WriteLine("Command type: =" + e.StatementType);
                Console.WriteLine("Command text: =" + e.Command.CommandText);
                Console.WriteLine("\nParameters:");
                foreach (OracleParameter p in e.Command.Parameters)
                {
                    Console.WriteLine("parameter name = " + p.ParameterName );
                    Console.WriteLine("parameter value = " + p.Value);
                }

                Console.WriteLine("nRow Updating…------------end debug--------------");
            }
        }

        private void InitDaoSelectCommand()
        {
            /* delete command */
            string deleteSql = @"DELETE FROM Employees WHERE Employee_id = :p_employee_id";
            OracleCommand deleteCmd = new OracleCommand(deleteSql, dbConn);
            deleteCmd.BindByName = true;
            // add parameter - name, type, size, column
            deleteCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6, "employee_id");
            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.DeleteCommand = deleteCmd;


            /* insert command */
            string insertSql = @"insert into employees
                    (employee_id, first_name, last_name, email, phone_number, hire_date, job_id, salary,
                            commission_pct, manager_id, department_id)
                    values(:p_employee_id, :p_first_name, :p_last_name, :p_email, :p_phone_number, 
                            :p_hire_date, :p_job_id, :p_salary, :p_commission_pct, :p_manager_id, :p_department_id)";
            // returning employee_id into :p_employee_id";
            OracleCommand insertCmd = new OracleCommand(insertSql, dbConn);
            insertCmd.BindByName = true;
            // add parameter - name, type, size, column
            //insertCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6,"employee_id", ParameterDirection.Output);   
            insertCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6, "employee_id");
            insertCmd.Parameters.Add(":p_first_name", OracleDbType.Varchar2, 20, "first_name");
            insertCmd.Parameters.Add(":p_last_name", OracleDbType.Varchar2, 25, "last_name");
            insertCmd.Parameters.Add(":p_email", OracleDbType.Varchar2, 25, "email");
            insertCmd.Parameters.Add(":p_phone_number", OracleDbType.Varchar2, 20, "phone_number");
            insertCmd.Parameters.Add(":p_hire_date", OracleDbType.Date, 10, "hire_date");
            insertCmd.Parameters.Add(":p_job_id", OracleDbType.Varchar2, 10, "job_id");
            insertCmd.Parameters.Add(":p_salary", OracleDbType.Decimal, 10, "salary");
            insertCmd.Parameters.Add(":p_commission_pct", OracleDbType.Decimal, 10, "commission_pct");
            insertCmd.Parameters.Add(":p_manager_id", OracleDbType.Int32, 10, "manager_id");
            insertCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 10, "department_id");

            //insertCmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.InsertCommand = insertCmd;

            objDataAdapter.RowUpdated += new OracleRowUpdatedEventHandler(myRowUpdatedHandler);


            /* update command */
            string updateSql = @"update Employees set 
                    first_name=:p_first_name,
                    last_name=:p_last_name,
                    email=:p_email,
                    phone_number=:p_phone_number,
                    hire_date=:p_hire_date,
                    job_id=:p_job_id,
                    salary=:p_salary,
                    commission_pct=:p_commission_pct,
                    manager_id=:p_manager_id,
                    department_id=:p_department_id
                    WHERE Employee_id = :p_employee_id";
            OracleCommand updateCmd = new OracleCommand(updateSql, dbConn);
            updateCmd.BindByName = true;
            // add parameter - name, type, size, column
            updateCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6, "employee_id");
            updateCmd.Parameters.Add("p_first_name", OracleDbType.Varchar2, 20, "first_name");
            updateCmd.Parameters.Add(":p_last_name", OracleDbType.Varchar2, 25, "last_name");
            updateCmd.Parameters.Add(":p_email", OracleDbType.Varchar2, 25, "email");
            updateCmd.Parameters.Add(":p_phone_number", OracleDbType.Varchar2, 20, "phone_number");
            updateCmd.Parameters.Add(":p_hire_date", OracleDbType.Date, 10, "hire_date");
            updateCmd.Parameters.Add(":p_job_id", OracleDbType.Varchar2, 10, "job_id");
            updateCmd.Parameters.Add(":p_salary", OracleDbType.Decimal, 10, "salary");
            updateCmd.Parameters.Add(":p_commission_pct", OracleDbType.Decimal, 10, "commission_pct");
            updateCmd.Parameters.Add(":p_manager_id", OracleDbType.Int32, 10, "manager_id");
            updateCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 10, "department_id");


            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.UpdateCommand = updateCmd;



            // select command 
            try
            {
                objDataAdapter.SelectCommand = new OracleCommand(@"select employee_id, first_name, 
                            last_name, email, phone_number, hire_date, job_id, salary,
                            commission_pct, manager_id, department_id from employees", dbConn);
                objDataAdapter.SelectCommand.BindByName = true;

                objDataSet.Tables.Add("Employees");
                objDataAdapter.Fill(objDataSet.Tables["Employees"]);

                //objDataSet.Tables["Employees"].Columns["employee_id"].AutoIncrement = true;
                //objDataSet.Tables["Employees"].Columns["employee_id"].AutoIncrementSeed = -1;
                //objDataSet.Tables["Employees"].Columns["employee_id"].AutoIncrementStep = -1;

            }
            catch (Exception ex)
            {
                Console.WriteLine("============ data adapter employee===========");
                Console.WriteLine(ex);
            }


            // jobs - job_id, job_title
            objDataAdapter.SelectCommand = new OracleCommand(@"select job_id, job_title from jobs", dbConn);
            objDataSet.Tables.Add("Jobs");
            objDataAdapter.Fill(objDataSet.Tables["Jobs"]);

            // managers - manager_id, name (first_name last_name)
            objDataAdapter.SelectCommand = new OracleCommand(@"select employee_id, first_name|| ' ' || last_name name from employees", dbConn);
            objDataSet.Tables.Add("Managers");
            objDataAdapter.Fill(objDataSet.Tables["Managers"]);
            // departments - department_id, department_name 
            objDataAdapter.SelectCommand = new OracleCommand(@"select department_id, department_name from departments", dbConn);
            objDataSet.Tables.Add("Departments");
            objDataAdapter.Fill(objDataSet.Tables["Departments"]);



        }
        private void InitDao()
        {



            /* delete command */
            string deleteSql = @"DELETE FROM Employees WHERE Employee_id = :p_employee_id";
            OracleCommand deleteCmd = new OracleCommand(deleteSql, dbConn);
            deleteCmd.BindByName = true;
            // add parameter - name, type, size, column
            deleteCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32,6, "employee_id");
            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.DeleteCommand = deleteCmd;


            /* insert command */
            string insertSql = @"insert into employees
                    (employee_id, first_name, last_name, email, phone_number, hire_date, job_id, salary,
                            commission_pct, manager_id, department_id)
                    values(p_employee_id, :p_first_name, :p_last_name, :p_email, :p_phone_number, 
                            :p_hire_date, :p_job_id, :p_salary, :p_commission_pct, :p_manager_id, :p_department_id)";
                    // returning employee_id into :p_employee_id";
            OracleCommand insertCmd = new OracleCommand(insertSql, dbConn);
            insertCmd.BindByName = true;
            // add parameter - name, type, size, column
            //insertCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6,"employee_id", ParameterDirection.Output);   
            insertCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6, "employee_id");
            insertCmd.Parameters.Add(":p_first_name", OracleDbType.Varchar2, 20,  	"first_name");        
            insertCmd.Parameters.Add(":p_last_name", OracleDbType.Varchar2, 25,  	"last_name");         
            insertCmd.Parameters.Add(":p_email", OracleDbType.Varchar2, 25,  		"email");             
            insertCmd.Parameters.Add(":p_phone_number", OracleDbType.Varchar2, 20,  	"phone_number");      
            insertCmd.Parameters.Add(":p_hire_date", OracleDbType.Date, 10,  	"hire_date");         
            insertCmd.Parameters.Add(":p_job_id", OracleDbType.Varchar2, 10,  		"job_id");            
            insertCmd.Parameters.Add(":p_salary", OracleDbType.Decimal, 10,  		"salary");            
            insertCmd.Parameters.Add(":p_commission_pct", OracleDbType.Decimal, 10,  	"commission_pct");    
            insertCmd.Parameters.Add(":p_manager_id", OracleDbType.Int32, 10,  	"manager_id");        
            insertCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 10, 	"department_id");

            //insertCmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.InsertCommand = insertCmd;

            objDataAdapter.RowUpdated += new OracleRowUpdatedEventHandler(myRowUpdatedHandler);


            /* update command */
            string updateSql = @"update Employees set 
                    first_name=:p_first_name,
                    last_name=:p_last_name,
                    email=:p_email,
                    phone_number=:p_phone_number,
                    hire_date=:p_hire_date,
                    job_id=:p_job_id,
                    salary=:p_salary,
                    commission_pct=:p_commission_pct,
                    manager_id=:p_manager_id,
                    department_id=:p_department_id
                    WHERE Employee_id = :p_employee_id";
            OracleCommand updateCmd = new OracleCommand(updateSql, dbConn);
            updateCmd.BindByName = true;
            // add parameter - name, type, size, column
            updateCmd.Parameters.Add(":p_employee_id", OracleDbType.Int32, 6, "employee_id");
            updateCmd.Parameters.Add("p_first_name", OracleDbType.Varchar2, 20, "first_name");
            updateCmd.Parameters.Add(":p_last_name", OracleDbType.Varchar2, 25, "last_name");
            updateCmd.Parameters.Add(":p_email", OracleDbType.Varchar2, 25, "email");
            updateCmd.Parameters.Add(":p_phone_number", OracleDbType.Varchar2, 20, "phone_number");
            updateCmd.Parameters.Add(":p_hire_date", OracleDbType.Date, 10, "hire_date");
            updateCmd.Parameters.Add(":p_job_id", OracleDbType.Varchar2, 10, "job_id");
            updateCmd.Parameters.Add(":p_salary", OracleDbType.Decimal, 10, "salary");
            updateCmd.Parameters.Add(":p_commission_pct", OracleDbType.Decimal, 10, "commission_pct");
            updateCmd.Parameters.Add(":p_manager_id", OracleDbType.Int32, 10, "manager_id");
            updateCmd.Parameters.Add(":p_department_id", OracleDbType.Int32, 10, "department_id");
            

            //parameter.SourceVersion = DataRowVersion.Original;
            objDataAdapter.UpdateCommand = updateCmd;



            // select command 
            try { 
            objDataAdapter.SelectCommand = new OracleCommand(@"select employee_id, first_name, 
                            last_name, email, phone_number, hire_date, job_id, salary,
                            commission_pct, manager_id, department_id from employees", dbConn);
            objDataAdapter.SelectCommand.BindByName = true;
               
                objDataSet.Tables.Add("Employees");
            objDataAdapter.Fill(objDataSet.Tables["Employees"]);

            //objDataSet.Tables["Employees"].Columns["employee_id"].AutoIncrement = true;
            //objDataSet.Tables["Employees"].Columns["employee_id"].AutoIncrementSeed = -1;
            //objDataSet.Tables["Employees"].Columns["employee_id"].AutoIncrementStep = -1;

            }
            catch (Exception ex)
            {
                Console.WriteLine("============ data adapter employee===========");
                Console.WriteLine(ex);
            }


            // jobs - job_id, job_title
            objDataAdapter.SelectCommand = new OracleCommand(@"select job_id, job_title from jobs", dbConn);
            objDataSet.Tables.Add("Jobs");
            objDataAdapter.Fill(objDataSet.Tables["Jobs"]);

            // managers - manager_id, name (first_name last_name)
            objDataAdapter.SelectCommand = new OracleCommand(@"select employee_id, first_name|| ' ' || last_name name from employees", dbConn);
            objDataSet.Tables.Add("Managers");
            objDataAdapter.Fill(objDataSet.Tables["Managers"]);
            // departments - department_id, department_name 
            objDataAdapter.SelectCommand = new OracleCommand(@"select department_id, department_name from departments", dbConn);
            objDataSet.Tables.Add("Departments");
            objDataAdapter.Fill(objDataSet.Tables["Departments"]);



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
