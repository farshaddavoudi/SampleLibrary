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
#if DEBUG
                throw new InvalidOperationException(msg);
#endif
            }

            if (!categoriesResult!.IsSuccess)
            {
                _toastService.ShowError(categoriesResult.Message);
#if DEBUG
                throw new InvalidOperationException(categoriesResult.Message);
# endif
            }

            var categories = categoriesResult.Data;

            if (categories == null || categories.Count == 0)
            {
                var errorMessage = "هیچ دسته‌ی مجازی برای شما وجود ندارد. با پشتیبانی تماس بگیرید";
                _toastService.ShowError(errorMessage);
                return new List<CategoryDto>();
            }

            return categories;
        }

        public async Task AddCategory(CategoryDto category)
        {
            var addResult = await _categoryHostService.PostAddCategory(category);

            if (addResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
#if DEBUG
                throw new DomainLogicException(msg);
#endif
            }

            if (!addResult.IsSuccess)
            {
                _toastService.ShowError(addResult.Message);
#if DEBUG
                throw new DomainLogicException(addResult.Message ?? "HttpPost call failed");
#endif
            }
        }

        public async Task EditCategory(CategoryDto category)
        {
            var editResult = await _categoryHostService.PutEditCategory(category);

            if (editResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
#if DEBUG
                throw new DomainLogicException(msg);
#endif
            }

            if (!editResult.IsSuccess)
            {
                _toastService.ShowError(editResult.Message);
#if DEBUG
                throw new DomainLogicException(editResult.Message ?? "HttpPut call failed");
#endif
            }
        }

        public async Task DeleteCategory(CategoryDto category)
        {
            var deleteResult = await _categoryHostService.DeleteCategory(category);

            if (deleteResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
#if DEBUG
                throw new DomainLogicException(msg);
#endif
            }

            if (!deleteResult.IsSuccess)
            {
                _toastService.ShowError(deleteResult.Message);
#if DEBUG
                throw new DomainLogicException(deleteResult.Message ?? "HttpDelete call failed");
#endif
            }
        }
    }
}