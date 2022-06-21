using AndroidX.Core.Content;
using Newtonsoft.Json;
using Soundpad.Models;
using Soundpad.Services.Interfaces;
using Environment = Android.OS.Environment;


namespace Soundpad.Platforms.Android.Services
{
    internal class StorageService //: IStorageService
    {
        /*private string categoriesFile = Path.Combine(
            ContextCompat.GetExternalFilesDirs(MainActivity.Instance, Environment.DirectoryDocuments)[0].ToString(),
            "categories.txt");

        private string soundsFile = Path.Combine(
            ContextCompat.GetExternalFilesDirs(MainActivity.Instance, Environment.DirectoryDocuments)[0].ToString(),
            "sounds.txt");

        public async Task<ResultModel<string>> CreateCategory(CategoryModel category)
        {
            category.Name = category.Name.Trim();
            ResultModel<List<CategoryModel>> result = await RetrieveAllCategories();
            if(result.Success == false)
                return new ResultModel<string>(false) { Message = result.Message };

            List<CategoryModel> categories = result.Content;
            if (categories.Select(c => c.Name).Contains(category.Name))
                return new ResultModel<string>(false) { Message = $"{category.Name} already exists" };

            try
            {
                string categoryId = Guid.NewGuid().ToString();
                using (var writer = File.AppendText(categoriesFile))
                {
                    await writer.WriteLineAsync($"{categoryId},{category.Name}");
                }
            }
            catch
            {
                return new ResultModel<string>(false) { Message = "There was an error writing to the file system" };
            }

            return new ResultModel<string>(true);
        }

        public async Task<ResultModel<CategoryModel>> RetrieveCategory(string categoryId)
        {
            ResultModel<List<CategoryModel>> catResult = await RetrieveAllCategories();
            if(catResult.Success == false)
                return new ResultModel<CategoryModel>(false) { Message = catResult.Message };

            List<CategoryModel> categories = catResult.Content;   
            CategoryModel category = categories.Where(c => c.Id == categoryId).FirstOrDefault();

            if (category is null)
                return new ResultModel<CategoryModel>(false) { Message = "Category not found" };

            ResultModel<List<SoundModel>> soundsResult = await RetrieveAllSoundsInCategory(categoryId);
            if(soundsResult.Success == false)
                return new ResultModel<CategoryModel>(false) { Message = soundsResult.Message };

            category.Sounds = soundsResult.Content;

            return new ResultModel<CategoryModel>(true) { Content = category };
        }


        public Task<ResultModel<string>> UpdateCategory(CategoryModel category, string newCategoryName)
        {
            throw new NotImplementedException();
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
                using (var reader = new StreamReader(categoriesFile, true))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] categoryString = line.Split(',');
                        var category = new CategoryModel() { Id = categoryString[0], Name = categoryString[1] };
                        categories.Add(category);
                    }
                }

                return new ResultModel<List<CategoryModel>>(true) { Content = categories }; ;
            }
            catch
            {
                return new ResultModel<List<CategoryModel>>(false) { Message = "Error reading category file" }; ;
            }
        }


        public async Task<ResultModel<string>> CreateSound(SoundModel sound)
        {
            try
            {
                using (var writer = File.AppendText(soundsFile))
                {
                    var jsonSound = JsonConvert.SerializeObject(sound);
                    await writer.WriteLineAsync(JsonConvert.SerializeObject(sound));
                }
            }
            catch
            {
                return new ResultModel<string>(false) { Message = "There was an error writing to the file system" };
            }

            return new ResultModel<string>(true);
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

        public async Task<ResultModel<List<SoundModel>>> RetrieveAllSounds()
        {
            List<SoundModel> sounds = new List<SoundModel>();
            if (File.Exists(soundsFile) == false)
                return new ResultModel<List<SoundModel>>(true) { Content = sounds };

            try
            {
                using (var reader = new StreamReader(soundsFile, true))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        SoundModel sound = JsonConvert.DeserializeObject<SoundModel>(line);
                        sounds.Add(sound);
                    }
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

        public void DeleteCategories()
        {
            if (File.Exists(categoriesFile))
                File.Delete(categoriesFile);
        }


        public void DeleteSounds()
        {
            if (File.Exists(soundsFile))
                File.Delete(soundsFile);
        }

        public Task<ResultModel<string>> UpdateSound(SoundModel oldSound, SoundModel newSound)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<string>> DeleteSound(string SoundId)
        {
            throw new NotImplementedException();
        }*/
    }
}
