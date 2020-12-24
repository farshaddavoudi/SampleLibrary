using ATA.Library.Server.Model.Entities.Category;
using ATA.Library.Server.Service.Category.Contracts;
using ATA.Library.Shared.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Controllers.api.Category
{
    /// <summary>
    /// Book Categories based on subjects and related units
    /// </summary>
    [ApiVersion("1")]
    [AllowAnonymous]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        #region Constructor Injections

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        #endregion

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            return Ok(await _categoryService.GetAsync(cancellationToken));
        }

        /// <summary>
        /// Add a category to the categories list
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<IActionResult> AddCategory(CategoryDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<CategoryEntity>(dto);

            var addedEntity = await _categoryService.AddAsync(entity, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, addedEntity);
        }

        /// <summary>
        /// Edit an already existing category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> EditCategory(int categoryId, CategoryDto dto, CancellationToken cancellationToken)
        {
            var entity = await _categoryService.GetByIdAsync(categoryId, cancellationToken);

            if (entity == null)
                return NotFound($"هیچ دسته‌ای با این شناسه پیدا نشد. شناسه‌ی ارسالی = {categoryId}");

            dto.CreatedAt = entity.CreatedAt; //fix CreateAt mapping issue

            _mapper.Map(dto, entity);

            await _categoryService.UpdateAsync(entity, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete a category from list of categories
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteCategory(int categoryId, CancellationToken cancellationToken)
        {
            var entity = await _categoryService.GetByIdAsync(categoryId, cancellationToken);

            if (entity == null)
                return NotFound($"هیچ دسته‌ای با این شناسه پیدا نشد. شناسه‌ی ارسالی = {categoryId}");

            // todo: Check any book is using that categoryId

            await _categoryService.DeleteAsync(categoryId, cancellationToken);

            return NoContent();
        }
    }
}