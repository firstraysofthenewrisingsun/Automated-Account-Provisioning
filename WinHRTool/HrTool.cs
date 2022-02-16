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

            if ((tbFName.Text ?? tbLName.Text ?? tbEmail.Text ?? tbPhone.Text ?? tbDesc.Text) == null)
            {

                MessageBox.Show("Select & fill in at least one field.");

            } else
            {
                pbEdit.Visible=true;
                adEditBW.RunWorkerAsync();
            }
            
           
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

        private void adBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

                userCreated = controller.AddADUser(adFName, adLName, adPhoneNum, adTitle, adDesc);

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
                MessageBox.Show("Operation failed. Contact your System Administrator.");
            }            

        }

        private void adEditBW_DoWork(object sender, DoWorkEventArgs e)
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

        private void adEditBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbEdit.Visible = false;
        }

        private void adDeleteBW_DoWork(object sender, DoWorkEventArgs e)
        {

            controller.deleteUsers(tbDelete);

        }

        private void adDeleteBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbDelete.Visible = false;
        }

        private void adSearchBW_DoWork(object sender, DoWorkEventArgs e)
        {

                string[] vs = controller.retrieveAD(EditSearchBox.Text);

                tbFName.Text = vs[0];
                tbLName.Text = vs[1];
                tbEmail.Text = vs[2];

                userActLbl.Text = vs[3];

        }

        private void adSearchBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            pbEdit.Visible = false;



        }

        private void adSearchDeleteBW_DoWork(object sender, DoWorkEventArgs e)
        {

                string[] toDelete = controller.retrieveAD(tbToDelete.Text);
                name = toDelete[3];

                tbDelete.Text = name;

                MessageBox.Show("Account information for "+ name +" returned successfully!");

        }

        private void adSearchDeleteBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbDelete.Visible = false;
        }

      
        private void editSearch_Onclick(object sender, EventArgs e)
        {

            if (EditSearchBox.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to makes changes to.");
            }
            else
            {

                var handle = this.Handle;

                NativeWindow win32Parent = new NativeWindow();
                win32Parent.AssignHandle(handle);

                DialogResult result = MessageBox.Show(win32Parent, "Test", "New Account for !", MessageBoxButtons.YesNo);

                switch (result)
                {
                    case DialogResult.Yes:
                        pbEdit.Visible=true;
                        adSearchBW.RunWorkerAsync();

                        break;
                    case DialogResult.No:
                        break;
                }

            }


            

        }

        private void add_OnClick(object sender, EventArgs e)
        {

            if ((adFName.Text ?? adLName.Text ?? adPhoneNum.Text ?? adPhoneNum.Text ?? adTitle.Text ?? adDesc.Text) == null)
            {

                MessageBox.Show("Please fill in all fields.");

            }
            else
            {

                var handle = this.Handle;

                NativeWindow win32Parent = new NativeWindow();
                win32Parent.AssignHandle(handle);

                DialogResult result = MessageBox.Show(win32Parent, "Are you sure you'd like to create accounts for "+adFName.Text+" "+adLName.Text+"?", "Creation Confirmation", MessageBoxButtons.YesNo);

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

        }

        private void deleteSearch_OnClick(object sender, EventArgs e)
        {

            if (tbToDelete.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to makes changes to.");
            } else
            {
                pbDelete.Visible=true;
                adSearchDeleteBW.RunWorkerAsync();
            }
            
        }

        private void delete_OnClick(object sender, EventArgs e)
        {

            if (tbDelete.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to delete.");
            } else
            {
                pbDelete.Visible = true;
                adDeleteBW.RunWorkerAsync();
            }
            
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {

        }
    }

   

}
