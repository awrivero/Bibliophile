﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibliophileApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Models.Admin user = new Models.Admin()
            //{
            //    UserId = 1,
            //    FirstName = "Andy",
            //    LastName = "Rivero",
            //    Age = 35,
            //    Email = "andyrivero@mail.usf.edu",
            //    UserName = "andyrivero",
            //    PassWord = Others.PasswordHasher.HashPassword("admin1234"),
            //    HireDate = DateTime.Now
            //};

            //using (var db = new Models.BibliophileContext())
            //{
            //    db.Users.Add(user);
            //    db.SaveChanges();
            //}
        }

        private void Admin_Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(new ViewModels.LoginWindowViewModel())
            {
                Owner = this
            };

            loginWindow.Closed += (sender2, e2) =>
            {
                Models.Admin admin = FindAdmin (loginWindow.viewModel.UserName, loginWindow.viewModel.Password);

                if (admin == null)
                {
                    MessageBox.Show("UserName/Password mismatch. Try again", "Error", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show($"Welcome back {admin.FirstName} {admin.LastName}");
                }
            };

            loginWindow.ShowDialog();
        }

        private void User_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private Models.Admin FindAdmin (string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return null;

            Models.Admin admin = null;

            using (var db = new Models.BibliophileContext())
            {
                var admins = (from user in db.Users
                              where user is Models.Admin
                              select user as Models.Admin).AsEnumerable();

                admin = admins.FirstOrDefault(a => a.UserName == username && Others.PasswordHasher.VerifyPassword(password, a.PassWord));
            }

            return admin;
        }
    }
}
