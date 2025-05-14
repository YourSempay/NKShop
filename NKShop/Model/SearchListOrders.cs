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
   public class SearchListOrders
    {

        private readonly DbConnection myConnection;
        public List<Order> SearchOrder(string search)
        {
            List<Order> result = new();
            List<Courier> couriers = new();
            string query = $"SELECT o.id, `quantity_prod`, `full_price`, `coordinates`, `courier_id`, `product_id`, `is_ready`, c.`first_name`, c.`pledge`, c.`work_start`, c.`quantity_product`, c.`last_name`, c.`patronymic`, p.`title`, p.`quantity`, p.`price`, p.`is_ready_prod` FROM `order` AS o JOIN `courier` c ON o.`courier_id` = c.`id` JOIN `product` p ON o.`product_id` = p.`id` WHERE p.`title` AND o.`is_ready` = {1} LIKE CONCAT('%', @search, '%') OR c.`last_name` LIKE CONCAT('%', @search, '%')";
            if (myConnection.OpenConnection())
            {
                using (var mc = myConnection.CreateCommand(query))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var order = new Order
                            {
                                Id = dr.GetInt32("id"),
                                Quantity = dr.GetInt32("quantity_prod"),
                                FullPrice = dr.GetDecimal("full_price"),
                                Coordinates = dr.GetString("coordinates"),
                                CourierID = dr.GetInt32("courier_id"),
                                ProductID = dr.GetInt32("product_id"),
                                IsReady = dr.GetBoolean("is_ready")
                            };

                            // Поиск объекта-курьера в коллекции couriers по ID
                            var courier = couriers.FirstOrDefault(c => c.Id == order.CourierID);
                            if (courier == null)
                            {
                                // Создание курьера, если его не было в коллекции
                                courier = new Courier
                                {
                                    Id = order.CourierID,
                                    FirstName = dr.GetString("first_name"),
                                    LastName = dr.GetString("last_name"),
                                    Patronymic = dr.GetString("patronymic")
                                };
                                // Добавление курьера в коллекцию
                                couriers.Add(courier);
                            }
                            // Указание заказу курьера
                            order.Courier = courier;

                            // Добавление заказа в результат запроса
                            result.Add(order);
                        }
                    }
                }
                myConnection.CloseConnection();
            }
            return result;
        }
        static SearchListOrders db;
        private SearchListOrders(DbConnection myConnection)
        {
            this.myConnection = myConnection;
        }
        public static SearchListOrders GetDB()
        {
            if (db == null)
                db = new SearchListOrders(DbConnection.GetDbConnection());
            return db;
        }

    }
}
