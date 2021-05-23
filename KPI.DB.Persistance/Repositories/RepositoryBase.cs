using KPI.DB.Persistance.Configurations;

namespace KPI.DB.Persistance.Repositories
{
    public class RepositoryBase
    {
        protected IConnectionAccessor Accessor { get; }

        public RepositoryBase(IConnectionAccessor accessor)
        {
            Accessor = accessor;
        }
    }
}
