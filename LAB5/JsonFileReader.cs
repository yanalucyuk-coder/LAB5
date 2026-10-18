using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace LAB5
{
    public class JsonFileReader : IFileLoader
    {
        public async Task<List<T>?> LoadAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null; 
            }
            //читання тексту з файла
            var json = await File.ReadAllTextAsync(filePath);
            //десереалізація JSON-рядка у колекцію
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
