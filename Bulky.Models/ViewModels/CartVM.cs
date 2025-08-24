
    using Bulky.Models.Models;

    namespace Bulky.Models.ViewModels;

    public class CartVM
    {
        public IEnumerable<Cart> CartItems { get; set; } = new List<Cart>();
        public OrderHeader OrderHeader { get; set; }

    }
