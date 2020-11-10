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
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

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
            string WebSite;
            string ConnectionString;
            do
            {
                Console.Write("Введите строку подключения: ");
                ConnectionString = Console.ReadLine();
                Console.Write("Введите логин: ");
                Login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                Password = Console.ReadLine();
                Console.Write("Введите дату рождения: ");
                Date = Console.ReadLine();
                Console.Write("Введите номер телефона: ");
                Phone = Console.ReadLine();
                Console.Write("Введите E-Mail: ");
                Email = Console.ReadLine();
                Console.Write("Введите веб-сайт: ");
                WebSite = Console.ReadLine();

                if (checkFields.IsDatabaseAccessible(ConnectionString) == true)
                {
                    if (checkFields.IsUserExists(Login, checkFields.HashPassword(Password)) == true)
                    {
                        if (checkFields.IsDateValid(Date) == true)
                        {
                            if (checkFields.IsPhoneValid(Phone) == true)
                            {
                                if (checkFields.IsEmailValid(Email) == true)
                                {
                                    if (checkFields.IsUserRoot() == true)
                                    {
                                        if (checkFields.IsWebPageAvailable(WebSite) == true)
                                        {
                                            if (checkFields.IsPasswordValid(Password) == true)
                                            {
                                                User CurrentUser = new User()
                                                {
                                                    Login = Login,
                                                    Password = checkFields.HashPassword(Password),
                                                    Email = Email,
                                                    Phone = Phone,
                                                    DateOfBirth = Convert.ToDateTime(Date),
                                                };
                                                AppData.Context.User.Add(CurrentUser);
                                                AppData.Context.SaveChanges();
                                                Console.WriteLine("Пользователь успешно добавлен!");
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Пароль не соответствует формату!");
                                            } 
                                        }
                                        else
                                        {
                                            Console.WriteLine("Веб-сайт указан некорректно!");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Вы не обладаете правами администратора!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("E-Mail указан некоррктно!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Телефон указан некорректно!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Дата указана некорректно!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Пользователь с таким логином и паролем существует!");
                    }
                }
                else
                {
                    Console.WriteLine("Отсутствует подключение к базе данных!");
                }

            } while (true);

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
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
           
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
            string symbols = "!№;%:?*()_+=-";
            bool IsSpecSymbols = false;
            bool IsWhiteSpace = false;
            bool IsDate = false;
            foreach (var item in password)
            {
                if (!symbols.Contains(item))
                {
                    IsSpecSymbols = true;
                }
                else
                {
                    IsSpecSymbols = false;
                } 
            }

            foreach (var item in password)
            {
                if (!Char.IsWhiteSpace(item))
                {
                    IsWhiteSpace = true;
                }
                else
                {
                    IsWhiteSpace = false;
                }
            }

            Regex Date = new Regex(@"\d\d[.]\d\d[.]\d\d\d\d");
            MatchCollection match = Date.Matches(password);
            if (match.Count > 0)
            {
                IsDate = false;
            }
            else
            {
                IsDate = true;
            }

            if (password.Length > 22 && password.Length < 25)
            {
                if (IsSpecSymbols == true && IsWhiteSpace == true && IsDate == true)
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
            else
            {
                return false;
            } 
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
                IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
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
            return Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");

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
