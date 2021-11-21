using DataAccessFakes;
using DataObjects;
using LogicLayer;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace RubricNorming
{

    public partial class MainWindow : Window
    {

        UserManager _userManager = null;
        User _user = null;
        RubricManager _rubricManager = null;
        FacetManager _facetManager = null;
        CriteriaManager _criteriaManager = null;

        string _executionChoice = "";

        double HEADING_ONE_MULTIPLIER = 1.5;

        RubricVM _rubricVM = null;

        public MainWindow()
        {
            // uses default accessors
            _userManager = new UserManager();
            _rubricManager = new RubricManager();
            _facetManager = new FacetManager();
            _criteriaManager = new CriteriaManager();

            // uses the fake accessors
            //_userManager = new UserManager(new UserAccessorFake());
            //_rubricManager = new RubricManager(new RubricAccessorFake(), new UserAccessorFake());
            //_facetManager = new FacetManager(new FacetAccessorFake());
            //_criteriaManager = new CriteriaManager(new CriteriaAccessorFake());

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

        private void prepareDckFormForRetrieveFacetsByRubricID()
        {
            txtblkDockPanelTitle.Text = "Retrieve Facet by Rubric ID";
            lblInput1.Content = "Rubric ID:";
            _executionChoice = "FacetsByRubricID";

            dckForm.Visibility = Visibility.Visible;
            txtBoxInput1.Focus();
        }

        private void prepareDckFormForRetrieveCriteriaByRubicID()
        {
            txtblkDockPanelTitle.Text = "Retrieve Criteria by Rubric ID";
            lblInput1.Content = "Rubric ID:";
            _executionChoice = "CriteriaByRubricID";

            dckForm.Visibility = Visibility.Visible;
            txtBoxInput1.Focus();
        }

        private void frmMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtUserID.Focus();

            stkRubricControls.Visibility = Visibility.Hidden;
            datViewList.Visibility = Visibility.Hidden;
            dckForm.Visibility = Visibility.Hidden;
            
            // for testing purposes, comment out
            //mnuView.Visibility = Visibility.Hidden;
            

            viewAllActiveRubrics();
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
            //var rubricListSorted = rubricList.Select(r => new { r.Name, r.Description, r.DateCreated, r.DateUpdated, r.ScoreTypeID, RubricCreatorName = r.RubricCreator.GivenName + " " + r.RubricCreator.FamilyName });

            datViewList.ItemsSource = rubricList;
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

            var facetListSorted = facets.Select(f => new { f.FacetID, f.Description, f.DateCreated, f.DateUpdated, f.FacetType });            
            
            datViewList.ItemsSource = facetListSorted;
            datViewList.Visibility = Visibility.Visible;
        }

        private void btnConfirmForm_Click(object sender, RoutedEventArgs e)
        {
            switch (_executionChoice)
            {
                case "FacetsByRubricID":

                    List<Facet> facetList = null;
                    try
                    {
                        facetList = _facetManager.RetrieveFacetsByRubricID(Int32.Parse(txtBoxInput1.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was a problem retrieving the list of facets." + ex.Message);
                    }

                    var facetListSorted = facetList.Select(f => new { f.FacetID, f.Description, f.FacetType });

                    datViewList.ItemsSource = facetListSorted;
                    datViewList.Visibility = Visibility.Visible;

                    break;
                case "CriteriaByRubricID":

                    List<Criteria> criteriaList = null;
                    try
                    {
                        criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(Int32.Parse(txtBoxInput1.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was a problem retrieving the list of criteria." + ex.Message);
                    }

                    var criteriaListSorted = criteriaList.Select(c => new { c.FacetID, c.CriteriaID, c.Content, c.Score });


                    datViewList.ItemsSource = criteriaListSorted;
                    datViewList.Visibility = Visibility.Visible;

                    break;
                default:
                    break;
            }           

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
        private void mnuRetrieveFacetsByRubricID_Click(object sender, RoutedEventArgs e)
        {
            prepareDckFormForRetrieveFacetsByRubricID();
        }

        private void mnuRetrieveCriteriaByRubricID_Click(object sender, RoutedEventArgs e)
        {
            prepareDckFormForRetrieveCriteriaByRubicID();
        }

        private RubricVM createRubricVM(Rubric rubric)
        {
            List<Facet> facetList = null;
            List<Criteria> criteriaList = null;
            try
            {
                facetList = _facetManager.RetrieveFacetsByRubricID(rubric.RubricID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem retrieving the list of facets." + ex.Message);
            }
            try
            {
                criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(rubric.RubricID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem retrieving the list of criteria." + ex.Message);
            }

            RubricVM rubricVM = new RubricVM(rubric, facetList, criteriaList);
            return rubricVM;
        }

        private void datViewList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Build rubric the hard way


            var rubric = (Rubric)datViewList.SelectedItem;

            datViewList.Visibility = Visibility.Collapsed;


            _rubricVM = createRubricVM(rubric);

            this.DataContext = _rubricVM;
            

            icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;

            icFacetCriteria.Visibility = Visibility.Visible;
            


            //int rowCount = 0;

            //for (int i = 0; i < _rubricVM.FacetCriteria.Count; i++)
            //{
            //    ColumnDefinition column = new ColumnDefinition();
            //    grdActionArea.ColumnDefinitions.Add(column);

            //    if (_rubricVM.FacetCriteria.ElementAt(i).Value.Count > rowCount)
            //    {
            //        rowCount = _rubricVM.FacetCriteria.ElementAt(i).Value.Count;
            //    }
            //}

            //foreach (var entry in _rubricVM.FacetCriteria)
            //{
            //    ColumnDefinition column = new ColumnDefinition();
            //    grdActionArea.ColumnDefinitions.Add(column);

            //    if (entry.Value.Count > rowCount)
            //    {
            //        rowCount = entry.Value.Count;
            //    }
            //}

            //// add one extra row for the header
            //for (int i = 0; i < rowCount + 1; i++)
            //{
            //    RowDefinition row = new RowDefinition();
            //    grdActionArea.RowDefinitions.Add(row);
            //}


            //for (int i = 0; i < _rubricVM.FacetCriteria.Count; i++)
            //{
            //    Label facetHeader = new Label();
            //    facetHeader.Content = _rubricVM.FacetCriteria.ElementAt(i).Key.FacetID;

            //    // add more formating for header   
            //    facetHeader.FontWeight = FontWeights.Bold;
            //    facetHeader.FontSize = facetHeader.FontSize * HEADING_ONE_MULTIPLIER;
            //    facetHeader.HorizontalAlignment = HorizontalAlignment.Center;
            //    facetHeader.VerticalAlignment = VerticalAlignment.Center;




            //    //Grid.SetColumn(facetHeader, i + 1);
            //    //Grid.SetRow(facetHeader, 0);
            //    //grdActionArea.Children.Add(facetHeader);

            //    facetHeader.SetValue(Grid.ColumnProperty, i + 1);
            //    facetHeader.SetValue(Grid.RowProperty, 0);
            //    grdActionArea.Children.Add(facetHeader);

            //    foreach (Criteria criteria in _rubricVM.FacetCriteria.ElementAt(i).Value)
            //    {

            //        //TextBox textBox = new TextBox();
            //        //textBox.Text = criteria.Content;
            //        //Grid.SetColumn(textBox, i + 1);
            //        //Grid.SetRow(textBox, rubricVM.FacetCriteria.ElementAt(i).Value.IndexOf(criteria) + 1);
            //        //grdActionArea.Children.Add(textBox);

            //        RichTextBox criteriaTxtBox = new RichTextBox();
            //        FlowDocument document = new FlowDocument();
            //        Paragraph paragraph = new Paragraph();

            //        paragraph.Inlines.Add(criteria.Content);
            //        document.Blocks.Add(paragraph);
            //        criteriaTxtBox.Document.Blocks.Add(paragraph);

            //        //Grid.SetColumn(criteriaTxtBox, i + 1);
            //        //Grid.SetRow(criteriaTxtBox, rubricVM.FacetCriteria.ElementAt(i).Value.IndexOf(criteria) + 1);

            //        criteriaTxtBox.SetValue(Grid.ColumnProperty, i + 1);
            //        criteriaTxtBox.SetValue(Grid.RowProperty, _rubricVM.FacetCriteria.ElementAt(i).Value.IndexOf(criteria) + 1);

            //        grdActionArea.Children.Add(criteriaTxtBox);
            //    }
            //}
        }
    }
}
