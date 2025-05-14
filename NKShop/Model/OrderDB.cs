using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NKShop.Model
{
    internal class OrderDB
    {
        DbConnection connection;

        private OrderDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Order order)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `order` Values (0, @Quantity, @FullPrice, @Coordinates, @CourierID, @ProductID, @IsReady);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Quantity", order.Quantity));
                cmd.Parameters.Add(new MySqlParameter("FullPrice", order.FullPrice));
                cmd.Parameters.Add(new MySqlParameter("Coordinates", order.Coordinates));
                cmd.Parameters.Add(new MySqlParameter("CourierID", order.CourierID));
                cmd.Parameters.Add(new MySqlParameter("ProductID", order.ProductID));
                cmd.Parameters.Add(new MySqlParameter("IsReady", order.IsReady));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        order.Id = id;
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

        internal List<Order> SelectAll()
        {
            List<Order> orders = new List<Order>();
            if (connection == null)
                return orders;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT o.`id`, o.`quantity_prod`, o.`full_price`, o.`coordinates`, o.`courier_id`, o.`product_id`, o.`is_ready`, c.`first_name`, c.`pledge`, c.`work_start`, c.`quantity_product`, c.`last_name`, c.`patronymic`, p.`title`, p.`quantity`, p.`price`, p.`is_ready_prod` FROM `order` o JOIN `courier` c ON o.`courier_id` = c.`id` JOIN `product` p ON o.`product_id` = p.`id` WHERE o.`is_ready` = {0} ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string coordinates = string.Empty;
                        if (!dr.IsDBNull(1))
                            coordinates = dr.GetString("coordinates");

                        decimal full_price = 0;
                        if (!dr.IsDBNull(2))
                            full_price = dr.GetDecimal("full_price");

                        int quantity_product = 0;
                        if (!dr.IsDBNull(3))
                            quantity_product = dr.GetInt32("quantity_product");


                        int courier_id = dr.GetInt32(4);

                        int product_id = dr.GetInt32(5);

                        string first_name = string.Empty;
                        if (!dr.IsDBNull(6))
                            first_name = dr.GetString("first_name");

                        int pledge = 0;
                        if (!dr.IsDBNull(7))
                            pledge = dr.GetInt32("pledge");

                        DateTime work_start = new DateTime();
                        if (!dr.IsDBNull(8))
                            work_start = dr.GetDateTime("work_start");

                        int quantity_prod = 0;
                        if (!dr.IsDBNull(9))
                            quantity_prod = dr.GetInt32("quantity_prod");

                        string last_name = string.Empty;
                        if (!dr.IsDBNull(10))
                            last_name = dr.GetString("last_name");

                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(11))
                            patronymic = dr.GetString("patronymic");

                        string title = string.Empty;
                        if (!dr.IsDBNull(12))
                            title = dr.GetString("title");

                        int quantity = 0;
                        if (!dr.IsDBNull(13))
                            quantity = dr.GetInt32("quantity");

                        decimal price = 0;
                        if (!dr.IsDBNull(14))
                            price = dr.GetDecimal("price");

                        bool is_ready = dr.GetBoolean("is_ready");

                        bool is_ready_prod = dr.GetBoolean("is_ready_prod");

                        Product product = new Product
                        {
                            Id = product_id,
                            Title = title,
                            Quantity = quantity,
                            Price = price,
                            IsReadyProd = is_ready_prod,
                        };

                        Courier courier = new Courier
                        {
                            Id = courier_id,
                            Pledge = pledge,
                            QuantityProduct = quantity_product,
                            WorkStart = work_start,
                            Patronymic = patronymic,
                            LastName = last_name,
                            FirstName = first_name,
                        };

                        orders.Add(new Order
                        {
                            Id = id,
                            Coordinates = coordinates,
                            FullPrice = full_price,
                            Quantity = quantity_prod,
                            CourierID = courier_id,
                            ProductID = product_id,
                            IsReady = is_ready,
                            Courier = courier,
                            Product = product,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return orders;
        }


        internal List<Order> SelectAllHis()
        {
            List<Order> orders = new List<Order>();
            if (connection == null)
                return orders;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT o.`id`, o.`quantity_prod`, o.`full_price`, o.`coordinates`, o.`courier_id`, o.`product_id`, o.`is_ready`, c.`first_name`, c.`pledge`, c.`work_start`, c.`quantity_product`, c.`last_name`, c.`patronymic`, p.`title`, p.`quantity`, p.`price`, p.`is_ready_prod` FROM `order` o JOIN `courier` c ON o.`courier_id` = c.`id` JOIN `product` p ON o.`product_id` = p.`id` WHERE o.`is_ready` = {1}");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string coordinates = string.Empty;
                        if (!dr.IsDBNull(1))
                            coordinates = dr.GetString("coordinates");

                        decimal full_price = 0;
                        if (!dr.IsDBNull(2))
                            full_price = dr.GetDecimal("full_price");

                        int quantity_product = 0;
                        if (!dr.IsDBNull(3))
                            quantity_product = dr.GetInt32("quantity_product");


                        int courier_id = dr.GetInt32(4);

                        int product_id = dr.GetInt32(5);

                        string first_name = string.Empty;
                        if (!dr.IsDBNull(6))
                            first_name = dr.GetString("first_name");

                        int pledge = 0;
                        if (!dr.IsDBNull(7))
                            pledge = dr.GetInt32("pledge");

                        DateTime work_start = new DateTime();
                        if (!dr.IsDBNull(8))
                            work_start = dr.GetDateTime("work_start");

                        int quantity_prod = 0;
                        if (!dr.IsDBNull(9))
                            quantity_prod = dr.GetInt32("quantity_prod");

                        string last_name = string.Empty;
                        if (!dr.IsDBNull(10))
                            last_name = dr.GetString("last_name");

                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(11))
                            patronymic = dr.GetString("patronymic");

                        string title = string.Empty;
                        if (!dr.IsDBNull(12))
                            title = dr.GetString("title");

                        int quantity = 0;
                        if (!dr.IsDBNull(13))
                            quantity = dr.GetInt32("quantity");

                        decimal price = 0;
                        if (!dr.IsDBNull(14))
                            price = dr.GetDecimal("price");

                        bool is_ready = dr.GetBoolean("is_ready");

                        bool is_ready_prod = dr.GetBoolean("is_ready_prod");

                        Product product = new Product
                        {
                            Id = product_id,
                            Title = title,
                            Quantity = quantity,
                            Price = price,
                            IsReadyProd = is_ready_prod,
                        };

                        Courier courier = new Courier
                        {
                            Id = courier_id,
                            Pledge = pledge,
                            QuantityProduct = quantity_product,
                            WorkStart = work_start,
                            Patronymic = patronymic,
                            LastName = last_name,
                            FirstName = first_name,
                        };

                        orders.Add(new Order
                        {
                            Id = id,
                            Coordinates = coordinates,
                            FullPrice = full_price,
                            Quantity = quantity_prod,
                            CourierID = courier_id,
                            ProductID = product_id,
                            IsReady = is_ready,
                            Courier = courier,
                            Product = product,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return orders;
        }
        internal bool Update(Order edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `order` set `quantity_prod`=@quantity_prod, `full_price`=@full_price, `coordinates`=@coordinates, `courier_id`=@courier_id, `is_ready`=@is_ready, `product_id`=@product_id  where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("quantity_prod", edit.Quantity));
                mc.Parameters.Add(new MySqlParameter("full_price", edit.FullPrice));
                mc.Parameters.Add(new MySqlParameter("coordinates", edit.Coordinates));
                mc.Parameters.Add(new MySqlParameter("courier_id", edit.CourierID));
                mc.Parameters.Add(new MySqlParameter("is_ready", edit.IsReady));
                mc.Parameters.Add(new MySqlParameter("product_id", edit.ProductID));

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

        static OrderDB db;

        public static OrderDB GetDb()
        {
            if (db == null)
                db = new OrderDB(DbConnection.GetDbConnection());
            return db;
        }

    }
}
