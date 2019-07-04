using Ekart.Models;
using Ekart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekart.Services
{
    class DataService
    {
        MainViewModel viewModelObject;
        List<Products> listOfProducts;
        public DataService()
        {
            listOfProducts = new List<Products>();
            GetProductsList();
            //viewModelObject = new MainViewModel();
            //listOfProducts = viewModelObject.GetProductsList();
        }
        public List<Products> GetProductsList()
        {
            listOfProducts = new List<Products>();
            for (int i = 0; i < 1000; i++)
            {
                listOfProducts.Add(new Products() { ProductCategory = "Electronics", ProductID = (i + 1).ToString(), ProductName = "OnePlus 7 Pro", ProductPrice = "Rs. 57999" });
            }

            return listOfProducts;
        }
        public async Task<List<Products>> GetProductsAsync (int pageIndex,int pageSize)
        {
            await Task.Delay(2000);
            return listOfProducts.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
    }
}
