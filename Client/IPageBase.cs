using System.Threading.Tasks;

namespace KPI.DB.Client
{
    public interface IPageBase
    {
        Task<string> ReadCookies(string name);
        Task WriteCookies(string name, string value, int time = 45);
    }
}