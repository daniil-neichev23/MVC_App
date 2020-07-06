using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        //Когда инфраструктуре MVC необходимо создать новый экземпляр класса
        //ProductController для обработки HTTP-запроса, MVC проинспектирует конструктор и 
        //выяснит, что он требует объекта, который реализует интерфейс IProductRepository
        //Чтобы определить какой класс реализации должен использоваться, MVC обращается
        //к конфигурации в классе StartUp, которая сообщает о том, что нужно применять
        //класс FakeRepository и каждый раз создавать его новый экземпляр
        //Такой подход называется ВНЕДРЕНИЕ ЗАВИСИМОСТЕЙ!!!!!!
        public ViewResult List(string category,int productPage = 1) =>
            View(new ProductListViewModel
            {
                Products = repository.Products
                .Where(p=>category == null || p.Category==category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
            .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null?
                    repository.Products.Count():
                    repository.Products.Where(e=>
                    e.Category==category).Count()
                },
                CurrentCategory=category
            });
    }
}
