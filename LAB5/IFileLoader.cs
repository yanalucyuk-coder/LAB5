using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB5
{
    public interface IFileLoader
    {
        // Task<List<T>?> - асинхронна операція
        // ? - результат може бути null
        Task<List<T>?> LoadAsync<T>(string filePath);
    }
}
