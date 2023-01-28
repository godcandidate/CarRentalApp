using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainForm : Form
    {
        private login _login;
        public loginInfo _user;
        public string _roleName;
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(login login, loginInfo user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.userRoles.FirstOrDefault().Role.shortname;
        }


        // preventing a particlar form from apper=aring more than once
        public bool ItExist(string form_name)
        {
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(window => window.Name == form_name);
            return isOpen;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // using uitils class
            if (!Utils.FormsIsOpen("AddAddEditRentalForm"))
            {
                // using MDI form
                var addRentalform = new AddEditRentalForm();
                addRentalform.ShowDialog();
                addRentalform.MdiParent = this;
            }
        }

        private void manageVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using MDI form
            if (!ItExist("ManageVehicleListing"))
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;
                vehicleListing.Show();
            }
            
           
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using MDI form
            var rentalRecordform = new ManageRentalRecord();
            rentalRecordform.MdiParent = this;
            rentalRecordform.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_user.pass_word == Utils.DefaultPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();
            }
            var username = _user.username;
            tssloginstatus.Text = $"Logged in As: {username}";

            if (_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Visible = false;

            }
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // using uitils class
            if (!Utils.FormsIsOpen("ManageUser"))
            {
                // using MDI form
                var manageuserform = new ManageUser();
                manageuserform.MdiParent = this;
                manageuserform.Show();
                
            }
        }
    }
}
