using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        public Account(string userName, string displayName, int role, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Role = role;
            this.Password = password;
        }

        public Account(DataRow row)
        {
            this.UserName = row["A_userName"].ToString();
            this.DisplayName = row["A_displayName"].ToString();
            this.Role = (int)row["A_role"];
            this.Password = row["A_passWord"].ToString();
        }

        private int role;

        private string password;

        private string displayName;

        private string userName;

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public int Role
        {
            get
            {
                return role;
            }

            set
            {
                role = value;
            }
        }
    }
}
