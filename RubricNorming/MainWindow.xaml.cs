using DataAccessFakes;
using DataObjects;
using LogicLayer;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using ModernWpf;
using ModernWpf.Controls;
using Windows.Foundation.Metadata;

namespace RubricNorming
{
    public partial class MainWindow : Window
    {

        UserManager _userManager = null;
        User _user = null;
        FacetManager _facetManager = null;
        CriteriaManager _criteriaManager = null;
        RubricVMManager _rubricVMManager = null;
        ScoreTypeManager _scoreTypeManager = null;
        FacetTypeManager _facetTypeManager = null;
        SubjectManager _subjectManager = null;
        RubricSubjectManager _rubricSubjectManager = null;

        

        UIState _currentUIState;

        IRubricManager<Rubric> _rubricManager = null;

        string _executionChoice = "";

        List<ScoreType> _scoreTypes = new List<ScoreType>();
        List<FacetType> _facetTypes = new List<FacetType>();

        Rubric _rubric = null;
        RubricVM _rubricVM = null;
        RubricVM _oldRubricVM = null;
        List<Facet> _facets = null;
        List<Criteria> _criteriaList = null;
        List<RubricSubject> _rubricSubjects = null;

        bool _criteriaIDChangedFlag = false;
        bool _criteriaContentsChangedFlag = false;
        bool _facetDescriptionContentChangedFlag = false;
        bool _unsavedWorkFlag = false;


