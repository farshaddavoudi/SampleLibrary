using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Shared.Dto;
using ATA.Library.Shared.Service.Exceptions;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.Service.Category
{
    public class CategoryWebService : ICategoryWebService
    {
        private readonly ICategoryHostService _categoryHostService;
        private readonly IToastService _toastService;

        #region Constructor Injections

        public CategoryWebService(ICategoryHostService categoryHostService, IToastService toastService)
        {
            _categoryHostService = categoryHostService;
            _toastService = toastService;
        }

        #endregion

        public async Task<List<CategoryDto>> GetCategories()
        {
            var categoriesResult = await _categoryHostService.GetCategories();

            if (categoriesResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
                throw new InvalidOperationException(msg);
            }

            if (!categoriesResult!.IsSuccess)
            {
                _toastService.ShowError(categoriesResult.Message);
                throw new InvalidOperationException(categoriesResult.Message);
            }

            var categories = categoriesResult.Data;

            if (categories == null || categories.Count == 0)
            {
                var errorMessage = "هیچ دسته‌ی مجازی برای شما وجود ندارد. با پشتیبانی تماس بگیرید";
                _toastService.ShowError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            return categories;
        }

        public async Task AddCategory(CategoryDto category)
        {
            var addResult = await _categoryHostService.AddCategory(category);

            if (addResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
                throw new DomainLogicException(msg);
            }

            if (!addResult.IsSuccess)
            {
                _toastService.ShowError(addResult.Message);
                throw new DomainLogicException(addResult.Message ?? "HttpPost call failed");
            }
        }

        public async Task EditCategory(CategoryDto category)
        {
            var editResult = await _categoryHostService.EditCategory(category);

            if (editResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
                throw new DomainLogicException(msg);
            }

            if (!editResult.IsSuccess)
            {
                _toastService.ShowError(editResult.Message);
                throw new DomainLogicException(editResult.Message ?? "HttpPut call failed");
            }
        }

        public async Task DeleteCategory(CategoryDto category)
        {
            var deleteResult = await _categoryHostService.DeleteCategory(category);

            if (deleteResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
                throw new DomainLogicException(msg);
            }

            if (!deleteResult.IsSuccess)
            {
                _toastService.ShowError(deleteResult.Message);
                throw new DomainLogicException(deleteResult.Message ?? "HttpDelete call failed");
            }
        }
    }
}