using System;
using System.Web.UI;

namespace ChatApp
{
    public partial class Login : Page
    {
        //Class Object
        ConnectionClass connectionToDb = new ConnectionClass();
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            string Query = "select * from tbl_Users where Email='" + txtEmail.Value + "' and Password='" + txtPassword.Value + "'";
            if (connectionToDb.IsExist(Query))
            {
                string UserName = connectionToDb.GetColumnVal(Query, "UserName");
                Session["UserName"] = UserName;
                Session["Email"] = txtEmail.Value;
                Response.Redirect("Chat.aspx");
            }
            else
                txtEmail.Value = "Invalid Email or Password!!";
        }
    }
}