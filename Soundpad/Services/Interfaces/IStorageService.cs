using Soundpad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Services.Interfaces
{
    public interface IStorageService
    {
        Task<ResultModel<string>> CreateCategory(CategoryModel category);
        Task<ResultModel<string>> UpdateCategory(CategoryModel category, string newCategoryName);
        Task<ResultModel<string>> DeleteCategory(string categoryId);
        Task<ResultModel<CategoryModel>> RetrieveCategory(string categoryId);
        Task<ResultModel<List<CategoryModel>>> RetrieveAllCategories();

        Task<ResultModel<string>> CreateSound(SoundModel sound);
        Task<ResultModel<SoundModel>> RetrieveSound(string soundId);
        Task<ResultModel<List<SoundModel>>> RetrieveAllSounds();
        Task<ResultModel<List<SoundModel>>> RetrieveAllSoundsInCategory(string categoryId);
        public Task<ResultModel<string>> UpdateSound(SoundModel oldSound, SoundModel newSound);
        public Task<ResultModel<string>> DeleteSound(string SoundId);
        void DeleteCategories();
        void DeleteSounds();
        void DeleteRecordings();
    }
}
