namespace mvvm.Models
{
    public class DbProduct
    {
        public string Image { get; set; }
        public string DisplayedImage
        {
            get { return Path.GetFullPath($@"Resources\Image\{Image}"); }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public float Price { get; set; }
        public int Discount { get; set; }
        public int Quantity { get;set; }
        public string Article { get; set; }
        public int Count { get; set; }
        public float? DisplayedPrice
        {
            get 
            {
                //return Price - (Price * Discount / 100);
                if (Discount != 0)
                    return Price - (Price * Discount / 100);
                else
                    return null;
            }
        }

    }
}
