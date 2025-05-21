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
                MySqlCommand cmd = connection.CreateCommand("insert into `order` Values (0, @Quantity, @FullPrice, @Coordinates, @CourierID, @IsReady, @ProductID, @AccountID);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Quantity", order.Quantity));
                cmd.Parameters.Add(new MySqlParameter("FullPrice", order.FullPrice));
                cmd.Parameters.Add(new MySqlParameter("Coordinates", order.Coordinates));
                cmd.Parameters.Add(new MySqlParameter("CourierID", order.CourierID));
                cmd.Parameters.Add(new MySqlParameter("IsReady", order.IsReady));
                cmd.Parameters.Add(new MySqlParameter("ProductID", order.ProductID));
                cmd.Parameters.Add(new MySqlParameter("AccountID", order.AccountID));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show("Заказ успешно создан!");
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
                var command = connection.CreateCommand($"SELECT \r\n    o.`id`, \r\n    o.`quantity_prod`, \r\n    o.`full_price`, \r\n    o.`coordinates`, \r\n    o.`courier_id`, \r\n    o.`product_id`, \r\n    o.`is_ready`, \r\n    c.`first_name`, \r\n    c.`phone_number`, \r\n    c.`work_start`, \r\n    c.`quantity_product`, \r\n    c.`last_name`, \r\n    c.`patronymic`, \r\n    p.`title`, \r\n    p.`quantity`, \r\n    p.`price`, \r\n    p.`is_ready_prod`,\r\n    a.`id` AS account_id,\r\n    a.`login`,\r\n    a.`password`,\r\n    a.`phone_number_acc`,\r\n    a.`adm`,\r\n    a.`first_name_acc`,\r\n    a.`last_name_acc`,\r\n    a.`patronymic_acc`,\r\n    a.`address_standart`\r\nFROM `order` o\r\nJOIN `courier` c ON o.`courier_id` = c.`id`\r\nJOIN `product` p ON o.`product_id` = p.`id`\r\nJOIN `account` a ON o.`account_id` = a.`id`\r\nWHERE o.`is_ready` = {0}");
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

                        string pledge = string.Empty;
                        if (!dr.IsDBNull(7))
                            pledge = dr.GetString("phone_number");

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

                        int account_id = dr.GetInt32(15);

                        string login = string.Empty;
                        if (!dr.IsDBNull(16))
                            login = dr.GetString("login");

                        string password = string.Empty;
                        if (!dr.IsDBNull(17))
                            password = dr.GetString("password");

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(18))
                            phone_number_acc = dr.GetString("phone_number_acc");

                        string first_name_acc = string.Empty;
                        if (!dr.IsDBNull(19))
                            first_name_acc = dr.GetString("first_name_acc");

                        string last_name_acc = string.Empty;
                        if (!dr.IsDBNull(20))
                            last_name_acc = dr.GetString("last_name_acc");

                        string patronymic_acc = string.Empty;
                        if (!dr.IsDBNull(21))
                            patronymic_acc = dr.GetString("patronymic_acc");

                        string address_standart = string.Empty;
                        if (!dr.IsDBNull(22))
                            address_standart = dr.GetString("address_standart");

                        bool adm = dr.GetBoolean("adm");

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

                        Account account = new Account
                        {
                            Id = account_id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm,
                            AddressStandart = address_standart,
                            FirstName = first_name_acc,
                            LastName = last_name_acc,
                            Patronymic = patronymic_acc,
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
                            AccountID = account_id,
                            Account = account,
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
                var command = connection.CreateCommand($"SELECT\r\n  o.`id`,\r\n  o.`quantity_prod`,\r\n  o.`full_price`,\r\n  o.`coordinates`,\r\n  o.`is_ready`, -- статус заказа\r\n  COALESCE(c1.`id`, 1) AS `courier_id`,\r\n  COALESCE(c1.`first_name`, c2.`first_name`) AS `first_name`,\r\n  COALESCE(c1.`phone_number`, c2.`phone_number`) AS `phone_number`,\r\n  COALESCE(c1.`work_start`, c2.`work_start`) AS `work_start`,\r\n  COALESCE(c1.`quantity_product`, c2.`quantity_product`) AS `quantity_product`,\r\n  COALESCE(c1.`last_name`, c2.`last_name`) AS `last_name`,\r\n  COALESCE(c1.`patronymic`, c2.`patronymic`) AS `patronymic`,\r\n  COALESCE(p.`id`, 1) AS `product_id`, -- если нет товара, используем 1\r\n  COALESCE(p.`title`, 'Не назначен') AS `title`, -- если название отсутствует\r\n  COALESCE(p.`quantity`, 0) AS `quantity`, -- если quantity NULL\r\n  COALESCE(p.`price`, 1) AS `price`, -- если цена NULL\r\n  COALESCE(p.`is_ready_prod`, 0) AS `is_ready_prod`, -- если статус NULL\r\n  a.`id` AS `account_id`,\r\n  a.`login`,\r\n  a.`password`,\r\n  a.`phone_number_acc`,\r\n  a.`adm`,\r\n  a.`first_name_acc`,\r\n  a.`last_name_acc`,\r\n  a.`patronymic_acc`,\r\n  a.`address_standart`\r\nFROM\r\n  `order` o\r\nLEFT JOIN `courier` c1 ON o.`courier_id` = c1.`id`\r\nLEFT JOIN `courier` c2 ON c2.`id` = 1\r\nLEFT JOIN `product` p ON o.`product_id` = p.`id`\r\nLEFT JOIN `account` a ON o.`account_id` = a.`id`\r\nWHERE\r\n  o.`is_ready` = 1");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(dr.GetOrdinal("id"));

                        string coordinates = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("coordinates")))
                            coordinates = dr["coordinates"].ToString();

                        decimal full_price = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("full_price")))
                            full_price = Convert.ToDecimal(dr["full_price"]);

                        int quantity_product = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("quantity_product")))
                            quantity_product = dr.GetInt32(dr.GetOrdinal("quantity_product"));

                        int courier_id = dr.GetInt32(dr.GetOrdinal("courier_id"));

                        int product_id = dr.GetInt32(dr.GetOrdinal("product_id"));

                        string first_name = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("first_name")))
                            first_name = dr["first_name"].ToString();

                        string pledge = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("phone_number")))
                            pledge = dr["phone_number"].ToString();

                        DateTime work_start = DateTime.MinValue;
                        if (!dr.IsDBNull(dr.GetOrdinal("work_start")))
                            work_start = dr.GetDateTime(dr.GetOrdinal("work_start"));

                        int quantity_prod = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("quantity_prod")))
                            quantity_prod = dr.GetInt32(dr.GetOrdinal("quantity_prod"));

                        string last_name = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("last_name")))
                            last_name = dr["last_name"].ToString();

                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("patronymic")))
                            patronymic = dr["patronymic"].ToString();

                        string title = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("title")))
                            title = dr["title"].ToString();

                        int quantity = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("quantity")))
                            quantity = dr.GetInt32(dr.GetOrdinal("quantity"));

                        decimal price = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("price")))
                            price = Convert.ToDecimal(dr["price"]);

                        int account_id = dr.GetInt32(dr.GetOrdinal("account_id"));

                        string login = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("login")))
                            login = dr["login"].ToString();

                        string password = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("password")))
                            password = dr["password"].ToString();

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("phone_number_acc")))
                            phone_number_acc = dr["phone_number_acc"].ToString();

                        string first_name_acc = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("first_name_acc")))
                            first_name_acc = dr["first_name_acc"].ToString();

                        string last_name_acc = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("last_name_acc")))
                            last_name_acc = dr["last_name_acc"].ToString();

                        string patronymic_acc = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("patronymic_acc")))
                            patronymic_acc = dr["patronymic_acc"].ToString();

                        string address_standart = string.Empty;
                        if (!dr.IsDBNull(dr.GetOrdinal("address_standart")))
                            address_standart = dr["address_standart"].ToString();

                        bool adm = dr.GetBoolean(dr.GetOrdinal("adm"));

                        bool is_ready = dr.GetBoolean(dr.GetOrdinal("is_ready"));

                        bool is_ready_prod = dr.GetBoolean(dr.GetOrdinal("is_ready_prod"));

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

                        Account account = new Account
                        {
                            Id = account_id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm,
                            FirstName = first_name_acc,
                            LastName = last_name_acc,
                            Patronymic = patronymic_acc,
                            AddressStandart = address_standart
                        };

                        orders.Add(new Order
                        {
                            Id = id,
                            Coordinates = coordinates,
                            FullPrice = full_price,
                            Quantity = quantity,
                            CourierID = courier_id,
                            ProductID = product_id,
                            IsReady = is_ready,
                            Courier = courier,
                            Product = product,
                            AccountID = account_id,
                            Account = account
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
                var mc = connection.CreateCommand($"update `order` set `quantity_prod`=@quantity_prod, `full_price`=@full_price, `coordinates`=@coordinates, `courier_id`=@courier_id, `is_ready`=@is_ready, `product_id`=@product_id, `account_id`=@account_id  where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("quantity_prod", edit.Quantity));
                mc.Parameters.Add(new MySqlParameter("full_price", edit.FullPrice));
                mc.Parameters.Add(new MySqlParameter("coordinates", edit.Coordinates));
                mc.Parameters.Add(new MySqlParameter("courier_id", edit.CourierID));
                mc.Parameters.Add(new MySqlParameter("is_ready", edit.IsReady));
                mc.Parameters.Add(new MySqlParameter("product_id", edit.ProductID));
                mc.Parameters.Add(new MySqlParameter("account_id", edit.AccountID));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                    MessageBox.Show("Данные обновлены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal List<Order> SelectAllUserNotReady(int idi)
        {
            List<Order> orders = new List<Order>();
            if (connection == null)
                return orders;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT \r\n    o.`id`, \r\n    o.`quantity_prod`, \r\n    o.`full_price`, \r\n    o.`coordinates`, \r\n    o.`courier_id`, \r\n    o.`product_id`, \r\n    o.`is_ready`, \r\n    c.`first_name`, \r\n    c.`phone_number`, \r\n    c.`work_start`, \r\n    c.`quantity_product`, \r\n    c.`last_name`, \r\n    c.`patronymic`, \r\n    p.`title`, \r\n    p.`quantity`, \r\n    p.`price`, \r\n    p.`is_ready_prod`,\r\n    a.`id` AS account_id,\r\n    a.`login`,\r\n    a.`password`,\r\n    a.`phone_number_acc`,\r\n    a.`adm`,\r\n    a.`first_name_acc`,\r\n    a.`last_name_acc`,\r\n    a.`patronymic_acc`,\r\n    a.`address_standart`\r\nFROM `order` o\r\nJOIN `courier` c ON o.`courier_id` = c.`id`\r\nJOIN `product` p ON o.`product_id` = p.`id`\r\nJOIN `account` a ON o.`account_id` = a.`id`\r\nWHERE o.`is_ready` = {0}\r\nAND a.`id` = {idi}");
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

                        string pledge = string.Empty;
                        if (!dr.IsDBNull(7))
                            pledge = dr.GetString("phone_number");

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

                        int account_id = dr.GetInt32(15);

                        string login = string.Empty;
                        if (!dr.IsDBNull(16))
                            login = dr.GetString("login");

                        string password = string.Empty;
                        if (!dr.IsDBNull(17))
                            password = dr.GetString("password");

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(18))
                            phone_number_acc = dr.GetString("phone_number_acc");

                        string first_name_acc = string.Empty;
                        if (!dr.IsDBNull(19))
                            first_name_acc = dr.GetString("first_name_acc");

                        string last_name_acc = string.Empty;
                        if (!dr.IsDBNull(20))
                            last_name_acc = dr.GetString("last_name_acc");

                        string patronymic_acc = string.Empty;
                        if (!dr.IsDBNull(21))
                            patronymic_acc = dr.GetString("patronymic_acc");

                        string address_standart = string.Empty;
                        if (!dr.IsDBNull(22))
                            address_standart = dr.GetString("address_standart");

                        bool adm = dr.GetBoolean("adm");

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

                        Account account = new Account
                        {
                            Id = account_id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm,
                            AddressStandart = address_standart,
                            FirstName = first_name_acc,
                            LastName = last_name_acc,
                            Patronymic = patronymic_acc,
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
                            AccountID = account_id,
                            Account = account,
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

        internal List<Order> SelectAllUserReady(int idi)
        {
            List<Order> orders = new List<Order>();
            if (connection == null)
                return orders;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT \r\n    o.`id`, \r\n    o.`quantity_prod`, \r\n    o.`full_price`, \r\n    o.`coordinates`, \r\n    o.`courier_id`, \r\n    o.`product_id`, \r\n    o.`is_ready`, \r\n    c.`first_name`, \r\n    c.`phone_number`, \r\n    c.`work_start`, \r\n    c.`quantity_product`, \r\n    c.`last_name`, \r\n    c.`patronymic`, \r\n    p.`title`, \r\n    p.`quantity`, \r\n    p.`price`, \r\n    p.`is_ready_prod`,\r\n    a.`id` AS account_id,\r\n    a.`login`,\r\n    a.`password`,\r\n    a.`phone_number_acc`,\r\n    a.`adm`,\r\n    a.`first_name_acc`,\r\n    a.`last_name_acc`,\r\n    a.`patronymic_acc`,\r\n    a.`address_standart`\r\nFROM `order` o\r\nJOIN `courier` c ON o.`courier_id` = c.`id`\r\nJOIN `product` p ON o.`product_id` = p.`id`\r\nJOIN `account` a ON o.`account_id` = a.`id`\r\nWHERE o.`is_ready` = {1}\r\nAND a.`id` = {idi}");
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

                        string pledge = string.Empty;
                        if (!dr.IsDBNull(7))
                            pledge = dr.GetString("phone_number");

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

                        int account_id = dr.GetInt32(15);

                        string login = string.Empty;
                        if (!dr.IsDBNull(16))
                            login = dr.GetString("login");

                        string password = string.Empty;
                        if (!dr.IsDBNull(17))
                            password = dr.GetString("password");

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(18))
                            phone_number_acc = dr.GetString("phone_number_acc");

                        string first_name_acc = string.Empty;
                        if (!dr.IsDBNull(19))
                            first_name_acc = dr.GetString("first_name_acc");

                        string last_name_acc = string.Empty;
                        if (!dr.IsDBNull(20))
                            last_name_acc = dr.GetString("last_name_acc");

                        string patronymic_acc = string.Empty;
                        if (!dr.IsDBNull(21))
                            patronymic_acc = dr.GetString("patronymic_acc");

                        string address_standart = string.Empty;
                        if (!dr.IsDBNull(22))
                            address_standart = dr.GetString("address_standart");

                        bool adm = dr.GetBoolean("adm");

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

                        Account account = new Account
                        {
                            Id = account_id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm,
                            AddressStandart = address_standart,
                            FirstName = first_name_acc,
                            LastName = last_name_acc,
                            Patronymic = patronymic_acc,
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
                            AccountID = account_id,
                            Account = account,
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


        internal bool Delete(Order order)
        {
            bool result = false;
            if (connection == null) return result;
            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"DELETE FROM `order` WHERE `id` = {order.Id}");
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    result = rows > 0;
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
