using MediumServer.Models;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MediumServerExample.Tools
{
    public class IOTools
    {
        /// <summary>
        /// Get Local folder path or <see cref="StorageFolder"/>
        /// </summary>
        /// <returns></returns>
        public static async Task<dynamic> GetSavePath(bool isStorage = false)
        {
            var defPath = ApplicationData.Current.LocalFolder.Path;
            if (!isStorage)
            {
                return defPath;
            }
            else
            {
                var folder = await StorageFolder.GetFolderFromPathAsync(defPath);
                return folder;
            }

        }

        /// <summary>
        /// Save Medium token in local storage
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async static Task SaveMediumToken(Token token)
        {
            if (token == null) { return; }
            var local = (StorageFolder)await GetSavePath(true);
            var dataFolder = await local.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);
            var tokenFile = await dataFolder.CreateFileAsync("MediumApplicationToken.json", CreationCollisionOption.OpenIfExists);
            string json = JsonConvert.SerializeObject(token);
            await FileIO.WriteTextAsync(tokenFile, json);
        }

        /// <summary>
        /// Get Medium token from local storage
        /// </summary>
        /// <returns></returns>
        public async static Task<Token> GetMediumToken()
        {
            var local = (StorageFolder)await GetSavePath(true);
            var dataFolder = await local.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);
            var tokenFile = await dataFolder.CreateFileAsync("MediumApplicationToken.json", CreationCollisionOption.OpenIfExists);
            string json = await FileIO.ReadTextAsync(tokenFile);
            if (String.IsNullOrEmpty(json.Trim())) { return null; }
            return JsonConvert.DeserializeObject<Token>(json);
        }

        /// <summary>
        /// Open Local File
        /// </summary>
        /// <param name="types">Extend names (like .jpg,.mp3)</param>
        /// <returns></returns>
        public async static Task<StorageFile> OpenLocalFile(params string[] types)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            Regex typeReg = new Regex(@"^\.[a-zA-Z0-9]+$");
            foreach (var type in types)
            {
                if (type == "*" || typeReg.IsMatch(type))
                    picker.FileTypeFilter.Add(type);
                else
                    throw new InvalidCastException("Extend names invalid.");
            }
            var file = await picker.PickSingleFileAsync();
            if (file != null)
                return file;
            else
                return null;
        }
    }
}
