
using BookShop.Models;
using System.Collections.Generic;

namespace BookShop.Infrastructure.Dto
{
    public class OrderItemsGroup
    {
        private int _totalItemsCount = 0;
        private decimal _totalItemsCost = 0;
        public int TotalItemsCount {
            get
            {
                if(_totalItemsCount == 0)
                    OrderItems.ForEach(orderItem => _totalItemsCount += orderItem.Count);

                return _totalItemsCount;
            }
        }
        public decimal TotalItemsCost
        {
            get
            {
                if(_totalItemsCost == 0)
                    OrderItems.ForEach(orderItem => _totalItemsCost += orderItem.TotalСost);

                return _totalItemsCost;

            }
        }
        public List<BasketProductInfo> OrderItems { get; set; } = new();
    }
}
