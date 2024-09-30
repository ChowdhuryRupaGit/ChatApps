using System;
using System.Web.UI;

namespace ChatApp
{
    public partial class Register : Page
    {
        ConnectionClass connectionToDb = new ConnectionClass();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegister_ServerClick(object sender, EventArgs e)
        {
            string Query = "insert into tbl_Users(UserName,Email,Password)Values('"+txtName.Value+"','"+txtEmail.Value+"','"+txtPassword.Value+"')";
            string ExistQ = "select * from tbl_Users where Email='"+txtEmail.Value+"'";
            if (!connectionToDb.IsExist(ExistQ))
            {
                if (connectionToDb.ExecuteQuery(Query))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Congratulations!! You have successfully registered..');", true);
                    Session["UserName"] = txtName.Value;
                    Session["Email"] = txtEmail.Value;
                    Response.Redirect("Chat.aspx");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Email is already Exists!! Please Try Different Email..');", true);
            }
        } 
    }
}