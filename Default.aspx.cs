using System;
using System.Web.UI;

namespace _23049999_Sewanta_Luitel
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Home.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
