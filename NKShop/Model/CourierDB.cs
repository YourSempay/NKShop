using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NKShop.Model
{
    internal class CourierDB
    {
        DbConnection connection;

        private CourierDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Courier courier)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `courier` Values (0, @FirstName, @LastName, @Patronymic, @Pledge, @WorkStart, @QuantityProduct);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("FirstName", courier.FirstName));
                cmd.Parameters.Add(new MySqlParameter("LastName", courier.LastName));
                cmd.Parameters.Add(new MySqlParameter("Patronymic", courier.Patronymic));
                cmd.Parameters.Add(new MySqlParameter("Pledge", courier.Pledge));
                cmd.Parameters.Add(new MySqlParameter("WorkStart", courier.WorkStart));
                cmd.Parameters.Add(new MySqlParameter("QuantityProduct", courier.QuantityProduct));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show("Курьер успешно добавлен!");
                        courier.Id = id;
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

        internal bool Remove(Courier remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `courier` where `id` = {remove.Id}");
                try
                {
                    MessageBox.Show("Курьер успешно удалён!");
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

        internal List<Courier> SelectAll()
        {
            List<Courier> couriers = new List<Courier>();
            if (connection == null)
                return couriers;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"select `id`, `first_name`, `pledge`, `work_start`, `quantity_product`, `last_name`, `patronymic` from `courier`");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string first_name = string.Empty;
                        if (!dr.IsDBNull(1))
                            first_name = dr.GetString("first_name");

                        int pledge = 0;
                        if (!dr.IsDBNull(2))
                            pledge = dr.GetInt32("pledge");

                        int quantity_product = 0;
                        if (!dr.IsDBNull(3))
                            quantity_product = dr.GetInt32("quantity_product");

                        DateTime work_start = new DateTime();
                        if (!dr.IsDBNull(4))
                            work_start = dr.GetDateTime("work_start");

                        string last_name = string.Empty;
                        if (!dr.IsDBNull(5))
                            last_name = dr.GetString("last_name");

                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(6))
                            patronymic = dr.GetString("patronymic");

                        couriers.Add(new Courier
                        {
                            Id = id,
                            Pledge = pledge,
                            QuantityProduct = quantity_product,
                            WorkStart = work_start,
                            LastName = last_name,
                            Patronymic = patronymic,
                            FirstName = first_name,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return couriers;
        }

        internal List<Courier> SelectAllProtect()
        {
            List<Courier> couriers = new List<Courier>();
            if (connection == null)
                return couriers;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT c.`id`, c.`first_name`, c.`pledge`, c.`work_start`, c.`quantity_product`, c.`last_name`, c.`patronymic`, COALESCE(MAX(CASE WHEN o.`is_ready` = false THEN true WHEN o.`is_ready` = true THEN false ELSE NULL END), false) AS `is_ready` FROM `courier` c LEFT JOIN `order` o ON o.`courier_id` = c.`id` WHERE c.`id` > {1} GROUP BY c.`id`;");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string first_name = string.Empty;
                        if (!dr.IsDBNull(1))
                            first_name = dr.GetString("first_name");

                        int pledge = 0;
                        if (!dr.IsDBNull(2))
                            pledge = dr.GetInt32("pledge");

                        int quantity_product = 0;
                        if (!dr.IsDBNull(3))
                            quantity_product = dr.GetInt32("quantity_product");

                        DateTime work_start = new DateTime();
                        if (!dr.IsDBNull(4))
                            work_start = dr.GetDateTime("work_start");

                        string last_name = string.Empty;
                        if (!dr.IsDBNull(5))
                            last_name = dr.GetString("last_name");

                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(6))
                            patronymic = dr.GetString("patronymic");

                        bool is_ready = dr.GetBoolean("is_ready");

                        couriers.Add(new Courier
                        {
                            Id = id,
                            Pledge = pledge,
                            QuantityProduct = quantity_product,
                            WorkStart = work_start,
                            LastName = last_name,
                            Patronymic = patronymic,
                            FirstName = first_name,
                            CourierIsFree = is_ready,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return couriers;
        }

        internal bool Update(Courier edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `courier` set `pledge`=@pledge, `work_start`=@work_start, `quantity_product`=@quantity_product, `first_name`=@first_name, `last_name`=@last_name, `patronymic`=@patronymic  where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("quantity_product", edit.QuantityProduct));
                mc.Parameters.Add(new MySqlParameter("pledge", edit.Pledge));
                mc.Parameters.Add(new MySqlParameter("work_start", edit.WorkStart));
                mc.Parameters.Add(new MySqlParameter("first_name", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("last_name", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("patronymic", edit.Patronymic));

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

        static CourierDB db;

        public static CourierDB GetDb()
        {
            if (db == null)
                db = new CourierDB(DbConnection.GetDbConnection());
            return db;
        }
    }


}
