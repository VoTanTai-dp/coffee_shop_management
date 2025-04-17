using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Menu
    {
        public Menu(string beverageName, int count, float price, float totalPrice = 0)
        {
            this.BeverageName = beverageName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            this.BeverageName = row["BE_name"].ToString();
            this.Count = (int)row["BI_count"];
            this.Price = (float)Convert.ToDouble(row["BE_price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }

        private float totalPrice;

        private float price;

        private int count;

        private string beverageName;

        public string BeverageName
        {
            get
            {
                return beverageName;
            }

            set
            {
                beverageName = value;
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

        public float Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public float TotalPrice
        {
            get
            {
                return totalPrice;
            }

            set
            {
                totalPrice = value;
            }
        }
    }
}
