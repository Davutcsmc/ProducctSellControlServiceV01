using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Session
{
    public class Broker
    {
        SqlConnection conn;
        SqlConnectionStringBuilder connStringBuilder;

        string m_DBConnString = string.Empty;
        static int uniqVal = 2;

        void ConnectTo()
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "OnLogging.txt", true))
                {
                    file.WriteLine(m_DBConnString);
                }
                //Data Source = DCESM - PC; Initial Catalog = ProductDB; Integrated Security = True
                connStringBuilder = new SqlConnectionStringBuilder(m_DBConnString);
                conn = new SqlConnection(connStringBuilder.ToString());
            }
            catch(Exception e)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "OnLogging.txt", true))
                {
                    file.WriteLine(e.Message);
                }
            }
            
        }

        public Broker()
        {
            m_DBConnString = ReadConfigurationValues.GetConfiguration.Instance.GetDBConnString();
            ConnectTo();
        }

        public void Insert(Model.Product p)
        {
            try
            {
                string cmdText = "INSERT INTO dbo.ProductList(UniqueIdentifier,Barcode,Name,Model,Color) VALUES (@UniqueIdentifier, @Barcode, @Name, @Model, @Color)";
                SqlCommand cmd = new SqlCommand(cmdText,conn);
                cmd.Parameters.AddWithValue("@UniqueIdentifier", p.UniqueIdentifier);
                cmd.Parameters.AddWithValue("@Barcode", p.Barcode);
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@Model", p.Model);
                cmd.Parameters.AddWithValue("@Color", p.Color);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "OnLogging.txt", true))
                {
                    file.WriteLine(ex.Message);
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }


        public Model.Product GetProduct(int id)
        {
            Model.Product selectedProduct = new Model.Product();
            try
            {
                List<Model.Product> productList = new List<Model.Product>();
                
                string cmdText = "SELECT TOP 100 * FROM dbo.ProductList Order by Id asc";
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    Model.Product p = new Model.Product();
                    p.UniqueIdentifier = reader["UniqueIdentifier"].ToString();
                    p.Barcode = reader["Barcode"].ToString();
                    p.Name = reader["Name"].ToString();
                    p.Model = reader["Model"].ToString();
                    p.Color = reader["Color"].ToString();

                    productList.Add(p);
                }

                selectedProduct = productList.ElementAt(id);

            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "OnLogging.txt", true))
                {
                    file.WriteLine(ex.Message);
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return selectedProduct;

        }


    }
}
