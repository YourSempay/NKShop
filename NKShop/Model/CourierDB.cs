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
                MySqlCommand cmd = connection.CreateCommand("insert into `courier` Values (0, @FIO, @Pledge, @WorkStart, @QuantityProduct);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("FIO", courier.FIO));
                cmd.Parameters.Add(new MySqlParameter("Pledge", courier.Pledge));
                cmd.Parameters.Add(new MySqlParameter("WorkStart", courier.WorkStart));
                cmd.Parameters.Add(new MySqlParameter("QuantityProduct", courier.QuantityProduct));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
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

        internal List<Courier> SelectAll()
        {
            List<Courier> couriers = new List<Courier>();
            if (connection == null)
                return couriers;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `fio`, `pledge`, `work_start`, `quantity_product` from `courier` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string fio = string.Empty;
                        if (!dr.IsDBNull(1))
                            fio = dr.GetString("fio");

                        int pledge = 0;
                        if (!dr.IsDBNull(2))
                            pledge = dr.GetInt32("pledge");

                        int quantity_product = 0;
                        if (!dr.IsDBNull(3))
                            quantity_product = dr.GetInt32("quantity_product");

                        DateTime work_start = new DateTime();
                        if (!dr.IsDBNull(4))
                            work_start = dr.GetDateTime("work_start");

                        couriers.Add(new Courier
                        {
                            Id = id,
                            FIO = fio,
                            Pledge = pledge,
                            QuantityProduct = quantity_product,
                            WorkStart = work_start
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


        /*

        internal bool Update(Author edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `author` set `first_name`=@first_name, `patronymic`=@patronymic, `last_name`=@last_name, `birthday`=@birthday where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("first_name", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("patronymic", edit.Patronymic));
                mc.Parameters.Add(new MySqlParameter("last_name", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("birthday", edit.Birthday));

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


        internal bool Remove(Author remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `author` where `id` = {remove.Id}");
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

        static AuthorDB db;
        public static AuthorDB GetDb()
        {
            if (db == null)
                db = new AuthorDB(DbConnection.GetDbConnection());
            return db;
        }

        */

    }
}
