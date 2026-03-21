using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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

        static void Main(string[] args)
        {
            //PrintCarsDetails();
            GetCarsWihtName("Su");

            Console.ReadKey();  
        }
    }
}
