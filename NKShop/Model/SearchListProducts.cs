using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using NKShop.View;

namespace NKShop.Model
{
    public class SearchListProducts
    {
        private readonly DbConnection myConnection;
        public List<Product> SearchOrder(string search)
        {
            List<Product> result = new();
            string query = $"SELECT p.id, p.title, p.quantity, p.price, p.is_ready_prod FROM `product` p WHERE p.title LIKE CONCAT('%', @search, '%') AND p.id <> 1;";
            if (myConnection.OpenConnection())
            {
                using (var mc = myConnection.CreateCommand(query))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {


                            Product product;

                                product = new Product
                                {
                                    Id = dr.GetInt32("id"),
                                    Title = dr.GetString("title"),
                                    Quantity = dr.GetInt32("quantity"),
                                    Price = dr.GetDecimal("price"),
                                    IsReadyProd = dr.GetBoolean("is_ready_prod"),
                                };

                            result.Add(product);
                        }
                    }
                }
                myConnection.CloseConnection();
            }
            return result;
        }
        static SearchListProducts db;
        private SearchListProducts(DbConnection myConnection)
        {
            this.myConnection = myConnection;
        }
        public static SearchListProducts GetDB()
        {
            if (db == null)
                db = new SearchListProducts(DbConnection.GetDbConnection());
            return db;
        }
    }
}
