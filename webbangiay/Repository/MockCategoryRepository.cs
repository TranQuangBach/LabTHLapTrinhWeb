using webbangiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace webbangiay.Repository
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private static readonly List<Category> _categoryList = new List<Category>();
        private static int _nextId;
        private static readonly object _lock = new object();

        public MockCategoryRepository()
        {
            lock (_lock)
            {
                if (_categoryList == null)
                {
                    _categoryList = new List<Category>
                {
                    new Category { Id = 1, Name = "Style" },
                    new Category { Id = 2, Name = "Running" },
                    new Category { Id = 3, Name = "Basketball" },
                    new Category { Id = 4, Name = "Skateboard" }
                };
                    _nextId = _categoryList.Max(c => c.Id) + 1;
                }
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }

        public Category GetCategoryById(int id)
        {
            return _categoryList.FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Category category)
        {
            category.Id = _nextId++;
            _categoryList.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            var index = _categoryList.FindIndex(c => c.Id == category.Id);
            if (index != -1)
            {
                _categoryList[index] = category;
            }
        }

        public void DeleteCategory(int id)
        {
            var category = _categoryList.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categoryList.Remove(category);
            }
        }

        public void SaveChanges()
        {
            System.Console.WriteLine("Mock: Category changes saved successfully");
        }
    }
}