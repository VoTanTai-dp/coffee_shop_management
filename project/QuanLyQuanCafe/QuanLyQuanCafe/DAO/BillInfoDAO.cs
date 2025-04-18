﻿using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get
            {
                if (instance == null) instance = new BillInfoDAO(); return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private BillInfoDAO() { }

        public void DeteleBillInfoByBeverageID(int id)
        {
            DataProvider.Instance.ExecuteQuery("Delete dbo.BillInfo where BI_idBeverage = " + id);
        }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.BillInfo where BI_idBill = " + id);

            foreach(DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idBeverage, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idBill , @idBeverage , @count", new object[] { idBill, idBeverage, count });
        }
    }
}
