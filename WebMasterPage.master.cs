using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAboneOl_Click(object sender, EventArgs e)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
       Temp_EmailList yeniMailListeleri=new Temp_EmailList();
       yeniMailListeleri.Email = txtAboneEmail.Text;  
       db.Temp_EmailLists.InsertOnSubmit(yeniMailListeleri);
       db.SubmitChanges();
       txtAboneEmail.Text = "kayit alindi";
    }
}
