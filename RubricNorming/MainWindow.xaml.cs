using DataAccessFakes;
using DataObjects;
using LogicLayer;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace RubricNorming
{

    public partial class MainWindow : Window
    {

        UserManager _userManager = null;
        User _user = null;
        RubricManager _rubricManager = null;
        FacetManager _facetManager = null;


        public MainWindow()
        {
            // uses default accessors
            _userManager = new UserManager();
            _rubricManager = new RubricManager();
            _facetManager = new FacetManager();

            // uses the fake accessors
            //_userManager = new UserManager(new UserAccessorFake());
            //_rubricManager = new RubricManager(new RubricAccessorFake(), new UserAccessorFake());
            //_facetManager = new FacetManager(new FacetAccessorFake());


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
                            updateUIforUser();
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
                            updateUIforLogOut();
                            MessageBox.Show("You did not update your password. You will be logged out.");
                        }
                    }
                    else if (_user != null)
                    {
                        updateUIforUser();
                    }

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
                updateUIforLogOut();
            }

            
        }

        private void updateUIforUser()
        {
            string rolesList = "";
            for (int i = 0; i < _user.Roles.Count; i++)
            {
                rolesList += " " + _user.Roles[i];
                if (i == _user.Roles.Count -2)
                {
                    rolesList += " and";
                }
                else if (i< _user.Roles.Count-2)
                {
                    rolesList += ",";
                }
            }
            MessageBox.Show("Welcome back, " + _user.GivenName +
                "\n\n" + "Your roles are:" + rolesList);

            lblLogin.Content = "";
            staMessage.Content = _user.UserID + " logged in as" + rolesList + " on " + DateTime.Now.ToShortTimeString();

            txtUserID.Text = "";
            pwdPassword.Password = "";
            txtUserID.Visibility = Visibility.Hidden;
            pwdPassword.Visibility = Visibility.Hidden;
            lblUserID.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;

            stkRubricControls.Visibility = Visibility.Visible;
            datViewList.Visibility = Visibility.Visible;
            mnuView.Visibility = Visibility.Visible;
            viewAllActiveRubrics();

            btnLogin.Content = "Log Out";
            btnLogin.IsDefault = false;

            //showTabsForUser();

        }

        private void updateUIforLogOut()
        {
            _user = null;

            staMessage.Content = "Welcome. Please log in to continue.";

            txtUserID.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            lblUserID.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;

            stkRubricControls.Visibility = Visibility.Hidden;
            datViewList.Visibility = Visibility.Hidden;
            mnuView.Visibility = Visibility.Hidden;

            btnLogin.Content = "Login";
            btnLogin.Focus();

            btnLogin.IsDefault = true;

            //hide all user tabs

            txtUserID.Focus();
        }

        private void frmMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtUserID.Focus();

            stkRubricControls.Visibility = Visibility.Hidden;
            datViewList.Visibility = Visibility.Hidden;
            mnuView.Visibility = Visibility.Hidden;

            //viewAllActiveRubrics();
            //hideAllUserTabs();
        }

        private void viewAllActiveRubrics()
        {
            List<Rubric> rubricList = null;
            try
            {
                rubricList = _rubricManager.RetrieveActiveRubrics();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem retrieving the list of rubrics." + ex.Message);
            }

            // needs better time formating and column names
            var rubricListSorted = rubricList.Select(r => new { r.Name, r.Description, r.DateCreated, r.DateUpdated, r.ScoreTypeID, RubricCreatorName = r.RubricCreator.GivenName + " " + r.RubricCreator.FamilyName });


            datViewList.ItemsSource = rubricListSorted;
        }

        private void viewAllActivateFacets()
        {
            List<Facet> facets = null;

            try
            {
                facets = _facetManager.RetrieveActiveFacets();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem retrieving the list of facets." + ex.Message);
            }
            
            datViewList.ItemsSource = facets;


        }

        private void btnRetrieveActiveRubrics_Click(object sender, RoutedEventArgs e)
        {
            viewAllActiveRubrics();
        }

        private void mnuViewAllRubrics_Click(object sender, RoutedEventArgs e)
        {
            viewAllActiveRubrics();
        }

        private void mnuViewAllFacets_Click(object sender, RoutedEventArgs e)
        {
            viewAllActivateFacets();
        }
    }
}
