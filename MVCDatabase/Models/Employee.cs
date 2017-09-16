using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace MVCDatabase.Models
{
    public class Employee
    {
        [Required, Key, Range(minimum: 1, maximum: 99)]
        public int EmpId { get; set; }
        [Required, MaxLength(20)]
        public string LastName { get; set; }
        [RegularExpression("^[1-9][0]$", ErrorMessage = "Dept Id is not valid")]
        public int DeptId { get; set; }
        [Required, Range(minimum: 500, maximum: 2500)]
        public decimal Salary { get; set; }

        public static Employee GetEmployeeSingle(SqlConnection dbcon, int id)
        {
            Employee obj = new Employee();
            string strsql = "select * from Employee where EmpId = " + id;
            SqlCommand cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            if (myReader.HasRows)
            {
                myReader.Read();
                obj.EmpId = Convert.ToInt32(myReader["EmpId"].ToString());
                obj.LastName = myReader["LastName"].ToString();
                obj.DeptId = Convert.ToInt32(myReader["DeptId"].ToString());
                obj.Salary = Convert.ToDecimal(myReader["Salary"].ToString());
            }
            myReader.Close();
            cmd.Dispose();
            return obj;
        }

        public static List<Employee> GetEmployeeList(SqlConnection dbcon, string SqlClause)
        {
            List<Employee> itemlist = new List<Employee>();
            string strsql = "select * from Employee " + SqlClause;
            SqlCommand cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                Employee obj = new Employee();
                obj.EmpId = Convert.ToInt32(myReader["EmpId"].ToString());
                obj.LastName = myReader["LastName"].ToString();
                obj.DeptId = Convert.ToInt32(myReader["DeptId"].ToString());
                obj.Salary = Convert.ToDecimal(myReader["Salary"].ToString());
                itemlist.Add(obj);
            }
            myReader.Close();
            cmd.Dispose();
            return itemlist;
        }

        public static int CUDEmployee(SqlConnection dbcon, string CUDAction, Employee obj)
        {
            SqlCommand cmd = new SqlCommand();
            if (CUDAction == "create")
            {
                cmd.CommandText = "insert into Employee " +
                "Values (@EmpId,@LastName,@DeptId,@Salary)";
                cmd.Parameters.AddWithValue("@EmpId", SqlDbType.Int).Value = obj.EmpId;
                cmd.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value =
                obj.LastName;
                cmd.Parameters.AddWithValue("@DeptId", SqlDbType.Int).Value = obj.DeptId;
                cmd.Parameters.AddWithValue("@Salary", SqlDbType.Decimal).Value = obj.Salary;
            }
            else if (CUDAction == "update")
            {
                cmd.CommandText = "update Employee set LastName = @LastName," +
                "DeptId = @DeptId,Salary = @Salary Where EmpId = @EmpId";
                cmd.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value =
                obj.LastName;
                cmd.Parameters.AddWithValue("@DeptId", SqlDbType.Int).Value = obj.DeptId;
                cmd.Parameters.AddWithValue("@Salary", SqlDbType.Decimal).Value = obj.Salary;
                cmd.Parameters.AddWithValue("@EmpId", SqlDbType.Int).Value = obj.EmpId;
            }
            else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete Employee where EmpId = @EmpId";
                cmd.Parameters.AddWithValue("@EmpId", SqlDbType.Int).Value = obj.EmpId;
            }
            cmd.Connection = dbcon;
            int intResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intResult;
        }
    }
}