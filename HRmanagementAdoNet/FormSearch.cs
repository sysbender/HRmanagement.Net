using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace HRmanagement
{
    public partial class FormSearch : Form
    {
        OracleConnection dbConn =null ;
        DbDataAdapter objDataAdapter = new OracleDataAdapter();
        BindingSource objBindingSource = new BindingSource();
        

        DataTable employeesDataTable = new DataTable();
        DataSet objDataSet = new DataSet();

        public FormSearch()
        {
            InitializeComponent();
        }

        private void FormSearch_Load(object sender, EventArgs e)
        {
            dbConn = (OracleConnection)DbUtils.GetConnection();
            //command
            string sql = @"select e.employee_id id, e.first_name || ' ' || e.last_name name, e.email mail, e.phone_number phone, e.hire_date hire_date, j.job_title title, e.salary salary, e.commission_pct commission, ee.first_name || ' ' || ee.last_name manager, d.department_name department 
                    from employees e, employees ee, jobs j, departments d
                    where e.employee_id = ee.employee_id and d.department_id = e.department_id and e.job_id = j.job_id";
            OracleCommand employeesOracleCommand = new OracleCommand(sql, dbConn);


            // fill datatable
            objDataAdapter.SelectCommand = employeesOracleCommand;
            objDataSet.Tables.Add("employees");
            objDataAdapter.Fill(objDataSet.Tables["employees"]);

            // bind datatable to dataGridView
            objBindingSource.DataSource = objDataSet.Tables["employees"];
            dgvEmployees.DataSource = objBindingSource;
            dgvEmployees.AutoGenerateColumns = true;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;




            //employeesDataTable.
            var departmentList = (from emp in objDataSet.Tables["employees"].AsEnumerable() select emp.Field<string>("department")).Distinct().ToList();
            cmbDepartment.DataSource = departmentList;
            //var clientIds = (from r in table.AsEnumerable()
            //select r.Field<string>("CLIENT_ID")).ToList();

        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            // linq to filter by : department
            var employeesByDeptment = from empRow in objDataSet.Tables["employees"].AsEnumerable()
                                      where empRow.Field<string>("department") == (string)cmbDepartment.SelectedValue
                                      select empRow;
            // set dataGridView datasource = 
            dgvEmployees.DataSource = employeesByDeptment.AsDataView();
            dgvEmployees.AutoGenerateColumns = true;
 

        }

        private void FormSearch_Activated(object sender, EventArgs e)
        {
           
        }

 
    }
}
