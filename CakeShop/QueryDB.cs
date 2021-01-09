using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public List<TypeCake> getOriginTypeCakeList()
        {
            var query = db.TypeCakes;
            return query.ToList();
        }
        public int getLastIDTypeCake()
        {
            var query = db.TypeCakes.ToList();

            return query.Last().ID;
        }
        public string getNameTypeCakeByID(int ID)
        {
            var entityVal = db.TypeCakes.Find(ID);

            if (entityVal != null)
            {
                return entityVal.NameTypeCake;
            }
            else
            {
                return null;
            }
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
                        .Join(db.TypeCakes,
                            product => product.IDTypeCake,
                            type => type.ID,
                            (product, type) => new
                            {
                                ID = product.ID,
                                NameCake = product.Name,
                                Type = type.ID,
                                Price = product.Price,
                                Amount = product.Amount,
                                CountInValidDate = product.Amount,
                                Description = product.Description,
                            }
                        );
            //foreach (var item in query.ToList())
            //{
            //    Debug.WriteLine($"/> {item.Key.NameTypeCake}");
            //    Debug.WriteLine($"-- {item.liQua.Sum(x => x ?? 0)}");
            //}
            return query.ToList();
        }

        public int getLastIDCake()
        {
            var query = db.Products.ToList();
            if (query.Count == 0)
            {
                return 0;
            }
            else
            {
                return query.Last().ID;
            }
        }
        public dynamic getListImageOfProduct(int ID)
        {
            var query = db.ProductImages
                        .Where(
                            entity => entity.ID_Product == ID
                        );
            return query.ToList();
        }
        public bool hasSameCake(Product product)
        {
            var listCake = db.Products.ToList();
            foreach (var item in listCake)
            {
                if (item.ID != product.ID && item.Name.Equals(product.Name) && item.IDTypeCake == product.IDTypeCake)
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
        public void addCake(Product product, BindingList<ProductImage> insertImage)
        {
            db.Products.Add(product);
            db.SaveChanges();

            var realID = this.getLastIDCake();
            Debug.WriteLine($">{realID}");
            foreach (var item in insertImage)
            {
                ProductImage newImage = new ProductImage()
                {
                    ID_Product = realID,
                    ImageName = item.ImageName,
                    ID = db.ProductImages.Count() > 0 ? db.ProductImages.Last().ID + 1 : 0,
                };

                db.ProductImages.Add(newImage);
                db.SaveChanges();
            }
        }
        public void updateCake(Product product)
        {
            var cake = db.Products.Find(product.ID);

            cake.Name = product.Name;
            cake.IDTypeCake = product.IDTypeCake;
            cake.Price = product.Price;
            cake.Description = product.Description;

            db.SaveChanges();
        }
        public void deleteACake(Product product)
        {
            var cake = db.TypeCakes.Find(product.ID);
            db.TypeCakes.Remove(cake);
            db.SaveChanges();
        }

    }
}