        public MainWindow()
        {
            //// uses default accessors
            _userManager = new UserManager();
            _rubricManager = new RubricManager();
            _facetManager = new FacetManager();
            _criteriaManager = new CriteriaManager();
            _rubricVMManager = new RubricVMManager();
            _scoreTypeManager = new ScoreTypeManager();
            _facetTypeManager = new FacetTypeManager();
            _subjectManager = new SubjectManager();
            _rubricSubjectManager = new RubricSubjectManager();


            // uses the fake accessors
            //UserAccessorFake userAccessorFake = new UserAccessorFake();
            //_rubricManager = new RubricManager(new RubricAccessorFake(), userAccessorFake);
            //_userManager = new UserManager(userAccessorFake);
            //_facetManager = new FacetManager(new FacetAccessorFake());
            //_criteriaManager = new CriteriaManager(new CriteriaAccessorFake());
            //_rubricVMManager = new RubricVMManager(_rubricManager, _userManager, _facetManager, _criteriaManager);
            //_scoreTypeManager = new ScoreTypeManager(new ScoreTypeFake());
            //_facetTypeManager = new FacetTypeManager(new FacetTypeFakes());
            //_subjectManager = new SubjectManager(new SubjectAccessorFake());
            //_rubricSubjectManager = new RubricSubjectManager(new RubricSubjectAccessorFake());

            InitializeComponent();

            RoutedCommand closeWindow = new RoutedCommand();
            closeWindow.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(closeWindow, mnuExit_Click));
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
                                rolesList += "\n" + role;
                            }
                            //MessageBox.Show("Welcome back, " + _user.GivenName +
                            //    "\n\n" + "Your roles are:" + rolesList);
                            DialogControls.OneButton("Welcome Back", "Welcome back, " + _user.GivenName +
                                "\n\n" + "Your roles are:" + rolesList);
                        }
                        else
                        {
                            _user = null;
                            updateUIforLogOut();
                            //MessageBox.Show("You did not update your password. You will be logged out.");
                            DialogControls.OneButton("No Update", "You did not update your password. You will be logged out.");
                        }
                    }
                    else if (_user != null)
                    {
                        updateUIforUser();
                    }

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message + "\n\n" +
                    //    ex.InnerException.Message,
                    //    "Alert!",
                    //    MessageBoxButton.OK,
                    //    MessageBoxImage.Error);

                    DialogControls.OneButton("Alert!", ex.Message + "\n\n" +
                        ex.InnerException.Message, "Okay");

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
                if (i == _user.Roles.Count - 2)
                {
                    rolesList += " and";
                }
                else if (i < _user.Roles.Count - 2)
                {
                    rolesList += ",";
                }
            }
            //MessageBox.Show("Welcome back, " + _user.GivenName +
            //    "\n\n" + "Your roles are:" + rolesList);


            staMessage.Content = _user.UserID + " logged in as" + rolesList + " on " + DateTime.Now.ToShortTimeString();

            txtUserID.Text = "";
            pwdPassword.Password = "";
            txtUserID.Visibility = Visibility.Collapsed;
            pwdPassword.Visibility = Visibility.Collapsed;
            lblUserID.Visibility = Visibility.Collapsed;
            lblPassword.Visibility = Visibility.Collapsed;

            //grdEmail.Visibility = Visibility.Collapsed;
            //grdPassword.Visibility = Visibility.Collapsed;

            btnLogin.Content = "Log Out";
            btnLogin.IsDefault = false;

            prepareUIForRoles();

        }

        private void updateUIforLogOut()
        {
            _user = null;

            staMessage.Content = "Welcome. Please log in to continue.";

            txtUserID.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            lblUserID.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;

            hideAllControls();

            btnLogin.Content = "Login";
            btnLogin.Focus();
            btnLogin.IsDefault = true;

            //hide all user tabs

            txtUserID.Focus();
        }

        //private void preparegrdCreateControlsForRetrieveFacetsByRubricID()
        //{
        //    txtblkDockPanelTitle.Text = "Retrieve Facet by Rubric ID";
        //    lblInput1.Content = "Rubric ID:";
        //    _executionChoice = "FacetsByRubricID";

        //    grdCreateControls.Visibility = Visibility.Visible;
        //    txtBoxInput1.Focus();
        //}

        //private void preparegrdCreateControlsForRetrieveCriteriaByRubicID()
        //{
        //    txtblkDockPanelTitle.Text = "Retrieve Criteria by Rubric ID";
        //    lblInput1.Content = "Rubric ID:";
        //    _executionChoice = "CriteriaByRubricID";

        //    grdCreateControls.Visibility = Visibility.Visible;
        //    txtBoxInput1.Focus();
        //}

        private void frmMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtUserID.Focus();

            try
            {
                _scoreTypes = _scoreTypeManager.RetrieveScoreTypes();
                cmbBoxScoreTypes.ItemsSource = _scoreTypes.Select(st => st.ScoreTypeID);
                cmbBoxScoreTypes.SelectedItem = _scoreTypes.ElementAt(0).ScoreTypeID;
                txtBlockScoreTypeDescription.Text = _scoreTypes.ElementAt(0).Description;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Problem retrieving score types.\n" + ex.Message);
                DialogControls.OneButton("Error", "Problem retrieving score types.\n" + ex.Message);


            }

            try
            {
                _facetTypes = _facetTypeManager.RetrieveFacetTypes();
                cmbBoxFacetType.ItemsSource = _facetTypes.Select(ft => ft.FacetTypeID);
                cmbBoxFacetType.SelectedItem = _facetTypes.ElementAt(0).FacetTypeID;
                txtBlockFacetTypeDescription.Text = _facetTypes.ElementAt(0).Description;

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Problem retrieving facet types.\n" + ex.Message);
                DialogControls.OneButton("Error", "Problem retrieving facet types.\n" + ex.Message);
            }

            try
            {
                datSubjects.ItemsSource = _subjectManager.RetrieveSubjects();
            }
            catch (Exception ex)
            {
                DialogControls.OneButton("Error", "Problem retrieving subjects.\n" + ex.Message);
            }


            hideAllControls();


            // for testing purposes only

            //datViewList.Visibility = Visibility.Visible;
            //viewAllActiveRubrics();

        }

        private void hideAllControls()
        {
            stkRubricControls.Visibility = Visibility.Hidden;
            stkCreateRubric.Visibility = Visibility.Hidden;

            datViewList.Visibility = Visibility.Hidden;
            tabsetCreateControls.Visibility = Visibility.Collapsed;
            brdTabSetCreateControlsBorder.Visibility = Visibility.Hidden;

            detailActionArea.Visibility = Visibility.Hidden;

            mnuView.Visibility = Visibility.Collapsed;
            mnuEdit.Visibility = Visibility.Collapsed;
            mnuCreate.Visibility = Visibility.Collapsed;
            mnuAdmin.IsEnabled = false;
            mnuAdmin.Visibility = Visibility.Collapsed;
            mnuSaveRubric.IsEnabled = false;



            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnDeleteRubric.Visibility = Visibility.Collapsed;

        }

        private void prepareUIForRoles()
        {
            // ('Creator', 'Can create new rubrics and add examples')
            //,('Admin', 'Manages users, rubrics, tests, examples')
            //,('Assessor', 'Can view rubrics')
            //,('Norming Trainee', 'Can train and take tests for rubrics')

            foreach (var role in _user.Roles)
            {
                switch (role)
                {
                    case "Admin":
                        creatorUI();
                        adminUI();
                        break;
                    case "Creator":
                        creatorUI();
                        break;
                    case "Assessor":
                        viewerUI();
                        break;
                    default:
                        break;
                }
            }
        }

        private void adminUI()
        {
            mnuAdmin.IsEnabled = true;
            mnuAdmin.Visibility = Visibility.Visible;
        }

        private void creatorUI()
        {
            stkRubricControls.Visibility = Visibility.Visible;
            stkCreateRubric.Visibility = Visibility.Visible;

            datViewList.Visibility = Visibility.Visible;            

            mnuView.Visibility = Visibility.Visible;
            mnuEdit.Visibility = Visibility.Visible;
            mnuCreate.Visibility = Visibility.Visible;

            detailActionArea.Visibility = Visibility.Visible;

            viewAllActiveRubrics();

        }

        private void viewerUI()
        {
            stkRubricControls.Visibility = Visibility.Visible;

            detailActionArea.Visibility = Visibility.Visible;
            datViewList.Visibility = Visibility.Visible;

            mnuView.Visibility = Visibility.Visible;

            viewAllActiveRubrics();
        }

        private void viewAllActiveRubrics()
        {


            setCurrentUIState(UIState.ViewAll);

            

            btnCreateNewRubric.Visibility = Visibility.Visible;

            btnDeleteRubric.Visibility = Visibility.Collapsed;
            mnuAdminDeleteRubric.IsEnabled = false;

            tabsetCreateControls.Visibility = Visibility.Visible;
            brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;

            //tabRubricSubject.Visibility = Visibility.Visible;
            //tabCreate.Visibility = Visibility.Hidden;
            //tabFacetDetailView.Visibility = Visibility.Hidden;
            //tabFacets.Visibility = Visibility.Hidden;
            //tabScoreRange.Visibility = Visibility.Hidden;


            List<Rubric> rubricList = null;
            try
            {
                rubricList = _rubricManager.RetrieveActiveRubrics();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There was a problem retrieving the list of rubrics." + ex.Message);
                DialogControls.OneButton("Error","There was a problem retrieving the list of rubrics." + ex.Message);
            }

            lblActionAreaTitle.Content = "All Rubrics";
            lblActionAreaTitle.Visibility = Visibility.Visible;

            datViewList.ItemsSource = rubricList;

            datViewList.Visibility = Visibility.Visible;
            toggleListAndDetails();

            icFacetCriteria.Visibility = Visibility.Collapsed;
            //icScores.Visibility = Visibility.Collapsed;

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
                //MessageBox.Show("There was a problem retrieving the list of facets." + ex.Message);
                DialogControls.OneButton("Error", "There was a problem retrieving the list of facets." + ex.Message);
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
                        //facetList = _facetManager.RetrieveFacetsByRubricID(Int32.Parse(txtBoxInput1.Text));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("There was a problem retrieving the list of facets." + ex.Message);
                        DialogControls.OneButton("Error", "There was a problem retrieving the list of facets." + ex.Message);
                    }

                    var facetListSorted = facetList.Select(f => new { f.FacetID, f.Description, f.FacetType });

                    datViewList.ItemsSource = facetListSorted;
                    datViewList.Visibility = Visibility.Visible;

                    break;
                case "CriteriaByRubricID":

                    List<Criteria> criteriaList = null;
                    try
                    {
                        //criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(Int32.Parse(txtBoxInput1.Text));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("There was a problem retrieving the list of criteria." + ex.Message);
                        DialogControls.OneButton("Error", "There was a problem retrieving the list of criteria." + ex.Message);
                        
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

        private void datViewList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _rubric = (Rubric)datViewList.SelectedItem;

            setCurrentRubricVM();
            rubricVMDetailView();
            stkRubricControls.Visibility = Visibility.Visible;
        }

        private void setCurrentRubricVM()
        {
            try
            {
                // errors with fakes here
                _rubricVM = _rubricVMManager.RetrieveRubricByRubricID(_rubric.RubricID);

                staMessage.Content = "Viewing the rubric. Click \"Edit Rubric\" if you would like to make changes.";

                txtBoxTitle.Text = _rubricVM.Name;
                txtBoxDescription.Text = _rubricVM.Description;
                cmbBoxScoreTypes.SelectedItem = _rubricVM.ScoreTypeID;
                icFacetControls.ItemsSource = _rubricVM.Facets;



                _rubricSubjects = _rubricSubjectManager.RetrieveRubricSubjectsByRubricID(_rubricVM.RubricID);
                icRubricSubjects.ItemsSource = _rubricSubjects;

                //string rubricSubjectString = "";

                //int count = 0;
                //for (int i = _rubricSubjects.Count; i > 0; i--)
                //{
                //    if (i > 1)
                //    {
                //        rubricSubjectString += _rubricSubjects[count].SubjectID + ", ";

                //    }
                //    else
                //    {
                //        rubricSubjectString += _rubricSubjects[count].SubjectID;
                //    }

                //    count++;

                //}

                //txtBoxRubricSubject.Text = rubricSubjectString;

                foreach (ScoreType scoreType in _scoreTypes)
                {
                    if (scoreType.ScoreTypeID == _rubricVM.ScoreTypeID)
                    {
                        txtBlockScoreTypeDescription.Text = scoreType.Description;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Problem retrieving the single rubric." + ex.Message);
                DialogControls.OneButton("Error", "Problem retrieving the single rubric." + ex.Message);

                staMessage.Content = "Problem retrieving the single rubric." + ex.Message;
                viewAllActiveRubrics();
            }
        }

        private void rubricVMDetailView()
        {
            setCurrentUIState(UIState.ViewDetail);

            if (_user.Roles.Contains("Admin"))
            {
                btnDeleteRubric.Visibility = Visibility.Visible;
                mnuAdminDeleteRubric.IsEnabled = true;
            }


            lblActionAreaTitle.Content = _rubricVM.Name;

            tabCreate.Focus();

            this.DataContext = _rubricVM;
            icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;


        }


        private void rubricVMDetailViewForCreate()
        {

            datViewList.Visibility = Visibility.Hidden;
            toggleListAndDetails();
            

            lblActionAreaTitle.Content = _rubricVM.Name;

            grdActionArea.Visibility = Visibility.Visible;

            btnEditSelection.Visibility = Visibility.Collapsed;



            icFacetCriteria.IsEnabled = true;
            //icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;
            icFacetCriteria.Visibility = Visibility.Visible;

            //icScores.ItemsSource = _rubricVM.RubricScoreColumn();
            //icScores.Visibility = Visibility.Visible;

            
            lblActionAreaTitle.Visibility = Visibility.Visible;

        }

        private void toggleListAndDetails()
        {
            if (datViewList.Visibility == Visibility.Visible)
            {
                //btnSave.Visibility = Visibility.Hidden;
                //btnCancel.Visibility = Visibility.Hidden;
                //lblActionAreaTitle.Visibility = Visibility.Visible;

                mnuEditSelection.IsEnabled = false;
                
                btnEditSelection.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;
                btnDeactivateRubric.Visibility = Visibility.Collapsed;


                mnuConfirmUpdatesToRubric.Visibility = Visibility.Collapsed;
                mnuCancelUpdatesToRubric.Visibility = Visibility.Collapsed;
                mnuDeactivateRubric.Visibility = Visibility.Collapsed;
                mnuSaveRubric.IsEnabled = false;

                tabsetCreateControls.Visibility = Visibility.Visible;
                brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;

                tabRubricSubject.Visibility = Visibility.Visible;
                tabCreate.Visibility = Visibility.Collapsed;
                tabFacetDetailView.Visibility = Visibility.Collapsed;
                tabFacets.Visibility = Visibility.Collapsed;
                tabScoreRange.Visibility = Visibility.Collapsed;



                icFacetCriteria.Visibility = Visibility.Hidden;
                //icScores.Visibility = Visibility.Hidden;
            }
            else
            {
                //btnSave.Visibility = Visibility.Visible;
                //btnCancel.Visibility = Visibility.Visible;
                //lblActionAreaTitle.Visibility = Visibility.Visible;
                mnuEditSelection.IsEnabled = true;
                btnEditSelection.Visibility = Visibility.Visible;

                tabsetCreateControls.Visibility = Visibility.Visible;
                brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;



                icFacetCriteria.Visibility = Visibility.Visible;
                //icScores.Visibility = Visibility.Visible;
            }
        }

        private void mnuConfirmUpdatesToRubric_Click(object sender, RoutedEventArgs e)
        {
            updateRubric();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Are you sure you want to quit?\nAny unsaved work will be lost.", "Quit?", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            ////DialogControls.OkayCancel("Quit", "Are you sure you want to quit?\nAny unsaved work will be lost.");

            ////contentDialogOkayCancel("Quit", "Are you sure you want to quit?\nAny unsaved work will be lost.");
            //contentDialogQuit();

            //staMessage.Content = DialogControls.DialogResult.ToString();

            if (_unsavedWorkFlag)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to quit?\nAny unsaved work will be lost.", "Quit?", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                //DialogControls.OkayCancel("Quit", "Are you sure you want to quit?\nAny unsaved work will be lost.");

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        this.Close();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
                
            }
        }


        private async void contentDialogQuit()
        {
            ContentDialog oneButton = new ContentDialog
            {
                Title = "Quit",
                Content = "Are you sure you want to quit?\nAny unsaved work will be lost.",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            ContentDialogResult result = await oneButton.ShowAsync();

            switch (result)
            {
                case ContentDialogResult.None:
                    break;
                case ContentDialogResult.Primary:
                    staMessage.Content = "Did it";
                    break;
                case ContentDialogResult.Secondary:
                    break;
                default:
                    break;
            }
        }

        private bool validateRubricTitleAndDescription()
        {
            bool isValid = false;

            bool isValidTitle = ValidationHelpers.IsValidLength(txtBoxTitle.Text, 50);
            bool isValidDescription = ValidationHelpers.IsValidLength(txtBoxDescription.Text, 100);

            if (isValidTitle && isValidDescription)
            {
                isValid = true;
            }
            else if (!isValidTitle && !isValidDescription)
            {
                //MessageBox.Show("Invalid title and description for the rubric.", "Invalid Title and Description", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogControls.OneButton("Invalid Title and Description", "Invalid title and description for the rubric.");
                staMessage.Content = "Invalid title and description for the rubric.";
                txtBoxTitle.Focus();
            }
            else if (!isValidTitle)
            {
                //MessageBox.Show("Invalid title for the rubric.", "Invalid Title", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogControls.OneButton("Invalid Title", "Invalid title for the rubric.");
                staMessage.Content = "Invalid title for the rubric.";
                txtBoxTitle.Focus();

            }
            else if (!isValidDescription)
            {
                //MessageBox.Show("Invalid description for the rubric", "Invalid Description", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogControls.OneButton("Invalid Description", "Invalid description for the rubric");
                staMessage.Content = "Invalid description for the rubric.";
                txtBoxDescription.Focus();
            }

            return isValid;
        }

        private void btnCreateRubric_Click(object sender, RoutedEventArgs e)
        {
            
            if (validateRubricTitleAndDescription())
            {
                tabScoreRange.IsEnabled = true;
                tabScoreRange.Visibility = Visibility.Visible;
                sldCriteriaBottomRange.Value = 1;
                sldCriteriaTopRange.Value = 4;
                tabScoreRange.Focus();

                updateRubricFromForm();
            }
        }

        private void btnFacetAdd_Click(object sender, RoutedEventArgs e)
        {

            addFacetandCriteriaToRubricVM();
            updateICFacetCriteria();

            stkCreateRubric.Visibility = Visibility.Visible;


            btnSave.Visibility = Visibility.Visible;
            btnSave.IsEnabled = IsEnabled;
            mnuSaveRubric.IsEnabled = true;


        }

        private void sldCriteriaBottomRange_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldCriteriaBottomRange == null || sldCriteriaTopRange == null)
            {
                // do nothing, stops problems on load, saves results if changed during course of program
            }
            else if (sldCriteriaBottomRange.Value >= sldCriteriaTopRange.Value && sldCriteriaTopRange.Value <= sldCriteriaTopRange.Maximum)
            {
                sldCriteriaTopRange.Value++;
            }
        }

        private void sldCriteriaTopRange_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (sldCriteriaBottomRange == null || sldCriteriaTopRange == null)
            {
                // do nothing, stops problems on load, saves results if changed during course of program
            }
            else if (sldCriteriaTopRange.Value <= sldCriteriaBottomRange.Value && sldCriteriaBottomRange.Value >= sldCriteriaBottomRange.Minimum)
            {
                sldCriteriaBottomRange.Value--;
            }
        }

        private void mnuCancelUpdatesToRubric_Click(object sender, RoutedEventArgs e)
        {
            cancelUpdatesToRubric();
        }

        private void cancelUpdatesToRubric()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to undo all the edits? All changes will not be changed and the original rubric will be loaded.", "Cancel Edits", MessageBoxButton.OKCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    try
                    {
                        RubricVM oldRubricVM = _rubricVMManager.RetrieveRubricByRubricID(_rubric.RubricID);
                        _rubricVM = oldRubricVM;
                        this.DataContext = _rubricVM;
                        icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;
                        //icScores.ItemsSource = _rubricVM.RubricScoreColumn();
                        icFacetControls.ItemsSource = _rubricVM.Facets;

                        txtBoxTitle.Text = _rubricVM.Name;
                        txtBoxDescription.Text = _rubricVM.Description;
                        cmbBoxScoreTypes.SelectedItem = _rubricVM.ScoreTypeID;

                        setCurrentUIState(UIState.ViewAll);


                        btnEditSelection.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Collapsed;
                        mnuSaveRubric.IsEnabled = false;
                        btnCancel.Visibility = Visibility.Collapsed;
                        btnDeactivateRubric.Visibility = Visibility.Collapsed;


                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Problem retrieving old rubric \n " + ex.Message + " ");
                        DialogControls.OneButton("Error", "Problem retrieving old rubric \n " + ex.Message);
                        staMessage.Content = "Problem retrieving old rubric " + ex.Message + " ";
                    }
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    break;
            }

            
        }

        private void updateRubric()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save the edits? This can not be changed.", "Confirm Edits", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    try
                    {
                        _oldRubricVM = _rubricVMManager.RetrieveRubricByRubricID(_rubric.RubricID);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("There was a problem updating the rubric.\n " + ex.Message + " ", "Problem Updating Rubric");
                        DialogControls.OneButton("Problem Updating Rubric", "There was a problem updating the rubric.\n " + ex.Message);
                        staMessage.Content = "There was a problem creating the rubric. " + ex.Message + " ";
                    }


                    // not great, but removes focus if the event is triggered while the cursor is in the box
                    pwdPassword.Focus();

                    // find out if there were changes or not, send to the right place
                    string resultMessage = "";
                    bool rubricUpdated = false;
                    bool facetCriteriaDictionaryUpdated = false;
                    bool facetDescriptionUpdated = false;

                    if (isValidRubricUpdate())
                    {

                        try
                        {
                            rubricUpdated = _rubricManager.UpdateRubricByRubricID(_oldRubricVM.RubricID, _oldRubricVM.Name, txtBoxTitle.Text, _oldRubricVM.Description, txtBoxDescription.Text, _oldRubricVM.ScoreTypeID, cmbBoxScoreTypes.SelectedItem.ToString());

                            if (rubricUpdated)
                            {
                                resultMessage += "Successfully updated rubric. ";
                            }
                            else
                            {
                                resultMessage += "Did not update the rubric. ";
                            }
                        }
                        catch (Exception ex)
                        {
                            resultMessage += "There was a problem updating:\n " + ex.Message + " ";
                        }
                        //MessageBox.Show(resultMessage);
                        //staMessage.Content = resultMessage.Replace('\n', ' ');
                    }

                    if (isValidFacetCriteriaDictionary() && (_criteriaIDChangedFlag || _criteriaContentsChangedFlag))
                    {
                        //string resultMessage = "";
                        try
                        {
                            facetCriteriaDictionaryUpdated = _criteriaManager.UpdateCriteriaByCriteriaFacetDictionary(_oldRubricVM.FacetCriteria, _rubricVM.FacetCriteria);

                            resultMessage += "Successfully updated rubric. ";
                            _criteriaContentsChangedFlag = _criteriaIDChangedFlag = false;

                        }
                        catch (Exception ex)
                        {
                            resultMessage += "There was a problem updating:\n " + ex.Message + " ";
                        }
                        //MessageBox.Show(resultMessage);
                        //staMessage.Content = resultMessage.Replace('\n', ' ');
                    }

                    if (_facetDescriptionContentChangedFlag)
                    {
                        //string resultMessage = "";
                        try
                        {
                            foreach (Facet facet in _rubricVM.Facets)
                            {
                                facetDescriptionUpdated = _facetManager.UpdateFacetDescriptionByRubricIDAndFacetID(_oldRubricVM.RubricID, facet.FacetID, _oldRubricVM.Facets.First(f => f.FacetID == facet.FacetID).Description, facet.Description);
                            }

                            resultMessage += "Successfully updated rubric facets. ";
                            _facetDescriptionContentChangedFlag = false;

                        }
                        catch (Exception ex)
                        {
                            resultMessage += "There was a problem updating:\n " + ex.Message + " ";
                        }
                    }

                    if (rubricUpdated || facetCriteriaDictionaryUpdated || facetDescriptionUpdated)
                    {
                        //MessageBox.Show(resultMessage, "Rubric Save");

                        DialogControls.OneButton("Rubric Save", resultMessage);
                        staMessage.Content = resultMessage.Replace('\n', ' ');
                        _unsavedWorkFlag = false;
                    }

                    break;
                case MessageBoxResult.None:
                    
                case MessageBoxResult.Cancel:
                    
                case MessageBoxResult.No:
                    
                default:
                    break;
            }
        }

        private bool isValidRubricUpdate()
        {
            bool isValid = true;

            // see if there were any changes
            bool changedName = txtBoxTitle.Text == _oldRubricVM.Name; 
            bool changedDescription = txtBoxDescription.Text == _oldRubricVM.Description;
            bool changedScoreType = cmbBoxScoreTypes.SelectedItem.ToString() == _oldRubricVM.ScoreTypeID;

            if (changedName && changedDescription && changedScoreType)
            {
                isValid = false;
                return isValid;
            }

            bool isValidTitle = !ValidationHelpers.IsValidLength(txtBoxTitle.Text, 50);
            bool isValidDescription = !ValidationHelpers.IsValidLength(txtBoxDescription.Text, 100);

            if (isValidTitle && isValidDescription)
            {
                isValid = false;
            }

            return isValid;
        }


        private bool isValidFacetCriteriaDictionary()
        {
            bool isValid = true;
            foreach (var entry in _rubricVM.FacetCriteria)
            {
                foreach (Criteria criteria in entry.Value)
                {
                    if (!criteria.CriteriaID.IsValidLength(50))
                    {
                        //MessageBox.Show("The criteria name of:\n" + criteria.CriteriaID + "\nis too long. Please shorten.", "Criteria Name Too Long");
                        DialogControls.OneButton("Criteria Name Too Long", "The criteria name of:\n" + criteria.CriteriaID + "\nis too long. Please shorten.");

                        staMessage.Content = "The criteria name of: " + criteria.CriteriaID + " is too long. Please shorten.";
                        isValid = false;
                        break;
                    }
                    if (!criteria.Content.IsValidLength(255))
                    {
                        //MessageBox.Show("The criteria content of:\n" + criteria.Content + "\nis too long. Please shorten.", "Criteria Content Too Long");
                        DialogControls.OneButton("Criteria Content Too Long", "The criteria content of:\n" + criteria.Content + "\nis too long. Please shorten.");
                        staMessage.Content = "The criteria content of: " + criteria.Content + " is too long. Please shorten.";
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentUIState)
            {
                case UIState.Create:
                    saveRubricVM();
                    break;
                case UIState.Edit:
                    updateRubric();
                    break;
                default:
                    break;
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentUIState)
            {
                case UIState.Create:

                    break;
                case UIState.Edit:
                    cancelUpdatesToRubric();
                    break;
                default:
                    break;
            }
        }

        private void saveRubricVM()
        {
            bool isAdded = false;
            pwdPassword.Focus();
            bool savedRubric = false;

            // run validation again 
            if (validateRubricTitleAndDescription())
            {
                try
                {
                    foreach (var item in _rubricSubjects)
                    {
                        _rubricSubjectManager.CreateRubricSubjectBySubjectIDAndRubricID(item.SubjectID, _rubricVM.RubricID, item.SubjectID);
                    }

                    _rubricSubjects = _rubricSubjectManager.RetrieveRubricSubjectsByRubricID(_rubricVM.RubricID);
                    icRubricSubjects.ItemsSource = _rubricSubjects;
                    datSubjects.ItemsSource = _subjectManager.RetrieveSubjects();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problem adding subject to the rubric." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    staMessage.Content = "Problem adding subject to the rubric." + ex.Message;
                }


                try
                {
                    savedRubric = _rubricManager.CreateRubric(txtBoxTitle.Text, txtBoxDescription.Text, cmbBoxScoreTypes.SelectedItem.ToString(), _user.UserID);

                    RubricVM tempRubric = _rubricVMManager.RetrieveRubricByNameDescriptionScoreTypeIDRubricCreator(txtBoxTitle.Text, txtBoxDescription.Text, cmbBoxScoreTypes.SelectedItem.ToString(), _user.UserID);

                    _rubricVM.RubricID = tempRubric.RubricID;

                    foreach (Facet facet in _facets)
                    {
                        facet.RubricID = tempRubric.RubricID;
                    }

                    foreach (Criteria criteria in _criteriaList)
                    {
                        criteria.RubricID = tempRubric.RubricID;
                    }

                    foreach (var entry in _rubricVM.FacetCriteria)
                    {
                        entry.Key.RubricID = tempRubric.RubricID;
                        foreach (var item in entry.Value)
                        {
                            item.RubricID = tempRubric.RubricID;
                        }

                    }

                    tabFacets.IsEnabled = true;
                    tabFacets.Focus();
                    

                }
                catch (Exception ex)
                {
                    //MessageBox.Show("There was a problem creating the rubric.\n" + ex.Message, "Problem Creating Rubric");
                    DialogControls.OneButton("Problem Creating Rubric", "There was a problem creating the rubric.\n" + ex.Message);
                    staMessage.Content = "There was a problem creating the rubric. " + ex.Message;
                    
                }

                if (savedRubric && isValidFacetCriteriaDictionary())
                {
                    try
                    {
                        foreach (Facet facet in _facets)
                        {
                            _facetManager.CreateFacet(facet.RubricID, facet.FacetID, facet.Description, facet.FacetType);
                        }


                        isAdded = _criteriaManager.CreateCriteriaFromFacetCriteriaDictionary(_rubricVM.FacetCriteria);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Problem saving facets and criteria.\n" + ex.Message);
                        DialogControls.OneButton("Error", "Problem saving facets and criteria.\n" + ex.Message);
                        staMessage.Content = "Problem saving facets and criteria. " + ex.Message;
                    }

                    if (isAdded)
                    {
                        MessageBox.Show("Successfully saved the rubric.");
                        DialogControls.OneButton("Success", "Successfully saved the rubric.");
                        staMessage.Content = "Successfully saved the rubric.";
                        _unsavedWorkFlag = false;
                        
                        viewAllActiveRubrics();
                    }
                    else
                    {
                        //icScores.Visibility = Visibility.Visible;
                        icFacetCriteria.Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private void btnScoreRangeAdd_Click(object sender, RoutedEventArgs e)
        {
            // start building the rubric vm without sending it off to the data base
            _rubric = new Rubric(txtBoxTitle.Text, txtBoxDescription.Text, cmbBoxScoreTypes.SelectedItem.ToString(), _user);
            _rubricVM = new RubricVM(_rubric, new List<Facet>(), new List<Criteria>());

            tabFacets.IsEnabled = true;
            tabFacets.Focus();

            rubricVMDetailViewForCreate();

        }

        private void cmbBoxScoreTypes_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            string selectedItem = comboBox.Text;

            foreach (ScoreType scoreType in _scoreTypes)
            {
                if (scoreType.ScoreTypeID == selectedItem)
                {
                    txtBlockScoreTypeDescription.Text = scoreType.Description;
                    break;
                }
            }
        }

        private void cmbBoxFacetType_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            string selectedItem = comboBox.Text;

            foreach (FacetType facet in _facetTypes)
            {
                if (facet.FacetTypeID == selectedItem)
                {
                    txtBlockFacetTypeDescription.Text = facet.Description;
                    break;
                }
            }
        }

        private void mnuCreateNewRubric_Click(object sender, RoutedEventArgs e)
        {
            _facets = new List<Facet>();
            _criteriaList = new List<Criteria>();
            icFacetCriteria.ItemsSource = null;
            //icScores.ItemsSource = null;

            stkCreateRubric.Visibility = Visibility.Hidden;

            _unsavedWorkFlag = true;
            setCurrentUIState(UIState.Create);

            tabCreate.Focus();
            txtBoxTitle.Focus();
        }

        private void mnuEditSelection_Click(object sender, RoutedEventArgs e)
        {
            _unsavedWorkFlag = true;
            //buttons and menu options are availiable
            staMessage.Content = "Make desired changes to the rubric. Click save or cancel when finished.";
            btnEditSelection.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            mnuSaveRubric.IsEnabled = true;
            btnCancel.Visibility = Visibility.Visible;
            btnDeactivateRubric.Visibility = Visibility.Visible;
            
            mnuConfirmUpdatesToRubric.Visibility = Visibility.Visible;
            mnuCancelUpdatesToRubric.Visibility = Visibility.Visible;
            mnuDeactivateRubric.Visibility = Visibility.Visible;

            setCurrentUIState(UIState.Edit);

            _rubric = (Rubric)datViewList.SelectedItem;

            setCurrentRubricVM();

            if (_user.Roles.Contains("Admin"))
            {
                btnDeleteRubric.Visibility = Visibility.Visible;
                mnuAdminDeleteRubric.IsEnabled = true;
            }

            datViewList.Visibility = Visibility.Collapsed;
            toggleListAndDetails();

            lblActionAreaTitle.Content = _rubricVM.Name;

            tabCreate.Focus();

            this.DataContext = _rubricVM;
            icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;

        }

        private void updateRubricFromForm()
        {
            _rubric = new Rubric()
            {
                RubricID = 10001,
                Name = txtBoxTitle.Text,
                Description = txtBoxDescription.Text,
                RubricCreator = _user,
            };
        }

        private void addFacetandCriteriaToRubricVM()
        {
            bool isValidFacetTitle = ValidationHelpers.IsValidLength(txtBoxFacetTitle.Text, 50);
            bool isValidFacetDescription = ValidationHelpers.IsValidLength(txtBoxFacetDescription.Text, 100);

            if (isValidFacetTitle && isValidFacetDescription)
            {
                Facet facet = new Facet()
                {
                    FacetID = txtBoxFacetTitle.Text,
                    Description = txtBoxFacetDescription.Text,
                    FacetType = cmbBoxFacetType.SelectedItem.ToString()
                };
                _facets.Add(facet);

                for (int i = (int)sldCriteriaTopRange.Value; i >= (int)sldCriteriaBottomRange.Value; i--)
                {
                    //(int rubricID, string facetID, int score)
                    Criteria criteria = new Criteria(facet.FacetID, i, i + " Points", "Criteria to meet for " + facet.FacetID + " to earn " + i + " points.");
                    _criteriaList.Add(criteria);
                }

                txtBoxFacetTitle.Text = "";
                txtBoxFacetDescription.Text = "";
                staMessage.Content = "Added \"" + facet.FacetID + "\" to the rubric. Add contents to the criteria and save when finished.";

                txtBoxFacetTitle.Focus();
            }

        }


        private void updateICFacetCriteria()
        {

            _rubricVM = new RubricVM(_rubric, _facets, _criteriaList);

            this.DataContext = _rubricVM;

            icFacetCriteria.IsEnabled = true;
            icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;
            icFacetCriteria.Visibility = Visibility.Visible;

            //icScores.ItemsSource = _rubricVM.RubricScoreColumn();


            btnSave.Visibility = Visibility.Visible;
            btnSave.IsEnabled = IsEnabled;
            mnuSaveRubric.IsEnabled = true;


        }

        private void setCurrentUIState(UIState uiState)
        {
            _currentUIState = uiState;

            switch (_currentUIState)
            {
                case UIState.Create:

                    brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;

                    btnCreateNewRubric.Visibility = Visibility.Collapsed;
                    btnCreateRubricNext.Visibility = Visibility.Visible;
                    btnDeleteRubric.Visibility = Visibility.Collapsed;
                    btnEditSelection.Visibility = Visibility.Collapsed;
                    btnSave.IsEnabled = false;
                    btnSave.Visibility = Visibility.Visible;

                    cmbBoxScoreTypes.IsEnabled = true;

                    datViewList.Visibility = Visibility.Collapsed;
                    datSubjects.Visibility = Visibility.Collapsed;

                    icFacetControls.IsEnabled = true;
                    icFacetCriteria.Visibility = Visibility.Hidden;

                    lblActionAreaTitle.Visibility = Visibility.Hidden;

                    mnuSaveRubric.IsEnabled = false;

                    staMessage.Content = "Create a new rubric.";

                    tabCreate.Header = "Create Rubric";
                    tabCreate.Visibility = Visibility.Visible;
                    tabFacetDetailView.Visibility = Visibility.Collapsed;
                    tabFacets.IsEnabled = false;
                    tabFacets.Visibility = Visibility.Visible;
                    tabScoreRange.IsEnabled = false;
                    tabScoreRange.Visibility = Visibility.Visible;
                    tabRubricSubject.Visibility = Visibility.Collapsed;

                    tabsetCreateControls.IsEnabled = true;
                    tabsetCreateControls.Visibility = Visibility.Visible;

                    txtBlkTagTitle.Text = "Add Tags";
                    txtBlkTags.Text = "Optionally write short descriptive tags that give information about the rubric.";

                    //txtBoxCourse.IsReadOnly = false;
                    //txtBoxCourse.Text = "";
                    txtBoxDescription.IsEnabled = true;
                    txtBoxDescription.IsReadOnly = false;
                    txtBoxDescription.Text = "";
                    txtBoxRubricSubject.IsReadOnly = false;
                    txtBoxRubricSubject.Text = "";
                    //txtBoxSubject.IsReadOnly = false;
                    txtBoxTitle.IsEnabled = true;
                    txtBoxTitle.IsReadOnly = false;
                    txtBoxTitle.Text = "";
                    //txtBoxUnit.IsReadOnly = false;
                    //txtBoxUnit.Text = "";
                    txtblkInstructions.Text = "Add descriptive information to your rubric.";


                    break;
                case UIState.Edit:

                    datViewList.Visibility = Visibility.Collapsed;

                    tabsetCreateControls.IsEnabled = true;
                    brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;

                    mnuSaveRubric.IsEnabled = false;

                    tabCreate.Header = "Rubric Details";
                    txtblkInstructions.Text = "Information about the rubric.";
                    txtBlkTagTitle.Text = "Tags";
                    txtBlkTags.Text = "Short descriptive tags that give information about the rubric.";

                    txtBoxTitle.IsReadOnly = false;
                    txtBoxTitle.IsEnabled = true;
                    txtBoxDescription.IsReadOnly = false;
                    txtBoxDescription.IsEnabled = true;
                    cmbBoxScoreTypes.IsEnabled = true;

                    txtBoxRubricSubject.IsReadOnly = false;
                    //txtBoxCourse.IsReadOnly = false;
                    //txtBoxUnit.IsReadOnly = false;
                    //txtBoxSubject.IsReadOnly = false;

                    btnCreateRubricNext.Visibility = Visibility.Hidden;
                    tabFacets.Visibility = Visibility.Collapsed;

                    icFacetCriteria.IsEnabled = true;
                    icFacetControls.IsEnabled = true;

                    break;

                case UIState.ViewAll:

                    brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;

                    btnCancel.Visibility = Visibility.Collapsed;
                    btnCreateRubricNext.Visibility = Visibility.Hidden;
                    btnDeactivateRubric.Visibility = Visibility.Collapsed;
                    btnEditSelection.Visibility = Visibility.Collapsed;
                    btnEditSelection.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Collapsed;

                    cmbBoxScoreTypes.IsEnabled = false;

                    datViewList.Visibility = Visibility.Visible;
                    datSubjects.Visibility = Visibility.Visible;

                    icFacetControls.IsEnabled = false;
                    icFacetCriteria.Visibility = Visibility.Hidden;

                    mnuCancelUpdatesToRubric.Visibility = Visibility.Collapsed;
                    mnuConfirmUpdatesToRubric.Visibility = Visibility.Collapsed;
                    mnuDeactivateRubric.Visibility = Visibility.Collapsed;
                    mnuEditSelection.IsEnabled = false;
                    mnuSaveRubric.IsEnabled = false;

                    tabCreate.Header = "Rubric Details";
                    tabCreate.Visibility = Visibility.Collapsed;
                    tabFacetDetailView.Visibility = Visibility.Collapsed;
                    tabFacets.Visibility = Visibility.Collapsed;
                    tabRubricSubject.Visibility = Visibility.Collapsed;
                    tabRubricSubject.Visibility = Visibility.Visible;

                    tabRubricSubject.Focus();

                    tabScoreRange.Visibility = Visibility.Collapsed;
                    tabsetCreateControls.IsEnabled = true;
                    tabsetCreateControls.Visibility = Visibility.Visible;

                    txtBlkTagTitle.Text = "Tags";
                    txtBlkTags.Text = "Short descriptive tags that give information about the rubric.";

                    //txtBoxCourse.IsReadOnly = true;
                    txtBoxDescription.IsReadOnly = true;
                    txtBoxRubricSubject.IsReadOnly = true;
                    //txtBoxSubject.IsReadOnly = true;
                    txtBoxTitle.IsReadOnly = true;
                    //txtBoxUnit.IsReadOnly = true;

                    txtblkInstructions.Text = "Information about the rubric.";

                    //updateICFacetCriteria();



                    break;

                case UIState.ViewDetail:

                    brdTabSetCreateControlsBorder.Visibility = Visibility.Visible;

                    btnCancel.Visibility = Visibility.Collapsed;
                    btnCreateRubricNext.Visibility = Visibility.Hidden;
                    btnDeactivateRubric.Visibility = Visibility.Collapsed;
                    btnEditSelection.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Collapsed;

                    cmbBoxScoreTypes.IsEnabled = false;

                    datViewList.Visibility = Visibility.Collapsed;
                    datSubjects.Visibility = Visibility.Collapsed;

                    icFacetControls.IsEnabled = false;
                    icFacetCriteria.Visibility = Visibility.Visible;

                    mnuEditSelection.IsEnabled = true;
                    mnuSaveRubric.IsEnabled = false;

                    tabCreate.Header = "Rubric Details";
                    tabCreate.Visibility = Visibility.Visible;
                    tabFacetDetailView.Visibility = Visibility.Visible;
                    tabFacets.Visibility = Visibility.Collapsed;
                    tabRubricSubject.Visibility = Visibility.Collapsed;
                    tabScoreRange.Visibility = Visibility.Collapsed;

                    tabsetCreateControls.IsEnabled = true;
                    tabsetCreateControls.Visibility = Visibility.Visible;

                    txtBlkTagTitle.Text = "Tags";
                    txtBlkTags.Text = "Short descriptive tags that give information about the rubric.";

                    //txtBoxCourse.IsReadOnly = true;
                    txtBoxDescription.IsReadOnly = true;
                    txtBoxRubricSubject.IsReadOnly = true;
                    //txtBoxSubject.IsReadOnly = true;
                    txtBoxTitle.IsReadOnly = true;
                    //txtBoxUnit.IsReadOnly = true;

                    txtblkInstructions.Text = "Information about the rubric.";

                    break;
                default:
                    break;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _criteriaContentsChangedFlag = true;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            _criteriaContentsChangedFlag = true;
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            // facet description change
            _facetDescriptionContentChangedFlag = true;

            TextBox textBox = (TextBox)sender;

            if (!ValidationHelpers.IsValidLength(textBox.Text, 100))
            {
                textBox.Text = textBox.Text.Substring(0, 99);                

                //MessageBox.Show("The description for the facet is too long.", "Description too long");
                DialogControls.OneButton("Description too long", "The description for the facet is too long.");

                staMessage.Content = "The description for the facet is too long.";

                textBox.CaretIndex = 99;
            }
            

        }

        private void mnuDeactivateRubric_Click(object sender, RoutedEventArgs e)
        {
            deactivateRubric();

        }

        private void deactivateRubric()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you would like to deactivate this rubric?\nYou will need to ask an administrator to reactivate it.", "Confirm Deactivation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:

                    try
                    {
                        _rubricManager.DeactivateRubricByRubricID(_rubric.RubricID);
                        //MessageBox.Show("Deactivated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogControls.OneButton("Success", "Deactivated Successfully");

                        staMessage.Content = "Deactivated Successfully";

                        viewAllActiveRubrics();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("There was an error deactivating the rubric.\n" + ex.Message, "Problem Deactivating", MessageBoxButton.OK, MessageBoxImage.Error);
                        DialogControls.OneButton("Problem Deactivating", "There was an error deactivating the rubric.\n" + ex.Message);

                        staMessage.Content = "There was an error deactivating the rubric. " + ex.Message.Replace('\n', ' ');
                    }

                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void mnuAdminDeleteRubric_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this rubric?\nThere will be no way to recover it.", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (result)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    try
                    {
                        _rubricVMManager.DeleteRubricByRubricID(_rubricVM.RubricID);
                        //MessageBox.Show("Rubric successfully deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogControls.OneButton("Success", "Rubric successfully deleted.");
                        staMessage.Content = "Rubric successfully deleted.";
                        viewAllActiveRubrics();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Problem deleting the rubric.\n" + ex.Message, "Delete", MessageBoxButton.OK, MessageBoxImage.Error);
                        DialogControls.OneButton("Delete", "Problem deleting the rubric.\n" + ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }

        }


        private void btnDeleteFacet_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            switch (_currentUIState)
            {
                case UIState.Create:

                    MessageBoxResult result1 = MessageBox.Show("Are you sure you would like to delete the facet \"" + button.Tag.ToString() + "\" and all of it's criteria? This can not be undone.", "Delete Facet", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    switch (result1)
                    {
                        case MessageBoxResult.None:
                            break;
                        case MessageBoxResult.OK:
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        case MessageBoxResult.Yes:

                            if (_facets.Count == 1)
                            {
                                MessageBox.Show("There must be at least one facet to make a rubric.", "Facet Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                DialogControls.OneButton("Facet Warning", "There must be at least one facet to make a rubric.");
                                break;
                            }

                            //remove the facet
                            _facets.Remove(_facets.First(f => f.FacetID == button.Tag.ToString()));

                            // remove the criteria
                            List<Criteria> tempCriteriaList = new List<Criteria>();

                            foreach (Criteria criteria in _criteriaList)
                            {
                                if (criteria.FacetID != button.Tag.ToString())
                                {
                                    tempCriteriaList.Add(criteria);
                                }
                            }

                            _criteriaList = tempCriteriaList;

                            //_rubric = (Rubric)datViewList.SelectedItem;
                            if (validateRubricTitleAndDescription())
                            {
                                updateRubricFromForm();
                            }

                            _rubricVM = new RubricVM(_rubric, _facets, _criteriaList);


                            //setCurrentRubricVM();
                            //rubricVMDetailView();
                            this.DataContext = _rubricVM;
                            icFacetCriteria.ItemsSource = _rubricVM.FacetCriteria;

                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }

                    break;

                case UIState.Edit:
                    MessageBoxResult result2 = MessageBox.Show("Are you sure you would like to delete the facet \"" + button.Tag.ToString() + "\" and all of it's criteria? This can not be undone.", "Delete Facet", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    switch (result2)
                    {
                        case MessageBoxResult.None:
                            break;
                        case MessageBoxResult.OK:
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        case MessageBoxResult.Yes:
                            try
                            {
                                if (_rubricVM.FacetCriteria.Count == 1)
                                {
                                    deactivateRubric();
                                }
                                else
                                {
                                    _facetManager.DeleteFacetByRubricIDAndFacetID(_rubricVM.RubricID, button.Tag.ToString());
                                    staMessage.Content = "Deleted \"" + button.Tag.ToString() + "\" successfully.";

                                    _rubric = (Rubric)datViewList.SelectedItem;

                                    setCurrentRubricVM();
                                    rubricVMDetailView();
                                }

                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show("Problem deleting the facet" + ex.Message, "Problem Deleting Facet", MessageBoxButton.OK, MessageBoxImage.Error);
                                DialogControls.OneButton("Problem Deleting Facet", "Problem deleting the facet" + ex.Message);
                                staMessage.Content = "Problem deleting the facet" + ex.Message.Replace('\n', ' ');
                            }

                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }


                    break;
                case UIState.ViewAll:
                    break;
                default:
                    break;
            }


            

            
        }

        private void btnDeleteFacet_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Button button = (Button)sender;

            if (icFacetCriteria.IsEnabled == true)
            {
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;

            }
            else if (icFacetCriteria.IsEnabled == false)
            {
                button.IsEnabled = false;
                button.Visibility = Visibility.Hidden;
            }

        }

        private void mnuChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
            
            
        }

        private void txtCriteriaID_Initialized(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (_currentUIState == UIState.ViewAll)
            {
                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.IsReadOnly = false;
            }
        }

        private void btnDeleteFacet_Initialized(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (_currentUIState == UIState.ViewAll || _currentUIState == UIState.ViewDetail || _currentUIState == UIState.Create)
            {
                button.IsEnabled = false;
                button.Visibility = Visibility.Hidden;
            }
            else if (_currentUIState == UIState.Edit)
            {
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;
            }
        }

        private void btnRubricSubjectDelete_Click(object sender, RoutedEventArgs e)
        {

            if (_currentUIState == UIState.Edit || _currentUIState == UIState.Create)
            {

                Button button = (Button)sender;

                MessageBoxResult result = MessageBox.Show("Are you sure you would like to remove this subject from the rubiric?", "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        try
                        {
                            _rubricSubjectManager.RemoveRubricSubjectBySubjectIDAndRubricID(button.Tag.ToString(), _rubricVM.RubricID);

                            _rubricSubjects = _rubricSubjectManager.RetrieveRubricSubjectsByRubricID(_rubricVM.RubricID);
                            icRubricSubjects.ItemsSource = _rubricSubjects;
                            datSubjects.ItemsSource = _subjectManager.RetrieveSubjects();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Problem removing the subject from the rubric" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
            }
        }

        private void txtBoxRubricSubject_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;




            if (e.Key == Key.Return)
            {

                if (_currentUIState == UIState.Edit)
                {
                    try
                    {
                        _rubricSubjectManager.CreateRubricSubjectBySubjectIDAndRubricID(textBox.Text, _rubricVM.RubricID, textBox.Text);
                        _rubricSubjects = _rubricSubjectManager.RetrieveRubricSubjectsByRubricID(_rubricVM.RubricID);
                        icRubricSubjects.ItemsSource = _rubricSubjects;
                        datSubjects.ItemsSource = _subjectManager.RetrieveSubjects();
                        textBox.Text = "";

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Problem adding subject to the rubric." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        DialogControls.OneButton("Error", "Problem adding subject to the rubric." + ex.Message);
                        staMessage.Content = "Problem adding subject to the rubric." + ex.Message;
                    }
                }
                else if (_currentUIState == UIState.Create)
                {
                    _rubricSubjects = new List<RubricSubject>();

                    _rubricSubjects.Add(new RubricSubject() { RubricID = 10001, SubjectID = textBox.Text });
                    icRubricSubjects.ItemsSource = _rubricSubjects;
                    textBox.Text = "";
                }

                


            }
        }

        private void txtBoxRubricSubject_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length > 50)
            {
                textBox.Text = textBox.Text.Substring(0, 49);

                //MessageBox.Show("The description for the facet is too long.", "Description too long");
                DialogControls.OneButton("Long Subject", "The description for the subject is too long.");

                staMessage.Content = "The description for the facet is too long.";

                textBox.CaretIndex = 49;
            }

            if (ValidationHelpers.HasValidCharactors(textBox.Text))
            {
                staMessage.Content = "The subject can not contain special charactors like " + textBox.Text[textBox.Text.Length - 1];
                string subString = textBox.Text.Substring(0, textBox.Text.Length - 1);
                textBox.Text = subString;
                textBox.CaretIndex = textBox.Text.Length;
            }

        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }

    internal enum UIState
    {
        Create,
        Edit,
        ViewAll,
        ViewDetail
    }
}
