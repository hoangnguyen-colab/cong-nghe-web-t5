using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class BrandDAO
    {
        ShopDienThoaiDbContext db = null;
        public BrandDAO()
        {
            db = new ShopDienThoaiDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<List<BRAND>> LoadData()
        {
            return await db.BRANDs.AsNoTracking().ToListAsync();
        }

        public async Task<BRAND> LoadByID(int id)
        {
            return await db.BRANDs
                .AsNoTracking()
                .Where(x => x.BrandID.Equals(id))
                .SingleOrDefaultAsync();
        }

        public async Task<BRAND> LoadByURL(string url)
        {
            return await db.BRANDs
                .AsNoTracking()
                .Where(x => x.BrandURL.Equals(url))
                .SingleOrDefaultAsync();
        }

        public async Task<int> CreateCate(BRAND cate)
        {
            try
            {
                db.BRANDs.Add(cate);
                await db.SaveChangesAsync();
                return cate.BrandID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteCate(int ID)
        {
            try
            {
                var prod = await db.BRANDs.Where(x => x.BrandID == ID).SingleOrDefaultAsync();
                db.BRANDs.Remove(prod);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> EditCate(BRAND cate, int ID)
        {
            try
            {
                var item = db.BRANDs.Find(ID);
                if (item == null)
                {
                    return 0;
                }
                item.BrandName = cate.BrandName;
                item.BrandURL = cate.BrandURL;
                await db.SaveChangesAsync();
                return item.BrandID;
            }
            catch
            {
                return 0;
            }
        }
    }
}
