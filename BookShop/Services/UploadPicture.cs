using BookShop.Infrastructure;
using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;

namespace BookShop.Services
{
    public class UploadPicture
    {
        private readonly string _clientsDataFolder;
        public UploadPicture()
        {
            var configuration = new ConfigurationFactory().GetConfiguration();
            _clientsDataFolder = configuration["clientsDataFolder"];
        }

        public async Task<string> AddClientImageAsync(string filePath, int clientId)
        {
            byte[] data;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                data = new byte[file.Length];
                await file.ReadAsync(data, 0, data.Length);
            }
            var imageName = filePath.Substring(filePath.LastIndexOf(@"\"));
            var userFolder = _clientsDataFolder + $@"\{clientId}";

            DirectoryInfo info = new DirectoryInfo(userFolder);
            if (!info.Exists)
            {
                info.Create();
            }

            var pathToAdd = info.FullName + imageName;
            using (FileStream file = new FileStream(pathToAdd, FileMode.Create))
            {
                await file.WriteAsync(data, 0, data.Length);
            }

            return pathToAdd;
        }

        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            bool? result = openFileDialog.ShowDialog();
            var file_path = openFileDialog.FileName;
            return file_path;
        }
    }
}
