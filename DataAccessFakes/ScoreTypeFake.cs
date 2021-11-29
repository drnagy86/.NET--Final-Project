using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class ScoreTypeFake : IScoreTypeAccessor
    {
        List<ScoreType> _scoreTypes = new List<ScoreType>();


        public ScoreTypeFake()
        {
            _scoreTypes.Add(new ScoreType()
            {
                ScoreTypeID = "Percentage",
                Description = "Percentage, eg 75%"                
            });
            _scoreTypes.Add(new ScoreType()
            {
                ScoreTypeID = "Test",
                Description = "A little longer description"
            });
        }

        public List<ScoreType> SelectScoreTypes()
        {
            return _scoreTypes;
        }
    }
}
