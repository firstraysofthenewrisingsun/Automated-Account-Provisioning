using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WinHRTool
{

    

    public partial class HRTool : Form
    {
        private ADControls controller;

        private string name;
        private string[] radioButtons;
        private List<TextBox> boxes;
        private bool userCreated;

        public HRTool()
        {
            InitializeComponent();
            controller = new ADControls();
            radioButtons = new string[1];
            boxes = new List<TextBox>();
            tbDesc.ReadOnly = true;
            pbAdd.Style = ProgressBarStyle.Marquee;
            pbAdd.Visible = false;
            pbEdit.Style = ProgressBarStyle.Marquee;
            pbEdit.Visible = false;
            pbDelete.Style = ProgressBarStyle.Marquee;
            pbDelete.Visible = false;
            userCreated = false;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var handle = this.Handle;

            NativeWindow win32Parent = new NativeWindow();
            win32Parent.AssignHandle(handle);

            DialogResult result = MessageBox.Show(win32Parent, "Test", "New Account for !", MessageBoxButtons.YesNo);
            
            switch (result)
            {
                case DialogResult.Yes:
                    pbAdd.Visible=true;
                    adAddBW.RunWorkerAsync();

                    break;
                case DialogResult.No:  
                    break;
            }

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            adSearchBW.RunWorkerAsync();
        }

        private void HRToolForm1_Load(object sender, EventArgs e)
        {
            personnelDataGridView1.ColumnCount = 3;
            personnelDataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            personnelDataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            personnelDataGridView1.ColumnHeadersDefaultCellStyle.Font =
            new Font(personnelDataGridView1.Font, FontStyle.Bold);

            personnelDataGridView1.Columns[0].Name = "First Name";
            personnelDataGridView1.Columns[1].Name = "Last Name";
            personnelDataGridView1.Columns[2].Name = "Email";
        }

        private void loadUsers_Click(object sender, EventArgs e)
        {
            controller.retrieveAllAD();
        }

        private void editOn_Click(object sender, EventArgs e)
        {
            adEditBW.RunWorkerAsync();
           
        }

        private void rbEmpOp_CheckedChanged(object sender, EventArgs e)
        {
            tbDesc.Text = "Employee";           
        }

        private void rbConOp_CheckedChanged(object sender, EventArgs e)
        {
            tbDesc.Text = "Contractor";
        }

        private void rbEmp_CheckedChanged(object sender, EventArgs e)
        {
            adDesc.Text = "Employee";
        }

        private void rbCon_CheckedChanged(object sender, EventArgs e)
        {
            adDesc.Text = "Contractor";
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

           adSearchDeleteBW.RunWorkerAsync();

        }

        private void deleteUserBut_Click(object sender, EventArgs e)
        {
           
            adDeleteBW.RunWorkerAsync();
            
        }

        private void adBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            if ((adFName.Text ?? adLName.Text ?? adPhoneNum.Text ?? adPhoneNum.Text ?? adTitle.Text ?? adDesc.Text) == null)
            {
                MessageBox.Show("Please fill in all fields.");
            } else
            {

                userCreated = controller.AddADUser(adFName, adLName, adPhoneNum, adTitle, adDesc);

            }
 
            
 
        }

        private void adBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            pbAdd.Visible = false;

            if (userCreated == true)
            {
                MessageBox.Show("Accounts for "+adFName+" "+adLName+" have been created successfully! Operation complete.");
            }
            else
            {
                MessageBox.Show("Opoeration failed. Contact your System Administrator.");
            }            

        }

        private void adEditBW_DoWork(object sender, DoWorkEventArgs e)
        {

            if ((tbFName.Text ?? tbLName.Text ?? tbEmail.Text ?? tbPhone.Text ?? tbDesc.Text) == null)
            {

                MessageBox.Show("Select & fill in at least one field.");

            } else
            {

                tbFName.Tag = "GivenName";
                tbLName.Tag = "SurName";
                tbPhone.Tag = "MobilePhone";
                tbEmail.Tag = "EmailAddress";
                tbDesc.Tag = "description";


                if (cbFName.Checked)
                {
                    boxes.Add(tbFName);
                }

                if (cbLName.Checked)
                {
                    boxes.Add(tbLName);
                }

                if (cbEmail.Checked)
                {
                    boxes.Add(tbEmail);
                }

                if (cbPhone.Checked)
                {
                    boxes.Add(tbPhone);
                }

                if (cbDesc.Checked)
                {
                    boxes.Add(tbDesc);
                }

                controller.editADUser(userActLbl.Text, true, boxes);

            }

           

        }

        private void adEditBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void adDeleteBW_DoWork(object sender, DoWorkEventArgs e)
        {

            if (tbDelete.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to delete.");
            }

            controller.deleteUsers(tbDelete);

        }

        private void adDeleteBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void adSearchBW_DoWork(object sender, DoWorkEventArgs e)
        {

            if (EditSearchBox.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to makes changes to.");
            } else
            {
                string[] vs = controller.retrieveAD(EditSearchBox.Text);

                tbFName.Text = vs[0];
                tbLName.Text = vs[1];
                tbEmail.Text = vs[2];

                userActLbl.Text = vs[3];
            }

           

        }

        private void adSearchBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void adSearchDeleteBW_DoWork(object sender, DoWorkEventArgs e)
        {

            if (tbToDelete.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to makes changes to.");
            } else
            {

                string[] toDelete = controller.retrieveAD(tbToDelete.Text);
                name = toDelete[3];

                tbDelete.Text = name;

                MessageBox.Show("Account information for "+ name +" returned successfully!");

            }
            

         

        }

        private void adSearchDeleteBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //CMD cMD = new CMD();

            //cMD.startBAT();

            AppFuncs appFuncs = new AppFuncs();

            appFuncs.startBAT();

        }
    }

   

}
