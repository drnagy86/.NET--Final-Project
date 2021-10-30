using DataAccessFakes;
using DataObjects;
using LogicLayer;
using System;
using System.Windows;

namespace RubricNorming
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        UserManager _userManager = null;
        User _user = null;

        public MainWindow()
        {
            // uses default user accessor
            _userManager = new UserManager();

            // uses the fake user accessor
            //_userManager = new UserManager(new UserAccessorFake());

            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var userID = this.txtUserID.Text;
            var pwd = this.pwdPassword.Password;

            try
            {
                _user = _userManager.LoginUser(userID, pwd);

                string rolesList = "";
                foreach (var role in _user.Roles)
                {
                    rolesList += " " + role;
                }
                MessageBox.Show("Welcome back, " + _user.GivenName +
                    "\n\n" + "Your roles are:" + rolesList);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" +
                    ex.InnerException.Message,
                    "Alert!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                pwdPassword.Password = "";
            }
        }
    }
}
