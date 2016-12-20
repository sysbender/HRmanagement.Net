
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
    public partial class FormDepartment : Form
    {

        DepartmentADO objDepartmentADO = new DepartmentADO();
        BindingSource objBindingSource = new BindingSource();

        OracleDataAdapter objDataAdapter = null;
        DataSet objDataSet = null;

       // OracleConnection dbConn = (OracleConnection)DbUtils.GetConnection();




        public FormDepartment()
        {
            InitializeComponent();
            objDataAdapter = objDepartmentADO.objDataAdapter;
            objDataSet = objDepartmentADO.objDataSet;

        }

        private void FormDepartment_Load(object sender, EventArgs e)
        {
            Console.WriteLine("department started");
            BindDataSources();



        }



  

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //http://stackoverflow.com/questions/17478202/update-tableadapter-with-dataset-update-requires-a-valid-deletecommand-error
            // Create the DeleteCommand.

            DialogResult res = MessageBox.Show("Are you sure you want to delete department : " + dgvDepartment.Rows[dgvDepartment.CurrentRow.Index].Cells[1].Value.ToString(), "Delete confirmation",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {
                    dgvDepartment.Rows.RemoveAt(dgvDepartment.CurrentRow.Index);
                    objDataAdapter.Update((DataTable)objBindingSource.DataSource);

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

            dgvDepartment.CancelEdit();
            dgvDepartment.RefreshEdit();

        }

  
        private void BindDataSources()
        {
            objBindingSource.DataSource = objDataSet.Tables["Departments"];

            //dgvDepartment.DataSource = objDataSet.Tables["Departments"];
            dgvDepartment.AutoGenerateColumns = true;
            dgvDepartment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepartment.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvDepartment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvDepartment.DataSource = objBindingSource;
            dgvDepartment.Refresh();

            txtDepartment_id.DataBindings.Add("Text", objBindingSource, "department_id");
            txtDepartment_name.DataBindings.Add("Text", objBindingSource, "department_name");
            /* 1. the name of the property on the control that you want to be databound
             * 2. the data source, 
             * 3. the property on the data source that you want to bind to.
             */

            // comboBox for  manager_id
            cmbManager_id.DisplayMember = "name";                       // item display
            cmbManager_id.ValueMember = "employee_id";                  // item value
            cmbManager_id.DataSource = objDataSet.Tables["Employees"];  // fill list items
            cmbManager_id.DataBindings.Add("SelectedValue", objBindingSource, "Manager_id"); //sync with
            // comboxBox for Location_id

            cmbLocation_id.DisplayMember = "address";
            cmbLocation_id.ValueMember = "location_id";
            cmbLocation_id.DataSource = objDataSet.Tables["Locations"];
            cmbLocation_id.DataBindings.Add("SelectedValue", objBindingSource, "Location_id");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //objDataSet.Tables["Departments"].NewRow();
            objBindingSource.AddNew();
            //dgvDepartment.Rows.Add();
            //objBindingSource.CurrencyManager.AddNew();

            txtDepartment_id.Text = objDepartmentADO.GetNextDepartmentId();
            dgvDepartment.Refresh();

        }


        private bool validteInput(ref string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Errors:");


            ////
            bool result = true;

            if (String.IsNullOrWhiteSpace(txtDepartment_name.Text))  // depart name is empty
            {
                result = false;
                sb.AppendLine("depart name is empty ");

            }
            else if (cmbManager_id.SelectedItem == null)  // manger or location is empty
            {
                result = false;
                sb.AppendLine(" manger is empty ");
            }
            else if (cmbLocation_id.SelectedItem == null)
            {
                result = false;
                sb.AppendLine(" location is empty ");
            }

            msg = sb.ToString();
            return result;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // check input
            string msg="";
            if (! validteInput(ref msg))
            {
                MessageBox.Show(msg);
            }
            else
            {
               

                // Create the insert command .
                try
                {                   
                         
                     objBindingSource.EndEdit();
                 
                    DataTable dt = (DataTable)objBindingSource.DataSource;

                    // Just for test.... Try this with or without the EndEdit....
                    DataTable changedTable = dt.GetChanges();
                    // check if something changed
                    if(changedTable != null)        
                    {
                        Console.WriteLine("changed rows = " + changedTable.Rows.Count);


                        int rowsUpdated = objDataAdapter.Update(dt);
                        Console.WriteLine("updateded rows = " + rowsUpdated);

                        //

                        objDataSet.AcceptChanges();

                        objDataAdapter.Update(objDataSet.Tables["Departments"]);

                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }


                //refresh 
                
                dgvDepartment.Refresh();

            }

        }

        private void dgvDepartment_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
           // Console.WriteLine("Department dataGridView - row leaving: current =" + dgvDepartment.CurrentRow.Index + "; is dirty = " + dgvDepartment.IsCurrentRowDirty);
            CurrencyManager cm = (CurrencyManager)dgvDepartment.BindingContext[dgvDepartment.DataSource, dgvDepartment.DataMember];
            // cm.EndCurrentEdit();
            objBindingSource.EndEdit();

            DataTable dt = (DataTable)objBindingSource.DataSource;

            // Just for test.... Try this with or without the EndEdit....
            DataTable changedTable = dt.GetChanges();
            // check if something changed
            if (changedTable != null)
            {
               // Console.WriteLine("changed rows = " + changedTable.Rows.Count + " dept name : = " + txtDepartment_name.Text);
                dgvDepartment.Rows[dgvDepartment.CurrentRow.Index].Selected = true;
                
               

            }


            }

        private void dgvDepartment_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            Console.WriteLine("============row validating");
            objBindingSource.EndEdit();

            DataTable dt = (DataTable)objBindingSource.DataSource;

            // Just for test.... Try this with or without the EndEdit....
            DataTable changedTable = dt.GetChanges();
            // check if something changed
            if (changedTable != null)
            {
                Console.WriteLine("changed rows = " + changedTable.Rows.Count + " dept name : = " + txtDepartment_name.Text + "new row=" + dgvDepartment.Rows[dgvDepartment.CurrentRow.Index].IsNewRow);
                


            }

        }
    }
}
