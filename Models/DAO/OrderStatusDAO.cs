using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderStatusDAO
    {
        ShopDienThoaiDbContext db = null;
        public OrderStatusDAO()
        {
            db = new ShopDienThoaiDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<List<ORDERSTATU>> LoadStatus()
        {
            return await db.ORDERSTATUS.AsNoTracking().ToListAsync();
        }
    }
}
