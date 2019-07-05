using Ekart.Models;
using Ekart.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;
using XLabs.Forms.Behaviors;
using XLabs.Forms.Controls;

namespace Ekart.ViewModels
{
    class MainViewModel: INotifyPropertyChanged
    {
        public InfiniteScrollCollection<Products> productCollection { get; }
        public List<Products> listOfProducts;
        readonly DataService dataService=new DataService();
        private bool _isBusy;
        private const int PageSize = 10;
        public RelayGesture DumpGesture { get; set; }
        public bool IsLoadingVisible { get; set; }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            IsLoadingVisible = true;
            DumpGesture = new RelayGesture(OnGesture);
            //GetProductsCollection();
            productCollection = new InfiniteScrollCollection<Products>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    // load the next page
                    var page = productCollection.Count / PageSize;

                    var prods = await dataService.GetProductsAsync(page, PageSize);

                    IsBusy = false;

                    // return the items that need to be added
                    return prods;
                },
                OnCanLoadMore = () =>
                {
                    return productCollection.Count < 44;
                }
            };
            IsLoadingVisible = false;
            GetProductsListAsync();

        }

        //public InfiniteScrollCollection<Products> GetProductsCollection()
        //{
        //    productCollection = new InfiniteScrollCollection<Products>
        //    {
        //        OnLoadMore = async () =>
        //        {
        //            IsBusy = true;

        //            // load the next page
        //            var page = productCollection.Count / PageSize;

        //            var prods = await dataService.GetProductsAsync(page, PageSize);

        //            IsBusy = false;

        //            // return the items that need to be added
        //            return prods;
        //        },
        //        OnCanLoadMore = () =>
        //        {
        //            return productCollection.Count < 44;
        //        }
        //    }; ;
        //    return productCollection;
        //}
        //public List<Products> GetProductsList()
        //{
        //    listOfProducts = new List<Products>();
        //    for (int i = 0; i < 100; i++)
        //    {
        //        listOfProducts.Add(new Products() { ProductCategory = "Electronics", ProductID = (i + 1).ToString(), ProductName = "OnePlus 7 Pro", ProductPrice = "Rs. 57999" });
        //    }
            
        //    return listOfProducts;
        //}
        public async Task GetProductsListAsync()
        {
            var products = await dataService.GetProductsAsync(pageIndex: 0, pageSize: PageSize);
            productCollection.AddRange(products);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void OnGesture(GestureResult gr, object obj)
        {
            switch (gr.GestureType)
            {
                case GestureType.SingleTap:
                    await App.Current.MainPage.DisplayAlert("Product Selected", "You've tapped once to select a product", "Okay");
                    break;
                
                case GestureType.LongPress:
                    var action = await App.Current.MainPage.DisplayActionSheet("More Options", "Cancel", null, "Share", "Add to Cart", "Add to Wish List");
                    break;
                
            }
        }
    }
}
