using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductDAO
    {
        ShopDienThoaiDbContext db = null;
        public ProductDAO()
        {
            db = new ShopDienThoaiDbContext();
        }

        public async Task<int> CreateProduct(PRODUCT model)
        {
            try
            {
                db.PRODUCTs.Add(model);
                await db.SaveChangesAsync();
                return model.ProductID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteProduct(int ID)
        {
            try
            {
                var prod = await db.PRODUCTs.FindAsync(ID);
                if (prod == null)
                {
                    return false;
                }
                db.PRODUCTs.Remove(prod);
                await db.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<PRODUCT> LoadByID(int ID)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductID == ID).FirstOrDefaultAsync();
        }

        public async Task<List<PRODUCT>> LoadProduct()
        {
            return await db.PRODUCTs.AsNoTracking().ToListAsync();
        }

        public async Task<List<PRODUCT>> LoadName(string prefix)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductName.Contains(prefix)).ToListAsync();
        }

        public async Task<PRODUCT> LoadNameByID(int id)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductID == id).FirstOrDefaultAsync();
        }

        public async Task<List<PRODUCT>> LoadProduct(string url, string searchString, string sort, int pagesize, int pageindex)
        {
            // get list
            var list = (from s in db.PRODUCTs select s).AsNoTracking();

            if (!String.IsNullOrEmpty(url))
            {
                var brand = db.BRANDs.AsNoTracking().Where(c => c.BrandURL.Equals(url)).First().BrandID;
                list = list.Where(x => x.BrandID.Equals(brand));
            }

            //filter
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.ProductName.Contains(searchString));
            }
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.ProductName);
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.ProductName);
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.ProductPrice);
                    break;
                case "price_asc":
                    list = list.OrderBy(s => s.ProductPrice);
                    break;
                default:
                    list = list.OrderBy(s => s.ProductID);
                    break;
            }
            //return
            return await list.Skip(pageindex * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<PRODUCT> LoadByURL(string url)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductURL == url).FirstOrDefaultAsync();
        }
    }
}
