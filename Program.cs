using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static DBConnection.Program;

namespace DBConnection
{
    internal class Program
    {
        static string ConnectionString = "Server=.; Database=HR_Database; User Id=sa; Password=123456";
        public struct stEmployee 
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName{ get; set; }
            public string Gender{ get; set; }
            public DateTime DateOfBirth{ get; set; }
            public int CountryID{ get; set; }
            public int DepartmentID{ get; set; }
            public DateTime HireDate{ get; set; }
            public DateTime? ExitDate{ get; set; }
            public decimal Salary{ get; set; }
            public decimal BonusPerc{ get; set; }



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
                    Employee.DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Employee.CountryID = (int)Reader["CountryID"];
                    Employee.DepartmentID = (int)Reader["DepartmentID"];
                    Employee.HireDate = (DateTime)Reader["HireDate"];
                    Employee.ExitDate = Reader["ExitDate"] == DBNull.Value
                        ? (DateTime?)null
                        : (DateTime)Reader["ExitDate"];
                    Employee.Salary = (decimal)Reader["MonthlySalary"];
                    Employee.BonusPerc = Math.Round(Convert.ToDecimal(Reader["BonusPerc"]),2);

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

        //Insert into DataBase

        static void AddEmployee(stEmployee Employee) {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = @"insert into Employees 
            (FirstName, LastName, Gendor, DateOfBirth, CountryID, DepartmentID, HireDate, ExitDate, MonthlySalary, BonusPerc)
             values(@FirstName, @LastName, @Gendor, @DateOfBirth, @CountryID, @DepartmentID, 
             @HireDate, @ExitDate, @MonthlySalary, @BonusPerc)";

            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@FirstName", Employee.FirstName);
            Command.Parameters.AddWithValue("@LastName", Employee.LastName);
            Command.Parameters.AddWithValue("@Gendor", Employee.Gender);
            Command.Parameters.AddWithValue("@DateOfBirth", Employee.DateOfBirth);
            Command.Parameters.AddWithValue("@CountryID", Employee.CountryID);
            Command.Parameters.AddWithValue("@DepartmentID", Employee.DepartmentID);
            Command.Parameters.AddWithValue("@HireDate", Employee.HireDate);
            Command.Parameters.AddWithValue("@ExitDate", Employee.ExitDate ?? (object)DBNull.Value);
            Command.Parameters.AddWithValue("@MonthlySalary", Employee.Salary);
            Command.Parameters.AddWithValue("@BonusPerc", Employee.BonusPerc);

            try
            {

                connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    Console.WriteLine("Added Successfuly");
                }
                else
                {
                    Console.WriteLine("Error whiel Adding");
                }

            }
            catch (Exception E) { 
                Console.WriteLine(E.Message);
            }
            connection.Close();



        }
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
        //Get the Id After adding to DB
        static void AddEmployeeAndGetID(stEmployee Employee)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);

            string Query = @"insert into Employees 
            (FirstName, LastName, Gendor, DateOfBirth, CountryID, DepartmentID, HireDate, ExitDate, MonthlySalary, BonusPerc)
             values(@FirstName, @LastName, @Gendor, @DateOfBirth, @CountryID, @DepartmentID, 
             @HireDate, @ExitDate, @MonthlySalary, @BonusPerc); 
             select scope_identity();";

            SqlCommand Command = new SqlCommand(Query, connection);

                Command.Parameters.AddWithValue("@FirstName", Employee.FirstName);
                Command.Parameters.AddWithValue("@LastName", Employee.LastName);
                Command.Parameters.AddWithValue("@Gendor", Employee.Gender);
                Command.Parameters.AddWithValue("@DateOfBirth", Employee.DateOfBirth);
                Command.Parameters.AddWithValue("@CountryID", Employee.CountryID);
                Command.Parameters.AddWithValue("@DepartmentID", Employee.DepartmentID);
                Command.Parameters.AddWithValue("@HireDate", Employee.HireDate);
                Command.Parameters.AddWithValue("@ExitDate", Employee.ExitDate ?? (object)DBNull.Value);
                Command.Parameters.AddWithValue("@MonthlySalary", Employee.Salary);
                Command.Parameters.AddWithValue("@BonusPerc", Employee.BonusPerc);
            try {
                connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int ProcessID))
                {
                    Console.WriteLine($"Add succfully, ID: {ProcessID}");
                }
                else {
                    Console.WriteLine("Failed to insert");
                }
                connection.Close();

            
            
            } catch (Exception E) {
                Console.WriteLine("Error: " + E.Message);
                connection.Close( );
            }

        }
        //Update Employee
        static void UpdateEmployee(int ID, stEmployee Employee) { 
        
            SqlConnection connection = new SqlConnection(ConnectionString);

            string Query = @"update Employees set 
                FirstName = @FirstName, 
                LastName = @LastName, 
                Gendor = @Gendor, 
                DateOfBirth = @DateOfBirth, 
                CountryID = @CountryID, 
                DepartmentID = @DepartmentID, 
                HireDate = @HireDate, 
                ExitDate = @ExitDate, 
                MonthlySalary = @MonthlySalary, 
                BonusPerc = @BonusPerc 
                where ID = @ID ";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@ID", ID);
            Command.Parameters.AddWithValue("@FirstName", Employee.FirstName);
            Command.Parameters.AddWithValue("@LastName", Employee.LastName);
            Command.Parameters.AddWithValue("@Gendor", Employee.Gender);
            Command.Parameters.AddWithValue("@DateOfBirth", Employee.DateOfBirth);
            Command.Parameters.AddWithValue("@CountryID", Employee.CountryID);
            Command.Parameters.AddWithValue("@DepartmentID", Employee.DepartmentID);
            Command.Parameters.AddWithValue("@HireDate", Employee.HireDate);
            Command.Parameters.AddWithValue("@ExitDate", Employee.ExitDate ?? (object)DBNull.Value);
            Command.Parameters.AddWithValue("@MonthlySalary", Employee.Salary);
            Command.Parameters.AddWithValue("@BonusPerc", Employee.BonusPerc);

            try {
                connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    Console.WriteLine("Updated Successfully");

                }
                else
                {
                    Console.WriteLine("Failed to Update");

                }

                connection.Close();




            } catch (Exception E) { 
                
                connection.Close();
                Console.WriteLine(E.Message);
            }

        }

        //Delete Employee
        static void DeleteEmployee(int ID) { 
            

            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = @"delete from Employees where ID = @ID";
            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@ID", ID);
            try { 
                
                connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    Console.WriteLine("Deleted successfully");
                }
                else {
                    Console.WriteLine("Failed to delete");
                }
                connection.Close();
            
            } catch(Exception E) {
                connection.Close();
                Console.WriteLine("Error: " + E.Message);

            }
        
        }
        static void Main(string[] args)             
        {
            //stEmployee Carlos = new stEmployee();
            /*stEmployee Employee = new stEmployee {

                ID = 1285,
                FirstName = "Carlos",
                LastName = "Costa",
                Gender = "M",
                DateOfBirth = new DateTime(1998,7,8),
                CountryID = 2,
                DepartmentID = 1,
                HireDate = new DateTime(2026,8,15),
                ExitDate = null,
                Salary = 10000,
                BonusPerc = 0.35f,
            };*/
            //AddEmployee(Employee);
            /*if (FindEmployeeByID(1285, ref Carlos))
            {

                Console.WriteLine($"\nEmployeeID: {Carlos.ID}");
                Console.WriteLine($"FirstName: {Carlos.FirstName}");
                Console.WriteLine($"LastName: {Carlos.LastName}");
                Console.WriteLine($"Gender: {Carlos.Gender}");
                Console.WriteLine($"Date of brith: {Carlos.DateOfBirth}");
                Console.WriteLine($"Contry ID: {Carlos.CountryID}");
                Console.WriteLine($"Department ID: {Carlos.DepartmentID}");
                Console.WriteLine($"Hire Date: {Carlos.HireDate}");
                Console.WriteLine(Carlos.ExitDate == null? "ExiteDate: Still working" : $"ExiteDate: {Carlos.ExitDate}" );
                Console.WriteLine($"Salary: {Carlos.Salary}");
                Console.WriteLine($"Bouns Rate: {Carlos.BonusPerc}");
            }
            else {
                Console.WriteLine("Employee not found");
            
            }*/
            //PrintCarsDetails();
            //GetCarsWihtName("Su");
            //Console.WriteLine(GetCarName(10));
            /*stEmployee Ricardo = new stEmployee
            {
                ID = 1286,
                FirstName = "Ricardo",
                LastName = "Costa",
                Gender = "M",
                DateOfBirth = new DateTime(1988, 7, 8),
                CountryID = 2,
                DepartmentID = 1,
                HireDate = new DateTime(2026, 8, 15),
                ExitDate = null,
                Salary = 10000,
                BonusPerc = (decimal)0.35f,

            };
            AddEmployeeAndGetID(Ricardo);*/

            /* stEmployee Aloha = new stEmployee
             {
                 FirstName = "Aloha",
                 LastName = "Costa",
                 Gender = "F",
                 DateOfBirth = new DateTime(2001, 7, 8),
                 CountryID = 2,
                 DepartmentID = 7,
                 HireDate = new DateTime(2020, 8, 15),
                 ExitDate = null,
                 Salary = 1000,
                 BonusPerc = (decimal)0.25f,

             };
             UpdateEmployee(500, Aloha);*/
            DeleteEmployee(1);
           
            Console.ReadKey();  
        }
    }
}
