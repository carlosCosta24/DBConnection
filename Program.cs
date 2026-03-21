using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Web;

namespace DBConnection
{
    internal class Program
    {
        static string ConnectionString = "Server=.; Database=HR_Database; User Id=sa; Password=123456";
        public struct stEmployee 
        {
            public int ID;
            public string FirstName;
            public string LastName;
            public string   Gender;
            public DateTime DateOfBrith;
            public int    CountryID;
            public int    DepartmentID;
            public DateTime HireDate;
            public DateTime ExitDate;
            public decimal  Salary;
            public float  BounsPer;


        
        }
        //find single employee
        static bool FindEmployeeByID(int EmployeeID, ref stEmployee Employee) { 
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionString);
            string Query = "select * from Employees where ID = @EmployeeID";
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.AddWithValue("EmployeeID", EmployeeID);

            try {
                Connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    isFound = true;
                    Employee.ID = (int)Reader["ID"];
                    Employee.FirstName = (string)Reader["FirstName"];
                    Employee.LastName = (string)Reader["LastName"];
                    Employee.Gender = (string)Reader["Gendor"];
                    Employee.DateOfBrith = (DateTime)Reader["DateOfBirth"];
                    Employee.CountryID = (int)Reader["CountryID"];
                    Employee.DepartmentID = (int)Reader["DepartmentID"];
                    Employee.HireDate = (DateTime)Reader["HireDate"];
                    Employee.ExitDate = (DateTime)Reader["ExitDate"];
                    Employee.Salary = (decimal)Reader["MonthlySalary"];
                    Employee.BounsPer = (float)Reader["BonusPerc"];

                }
                else { 
                    isFound = false;
                }
                Reader.Close(); 
                Connection.Close();
            } 
            catch (Exception E){
                Console.WriteLine("Error: "+ E.Message);
            }
            return isFound;

        
        
        }
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
            stEmployee Employee = new stEmployee();
            if (FindEmployeeByID(300, ref Employee))
            {

                Console.WriteLine($"\nEmployeeID: {Employee.ID}");
                Console.WriteLine($"FirstName: {Employee.FirstName}");
                Console.WriteLine($"LastName: {Employee.LastName}");
                Console.WriteLine($"Gender: {Employee.Gender}");
                Console.WriteLine($"Date of brith: {Employee.DateOfBrith}");
                Console.WriteLine($"Contry ID: {Employee.CountryID}");
                Console.WriteLine($"Department ID: {Employee.DepartmentID}");
                Console.WriteLine($"Hire Date: {Employee.HireDate}");
                Console.WriteLine($"Salary: {Employee.Salary}");
                Console.WriteLine($"Bouns Rate: {Employee.BounsPer}");
                Console.WriteLine($"Exist Date: {Employee.ExitDate}");
            }
            else { 
            
            }
            //PrintCarsDetails();
            //GetCarsWihtName("Su");
            //Console.WriteLine(GetCarName(10));

            Console.ReadKey();  
        }
    }
}
