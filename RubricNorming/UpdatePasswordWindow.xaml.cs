using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RubricNorming
{
    /// <summary>
    /// Interaction logic for UpdatePasswordWindow.xaml
    /// </summary>
    public partial class UpdatePasswordWindow : Window
    {

        UserManager _userManager = null;
        User _user = null;
        bool _newUser;

        public UpdatePasswordWindow(UserManager userManager, User user, string instructions, bool newUser = false)
        {
            _userManager = userManager;
            _user = user;
            _newUser = newUser;

            InitializeComponent();

            txtInstructions.Text = instructions;
            pwdOldPassword.Focus();

            if (_newUser)
            {
                newUserUpdate();
            }
        }

        private void newUserUpdate()
        {
            pwdOldPassword.Password = "newuser";
            pwdOldPassword.IsEnabled = false;
            pwdNewPassword.Focus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (pwdNewPassword.Password != pwdRetypePassword.Password)
            {
                MessageBox.Show("New password and retype password must match");
                pwdNewPassword.Password = "";
                pwdOldPassword.Password = "";
                pwdRetypePassword.Password = "";
                pwdNewPassword.Focus();

                return;
            }

            try
            {
                string oldPassword = pwdOldPassword.Password;
                string newPassword = pwdNewPassword.Password;

                if (_userManager.ResetPassword(_user.UserID, oldPassword, newPassword))
                {
                    MessageBox.Show("Password successfully updated");
                    this.DialogResult = true;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Update failed.\n\n" + ex.Message);
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
