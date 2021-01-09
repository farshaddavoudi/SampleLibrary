using ATA.Library.Shared.Dto;
using System;
using System.Collections.Generic;

namespace ATA.Library.Client.Web.Service
{
    public class AppData
    {
        private List<CategoryDto>? _categories;
        public List<CategoryDto> Categories
        {
            get => _categories ?? new List<CategoryDto>();
            set
            {
                _categories = value;
                NotifyDataChanged();
            }
        }

        public event Action? OnChange;
        private void NotifyDataChanged() => OnChange?.Invoke();
    }
}