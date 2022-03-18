using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
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
        private int createResultCode;
        private int editResultCode;
        private int deleteResultCode;
        private AppFuncs appFuncs;
       

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
            appFuncs = new AppFuncs();
          
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            var handle = this.Handle;

            NativeWindow win32Parent = new NativeWindow();
            win32Parent.AssignHandle(handle);

            DialogResult result = MessageBox.Show(win32Parent, "Create accounts for this user?", "Confirmation", MessageBoxButtons.YesNo);
            
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
            pbEdit.Visible=true;
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
            var handle = this.Handle;

            NativeWindow win32Parent = new NativeWindow();
            win32Parent.AssignHandle(handle);

            DialogResult result = MessageBox.Show(win32Parent, "Save changes to this account?", "Confirmation", MessageBoxButtons.YesNo);

            switch (result)
            {
                case DialogResult.Yes:
                    pbEdit.Visible = true;
                    adEditBW.RunWorkerAsync();

                    break;
                case DialogResult.No:
                    break;
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

        private void deleteButton_Click(object sender, EventArgs e)
        {
            pbDelete.Visible = true;

           adSearchDeleteBW.RunWorkerAsync();

        }

        private void deleteUserBut_Click(object sender, EventArgs e)
        {
            pbDelete.Visible=true;

            var handle = this.Handle;

            NativeWindow win32Parent = new NativeWindow();
            win32Parent.AssignHandle(handle);

            DialogResult result = MessageBox.Show(win32Parent, "Save changes to this account?", "Confirmation", MessageBoxButtons.YesNo);

            switch (result)
            {
                case DialogResult.Yes:

                    pbDelete.Visible = true;
                    adDeleteBW.RunWorkerAsync();

                    break;
                case DialogResult.No:
                    break;
            }

            
            
        }

        private void adBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

          

                createResultCode = controller.AddADUser(adFName, adLName, adPhoneNum, adTitle, adDesc);

            
 
            
 
        }

        private void adBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            pbAdd.Visible = false;

            switch (createResultCode)
            {
                case 302:
                    appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "New AD Account Failure", "HR App couldn't create the AD account for "+adFName.Text+" "+adLName.Text+".\nPlease investigate.");

                    MessageBox.Show("Failed to add "+adFName.Text+" "+adLName.Text+" to Active Directory. IT has been notified and will reach out with next steps.");

                    break;
                case 305:
                    appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "New Harvest Account Failure", "HR App couldn't create the Harvest account for "+adFName.Text+" "+adLName.Text+".\nPlease investigate.");

                    MessageBox.Show("Failed to add "+adFName.Text+" "+adLName.Text+" to Harvest. IT has been notified and will reach out with next steps.");

                    break;
                case 300:

                    MessageBox.Show("Accounts for "+adFName.Text+" "+adLName.Text+" have been created successfully! Operation complete.");
                    break;
                case 448:
                    appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "GCDS Failure", "HR App couldn't run GCDS.\nPlease investigate.");

                    MessageBox.Show("Failed to sync Active Directory with Google Workspace. IT has been notified and will reach out with next steps.");

                    break;
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

               editResultCode = controller.editADUser(userActLbl.Text, true, boxes);

        }

        private void adEditBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbEdit.Visible = false;

            switch (editResultCode)
            {
                case 202:
                    //appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.techSupportEmail, "Edit AD Account Failure", "HR App couldn't edit the AD account for "+tbFName+" "+tbLName+".\nPlease investigate.");

                    MessageBox.Show("Failed to save changes to the Active Directory account for "+tbFName+" "+tbLName+". Please check that you've selected and filled all of the fields to edit. IT has been notified and will reach out with next steps.");

                    break;
                case 205:
                    //appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.techSupportEmail, "Edit Harvest Account Failure", "HR App couldn't edit the Harvest account for "+adFName+" "+adLName+".\nPlease investigate.");

                    MessageBox.Show("Failed to save changes to the Harvest account for "+adFName+" "+adLName+". IT has been notified and will reach out with next steps.");

                    break;
                case 200:

                    MessageBox.Show("Accounts for "+adFName+" "+adLName+" have been created successfully! Operation complete.");
                    break;
                case 448:
                    //appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.techSupportEmail, "GCDS Failure", "HR App couldn't run GCDS.\nPlease investigate.");

                    MessageBox.Show("Failed to sync Active Directory with Google Workspace. IT has been notified and will reach out with next steps.");

                    break;
            }

        }

        private void adDeleteBW_DoWork(object sender, DoWorkEventArgs e)
        {

            if (tbDelete.Text == null)
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to delete.");
            }

            deleteResultCode = controller.deleteUsers(tbDelete);

        }

        private void adDeleteBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbDelete.Visible = false;

            switch (deleteResultCode)
            {
                case 202:
                    appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "Delete AD Account Failure", "HR App couldn't delete the AD account for "+tbDelete+".\nPlease investigate.");

                    MessageBox.Show("Failed to delete the Active Directory account for "+tbDelete+". IT has been notified and will reach out with next steps.");

                    break;
                case 205:
                    appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "Archive Harvest Account Failure", "HR App couldn't archive the Harvest account for "+tbDelete+".\nPlease investigate.");

                    MessageBox.Show("Failed to archive the Harvest account for "+tbDelete+". IT has been notified and will reach out with next steps.");

                    break;
                case 200:

                    MessageBox.Show("Accounts for "+adFName+" "+adLName+" have been created successfully! Operation complete.");
                    break;
                case 448:
                    appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "GCDS Failure", "HR App couldn't run GCDS.\nPlease investigate.");

                    MessageBox.Show("Failed to sync Active Directory with Google Workspace. IT has been notified and will reach out with next steps.");

                    break;
            }

        }

        private void adSearchBW_DoWork(object sender, DoWorkEventArgs e)
        {

            string[] vs = controller.retrieveAD(EditSearchBox.Text);
                
            if (vs[0] == "System.ArgumentException")
            {
                MessageBox.Show("Enter the last, full or username of the account you'd wish to makes changes to.");
            } else
            {

                tbFName.Text = vs[0];
                tbLName.Text = vs[1];
                tbEmail.Text = vs[2];

                userActLbl.Text = vs[3];

            }

               

        }

        private void adSearchBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbEdit.Visible = false;

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
            pbDelete.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LastPassAPI lastPassAPI = new LastPassAPI();

            lastPassAPI.deleteLP("hrapp@cardinalpeak.com");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GAdminSDK gAdminSDK = new GAdminSDK();

            gAdminSDK.listUsers();
        }
    }

   

}
