using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRmanagement
{
    public partial class FormEmployee : Form
    {
        EmployeeADO objEmployeeADO = new EmployeeADO();
        BindingSource objBindingSource = new BindingSource();
        public FormEmployee()
        {
            InitializeComponent();
        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {

            BindDataSource();
            SetDataGridViewEmplyeesHeaders();
        }



        private void FormEmployee_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("Form Employee active");

        }

        private void BindDataSource()
        {


            // bindsource
            objBindingSource.DataSource = objEmployeeADO.objDataSet.Tables["Employees"];

            // dataGridView
            dgvEmployees.DataSource = objBindingSource;
 
            // textboxes
            txtEmployee.DataBindings.Add("Text", objBindingSource, "employee_id");
            txtFirst_name.DataBindings.Add("Text", objBindingSource, "first_name");
            txtLast_name.DataBindings.Add("Text", objBindingSource, "last_name");
            txtPhone.DataBindings.Add("Text", objBindingSource, "phone_number");
            txtEmail.DataBindings.Add("Text", objBindingSource, "email");
            txtSalary.DataBindings.Add("Text", objBindingSource, "salary");
            txtCommission.DataBindings.Add("Text", objBindingSource, "commission_pct");
            
            // dateTimePicker
            //txtHire.DataBindings.Add("Text",objBindingSource,"hire_date");
            dtpHire.DataBindings.Add("Text", objBindingSource,"hire_date");
            dtpHire.ShowCheckBox = true;


            // comboBox for  manager_id
            cmbDepartment.DataBindings.Add("SelectedValue", objBindingSource, "department_id"); //sync with
            cmbDepartment.DataSource = objEmployeeADO.objDataSet.Tables["Departments"];  // fill list items
            cmbDepartment.DisplayMember = "department_name";
            cmbDepartment.ValueMember = "department_id";                // item value

            // comboBox for job_id
            cmbJob.DataBindings.Add("SelectedValue", objBindingSource, "job_id");
            cmbJob.DataSource = objEmployeeADO.objDataSet.Tables["Jobs"];
            cmbJob.DisplayMember = "job_title";
            cmbJob.ValueMember = "job_id";
            // bomboBox for Manager_id
            cmbManager.DataBindings.Add("SelectedValue", objBindingSource, "manager_id");
            cmbManager.DataSource = objEmployeeADO.objDataSet.Tables["Managers"];
            cmbManager.DisplayMember = "name";
            cmbManager.ValueMember = "employee_id";



        }

        private void SetDataGridViewEmplyeesHeaders()
        {
            dgvEmployees.AutoGenerateColumns = true;
            
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvEmployees.Columns[0].HeaderText = "Employee ID";
            dgvEmployees.Columns[1].HeaderText = "First Name";
            dgvEmployees.Columns[2].HeaderText = "Last Name";
            dgvEmployees.Columns[3].HeaderText = "Email Address";
            dgvEmployees.Columns[4].HeaderText = "Phone No";
            dgvEmployees.Columns[5].HeaderText = "Hire Date";
            dgvEmployees.Columns[6].HeaderText = "Job Id";
            dgvEmployees.Columns[7].HeaderText = "Salary";
            dgvEmployees.Columns[8].HeaderText = "Commission";
            dgvEmployees.Columns[9].HeaderText = "Manager Id";
            dgvEmployees.Columns[10].HeaderText = "Deptment ID";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            objBindingSource.AddNew();
            txtEmployee.Text = objEmployeeADO.GetNextEmployeeId();
            dgvEmployees.Refresh();


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string msg="";

            if(! ValidateInput(ref msg))
            {
                MessageBox.Show(msg);
            }else
            {

                try
                {


                    objBindingSource.EndEdit();

                    DataTable dt = (DataTable)objBindingSource.DataSource;

                    // Just for test.... Try this with or without the EndEdit....
                    DataTable changedTable = dt.GetChanges();
                    // check if something changed
                    if (changedTable != null)
                    {
                        Console.WriteLine("changed rows = " + changedTable.Rows.Count);


                        int rowsUpdated = objEmployeeADO.objDataAdapter.Update(dt);
                        Console.WriteLine("updateded rows = " + rowsUpdated);

                        //

                        objEmployeeADO.objDataSet.AcceptChanges();

                        //objDataAdapter.Update(objDataSet.Tables["Departments"]);

                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(ex.Message.ToString());
                }


            }


            //objEmployeeADO.objDataAdapter.Update(objEmployeeADO.objDataSet.Tables["Employees"]);


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            objBindingSource.EndEdit();
            DataTable dt = (DataTable)objBindingSource.DataSource;
           
            if (dt != null)
            {
                DataTable changedTable = dt.GetChanges();
                // Just for test.... Try this with or without the EndEdit....

                Console.WriteLine("changed rows = " + changedTable.Rows.Count);
                int rowsUpdated;
                try { 
                rowsUpdated = objEmployeeADO.objDataAdapter.Update(dt);

                Console.WriteLine("updateded rows = " + rowsUpdated);

                Console.WriteLine("last id=========" + objEmployeeADO.objDataAdapter.InsertCommand.Parameters[":p_employee_id"].Value.ToString());
                     }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                    objBindingSource.EndEdit();
                try { 
                rowsUpdated = objEmployeeADO.objDataAdapter.Update(dt);
                Console.WriteLine("updateded rows = " + rowsUpdated);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dgvEmployees.Refresh();

            }



        }

        private bool ValidateInput(ref string msg)
        {
            bool result = true;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Errors:");
             
            if (String.IsNullOrWhiteSpace(txtLast_name.Text))
            {
                result = false;
                sb.AppendLine("last name is empty!");
            }
            if (String.IsNullOrWhiteSpace(txtEmail.Text))
            {
                result = false;
                sb.AppendLine("Email is empty!");
                
            }
            if (String.IsNullOrWhiteSpace(dtpHire.Text))
            {
                result = false;
                sb.AppendLine("Hire Date is empty!");
            }


            if (cmbJob.SelectedIndex < 0)
            {
                result = false;
                sb.AppendLine("Job is not selected. ");
            }
            if (!dtpHire.Checked)
            {
                result = false;
                sb.AppendLine("Hire Date is not choosed. ");
            }
            /*
            if (cmbDepartment.SelectedIndex < 0)
            {
                sb.AppendLine("Department is not selected. ");
            }

            if(cmbManager.SelectedIndex < 0)
            {
                sb.AppendLine("Manager is not selected. ");
            }
            */
            //MessageBox.Show(dtpHire.);

            msg = sb.ToString();
            return result;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to delete Employee : " + dgvEmployees.Rows[dgvEmployees.CurrentRow.Index].Cells[1].Value.ToString(), "Delete confirmation",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {

                try
                {
                    dgvEmployees.Rows.RemoveAt(dgvEmployees.CurrentRow.Index);
                    objEmployeeADO.objDataAdapter.Update((DataTable)objBindingSource.DataSource);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            objBindingSource.CancelEdit();
            // objBindingSource.ResetBindings(false);
             
            DataTable dt = objBindingSource.DataSource as DataTable;

            dt.RejectChanges();

            dgvEmployees.CancelEdit();
            dgvEmployees.RefreshEdit();


            //dgvEmployees.DataSource = null;
            //dgvEmployees.DataSource = objBindingSource;
        }
    }
}
