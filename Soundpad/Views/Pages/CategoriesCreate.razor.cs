using Microsoft.AspNetCore.Components;
using Microsoft.Maui.ApplicationModel;
using Soundpad.Models;
using Soundpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soundpad.Views.Pages
{
    public partial class CategoriesCreate
    {
        [Inject] private IStorageService _storageService { get; set; }
        [Inject] private NavigationManager _navigation { get; set; }

        public CategoryModel Category { get; set; } = new CategoryModel();
        public string Error = "";

        private string _inputErrorCss = "input-error";
        //private string _inputWarningCss = "input-warning";


        public async Task CreateCategory()
        {
            // .NET Maui is kinda of shitty right now so validation forms don't work properly, because you can submit a form without the underlying Java code crashing
            if(string.IsNullOrEmpty(Category.Name))
            {
                Error = "Name field is required";
                Category.Name_Unique_CSS_ERROR = _inputErrorCss;
            }

            Error = "";
            Category.Name_Unique_CSS_ERROR = "";

            var result = await _storageService.CreateCategory(Category);
            if (result.Success)
            {
                _navigation.NavigateTo("/");
                return;
            }

            Error = result.Message;
            Category.Name_Unique_CSS_ERROR = _inputErrorCss;
        }

        public void Cancel()
        {
            _navigation.NavigateTo("/");
        }
    }

}
