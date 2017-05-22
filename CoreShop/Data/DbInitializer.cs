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
                new Product {ProductNumber="PR-0001", Name="aaaaaa", Price=100, OnSale=true, ImgURL1="http://placehold.it/400?text=[A1]", ImgURL2="http://placehold.it/400?text=[A2]", ImgURL3="http://placehold.it/400?text=[A3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0002", Name="bbbbbb", Price=200, OnSale=true, ImgURL1="http://placehold.it/400?text=[B1]", ImgURL2="http://placehold.it/400?text=[B2]", ImgURL3="http://placehold.it/400?text=[B3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0003", Name="cccccc", Price=300, OnSale=true, ImgURL1="http://placehold.it/400?text=[C1]", ImgURL2="http://placehold.it/400?text=[C2]", ImgURL3="http://placehold.it/400?text=[C3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0004", Name="dddddd", Price=400, OnSale=true, ImgURL1="http://placehold.it/400?text=[D1]", ImgURL2="http://placehold.it/400?text=[D2]", ImgURL3="http://placehold.it/400?text=[D3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0005", Name="eeeeee", Price=500, OnSale=true, ImgURL1="http://placehold.it/400?text=[E1]", ImgURL2="http://placehold.it/400?text=[E2]", ImgURL3="http://placehold.it/400?text=[E3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0006", Name="ffffff", Price=600, OnSale=true, ImgURL1="http://placehold.it/400?text=[F1]", ImgURL2="http://placehold.it/400?text=[F2]", ImgURL3="http://placehold.it/400?text=[F3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0007", Name="gggggg", Price=700, OnSale=true, ImgURL1="http://placehold.it/400?text=[G1]", ImgURL2="http://placehold.it/400?text=[G2]", ImgURL3="http://placehold.it/400?text=[G3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."},
                new Product {ProductNumber="PR-0008", Name="hhhhhh", Price=800, OnSale=true, ImgURL1="http://placehold.it/400?text=[H1]", ImgURL2="http://placehold.it/400?text=[H2]", ImgURL3="http://placehold.it/400?text=[H3]", DescriptionShort="Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum.", DescriptionLong="Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit."}
            };

            foreach (Product p in products)
            {
                ctx.Products.Add(p);
            }

            ctx.SaveChanges();

            //
            // SEED CUSTOMER DATA
            if (ctx.Customers.Any()) return;

            var customers = new Customer[]
            {
                new Customer {CustomerNumber="CU-0001", Firstname="aaaaaa", Lastname="aaaaaa", Email="aaa@aaa.net"},
                new Customer {CustomerNumber="CU-0002", Firstname="bbbbbb", Lastname="bbbbbb", Email="bbb@bbb.net"},
                new Customer {CustomerNumber="CU-0003", Firstname="cccccc", Lastname="cccccc", Email="ccc@ccc.net"}
            };

            foreach (Customer c in customers)
            {
                ctx.Customers.Add(c);
            }

            ctx.SaveChanges();

            ////
            //// SEED ORDER DATA
            //if (ctx.Customers.Any()) return;

            //var orders = new Order[]
            //{
            //    new Order {}
            //};

            //foreach (Order o in orders)
            //{
            //    ctx.Orders.Add(o);
            //}

            //ctx.SaveChanges();

            ////
            //// SEED ORDER ITEM DATA
            //if (ctx.OrderItems.Any()) return;

            //var orderItems = new OrderItem[]
            //{
            //    new OrderItem {OrderID=1, ProductID=1, Quantity=1}
            //};

            //foreach (OrderItem oi in orderItems)
            //{
            //    ctx.OrderItems.Add(oi);
            //}

            //ctx.SaveChanges();
        }
    }
}
