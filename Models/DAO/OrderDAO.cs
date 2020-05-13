using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDAO
    {
        ShopDienThoaiDbContext db = null;
        public OrderDAO()
        {
            db = new ShopDienThoaiDbContext();
        }

        public async Task<int> AddOrder(int CustomerID, decimal total)
        {
            try
            {
                var order = new ORDER
                {
                    Total = total,
                    OrderDate = DateTime.Now,
                    OrderStatusID = 1,
                    CustomerID = CustomerID
                };
                db.ORDERs.Add(order);
                await db.SaveChangesAsync();
                return order.OrderID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<ORDER> LoadByID(int OrderID)
        {
            return await db.ORDERs.AsNoTracking().Where(x => x.OrderID == OrderID).FirstOrDefaultAsync();
        }

        public async Task<List<ORDER>> LoadOrder()
        {
            return await db.ORDERs.AsNoTracking().ToListAsync();
        }

        public async Task<List<ORDER>> LoadOrder(int CustomerID)
        {
            return await db.ORDERs.AsNoTracking().Where(x => x.CustomerID == CustomerID).ToListAsync();
        }

        public async Task<List<T>> LoadOrder<T>(int CustomerID)
        {
            var Param = new SqlParameter("@CustomerID", CustomerID);

            return await db.Database
                 .SqlQuery<T>("SelectOrder @CustomerID", Param)
                 .ToListAsync();
        }

        public async Task<List<ORDERDETAIL>> LoadProductOrder(int OrderID)
        {
            return await db.ORDERDETAILs.AsNoTracking().Where(x => x.OrderID == OrderID).ToListAsync();
        }

        public async Task<List<T>> LoadProductOrder<T>(int OrderID)
        {
            var Param = new SqlParameter("@OrderID", OrderID);

            return await db.Database
                 .SqlQuery<T>("SelectOrderProduct @OrderID", Param)
                 .ToListAsync();
        }

        public async Task<int> ChangeOrder(int OrderID, int StatusID)
        {
            try
            {
                var order = await db.ORDERs.FindAsync(OrderID);
                if (order == null)
                {
                    return 0;
                }
                var status = await db.ORDERSTATUS.FindAsync(StatusID);
                if (status == null)
                {
                    return 0;
                }
                if (order.OrderStatusID == StatusID)
                {
                    return 0;
                }
                order.OrderStatusID = StatusID;
                await db.SaveChangesAsync();
                return order.OrderID;
            }
            catch
            {
                return 0;
            }
        }
    }
}
