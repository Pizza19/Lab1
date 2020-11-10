using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace HomeWork
{
    class Program
    {

        static void Main(string[] args)
        {
            string Email;
            string Date;
            string Login;
            string Password;
            CheckFields checkFields = new CheckFields();
            do
            {
                Console.Write("Введите логин: ");
                Login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                Password = Console.ReadLine();
                Console.Write("Введите E-Mail: ");
                Email = Console.ReadLine();
                Console.Write("Введите дату: ");
                Date = (Console.ReadLine());
            } while (checkFields.IsEmailValid(Email) == false && checkFields.IsDateValid(Date) == false);
            ;
            Console.WriteLine("Удачно!");
            Console.ReadKey();
        }
    }
    public class CheckFields : Validator
    {
        public override string HashPassword(string password)
        {
            var Md5 = MD5.Create();
            var hash = Md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        public override bool IsDatabaseAccessible(string connectionString)
        {
            throw new NotImplementedException();
        }

        public override bool IsDateValid(string date)
        {
            DateTime ValidDate;
            if (DateTime.TryParse(date, out ValidDate) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool IsEmailValid(string email)
        {
            if (new EmailAddressAttribute().IsValid(email) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool IsPasswordValid(string password)
        {
            throw new NotImplementedException();
        }

        public override bool IsPhoneValid(string phone)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserExists(string login, string password)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserRoot()
        {
            throw new NotImplementedException();
        }

        public override bool IsWebPageAvailable(string url)
        {
            throw new NotImplementedException();
        }

        public override void Log()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class Validator
    {
        public abstract bool IsPasswordValid(string password);
        public abstract string HashPassword(string password);
        public abstract bool IsUserExists(string login, string password);
        public abstract bool IsEmailValid(string email);
        public abstract bool IsPhoneValid(string phone);
        public abstract bool IsWebPageAvailable(string url);
        public abstract bool IsDatabaseAccessible(string connectionString);
        public abstract bool IsDateValid(string date);
        public abstract bool IsUserRoot();
        public abstract void Log();
    }
}
