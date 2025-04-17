using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get
            {
                if (instance == null) instance = new MenuDAO(); return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "select be.BE_name, bi.BI_count, be.BE_price, be.BE_price*bi.BI_count as totalPrice from dbo.BillInfo as bi, dbo.Bill as b, dbo.Beverage as be where bi.BI_idBill = b.B_id and bi.BI_idBeverage = be.BE_id and b.B_status = 0 and b.B_idTable = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Menu menu = new DTO.Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
