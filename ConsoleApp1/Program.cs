﻿using System;
using System.Data;
using System.Data.SQLite;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DBMethods dbmethod = new DBMethods();
            //new comment
            Console.WriteLine("Please enter the command. 1 - to SELECT, 2 - to INSERT 3 - to UPDATE 4 - to DELETE  5 - Select book with author ");
            int userCommand = Convert.ToInt32(Console.ReadLine());
            switch (userCommand)
            {
                case 1:
                    dbmethod.Select();
                    break;
                case 2:
                    dbmethod.Insert();
                    break;
                case 3:
                    dbmethod.Update();
                    break;
                case 4:
                    dbmethod.Delete();
                    break;
                case 5:
                    dbmethod.SelectFromBothTables();
                    break;
            }

        }
    }
    class DBMethods
    {
        private string connectionString = @"Data source = C:\Users\Натали\Documents\Author_Book1.db";
        public void Select()
        {
            using var con = new SQLiteConnection(connectionString);
            con.Open();
            string stm = "SELECT * FROM BookNew";
            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            Console.WriteLine($"{rdr.GetName(0)} {rdr.GetName(1)} {rdr.GetName(2)}");
            while (rdr.Read())
            {
                Console.WriteLine($@"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetString(2)}");
            }
        }
        public void Insert()
        {
            using var con = new SQLiteConnection(connectionString);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            Console.WriteLine("Please enter name of the book");
            string nameInput = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Year of edition");
            int date = Convert.ToInt32(Console.ReadLine());
            cmd.CommandText = "INSERT INTO BookNew(name, yearOfEdition) VALUES(@name, @yearOfEdition)";
            cmd.Parameters.AddWithValue("@name", nameInput);
            cmd.Parameters.AddWithValue("@yearOfEdition", date);
            cmd.ExecuteNonQuery();
            Console.WriteLine("row inserted");
        }
        public void Update()
        {
            using var con = new SQLiteConnection(connectionString);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            Console.WriteLine("Please enter book id to update");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter new name");
            string name = Console.ReadLine();
            Console.WriteLine("Please enter new date");
            int date = Convert.ToInt32(Console.ReadLine());
            cmd.CommandText = String.Format("UPDATE BookNew SET name='{0}', yearOfEdition = '{1}' WHERE id={2}", name, date, id);
            int number = cmd.ExecuteNonQuery();
            Console.WriteLine("Обновлено объектов: {0}", number);
        }
        public void Delete()
        {
            using var con = new SQLiteConnection(connectionString);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            Console.WriteLine("Please enter book id to delete");
            int id = Convert.ToInt32(Console.ReadLine());
            cmd.CommandText = String.Format("DELETE  FROM BookNew WHERE id='{0}'", id);
            int number = cmd.ExecuteNonQuery();
            Console.WriteLine("Удалено объектов: {0}", number);
        }
        public void SelectFromBothTables()
        {
            using var con = new SQLiteConnection(connectionString);
            con.Open();
            string stm = "SELECT * FROM BookNew b LEFT JOIN auth_book ab on b.id = ab.books_id LEFT JOIN Author a on auth_id =  author_id";
            using var cmd = new SQLiteCommand(stm, con);
            try
            {
                using SQLiteDataReader rdr = cmd.ExecuteReader();
                int fieldCount = rdr.FieldCount;
                Console.WriteLine($"{rdr.GetName(0)} {rdr.GetName(1)} {rdr.GetName(2)}");
                Console.WriteLine($"{rdr.GetName(6)} {rdr.GetName(7)} ");

                for (int i = 0; i < fieldCount; i++)
                {
                    while (rdr.Read())
                    {
                     Console.WriteLine($@"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetString(2)} ");
                     Console.WriteLine($@"{rdr.GetString(6)} {rdr.GetString(7)}");
                        
                    }
                }
            }
            catch (InvalidCastException e) //here is a line to catch exception 
            {
                Console.WriteLine(e);
            }
            finally
            {
                con.Close();
            }
        }
    }
}



          
 

