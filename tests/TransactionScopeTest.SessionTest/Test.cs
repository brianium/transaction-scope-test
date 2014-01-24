using System;
using System.Linq;
using System.Transactions;
using NHibernate.Linq;
using NUnit.Framework;
using TransactionScopeTest.Core.Domain.Model;

namespace TransactionScopeTest.SessionTest
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void CreateProduct_should_create_single_product_using_TransactionScope()
        {
            var product = new Product()
            {
                Name = "New Product 1",
                Price = 10.50m
            };
            SaveProductInTransactionScope(product);
            //saving should have set the id on the product
            Assert.NotNull(product.Id);
        }

        [Test]
        public void CreateOrder_should_create_single_order_using_TransactionScope()
        {
            var order = new Order()
            {
                ProductId = 1,
                Quantity = 1
            };
            SaveOrderInTransactionScope(order);
            //saving should have set the id on the order
            Assert.NotNull(order.Id);
        }

        [Test]
        public void CreateOrder_before_failing_product_should_roll_back_new_order_in_TransactionScope()
        {
            var product1 = new Product()
            {
                Name = "New Product 2",
                Price = 9.25m
            };
            SaveProductInTransactionScope(product1);

            //violating a unique constraint should create an error worthy of rollback
            var product2 = new Product()
            {
                Name = "New Product 2",
                Price = 11.13m
            };
            var order = new Order()
            {
                ProductId = product1.Id + 1,
                Quantity = 99
            };

            try
            {
                using (var scope = new TransactionScope())
                {

                    using (var prodSession = SessionFactories.ProductConnectionFactory.OpenSession())
                    using (var prodTrans = prodSession.BeginTransaction())
                    using (var orderSession = SessionFactories.OrderConnectionFactory.OpenSession())
                    using (var orderTrans = orderSession.BeginTransaction())
                    {
                        //save and commit the order first
                        orderSession.Save(order);
                        orderTrans.Commit();

                        //try to save the product violating a unique key constraint resulting in an NHibernate.Exceptions.GenericADOException
                        prodSession.Save(product2);
                        prodTrans.Commit();
                    }
                    scope.Complete();
                }
            }
            catch (Exception e) { }

            using (var session = SessionFactories.OrderConnectionFactory.OpenSession())
            {
                var saved = session.Query<Order>().FirstOrDefault(o => o.Quantity == 99);
                Assert.Null(saved);
            }
        }

        [Test]
        public void CreateOrder_before_failing_product_should_not_roll_back_new_order_when_not_in_TransactionScope()
        {
            var product1 = new Product()
            {
                Name = "New Product 3",
                Price = 9.25m
            };
            SaveProductInTransactionScope(product1);

            //violating a unique constraint should create an error
            var product2 = new Product()
            {
                Name = "New Product 3",
                Price = 11.13m
            };
            var order = new Order()
            {
                ProductId = product1.Id + 1,
                Quantity = 98
            };

            try
            {
                using (var prodSession = SessionFactories.ProductConnectionFactory.OpenSession())
                using (var prodTrans = prodSession.BeginTransaction())
                using (var orderSession = SessionFactories.OrderConnectionFactory.OpenSession())
                using (var orderTrans = orderSession.BeginTransaction())
                {
                    //save and commit the order first
                    orderSession.Save(order);
                    orderTrans.Commit();

                    //try to save the product violating a unique key constraint resulting in an NHibernate.Exceptions.GenericADOException
                    prodSession.Save(product2);
                    prodTrans.Commit();
                }
            }
            catch (Exception e) { }

            //When not in TransactionScope the order is saved anyways
            using (var session = SessionFactories.OrderConnectionFactory.OpenSession())
            {
                var saved = session.Query<Order>().FirstOrDefault(o => o.Quantity == 98);
                Assert.NotNull(saved);
            }
        }

        private static void SaveProductInTransactionScope(Product product)
        {
            //save a product using transaction scope
            using (var scope = new TransactionScope())
            {
                using (var session = SessionFactories.ProductConnectionFactory.OpenSession())
                using (var trans = session.BeginTransaction())
                {
                    session.Save(product);
                    trans.Commit();
                }
                scope.Complete();
            }
        }

        private static void SaveOrderInTransactionScope(Order order)
        {
            //save an order using transaction scope
            using (var scope = new TransactionScope())
            {
                using (var session = SessionFactories.OrderConnectionFactory.OpenSession())
                using (var trans = session.BeginTransaction())
                {
                    session.Save(order);
                    trans.Commit();
                }
                scope.Complete();
            }
        }
    }
}
