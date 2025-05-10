using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NKShop.Model
{
    internal class ProductDB
    {
        DbConnection connection;

        private ProductDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Product product)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `product` Values (0, @Quantity, @Title, @Price, @IsReady);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Quantity", product.Quantity));
                cmd.Parameters.Add(new MySqlParameter("Title", product.Title));
                cmd.Parameters.Add(new MySqlParameter("Price", product.Price));
                cmd.Parameters.Add(new MySqlParameter("IsReady", product.IsReadyProd));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        product.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Product> SelectAll()
        {
            List<Product> products = new List<Product>();
            if (connection == null)
                return products;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"select `id`, `quantity`, `title`, `price`, `is_ready`  from `product`  WHERE `id` > {1}");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("title");

                        decimal price = 0;
                        if (!dr.IsDBNull(2))
                            price = dr.GetDecimal("price");

                        int quantity = 0;
                        if (!dr.IsDBNull(3))
                            quantity = dr.GetInt32("quantity");

                        bool is_ready = dr.GetBoolean("is_ready");

                        products.Add(new Product
                        {
                            Id = id,
                            Title = title,
                            Price = price,
                            Quantity = quantity,
                            IsReadyProd = is_ready,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return products;
        }

        internal bool Update(Product edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `product` set `title`=@title, `quantity`=@quantity, `price`=@price, `is_ready`=@is_ready where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("quantity", edit.Quantity));
                mc.Parameters.Add(new MySqlParameter("price", edit.Price));
                mc.Parameters.Add(new MySqlParameter("is_ready", edit.IsReadyProd));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static ProductDB db;
        public static ProductDB GetDb()
        {
            if (db == null)
                db = new ProductDB(DbConnection.GetDbConnection());
            return db;
        }



    }
}
