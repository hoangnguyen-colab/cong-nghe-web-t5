using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ConfigurationDAO
    {
        ShopDienThoaiDbContext db = null;
        public ConfigurationDAO()
        {
            db = new ShopDienThoaiDbContext();
        }

        public async Task<CONFIGURATION> LoadConfig(int id)
        {
            return await db.CONFIGURATIONs.AsNoTracking().Where(x => x.ProductID == id).FirstOrDefaultAsync();
        }
    }
}
