using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {

        BindingSource BeverageList = new BindingSource();
        BindingSource TableList = new BindingSource();
        BindingSource CaterogyList = new BindingSource();
        BindingSource AccountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();

            InitializeData();

        }


        #region Methods

        List<Beverage> SearchBeverageByName(string name)
        {
            List<Beverage> listBeverage = BeverageDAO.Instence.SearchBeverageByName(name);
            return listBeverage;
        }
        void InitializeData()
                {
                    dtgvBeverage.DataSource = BeverageList;
                    dtgvAccount.DataSource = AccountList;
                    dtgvCategory.DataSource = CaterogyList;
                    dtgvTable.DataSource = TableList;

                    LoadDateTimePickerBill();
                    LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value, 1);
                    LoadListBeverage();
                    LoadAccount();
                    LoadCategory();
                    LoadTable();
                    LoadCategoryIntoCombobox(cbBeverageCategory);

                    AddBeverageBinding();
                    AddAccountBinding();
                    AddTableBinding();
                    AddCategoryBinding();

                }
          
   
        private void label1_Click(object sender, EventArgs e)
        {
        }

       

        void LoadListBillByDate (DateTime checkIn, DateTime checkOut, int page)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(checkIn, checkOut, page);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadCategory()
        {
            CaterogyList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        void LoadListBeverage()
        {
            BeverageList.DataSource = BeverageDAO.Instence.GetListBeverage();

        }

        void LoadTable()
        {
            TableList.DataSource = TableDAO.Instance.LoadTableList();
        }

        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }

        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "A_userName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "A_displayName", true, DataSourceUpdateMode.Never));
            numericAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "A_role", true, DataSourceUpdateMode.Never));
        }

        void AddBeverageBinding()
        {
            txbBeverageName.DataBindings.Add(new Binding("Text", dtgvBeverage.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbBeverageID.DataBindings.Add(new Binding("Text", dtgvBeverage.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmBeveragePrice.DataBindings.Add(new Binding("Value", dtgvBeverage.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void AddAccount(string username, string displayName, int type)
        {
            if( AccountDAO.Instance.InsertAccount(username, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            LoadAccount();
        }

        void EditAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật  khoản thất bại");
            }
            LoadAccount();
        }

        void DeleteAccount(string username)
        {
            if (loginAccount.UserName.Equals(username))
            {
                MessageBox.Show("Xóa Tài Khoản thất bại. Bạn đang đăng nhập bằng tài khoản này!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            LoadAccount();
        }
        void ResetPass(string username)
        {
            if (AccountDAO.Instance.ResetPassword(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        #endregion

        #region Event
        //Account Event
        private void btnAddAccount_Click_1(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericAccountType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click_1(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            DeleteAccount(userName);
        }

        private void btnEditAccount_Click_1(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericAccountType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            ResetPass(userName);
        }

        private void btnSearchBeverage_Click(object sender, EventArgs e)
        {
           BeverageList.DataSource = SearchBeverageByName(txbSearchBeverageName.Text);
        }

        private void txbBeverageID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvBeverage.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvBeverage.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbBeverageCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbBeverageCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbBeverageCategory.SelectedIndex = index;
                }
            }
            catch
            {

            }

        }

        private void btnAddBeverage_Click(object sender, EventArgs e)
        {
            string name = txbBeverageName.Text;
            int categoryID = (cbBeverageCategory.SelectedItem as Category).ID;
            float price = (float)nmBeveragePrice.Value;

            if (BeverageDAO.Instence.InsertBeverage(name, categoryID, price))
            {
                MessageBox.Show("Thêm đồ uống thành công");
                LoadListBeverage();
                if (insertBeverage != null)
                    insertBeverage(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm đồ uống");
            }
        }

        private void btnEditBeverage_Click(object sender, EventArgs e)
        {
            string name = txbBeverageName.Text;
            int categoryID = (cbBeverageCategory.SelectedItem as Category).ID;
            float price = (float)nmBeveragePrice.Value;
            int id = Convert.ToInt32(txbBeverageID.Text);

            if (BeverageDAO.Instence.UpdateBeverage(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa đồ uống thành công");
                LoadListBeverage();
                if (updateBeverage != null)
                    updateBeverage(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi Sửa đồ uống");
            }
        }

        private void btnDeleteBeverage_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbBeverageID.Text);

            if (BeverageDAO.Instence.DeleteBeverage(id))
            {
                MessageBox.Show("Xóa đồ uống thành công");
                LoadListBeverage();
                if (deleteBeverage != null)
                    deleteBeverage(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi Xóa đồ uống");
            }
        }
        private void btnShowBeverage_Click(object sender, EventArgs e)
        {
            LoadListBeverage();
           
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value, 1);
        }

        private event EventHandler insertBeverage;
        public event EventHandler InsertBeverage
        {
            add { insertBeverage += value; }
            remove { insertBeverage -= value; }
        }

        private event EventHandler deleteBeverage;
        public event EventHandler DeleteBeverage
        {
            add { deleteBeverage += value; }
            remove { deleteBeverage -= value; }
        }

        private event EventHandler updateBeverage;
        public event EventHandler UpdateBeverage
        {
            add { updateBeverage += value; }
            remove { updateBeverage -= value; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }


        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;

            if (page < lastPage)
                page++;
            txbPageBill.Text = page.ToString();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }
            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

            this.rpViewer.RefreshReport();
        }

        private void rpViewer_Load(object sender, EventArgs e)
        {

        }

        private void btnAddTable_Click_1(object sender, EventArgs e)
        {
            string name = txbTableName.Text;

            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại");
            }
            LoadTable();
        }
        private void btnEditTable_Click_1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            string name = txbTableName.Text;
            string status = cbTableStatus.Text;

            if (TableDAO.Instance.UpdateTable(id, name, status))
            {
                MessageBox.Show("Cập nhật bàn thành công");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Cập nhật bàn thất bại");
            }
        }
        private void btnDeleteTable_Click_1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);

            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadTable();
            }
            else
            {
                MessageBox.Show("Xóa bàn thất bại");
            }
        }

        private void btnAddCategory_Click_1(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại");
            }
            LoadCategory();
        }

        private void btnEditCategory_Click_1(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công");
                LoadCategory();
            }
            else
            {
                MessageBox.Show("Sửa danh mục thất bại");
            }
        }

        private void btnDeleteCategory_Click_1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công");
            }
            else
            {
                MessageBox.Show("Xóa danh mục thất bại");
            }
            LoadCategory();
        }

        private void cbBeverageCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnViewTable_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hiển thị danh sách bàn thành công");
        }

        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
            MessageBox.Show("Hiển thị danh mục sản phẩm thành công");
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hiển thị danh mục tài khoản thành công");
        }


        #endregion

        private void dtgvBeverage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
