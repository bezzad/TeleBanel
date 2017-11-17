namespace TeleBanel.Models.Middlewares
{
    public interface IProductMiddleware
    {
        Product[] GetProducts();
        int[] GetProductsId();
        Product GetProduct(int pId);
        void SetProduct(Product p);
        void DeleteProduct(int pId);
    }
}