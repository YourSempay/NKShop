using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NKShop.Model
{
    internal class AccountDB
    {
        DbConnection connection;

        private AccountDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Account account)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `account` Values (0, @Login, @Password);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Login", account.Login));
                cmd.Parameters.Add(new MySqlParameter("Password", account.Password));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        account.Id = id;
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

        internal List<Account> SelectAll()
        {
            List<Account> accounts = new List<Account>();
            if (connection == null)
                return accounts;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `login`, `password` from `account` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string login = string.Empty;
                        if (!dr.IsDBNull(1))
                            login = dr.GetString("login");

                        string password = string.Empty;
                        if (!dr.IsDBNull(2))
                            password = dr.GetString("password");


                        accounts.Add(new Account
                        {
                            Id = id,
                            Login = login,
                            Password = password,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return accounts;
        }


    }
}
