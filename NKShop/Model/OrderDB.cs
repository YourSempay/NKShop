using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
                MySqlCommand cmd = connection.CreateCommand("insert into `order` Values (0, @Quantity, @FullPrice, @Coordinates);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Quantity", order.Quantity));
                cmd.Parameters.Add(new MySqlParameter("FullPrice", order.FullPrice));
                cmd.Parameters.Add(new MySqlParameter("Coordinates", order.Coordinates));
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
                var command = connection.CreateCommand("select `id`, `quantity`, `full_price`, `coordinates` from `order` ");
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

                        int quantity = 0;
                        if (!dr.IsDBNull(3))
                            quantity = dr.GetInt32("quantity");

                        orders.Add(new Order
                        {
                            Id = id,
                            Coordinates = coordinates,
                            FullPrice = full_price,
                            Quantity = quantity,
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


    }
}
