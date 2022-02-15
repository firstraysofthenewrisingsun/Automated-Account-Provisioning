using System;
using System.Text;
using System.Windows.Forms;

namespace WinHRTool
{
    public partial class InitialSetup : Form
    {
        private CredProtect Protect;
        public InitialSetup()
        {
            InitializeComponent();
            Protect = new CredProtect();
        }

        private void submitOnClick(object sender, EventArgs e)
        {

            string emailPass = Properties.Settings.Default.emailPass;
            string emailuser = Properties.Settings.Default.emailUser;
            byte[] vs1 = System.Convert.FromBase64String(emailPass);
            byte[] vs = Protect.Decrypt(vs1);

            string v = System.Text.Encoding.Default.GetString(vs);

            MessageBox.Show("Credentials saved. Proceeding to management console."+ " "+emailuser);

            HRTool hRTool = new HRTool();
            this.Hide();
            hRTool.ShowDialog();
            this.Show();

        }

        private void encryptEmail_Click(object sender, EventArgs e)
        {

            if ((initTbEmail.Text ?? initTbEmailPass.Text) == null)
            {
                MessageBox.Show("Enter credentials to the email used for sending notifications.");

            } else
            {
                Properties.Settings.Default.emailUser = initTbEmail.Text;

                byte[] convertedPass = Encoding.ASCII.GetBytes(initTbEmailPass.Text);

                byte[] task = Protect.Encrypt("EMAIL", convertedPass);

                if (task != null)
                {
                    MessageBox.Show("Email credentials encrypted! ");
                }
                else
                {
                    MessageBox.Show("Data was not encrypted. An error occurred.");
                }
            }
            
        }

        private void encryptAD_Click(object sender, EventArgs e)
        {
            if((initTbADDC.Text ?? initTbADDomain.Text ?? initTbADPass.Text ?? initTbADPath.Text ?? initTbADUser.Text) == null)
            {

                MessageBox.Show("Enter credentials for the LUS AD account.");

            } else
            {

                Properties.Settings.Default.adPDC = initTbADDC.Text;
                Properties.Settings.Default.adPath = initTbADPath.Text;
                Properties.Settings.Default.adDomain = initTbADDomain.Text;
                Properties.Settings.Default.adUser = initTbADUser.Text;



                byte[] convertedPass = Encoding.ASCII.GetBytes(initTbADPass.Text);

                byte[] task = Protect.Encrypt("AD", convertedPass);



                if (task != null)
                {
                    MessageBox.Show("Email credentials encrypted!");
                }
                else
                {
                    MessageBox.Show("Data was not encrypted. An error occurred.");
                }

            }
           
        }

      
    }
}
