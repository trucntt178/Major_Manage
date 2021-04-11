using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Major_Manage
{
    public partial class frmMain : Form
    {
        bool changed = false;
        SqlDataAdapter dtAdap;
        public static DataSet ds = new DataSet("MajorDatabase");
        public frmMain()
        {
            InitializeComponent();
            initDataSet();
            LoadMajors();
        }
        public void initDataSet()
        {
            SqlConnection con = DBHelper.getConnection();
            dtAdap = new SqlDataAdapter("Select * from Major", con);
            SqlCommandBuilder builder = new SqlCommandBuilder(dtAdap);
        }

        public void LoadMajors()
        {
            try
            {
                dtAdap.Fill(ds, "Major");
                dataGridView.DataSource = ds.Tables["Major"];
                cbMajorID.DataSource = ds.Tables["Major"];
                cbMajorID.DisplayMember = "ID";
                dataGridView.AutoResizeColumns();
                txtTotal.Text = ds.Tables["Major"].Rows.Count.ToString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                dataGridView.DataSource = null;
            }

        }

        private void cbMajorID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbMajorID.SelectedIndex;
            if (index >= 0)
            {
                this.txtMajorID.Text = dataGridView.Rows[index].Cells[0].Value.ToString();
                this.txtName.Text = dataGridView.Rows[index].Cells[1].Value.ToString();
                txtMajorID.Enabled = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMajorID.Enabled = true;
            txtMajorID.Text = "";
            txtName.Text = "";
        }

        private bool checkValid()
        {
            bool valid = true;
            if (txtMajorID.Text.Equals(""))
            {
                lbIdErr.Text = "Empty Id";
                valid = false;
            }
            else
            {
                bool duplicated = false;
                if (dataGridView.Rows.Count > 0)
                {
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {

                        if (txtMajorID.Text.Equals(dataGridView.Rows[i].Cells[0].Value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            duplicated = true;
                            break;
                        }
                    }
                }
                if (duplicated)
                {
                    lbIdErr.Text = "Id is duplicated.";
                    valid = false;
                }
                else
                {
                    lbIdErr.Text = "";
                }
            }
            if (txtName.Text.Equals(""))
            {
                lbNameErr.Text = "Empty Name";
                valid = false;
            }
            else
            {
                lbNameErr.Text = "";
            }
            return valid;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!txtMajorID.Enabled)
            {
                lbErr.Text = "Click clear button before add new";
            }
            else
            {
                lbErr.Text = "";
                bool valid = checkValid();
                if (valid)
                {

                    DataRow newMajor = ds.Tables["Major"].NewRow();
                    newMajor["ID"] = txtMajorID.Text;
                    newMajor["Name"] = txtName.Text;
                    ds.Tables["Major"].Rows.Add(newMajor);
                    txtTotal.Text = dataGridView.Rows.Count.ToString();
                    btnClear_Click(sender, e);
                    changed = true;
                }

            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lbIdErr.Text = "";
            lbNameErr.Text = "";
            if (txtMajorID.Enabled)
            {
                lbErr.Text = "Choose a major to update ";
            }
            else
            {
                lbErr.Text = "";
                if (txtName.Text.Equals(""))
                {
                    lbNameErr.Text = "Empty Name";
                }
                else
                {
                    lbNameErr.Text = "";
                    DataRow[] majorRowToUpdate = ds.Tables["Major"].Select(string.Format("ID = '{0}'", this.txtMajorID.Text));
                    majorRowToUpdate[0]["Name"] = this.txtName.Text;
                    btnClear_Click(sender, e);
                    changed = true;
                }

            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lbIdErr.Text = "";
            lbNameErr.Text = "";
            if (txtMajorID.Enabled)
            {
                lbErr.Text = "Choose a major to delete ";
            }
            else
            {
                if (MessageBox.Show("Sure you wanna delete?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    lbErr.Text = "";
                    DataRow[] majorRowRemove = ds.Tables["Major"].Select(string.Format("ID = '{0}'", this.txtMajorID.Text));
                    majorRowRemove[0].Delete();
                    txtTotal.Text = dataGridView.Rows.Count.ToString();
                    btnClear_Click(sender, e);
                    changed = true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dtAdap.Update(ds.Tables["Major"]);
                btnClear_Click(sender, e);
                changed = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (!changed)
            {
                DialogResult h = MessageBox.Show("Do you want to exit program ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (h == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            else
            {
                DialogResult h = MessageBox.Show("Do you want to save ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (h == DialogResult.Yes)
                {
                    btnSave_Click(sender, e);

                }
                Application.Exit();
            }
        }
    }
}

