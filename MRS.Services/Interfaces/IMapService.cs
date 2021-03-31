using MRS.Business.Models;

namespace MRS.Services.Interfaces
{
    public interface IMapService
    {
        Plateau ParseMap(string parameters);
    }
}