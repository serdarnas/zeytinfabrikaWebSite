using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

public partial class yonetim_MenuSiralama : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string menuHtml = GetMenuHtml();
            ltMenu.Text = menuHtml;
        }
    }

    private string GetMenuHtml()
    {
        DataTable dt = new DataTable();
        string connStr = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT [MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL] FROM [Menu] ORDER BY [Sira]", conn);
            da.Fill(dt);
        }
        return BuildMenuHtml(dt, null);
    }

    private string BuildMenuHtml(DataTable dt, int? parentId)
    {
        StringBuilder sb = new StringBuilder();
        DataRow[] rows = dt.Select(parentId == null ? "UstMenuID IS NULL" : "UstMenuID = " + parentId.ToString());
        if (rows.Length > 0)
        {
            sb.Append("<ol class='dd-list'>");
            foreach (DataRow row in rows)
            {
                sb.Append("<li class='dd-item dd3-item' data-id='" + row["MenuID"] + "'>");
                sb.Append("<div class='dd3-row'>");
                sb.Append("<div class='dd-handle dd3-handle'></div>");
                string ikon = row["Ikon"] != DBNull.Value && !string.IsNullOrEmpty(row["Ikon"].ToString())
                    ? "<i class='fa " + row["Ikon"] + "' style='margin-right:7px;'></i>"
                    : "";
                string menuAdi = row["MenuAdi"].ToString();
                string sayfaUrl = row.Table.Columns.Contains("SayfaURL") && row["SayfaURL"] != DBNull.Value
                    ? "<span class='menu-url'>" + row["SayfaURL"] + "</span>"
                    : "";
                sb.Append("<div class='dd3-content'>" + ikon + menuAdi + sayfaUrl + "</div>");
                sb.Append("</div>"); // dd3-row
                sb.Append(BuildMenuHtml(dt, Convert.ToInt32(row["MenuID"])));
                sb.Append("</li>");
            }
            sb.Append("</ol>");
        }
        return sb.ToString();
    }

    [System.Web.Services.WebMethod]
    public static void GuncelleSira(List<MenuItem> menuList)
    {
        int sira = 1;
        UpdateMenuOrder(menuList, null, ref sira);
    }

    public class MenuItem
    {
        public int id { get; set; }
        public List<MenuItem> children { get; set; }
    }

    private static void UpdateMenuOrder(List<MenuItem> menuList, int? ustMenuId, ref int sira)
    {
        string connStr = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            foreach (var item in menuList)
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Menu SET UstMenuID = @UstMenuID, Sira = @Sira WHERE MenuID = @MenuID", conn))
                {
                    cmd.Parameters.AddWithValue("@UstMenuID", (object)ustMenuId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Sira", sira++);
                    cmd.Parameters.AddWithValue("@MenuID", item.id);
                    cmd.ExecuteNonQuery();
                }
                if (item.children != null && item.children.Count > 0)
                {
                    UpdateMenuOrder(item.children, item.id, ref sira);
                }
            }
        }
    }
}