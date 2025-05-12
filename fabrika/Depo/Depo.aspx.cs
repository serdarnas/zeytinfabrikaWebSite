using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_Depo : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Depo";
            master.SayfaAdi = "Döküman yönetim sistemi";
        }
        Directory.CreateDirectory(Server.MapPath("~/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/"));
        //ASPxFileManager1.Settings.RootFolder = "/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/";
        ASPxFileManager1.Settings.RootFolder = "~/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/";
        ASPxFileManager1.Settings.ThumbnailFolder = "~/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/Thumbnails";
        ASPxFileManager1.Settings.InitialFolder = "~/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/Images";
        ASPxFileManager1.Settings.AllowedFileExtensions = new String[] { ".jpeg", ".jpg", ".gif", ".png", ".pdf", ".txt", ".csv", ".xlsx", ".xls", ".docx", ".doc" };
    }
}