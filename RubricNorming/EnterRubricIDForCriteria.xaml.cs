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
using LogicLayer;
using DataObjects;


namespace RubricNorming
{
    /// <summary>
    /// Interaction logic for EnterRubricIDForCriteria.xaml
    /// </summary>
    public partial class EnterRubricIDForCriteria : Window
    {
        ICriteriaManager _criteriaManager = null;

        public EnterRubricIDForCriteria(ICriteriaManager criteriaManager)
        {
            _criteriaManager = criteriaManager;

            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<Criteria> criteriaList = null;
            
            try
            {
                criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(Int32.Parse(txtBoxRubricID.Text));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
