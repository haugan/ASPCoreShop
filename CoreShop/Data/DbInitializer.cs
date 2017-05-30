using CoreShop.Models;
using System.Linq;

namespace CoreShop.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext ctx)
        {
            ctx.Database.EnsureCreated();

            //
            // SEED PRODUCT DATA
            if (ctx.Products.Any()) return;

            var products = new Product[]
            {
                new Product {ProductNumber="PR-011011", Name="AAAA", Price=100, OnSale=true, ImgURL1="http://placehold.it/400?text=[A1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-022022", Name="BBBB", Price=200, OnSale=true, ImgURL1="http://placehold.it/400?text=[B1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-033033", Name="CCCC", Price=300, OnSale=true, ImgURL1="http://placehold.it/400?text=[C1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-044044", Name="DDDD", Price=400, OnSale=true, ImgURL1="http://placehold.it/400?text=[D1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-055055", Name="EEEE", Price=500, OnSale=true, ImgURL1="http://placehold.it/400?text=[E1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-066066", Name="FFFF", Price=600, OnSale=true, ImgURL1="http://placehold.it/400?text=[F1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-070707", Name="GGGG", Price=600, OnSale=true, ImgURL1="http://placehold.it/400?text=[G1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-080808", Name="HHHH", Price=600, OnSale=true, ImgURL1="http://placehold.it/400?text=[H1]",  DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."}
            };

            foreach (Product p in products)
            {
                ctx.Products.Add(p);
            }

            //ctx.SaveChanges();

            //
            // SEED CUSTOMER DATA
            if (ctx.Customers.Any()) return;
            var customers = new Customer[]
            {
                new Customer {CustomerNumber="CU-011011", Firstname="AAAA", Lastname="AAAA", Email="aaa@aaa.net"},
                new Customer {CustomerNumber="CU-022022", Firstname="BBBB", Lastname="AAAA", Email="bbb@bbb.net"},
                new Customer {CustomerNumber="CU-033033", Firstname="CCCC", Lastname="CCCC", Email="ccc@ccc.net"}
            };
            foreach (Customer c in customers)
            {
                ctx.Customers.Add(c);
            }
            ctx.SaveChanges();
        }
    }
}
