using System.Data;

namespace KPI.DB.Persistance.Configurations
{
    public interface IConnectionAccessor
    {
        IDbConnection Connection { get; }
    }
}