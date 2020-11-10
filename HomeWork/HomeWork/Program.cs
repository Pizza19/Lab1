using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using HomeWork.Entities;
using System.Text.RegularExpressions;
using System.Security.Principal;

namespace HomeWork
{
    class Program
    {

        static void Main(string[] args)
        {
            CheckFields checkFields = new CheckFields();
            string Email;
            string Date;
            string Login;
            string Password;
            string Phone;
            if (checkFields.IsUserRoot() == true)
            {
                Console.WriteLine("Вы админ");
            }
            //do
            //{
            //    Console.Write("Введите логин: ");
            //    Login = Console.ReadLine();
            //    Console.Write("Введите пароль: ");
            //    Password = Console.ReadLine();
            //    Console.Write("Введите E-Mail: ");
            //    Email = Console.ReadLine();
            //    Console.Write("Введите дату: ");
            //    Date = (Console.ReadLine());
            //    Console.Write("Введите номер телефона: ");
            //    Phone = (Console.ReadLine());
            //} while (checkFields.IsPhoneValid(Phone) == false);
            //Console.WriteLine("Удачно!");
            Console.ReadKey();
        }
    }
    public class CheckFields : Validator
    {
        public override string HashPassword(string password)
        {
            throw new NotImplementedException();
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
            Regex PhoneNumber = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
            MatchCollection match = PhoneNumber.Matches(phone);
            if (match.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public override bool IsUserExists(string login, string password)
        {
            var CurrentUser = AppData.Context.User.Where(c => c.Login == login && c.Password == password).FirstOrDefault();
            if (CurrentUser == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool IsUserRoot()
        {
            bool IsAdmin = false;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                IsAdmin = principal.IsInRole(WindowsBuiltInRole.);
            }
            if (IsAdmin == true)
            {
                return true;
            }
            else
            {
                return false;
            }
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
