using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace NKShop.Model
{
    internal class StatisticDB
    {
        DbConnection connection;

        private StatisticDB(DbConnection db)
        {
            this.connection = db;
        }

        internal List<Statistic> SelectStatisticAll()
        {
            List<Statistic> statistics = new List<Statistic>();
            if (connection == null)
                return statistics;

            if (connection.OpenConnection())
            {
                // SELECT ROW_NUMBER() OVER (ORDER BY p.`id`) AS Id, p.`id` AS ProductId, p.`title` AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0) AS quantityorders, COALESCE(SUM(o.`quantity_prod`), 0) AS quantitysells, COALESCE(o.`product_id`, 0) AS product_id, COALESCE(SUM(o.`full_price`), 0) AS allprice FROM `product` p LEFT JOIN `order` o ON o.`product_id` = p.`id` WHERE p.`id` <> 1 GROUP BY p.`id`, p.`title`, o.`product_id`;
                var command = connection.CreateCommand($"SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY p.`id`) AS Id, p.`id` AS ProductId, p.`title` AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0) AS quantityorders, COALESCE(SUM(o.`quantity_prod`), 0) AS quantitysells, 0 AS product_id, COALESCE(SUM(o.`full_price`), 0) AS allprice FROM `product` p LEFT JOIN `order` o ON o.`product_id` = p.`id` WHERE p.`id` <> 1 GROUP BY p.`id`, p.`title` UNION ALL SELECT 1000 AS Id, 1 AS ProductId, 'Итого' AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0), COALESCE(SUM(o.`quantity_prod`), 0), 1 AS product_id, COALESCE(SUM(o.`full_price`), 0) FROM `order` o WHERE o.`product_id` > 1) AS combined;");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string Title = string.Empty;
                        if (!dr.IsDBNull(1))
                            Title = dr.GetString("Title");

                        int product_id = dr.GetInt32("product_id");

                        int quantityorders = 0;
                        if (!dr.IsDBNull(2))
                            quantityorders = dr.GetInt32("quantityorders");

                        decimal quantitysells = 0;
                        if (!dr.IsDBNull(3))
                            quantitysells = dr.GetDecimal("quantitysells");

                        decimal allprice = 0;
                        if (!dr.IsDBNull(3))
                            allprice = dr.GetDecimal("allprice");

                        statistics.Add(new Statistic
                        {
                            Id = id,
                            Title = Title,
                            ProductId = product_id,
                            QuantitySells = quantitysells,
                            QuantityOrders = quantityorders,
                            AllPrice = allprice,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return statistics;
        }

        internal List<Statistic> SelectStatisticReady()
        {
            List<Statistic> statistics = new List<Statistic>();
            if (connection == null)
                return statistics;

            if (connection.OpenConnection())
            {
               // var command = connection.CreateCommand($"SELECT ROW_NUMBER() OVER (ORDER BY p.`id`) AS Id, p.`id` AS ProductId, p.`title` AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0) AS quantityorders, COALESCE(SUM(o.`quantity_prod`), 0) AS quantitysells, COALESCE(o.`product_id`, 0) AS product_id, COALESCE(SUM(o.`full_price`), 0) AS allprice FROM `product` p LEFT JOIN `order` o ON o.`product_id` = p.`id` AND o.`is_ready` = 1 WHERE p.`id` <> 1 GROUP BY p.`id`, p.`title`, o.`product_id`;");

                var command = connection.CreateCommand($"SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY p.`id`) AS Id, p.`id` AS ProductId, p.`title` AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0) AS quantityorders, COALESCE(SUM(o.`quantity_prod`), 0) AS quantitysells, 0 AS product_id, COALESCE(SUM(o.`full_price`), 0) AS allprice FROM `product` p LEFT JOIN `order` o ON o.`product_id` = p.`id` WHERE p.`id` <> 1 AND o.is_ready = true GROUP BY p.`id`, p.`title` UNION ALL SELECT 1000 AS Id, 1 AS ProductId, 'Итого' AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0), COALESCE(SUM(o.`quantity_prod`), 0), 1 AS product_id, COALESCE(SUM(o.`full_price`), 0) FROM `order` o WHERE o.`product_id` > 1 AND o.is_ready = true) AS combined;");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string Title = string.Empty;
                        if (!dr.IsDBNull(1))
                            Title = dr.GetString("Title");

                        int product_id = dr.GetInt32("product_id");

                        int quantityorders = 0;
                        if (!dr.IsDBNull(2))
                            quantityorders = dr.GetInt32("quantityorders");

                        decimal quantitysells = 0;
                        if (!dr.IsDBNull(3))
                            quantitysells = dr.GetDecimal("quantitysells");

                        decimal allprice = 0;
                        if (!dr.IsDBNull(3))
                            allprice = dr.GetDecimal("allprice");

                        statistics.Add(new Statistic
                        {
                            Id = id,
                            Title = Title,
                            ProductId = product_id,
                            QuantitySells = quantitysells,
                            QuantityOrders = quantityorders,
                            AllPrice = allprice,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return statistics;
        }

        internal List<Statistic> SelectStatisticNotReady()
        {
            List<Statistic> statistics = new List<Statistic>();
            if (connection == null)
                return statistics;

            if (connection.OpenConnection())
            {
                // var command = connection.CreateCommand($"SELECT ROW_NUMBER() OVER (ORDER BY p.`id`) AS Id, p.`id` AS ProductId, p.`title` AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0) AS quantityorders, COALESCE(SUM(o.`quantity_prod`), 0) AS quantitysells, COALESCE(o.`product_id`, 0) AS product_id, COALESCE(SUM(o.`full_price`), 0) AS allprice FROM `product` p LEFT JOIN `order` o ON o.`product_id` = p.`id` AND o.`is_ready` = 0 WHERE p.`id` <> 1 GROUP BY p.`id`, p.`title`, o.`product_id`;");

                var command = connection.CreateCommand($"SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY p.`id`) AS Id, p.`id` AS ProductId, p.`title` AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0) AS quantityorders, COALESCE(SUM(o.`quantity_prod`), 0) AS quantitysells, 0 AS product_id, COALESCE(SUM(o.`full_price`), 0) AS allprice FROM `product` p LEFT JOIN `order` o ON o.`product_id` = p.`id` WHERE p.`id` <> 1 AND o.is_ready = false GROUP BY p.`id`, p.`title` UNION ALL SELECT 1000 AS Id, 1 AS ProductId, 'Итого' AS Title, COALESCE(COUNT(DISTINCT o.`id`), 0), COALESCE(SUM(o.`quantity_prod`), 0), 1 AS product_id, COALESCE(SUM(o.`full_price`), 0) FROM `order` o WHERE o.`product_id` > 1 AND o.is_ready = false) AS combined;");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string Title = string.Empty;
                        if (!dr.IsDBNull(1))
                            Title = dr.GetString("Title");

                        int product_id = dr.GetInt32("product_id");

                        int quantityorders = 0;
                        if (!dr.IsDBNull(2))
                            quantityorders = dr.GetInt32("quantityorders");

                        decimal quantitysells = 0;
                        if (!dr.IsDBNull(3))
                            quantitysells = dr.GetDecimal("quantitysells");

                        decimal allprice = 0;
                        if (!dr.IsDBNull(3))
                            allprice = dr.GetDecimal("allprice");

                        statistics.Add(new Statistic
                        {
                            Id = id,
                            Title = Title,
                            ProductId = product_id,
                            QuantitySells = quantitysells,
                            QuantityOrders = quantityorders,
                            AllPrice = allprice,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return statistics;
        }

        static StatisticDB db;
        public static StatisticDB GetDb()
        {
            if (db == null)
                db = new StatisticDB(DbConnection.GetDbConnection());
            return db;
        }
    }

}

