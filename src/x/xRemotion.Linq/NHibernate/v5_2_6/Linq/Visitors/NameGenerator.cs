using Remotion.Linq;

namespace NHibernate_v5_2_6.Linq.Visitors
{
    public class NameGenerator
    {
        private readonly QueryModel _model;

        public NameGenerator(QueryModel model)
        {
            _model = model;
        }

        public string GetNewName()
        {
            return _model.GetNewName("_");
        }
    }
}