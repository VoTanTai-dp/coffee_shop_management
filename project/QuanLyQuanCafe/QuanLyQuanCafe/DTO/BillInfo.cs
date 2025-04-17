using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int billID, int beverageID, int count)
        {
            this.ID = id;
            this.BillID = billID;
            this.BeverageID = beverageID;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.ID = (int)row["BI_id"];
            this.BillID = (int)row["BI_idBill"];
            this.BeverageID = (int)row["BI_idBeverage"];
            this.Count = (int)row["BI_count"];
        }

        private int count;

        private int beverageID;

        private int billID;

        private int iD;

        public int BeverageID
        {
            get
            {
                return beverageID;
            }

            set
            {
                beverageID = value;
            }
        }

        public int BillID
        {
            get
            {
                return billID;
            }

            set
            {
                billID = value;
            }
        }

        public int ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }
    }
}
