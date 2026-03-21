using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DBConnection
{
    internal class Program
    {
        static string ConnectionString = "Server=.; Database=CarsDetails; User Id=sa; Password=123456";
        static void PrintCarsDetails() { 
            SqlConnection Connection = new SqlConnection(ConnectionString);
            string Query = "select distinct top 10 Vehicle_Display_Name, Engine_CC, Model from CarsDataBase;";
            SqlCommand Command = new SqlCommand (Query, Connection);
            try { 
                Connection.Open ();

                SqlDataReader Reader = Command.ExecuteReader ();

                while (Reader.Read())
                {
                    string Name = (string)Reader["Vehicle_Display_Name"];
                    string Model = (string)Reader["Model"];
                    short EngineCC = (short)Reader["Engine_CC"];

                    Console.WriteLine($"Name: {Name}");
                    Console.WriteLine($"Model: {Model}");
                    Console.WriteLine($"Engine CC:{EngineCC}");
                    Console.WriteLine();   



                }


                Reader.Close();
                Connection.Close();
            
            }
            catch(Exception ex) { 
                Console.WriteLine("Error: " + ex.Message);
                Connection.Close();

            }
        }
        //Prameterized Query
        static void GetCarsWihtName(string CarName)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);
            string Query = "select top 50 Vehicle_Display_Name, Engine_CC, Model from CarsDataBase" +
                " where Vehicle_Display_Name like '%' + @CarName + '%';";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CarName", CarName);
            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    string Name = (string)Reader["Vehicle_Display_Name"];
                    string Model = (string)Reader["Model"];
                    short EngineCC = (short)Reader["Engine_CC"];

                    Console.WriteLine($"Name: {Name}");
                    Console.WriteLine($"Model: {Model}");
                    Console.WriteLine($"Engine CC:{EngineCC}");
                    Console.WriteLine();



                }


                Reader.Close();
                Connection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Connection.Close();

            }
        }
        // use ExecuteScalar
        static string GetCarName(int MakeID)
        {
            string Name = "";
            SqlConnection Connection = new SqlConnection(ConnectionString);
            string Query = "select Vehicle_Display_Name from CarsDataBase where MakeID = @MakeID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@MakeID", MakeID);
            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    Name = Result.ToString();
                }
                else {
                    Name = "Not Found";
                
                }
                Connection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Connection.Close();

            }
            return Name;
        }


        static void Main(string[] args)
        {
            //PrintCarsDetails();
            //GetCarsWihtName("Su");
            Console.WriteLine(GetCarName(10));

            Console.ReadKey();  
        }
    }
}
