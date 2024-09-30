using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ChatApp
{
    public class ConnectionClass
    {
        public SqlCommand cmd = new SqlCommand();
        public SqlDataAdapter sda;
        public SqlDataReader sdr;
        public DataSet ds = new DataSet();
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionToDB"].ToString());

        public bool IsExist(string query)
        {
            bool check = false;
            using (cmd = new SqlCommand(query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    check = true;
            }
            sdr.Close();
            con.Close();
            return check;

        }

        public bool ExecuteQuery(string query)
        {
            int j = 0;
            using (cmd = new SqlCommand(query, con))
            {
                con.Open();
                j = cmd.ExecuteNonQuery();
                con.Close();
            }

            if (j > 0)
                return true;
            else
                return false;

        }

        public string GetColumnVal(string query, string columnName)
        {
            string RetVal = "";
            using (cmd = new SqlCommand(query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    RetVal = sdr[columnName].ToString();
                    break;
                }
                sdr.Close();
                con.Close();
            }

            return RetVal;


        }

    }
}