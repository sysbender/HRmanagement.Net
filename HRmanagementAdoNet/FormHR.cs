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
    public partial class HrInfo : Form
    {


        public HrInfo()
        {
            InitializeComponent();
             
        }
        #region - open child forms
        private  FormDepartment formDepartment = null;
        private void departmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formDepartment = MDIOpenChildForm( formDepartment, this.GetType().Namespace+".FormDepartment") as FormDepartment;
        }

    

        private FormEmployee formEmployee = null; 
        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formEmployee = MDIOpenChildForm( formEmployee, this.GetType().Namespace + ".FormEmployee") as FormEmployee;
        }


        private FormSearch formSearch = null;
        private void chercherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSearch  =  MDIOpenChildForm(  formSearch, this.GetType().Namespace + ".FormSearch") as FormSearch;
        }

        static FormDB formDB = null;
        private void configurerDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formDB = MDIOpenChildForm(formDB, this.GetType().Namespace + ".FormDB") as FormDB;
        }

        #endregion - child forms
        #region - general method for managing child form
        /**
         * open a child instance form in MDI parent Form
         */

        private Form MDIOpenChildForm(  Form childForm, string formType)
        {
            if(childForm == null || childForm.IsDisposed)
            {
                // create child form from form name and type
                childForm = (Form)Activator.CreateInstance(Type.GetType(formType));
                childForm.MdiParent = this;
                childForm.FormClosed += ChildForm_FormClosed;
                Console.WriteLine("create new form: " + childForm.ToString());
                // set child form size
                childForm.Left = 0;
                childForm.Top = 0;
                childForm.Size = this.ClientRectangle.Size;

                // show
                childForm.Show();
                // maximize child form
                childForm.WindowState = FormWindowState.Maximized;
                


            }
            else
            {
                childForm.Activate();
            }
            return childForm;

        }

        /**
         * close event for child form
         */
        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender != null) { sender = null; }
            
        }


        #endregion - manage child form


    }
}
