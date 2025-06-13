using System;
using System.Web.UI;

public partial class fabrika_Zeytinyagi_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Simple test to ensure code-behind is working
            if (!IsPostBack)
            {
                lblTest.Text = "Page loaded successfully at: " + DateTime.Now.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError(ex);
            Response.Write("Error: " + ex.Message);
        }
    }
}
