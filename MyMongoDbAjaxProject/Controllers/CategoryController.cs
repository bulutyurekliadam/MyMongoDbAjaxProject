using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyMongoDbAjaxProject.DAL.Entities;
using MyMongoDbAjaxProject.DAL.Settings;

namespace MyMongoDbAjaxProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryController(IDataBaseSettings _dataBaseSettings)
        {
            var client = new MongoClient(_dataBaseSettings.ConnectionString);
            var database = client.GetDatabase(_dataBaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(_dataBaseSettings.CategoryCollectionName);

        }
        public async Task<IActionResult> Index()
        {
            var values = await _categoryCollection.Find(x => true).ToListAsync();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            await _categoryCollection.InsertOneAsync(category);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryCollection.DeleteOneAsync(x => x.CategoryID == id) ;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            var values = await _categoryCollection.Find<Category>(x => x.CategoryID == id).FirstOrDefaultAsync();
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            await _categoryCollection.FindOneAndReplaceAsync(x => x.CategoryID == category.CategoryID, category);
            return RedirectToAction("Index");
        }
    }
}
