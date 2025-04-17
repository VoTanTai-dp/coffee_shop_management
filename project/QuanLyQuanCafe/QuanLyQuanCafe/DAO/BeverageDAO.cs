using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BeverageDAO
    {
        private static BeverageDAO instence;

        public static BeverageDAO Instence
        {
            get
            {
                if (instence == null) instence = new BeverageDAO(); return instence;
            }

            private set
            {
                instence = value;
            }
        }
        private BeverageDAO() { }

        public List<Beverage> GetBeverageByCategoryID(int id)
        {
            List<Beverage> list = new List<Beverage>();

            string query = "select * from Beverage where BE_idCategory = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Beverage beverage = new Beverage(item);
                list.Add(beverage);
            }

            return list;
        }

        public List<Beverage> GetListBeverage()
        {
            List<Beverage> list = new List<Beverage>();

            string query = "select * from Beverage";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Beverage beverage = new Beverage(item);
                list.Add(beverage);
            }

            return list;
        }

        public List<Beverage> SearchBeverageByName(string name)
        {
            List<Beverage> list = new List<Beverage>();

            string query = string.Format("select * from Beverage where BE_name like N'%{0}%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Beverage beverage = new Beverage(item);
                list.Add(beverage);
            }

            return list;
        }

        public bool InsertBeverage(string name, int id, float price)
        {
            string query = string.Format("insert into dbo.Beverage(BE_name, BE_idCategory, BE_price) values (N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateBeverage(int idBeverage,string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Beverage SET BE_name = N'{0}', BE_idCategory = {1}, BE_price = {2} WHERE BE_id = {3}", name, id, price, idBeverage);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategoryBeverage(int id_Category)
        {
            string query = string.Format("UPDATE Beverage SET BE_idCategory = 32 WHERE BE_idCategory = {0}", id_Category);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteBeverage(int idBeverage)
        {
            BillInfoDAO.Instance.DeteleBillInfoByBeverageID(idBeverage);
            string query = string.Format("DELETE Beverage where BE_id = {0} ", idBeverage);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
