using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
                MySqlCommand cmd = connection.CreateCommand("insert into `account` Values (0, @Login, @Password, @NumberAccount, @Adm, @FirstName, @LastName, @Patronymic, @AddressStandart);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Login", account.Login));
                cmd.Parameters.Add(new MySqlParameter("Password", account.Password));
                cmd.Parameters.Add(new MySqlParameter("NumberAccount", account.NumberAccount));
                cmd.Parameters.Add(new MySqlParameter("Adm", account.Adm));
                cmd.Parameters.Add(new MySqlParameter("FirstName", "Не введено"));
                cmd.Parameters.Add(new MySqlParameter("LastName", "Не введено"));
                cmd.Parameters.Add(new MySqlParameter("Patronymic", "Не введено"));
                cmd.Parameters.Add(new MySqlParameter("AddressStandart", "Введите адрес"));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                       
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

        internal bool Update(Account edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `account` set `login`=@login, `password`=@password, `phone_number_acc`=@phone_number_acc, `adm`=@adm, `first_name_acc`=@first_name_acc, `last_name_acc`=@last_name_acc, `patronymic_acc`=@patronymic_acc, `address_standart`=@address_standart  where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("login", edit.Login));
                mc.Parameters.Add(new MySqlParameter("password", edit.Password));
                mc.Parameters.Add(new MySqlParameter("phone_number_acc", edit.NumberAccount));
                mc.Parameters.Add(new MySqlParameter("adm", edit.Adm));
                mc.Parameters.Add(new MySqlParameter("first_name_acc", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("last_name_acc", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("patronymic_acc", edit.Patronymic));
                mc.Parameters.Add(new MySqlParameter("address_standart", edit.AddressStandart));

                try
                {
                    MessageBox.Show("Данные аккаунта успешно изменены!");
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ex.Message");
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
                var command = connection.CreateCommand("select `id`, `login`, `password`, `phone_number_acc`, `adm` from `account` ");
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

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(3))
                            phone_number_acc = dr.GetString("phone_number_acc");

                        bool adm = dr.GetBoolean("adm");

                        accounts.Add(new Account
                        {
                            Id = id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm,
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

        internal List<Account> SelectAllUser()
        {
            List<Account> accounts = new List<Account>();
            if (connection == null)
                return accounts;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `login`, `password`, `phone_number_acc`, `adm`, `first_name_acc`, `last_name_acc`, `patronymic_acc`, `address_standart`  from `account` ");
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

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(3))
                            phone_number_acc = dr.GetString("phone_number_acc");

                        bool adm = dr.GetBoolean(4);

                        string first_name_acc = string.Empty;
                        if (!dr.IsDBNull(5))
                            first_name_acc = dr.GetString("first_name_acc");

                        string last_name_acc = string.Empty;
                        if (!dr.IsDBNull(6))
                            last_name_acc = dr.GetString("last_name_acc");

                        string patronymic_acc = string.Empty;
                        if (!dr.IsDBNull(7))
                            patronymic_acc = dr.GetString("patronymic_acc");

                        string address_standart = string.Empty;
                        if (!dr.IsDBNull(8))
                            address_standart = dr.GetString("address_standart");

                        



                        accounts.Add(new Account
                        {
                            Id = id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm,
                            FirstName = first_name_acc,
                            LastName = last_name_acc,
                            Patronymic = patronymic_acc,
                            AddressStandart = address_standart
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


        static AccountDB db;

        public static AccountDB GetDb()
        {
            if (db == null)
                db = new AccountDB(DbConnection.GetDbConnection());
            return db;
        }

        internal List<Account> SelectLogin(string log, string pas)
        {
            List<Account> accounts = new List<Account>();
            if (connection == null)
                return accounts;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT a.id, a.login, a.password, a.adm, a.phone_number_acc\r\nFROM account a\r\nWHERE '{log}' = a.login AND '{pas}' = a.password;");
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

                        string phone_number_acc = string.Empty;
                        if (!dr.IsDBNull(2))
                            phone_number_acc = dr.GetString("phone_number_acc");

                        bool adm = dr.GetBoolean("adm");


                        accounts.Add(new Account
                        {
                            Id = id,
                            Login = login,
                            Password = password,
                            NumberAccount = phone_number_acc,
                            Adm = adm

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
