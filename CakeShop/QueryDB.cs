using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop
{
    class QueryDB
    {
        CakeStoreDBEntities db;
        private static QueryDB _instance = null;

        public static QueryDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QueryDB();
                }
                return _instance;
            }
        }

        private QueryDB() {
            this.db = new CakeStoreDBEntities();
        }

        public dynamic getBindingTypeCake()
        {
            var query = db.TypeCakes
                        .GroupJoin(db.Products,
                            type => type.ID,
                            product => product.IDTypeCake,
                            (type, product) => new
                            {
                                TypeCakes = type,
                                Products = product,
                            }
                        )
                        .SelectMany(
                            empty => empty.Products.DefaultIfEmpty(),
                            (type, product) => new
                            {
                                TypeCakes = type.TypeCakes,
                                Products = product,
                            }
                        )
                        .GroupBy(
                            obj => obj.TypeCakes,
                            obj => obj.Products.Amount,
                            (key, listQuantity) => new
                            {
                                Key = key,
                                liQua = listQuantity.ToList()
                            }
                        )
                        .Select(type => new
                        {
                            ID = type.Key.ID,
                            NameTypeCake = type.Key.NameTypeCake,
                            Amount = type.liQua.Sum(x => x ?? 0),
                            AmountType = (type.liQua.Sum(x => x ?? 0) > 0 ? type.liQua.Count() : 0)
                        });
            //foreach (var item in query.ToList())
            //{
            //    Debug.WriteLine($"/> {item.Key.NameTypeCake}");
            //    Debug.WriteLine($"-- {item.liQua.Sum(x => x ?? 0)}");
            //}
            return query.ToList();
        }

        public int getLastIDTypeCake()
        {
            var query = db.TypeCakes.ToList();

            return query.Last().ID;
        }
        public bool hasSameNameTypeCake(TypeCake Type)
        {
            var listTypeCake = db.TypeCakes.ToList();
            foreach(var item in listTypeCake)
            {
                if (item.ID != Type.ID && item.NameTypeCake.Equals(Type.NameTypeCake))
                {
                    return true;
                }
                else
                {
                    // do nothing
                }
            }

            return false;
        }
        public void addATypeCake(TypeCake typeCake)
        {
            db.TypeCakes.Add(typeCake);
            db.SaveChanges();
        }
        public void updateATypeCake(TypeCake typeCake)
        {
            var type = db.TypeCakes.Find(typeCake.ID);
            type.NameTypeCake = typeCake.NameTypeCake;
            db.SaveChanges();
        }
        public void deleteATypeCake(TypeCake typeCake)
        {
            var type = db.TypeCakes.Find(typeCake.ID);
            db.TypeCakes.Remove(type);
            db.SaveChanges();
        }

        public dynamic getBindingCakeList()
        {
            var query = db.Products
                        .Select(product => new
                        {
                            NameCake = product.Name,
                            Type = "none",
                            Price = product.Price,
                            Amount = product.Amount,
                            CountInValidDate = 0,
                        });
            //foreach (var item in query.ToList())
            //{
            //    Debug.WriteLine($"/> {item.Key.NameTypeCake}");
            //    Debug.WriteLine($"-- {item.liQua.Sum(x => x ?? 0)}");
            //}
            return query.ToList();
        }
    }
}
