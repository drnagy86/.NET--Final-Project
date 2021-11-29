using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessLayer;

namespace LogicLayer
{
    public class ScoreTypeManager : IScoreTypeManager
    {

        IScoreTypeAccessor _scoreTypeAccessor = null;

        public ScoreTypeManager()
        {
            _scoreTypeAccessor = new ScoreTypeAccessor();
        }

        public ScoreTypeManager(IScoreTypeAccessor scoreTypeAccessor)
        {
            _scoreTypeAccessor = scoreTypeAccessor;

        }

        public List<ScoreType> RetrieveScoreTypes()
        {
            List<ScoreType> scoreTypes = null;

            try
            {
                scoreTypes = _scoreTypeAccessor.SelectScoreTypes();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (scoreTypes == null)
            {
                throw new ApplicationException("No score types retrieved.");
            }
            

            return scoreTypes;

        }
    }
}
