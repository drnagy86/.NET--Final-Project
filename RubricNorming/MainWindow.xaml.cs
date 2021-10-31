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


            if (btnLogin.Content.ToString() == "Login")
            {

                try
                {
                    _user = _userManager.LoginUser(userID, pwd);


                    string instructions = "On first login, all new users must choose a password to continue.";

                    if (_user != null && pwd == "newuser")
                    {
                        var upDateWindow = new UpdatePasswordWindow(_userManager, _user, instructions, true);

                        bool? result = upDateWindow.ShowDialog();
                        if (result == true)
                        {
                            //updateUIForUser();
                            string rolesList = "";
                            foreach (var role in _user.Roles)
                            {
                                rolesList += " " + role;
                            }
                            MessageBox.Show("Welcome back, " + _user.GivenName +
                                "\n\n" + "Your roles are:" + rolesList);
                        }
                        else
                        {
                            _user = null;
                            //updateUIforLogOut();
                            MessageBox.Show("You did not update your password. You will be logged out.");
                        }
                    }
                    else if (_user != null)
                    {
                        //updateUIforUser();
                    }



                    //string rolesList = "";
                    //foreach (var role in _user.Roles)
                    //{
                    //    rolesList += " " + role;
                    //}
                    //MessageBox.Show("Welcome back, " + _user.GivenName +
                    //    "\n\n" + "Your roles are:" + rolesList);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + "\n\n" +
                        ex.InnerException.Message,
                        "Alert!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    pwdPassword.Password = "";
                    txtUserID.Select(0, Int32.MaxValue);
                    txtUserID.Focus();
                }

            }
            else
            {
                //upDateUIforLogOut();
            }

            
        }
    }
}
