using Newtonsoft.Json;
using Soundpad.Data.Constants;
using Soundpad.Models;
using Soundpad.Services.Interfaces;

namespace Soundpad.Services
{
    public class StorageService : IStorageService
    {
        private string categoriesFile = Path.Combine(FileSystem.AppDataDirectory, "categories.txt");
        private string soundsFile = Path.Combine(FileSystem.AppDataDirectory, "sounds.txt");

        public async Task<ResultModel<string>> CreateCategory(CategoryModel category)
        {
            try
            {
                category.Id = Guid.NewGuid().ToString();
                var jsonCategory = JsonConvert.SerializeObject(category);
                await File.AppendAllTextAsync(categoriesFile, $"{jsonCategory}\r\n");

                return new ResultModel<string>(true);
            }
            catch (Exception ex)
            {
                return new ResultModel<string>(false) { Message = "There was an error writing to the file system" };
            }
        }

        public async Task<ResultModel<CategoryModel>> RetrieveCategory(string categoryId)
        {
            ResultModel<List<CategoryModel>> result = await RetrieveAllCategories();
            if (result.Success == false)
                return new ResultModel<CategoryModel>(false) { Message = result.Message };

            List<CategoryModel> categories = result.Content;
            CategoryModel category = categories.Where(c => c.Id == categoryId).FirstOrDefault();

            if (category is null)
                return new ResultModel<CategoryModel>(false) { Message = "Category not found" };

            ResultModel<List<SoundModel>> soundsResult = await RetrieveAllSoundsInCategory(categoryId);
            if (soundsResult.Success == false)
                return new ResultModel<CategoryModel>(false) { Message = soundsResult.Message };

            category.Sounds = soundsResult.Content;

            return new ResultModel<CategoryModel>(true) { Content = category };
        }

        public Task<ResultModel<string>> UpdateCategory(CategoryModel category, string newCategoryName)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategories()
        {
            if (File.Exists(categoriesFile))
                File.Delete(categoriesFile);
        }

        public Task<ResultModel<string>> DeleteCategory(string categoryId)
        {
            throw new NotImplementedException();
        }


        public async Task<ResultModel<List<CategoryModel>>> RetrieveAllCategories()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            if (File.Exists(categoriesFile) == false)
                return new ResultModel<List<CategoryModel>>(true) { Content = categories };

            try
            {
                string[] lines = await File.ReadAllLinesAsync(categoriesFile);
                foreach (string line in lines)
                {
                    var category = JsonConvert.DeserializeObject<CategoryModel>(line);
                    categories.Add(category);
                }

                return new ResultModel<List<CategoryModel>>(true) { Content = categories };
            }
            catch
            {
                return new ResultModel<List<CategoryModel>>(false) { Message = "Error reading categories file" };
            }
        }

        public async Task<ResultModel<string>> CreateSound(SoundModel sound)
        {
            try
            {
                var jsonSound = JsonConvert.SerializeObject(sound);
                await File.AppendAllTextAsync(soundsFile, $"{jsonSound}\r\n");
            }
            catch
            {
                return new ResultModel<string>(false) { Message = "There was an error writing to the file system" };
            }

            return new ResultModel<string>(true);
        }

        public async Task<ResultModel<List<SoundModel>>> RetrieveAllSounds()
        {
            List<SoundModel> sounds = new List<SoundModel>();
            if (File.Exists(soundsFile) == false)
                return new ResultModel<List<SoundModel>>(true) { Content = sounds };

            try
            {
                string[] lines = await File.ReadAllLinesAsync(soundsFile);
                foreach (string line in lines)
                {
                    var sound = JsonConvert.DeserializeObject<SoundModel>(line);
                    sounds.Add(sound);
                }

                return new ResultModel<List<SoundModel>>(true) { Content = sounds }; ;
            }
            catch
            {
                return new ResultModel<List<SoundModel>>(false) { Message = "Error reading sound file" }; ;
            }
        }

        public async Task<ResultModel<List<SoundModel>>> RetrieveAllSoundsInCategory(string categoryId)
        {
            ResultModel<List<SoundModel>> result = await RetrieveAllSounds();
            if (result.Success == false)
                return result;

            result.Content = result.Content.Where(s => s.CategoryId.Equals(categoryId)).ToList();

            return result;
        }

        public async Task<ResultModel<SoundModel>> RetrieveSound(string soundId)
        {
            ResultModel<List<SoundModel>> result = await RetrieveAllSounds();
            if (result.Success == false)
                return new ResultModel<SoundModel>(false) { Message = result.Message };

            List<SoundModel> sounds = result.Content;
            SoundModel sound = sounds.Where(c => c.Id == soundId).FirstOrDefault();

            if (sound is null)
                return new ResultModel<SoundModel>(false) { Message = "Sound not found" };


            return new ResultModel<SoundModel>(true) { Content = sound };
        }

        public Task<ResultModel<string>> UpdateSound(SoundModel oldSound, SoundModel newSound)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<string>> DeleteSound(string SoundId)
        {
            throw new NotImplementedException();
        }

        public void DeleteSounds()
        {
            if (File.Exists(soundsFile))
                File.Delete(soundsFile);
        }

        public void DeleteRecordings()
        {
            if (Directory.Exists(RConst.OUTPUT_DIRECTORY))
                Directory.Delete(RConst.OUTPUT_DIRECTORY, true);
        }
    }
}
