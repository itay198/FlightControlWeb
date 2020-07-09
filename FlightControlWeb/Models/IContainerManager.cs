using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{

    public interface IContainerManager<T>
    {
        IEnumerable<T> GetAllEllements();
        T GetEllement(string flight_id);
        void AddEllement(T flight);
        void UpdateEllement(T flight);
        void DeleteEllement(string flight_id);
        int GetSize();
    }
}
