using AutoMapper;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Talabat.DashBoard.Helpers;
using Talabat.DashBoard.Models;

namespace Talabat.DashBoard.Controllers
{
    public class ProductController(IUnitOfWork _unitOfWork, IMapper _mapper) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.GetRepository<Product,int>().GetAllAsync();
            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>((IReadOnlyList<Product>)products);
            return View(mappedProducts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                {
                    productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
                }
                else
                    productViewModel.PictureUrl = "images/products/glazed-donuts.png";
                //var mappedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);

                var mappedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
                mappedProduct.CreatedBy = User.Identity?.Name ?? "Admin";
                mappedProduct.CreatedOn = DateTime.UtcNow;
                mappedProduct.LastModifiedBy = User.Identity?.Name ?? "Admin";
                mappedProduct.NormalizedName = User.Identity?.Name ?? "Admin";
                mappedProduct.LastModifiedOn = DateTime.UtcNow;

                await _unitOfWork.GetRepository<Product, int>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }

            return View(productViewModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.GetRepository<Product,int>().GetAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(productViewModel);

            var existingProduct = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            if (existingProduct is null)
                return NotFound();

            existingProduct.Name = productViewModel.Name;
            existingProduct.Description = productViewModel.Description;
            existingProduct.Price = productViewModel.Price;
            existingProduct.BrandId = productViewModel.ProductBrandId;
            existingProduct.CategoryId = productViewModel.ProductTypeId;
            existingProduct.NormalizedName = productViewModel.Name.ToUpper();
            existingProduct.LastModifiedBy = User.Identity?.Name ?? "Admin";
            existingProduct.LastModifiedOn = DateTime.UtcNow;

            if (productViewModel.Image != null)
            {
                if (!string.IsNullOrEmpty(existingProduct.PictureUrl))
                    PictureSettings.DeleteFile(existingProduct.PictureUrl, "products");

                existingProduct.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
            }

            _unitOfWork.GetRepository<Product, int>().Update(existingProduct);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
                return RedirectToAction("Index");

            return View(productViewModel);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.GetRepository<Product ,int>().GetAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
                return NotFound();
            try
            {
                var product = await _unitOfWork.GetRepository<Product,int>().GetAsync(id);
                if (product.PictureUrl != null)
                {
                    PictureSettings.DeleteFile(product.PictureUrl, "products");
                }
                _unitOfWork.GetRepository<Product, int>().Delete(product);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                return View(productViewModel);
            }
        }
    }
}
