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
                            AmountType = type.liQua.Count(x => (x ?? -1) >= 0)
                        });
            //foreach (var item in query.ToList())
            //{
            //    Debug.WriteLine($"/> {item.Key.NameTypeCake}");
            //    Debug.WriteLine($"-- {item.liQua.Sum(x => x ?? 0)}");
            //}
            return query.ToList();
        }
        public dynamic getTopTypeCake(int rank)
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
                            AmountType = type.liQua.Count(x => (x ?? -1) >= 0)
                        })
                        .OrderByDescending(x => x.AmountType)
                        .ThenByDescending(x => x.Amount)
                        .Take(rank)
                        .Select(type => new
                        {
                            ID = type.ID,
                            NameTypeCake = type.NameTypeCake,
                        });
            return query.ToList();
        }
        public dynamic getBindingCakeImport()
        {
            var query = db.CakeImportOrders
                        .Join(db.Products,
                            cakeImport => cakeImport.ProductID,
                            product => product.ID,
                            (cakeImport, product) => new
                            {
                                ID = cakeImport.ID,
                                ImportOrderName = cakeImport.ImportOrderName,
                                ProductID = cakeImport.ProductID,
                                NameCake = product.Name,
                                ImportDate = cakeImport.ImportDate,
                                ExpirationDate = cakeImport.ExpirationDate,
                                Quantity = cakeImport.Quantity,
                                ImportPrice = cakeImport.ImportPrice,
                                Total = cakeImport.Total
                            }
                        );

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
            foreach (var item in listTypeCake)
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

        public dynamic getBindingCakeList(int option = 0, string keyword = null, int typeFilter = -1)
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
                        ).GroupJoin(db.ProductImages,
                            entity => entity.ID,
                            image => image.ID_Product,
                            (entity, image) => new
                            {
                                Product = entity,
                                Image = image,
                            }
                        )
                        .SelectMany(
                            empty => empty.Image.DefaultIfEmpty(),
                            (product, image) => new
                            {
                                ProductEntity = product.Product,
                                ImageEntity = image,
                            }
                        )
                        .GroupBy(
                            obj => obj.ProductEntity,
                            (key, listImage) => new
                            {
                                Key = key,
                                ListImages = listImage.ToList()
                            }
                        )
                        .Select(
                            entity => new
                            {
                                ID = entity.Key.ID,
                                NameCake = entity.Key.NameCake,
                                Type = entity.Key.Type,
                                Price = entity.Key.Price,
                                Amount = entity.Key.Amount,
                                CountInValidDate = entity.Key.CountInValidDate,
                                Description = entity.Key.Description,
                                Thumbnail = entity.ListImages.FirstOrDefault().ImageEntity.ImageName
                            }
                        );
            switch (option)
            {
                case ConstantVariable.FILTER_ALL:
                    // do nothing
                    break;
                case ConstantVariable.SORT_BY_AZ:
                    query = query.OrderBy(x => x.NameCake);
                    break;
                case ConstantVariable.SORT_BY_ZA:
                    query = query.OrderByDescending(x => x.NameCake);
                    break;
                case ConstantVariable.SORT_BY_INC_PRICE:
                    query = query.OrderBy(x => x.Price);
                    break;
                case ConstantVariable.SORT_BY_DEC_PRICE:
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case ConstantVariable.FILTER_BY_TYPE:
                    query = query.Where(x => x.Type == typeFilter);
                    break;
                case ConstantVariable.FILTER_BY_KEYWORD:
                    keyword = ConstantVariable.Convertor_UNICODE_ASCII(keyword, true);
                    string[] keys = keyword.ToLower().Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                    StringSplitOptions.RemoveEmptyEntries);
                    var bindingQuery = query.Select(entity => new
                                                {
                                                    ID = entity.ID,
                                                    Name = entity.NameCake
                                                }).AsEnumerable()
                                                .Select(entity => new {
                                                    ID = entity.ID,
                                                    Name = (ConstantVariable.Convertor_UNICODE_ASCII(entity.Name, true) as string)
                                                })
                                                .Where(res => keys.All(s => (res.Name).Contains(s)))
                                                .Join(query,
                                                    key => key.ID,
                                                    entity => entity.ID,
                                                    (key, entity) => entity
                                                );
                    //foreach (var item in tempList)
                    //{
                    //    Debug.WriteLine($"-- {item.ID} - {item.NameCake}");
                    //}
                    return bindingQuery.ToList();
                    break;
            }

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
        public string getThumbnailOf1Product(int ID)
        {
            string nameThumbnail = null;

            var query = db.ProductImages.Where(x => x.ID_Product == ID);

            if(query.Count() == 0)
            {
                nameThumbnail = string.Empty;
            }
            else
            {
                nameThumbnail = query.FirstOrDefault().ImageName;
            }

            return nameThumbnail;
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
        public bool hasSameCakeImport(CakeImportOrder cakeImportOrder)
        {
            var listCakeImport = db.CakeImportOrders.ToList();
            foreach (var item in listCakeImport)
            {
                if (item.ID != cakeImportOrder.ID && item.ImportOrderName.Equals(cakeImportOrder.ImportOrderName) && item.ProductID == cakeImportOrder.ProductID)
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
        public List<Product> getOriginProductList()
        {
            var query = db.Products;
            return query.ToList();
        }
        public Product findProductByID(int ID)
        {
            Product product = db.Products.Find(ID);

            return product;
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
                    ID = db.ProductImages.Count() > 0 ? db.ProductImages.ToList().Last().ID + 1 : 0,
                };

                db.ProductImages.Add(newImage);
                db.SaveChanges();
            }
        }
        public void updateCake(Product product, BindingList<ProductImage> insertImage, BindingList<ProductImage> removeImage)
        {
            var cake = db.Products.Find(product.ID);
            cake.Name = product.Name;
            cake.IDTypeCake = product.IDTypeCake;
            cake.Price = product.Price;
            cake.Description = product.Description;
            db.SaveChanges();


            foreach(var image in removeImage)
            {
                if (image.ID != -1)     // -1 means a image had already added
                {
                    var item = db.ProductImages.Find(image.ID);
                    db.ProductImages.Remove(item);
                    db.SaveChanges();
                }
                else
                {
                    // do nothing
                }
            }
            foreach (var image in insertImage)
            {
                if (image.ID == -1)     // -1 means a image had already added
                {
                    var item = new ProductImage();
                    item.ID = db.ProductImages.ToList().Last().ID + 1;
                    item.ID_Product = product.ID;
                    item.ImageName = image.ImageName;
                    db.ProductImages.Add(item);
                    db.SaveChanges();
                }
                else
                {
                    // do nothing
                }
            }
        }
        public void deleteCake(Product product)
        {
            var cake = db.Products.Find(product.ID);

            if(cake != null)
            {
                var listImageDelete = db.ProductImages
                                    .Where(
                                        image => image.ID_Product == product.ID
                                    );
                db.ProductImages.RemoveRange(listImageDelete);
                db.SaveChanges();

                db.Products.Remove(cake);
                db.SaveChanges();
            }
            else
            {
                // do nothing
            }
        }
        public string getNameCakeByID(int ID)
        {
            var entityVal = db.Products.Find(ID);

            if (entityVal != null)
            {
                return entityVal.Name;
            }
            else
            {
                return null;
            }
        }
        public void addCakeImport(CakeImportOrder cakeImportOrder)
        {
            //Save to CakeImportOrders
            db.CakeImportOrders.Add(cakeImportOrder);
            db.SaveChanges();

            //Add amount to Products
            var productQuery = db.Products
                .Find(
                    cakeImportOrder.ProductID
                );
            productQuery.Amount += cakeImportOrder.Quantity;
            db.SaveChanges();
        }
        public void updateCakeImport(CakeImportOrder cakeImportOrder, int previousQuatity)
        {
            var cakeImport = db.CakeImportOrders.Find(cakeImportOrder.ID);
            cakeImport.ImportOrderName = cakeImportOrder.ImportOrderName;
            cakeImport.ProductID = cakeImportOrder.ProductID;
            cakeImport.ImportDate = cakeImportOrder.ImportDate;
            cakeImport.ExpirationDate = cakeImportOrder.ExpirationDate;
            cakeImport.Quantity = cakeImportOrder.Quantity;
            cakeImport.ImportPrice = cakeImportOrder.ImportPrice;
            cakeImport.Total = cakeImportOrder.Total;
            db.SaveChanges();


            //Add amount to Products
            var productQuery = db.Products
                .Find(
                    cakeImportOrder.ProductID
                );
            productQuery.Amount = productQuery.Amount - previousQuatity + cakeImport.Quantity;
            db.SaveChanges();

        }
        public void deleteCakeImport(CakeImportOrder cakeImportOrder)
        {
            var cakeImport = db.CakeImportOrders.Find(cakeImportOrder.ID);

            if (cakeImport != null)
            {
                db.CakeImportOrders.Remove(cakeImport);
                db.SaveChanges();
            }
            else
            {
                // do nothing
            }
        }

        public int getOrderWaitingOrder()
        {
            int id = 0;
            if (db.Orders.Count() == 0)
            {
                id = 0;
            }
            else
            {
                id = db.Orders.Where(x => x.Status == 0).FirstOrDefault().ID;
            }

            return id;
        }
        public void addToCart(Product product, int quantity)
        {
            var productInCart = db.OrderDetails.Where(x => x.ID_Cake == product.ID && x.ID_Order == null);
            if (productInCart.Count() == 0)  // the products haven't in cart yet
            {
                // generate a Detail
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.ID = db.OrderDetails.Count() > 0 ? db.OrderDetails.ToList().Last().ID + 1 : 0;
                orderDetail.ID_Cake = product.ID;
                orderDetail.ID_Order = null;
                orderDetail.No = db.OrderDetails.Where(x => x.ID_Order == null).Count() + 1;
                orderDetail.Quantity = quantity;
                orderDetail.Amount = product.Price * quantity;

                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
            }
            else
            {
                var item = db.OrderDetails.Find(productInCart.FirstOrDefault().ID);
                int newQuantity = (item.Quantity + quantity) ?? 0;
                if (newQuantity > 0)
                {
                    item.Quantity = newQuantity > product.Amount ? product.Amount : newQuantity;
                    item.Amount = item.Quantity * product.Price;
                    db.SaveChanges();
                }
                else
                {
                    db.OrderDetails.Remove(item);
                    db.SaveChanges();
                }
            }
        }
        public dynamic getListProductInCart()
        {
            var query = db.OrderDetails
                                .Join(db.Products,
                                    detail => detail.ID_Cake,
                                    product => product.ID,
                                    (detail, product) => new
                                    {
                                        ID = detail.ID,
                                        ID_Product = product.ID,
                                        ID_Type = product.IDTypeCake,
                                        No = detail.No,
                                        Quantity = detail.Quantity,
                                        Price = product.Price,
                                        Amount = detail.Amount,
                                        Name = product.Name,
                                    }
                                ).AsEnumerable();
            return query.ToList();
        }
        public int countQuantityOfProductInCart(int idProduct)
        {
            int count = 0;

            var query = db.OrderDetails.Where(x => x.ID_Cake == idProduct && x.ID_Order == null);
            if(query.Count() > 0)   // has product in cart
            {
                count = query.FirstOrDefault().Quantity ?? 0;
            }
            else
            {
                count = 0;
            }

            return count;
        }
        public string getNameCustomerIfExistByPhoneNumber(string phonenumber)
        {
            var query = db.Customers.Find(phonenumber);

            return query.Name;
        }
    }
}
