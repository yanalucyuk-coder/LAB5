using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace LAB5
{
    public class JsonFileWriter : IFileSaver
    {
        public async Task SaveAsync<T>(string filePath, List<T> data)
        {
            string? directoryPath = Path.GetDirectoryName(filePath);
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            //серіалізація колекцію в JSON
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            //запис JSON-рядка у файл
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
