﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using SD = System.Drawing;

namespace ChatApp
{
    public partial class Chat : System.Web.UI.Page
    {
        public string UserName = "admin";
        public string UserImage = "/images/dummys.png";
        protected string UploadFolderPath = "~/Uploads/";
        ConnectionClass connectionToDb = new ConnectionClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                UserName = Session["UserName"].ToString();
                GetUserImage(UserName);
            }
            else
                Response.Redirect("Login.aspx");

             Header.DataBind();
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        public void GetUserImage(string Username)
        {
            if (Username != null)
            {
                string query = "select Photo from tbl_Users where UserName='" + Username + "'";

                string ImageName = connectionToDb.GetColumnVal(query, "Photo");
                if (ImageName != "")
                    UserImage = "images/DP/" + ImageName;
            }


        }

        protected void btnChangePicModel_Click(object sender, EventArgs e)
        {

            string serverPath = HttpContext.Current.Server.MapPath("~/");
            //path = serverPath + path;
            if (FileUpload1.HasFile)
            {
                string FileWithPat = serverPath + @"images/DP/" + UserName + FileUpload1.FileName;

                FileUpload1.SaveAs(FileWithPat);
                Image img = Image.FromFile(FileWithPat);
                Image img1 = RezizeImage(img, 151, 150);
                img1.Save(FileWithPat);
                if (File.Exists(FileWithPat))
                {
                    FileInfo fi = new FileInfo(FileWithPat);
                    string ImageName = fi.Name;
                    string query = "update tbl_Users set Photo='" + ImageName + "' where UserName='" + UserName + "'";
                    if (connectionToDb.ExecuteQuery(query))
                        UserImage = "images/DP/" + ImageName;
                }
            }
        }


        #region Resize Image With Best Qaulity

        private Image RezizeImage(Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, SD.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    gr.Clear(Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new Rectangle(0, 0, nnx, nny),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        #endregion

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(AsyncFileUpload1.FileName);
            AsyncFileUpload1.SaveAs(Server.MapPath(this.UploadFolderPath) + filename);
        }
    }
}