using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB5
{
    public interface IFileSaver
    {
        // Task - асинхронна операція
        // <T> - метод працює зі списком будь-яких об'єктів (Student)
        // filePath - шлях до файлу, data - список, який зберігаємо
        Task SaveAsync<T>(string filePath, List<T> data);
    }
}
