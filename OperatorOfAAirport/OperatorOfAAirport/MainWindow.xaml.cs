using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();

           

        }

        private void Click_ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {

            MyDataContext dboperator = new MyDataContext(connectionString);
           // var entry = dboperator.GetTable<Operator>().Where(op => op.Login == LoginTextBox.Text).Where(op => op.Password == PasswordBox.Password);

            //foreach (var item in entry)
            //{

            //}

            var entry = dboperator.ExecuteQuery<Operator>("SELECT * FROM Operator WHERE OperatorLogin COLLATE Latin1_General_CS_AS = {0} AND OperatorPassword COLLATE Latin1_General_CS_AS = {1}", LoginTextBox.Text,PasswordBox.Password).ToList();
          

            if (entry.Count() == 0)
            {
                TextBlockMessage.Text = "Неверный логин или пароль, повторите попытку";
                WrongPassOrLogin.IsOpen = true;
            }
            else
            {
                CurrentUser.CurrentID = entry.Single().OperatorID;
                Window1 window1 = new Window1();
                window1.Show();
                this.Close();
            }
        }

        private void ButtonRegestry(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (_TextBoxFirstName.Text.Length > 0 && _TextBoxSecondName.Text.Length > 0 && _TextBoxLoginReg.Text.Length > 0 && _TextBoxPasswordReg.Password.Length > 0)
                {
                    if (_TextBoxPasswordReg.Password.Length >= 5)
                    {
                        TextBlockChangedPass.Text = "Повторите пароль";
                        TextBlockChangedPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1976D2"));
                        _RepitPasswordBox.Clear();
                        RepitPassword.IsOpen = true;
                    }
                    else
                    {
                        TextBlockMessage.Text = "Длина пароля должна быть больше 5 символов";
                        WrongPassOrLogin.IsOpen = true;
                    }
                }
                else
                {
                    TextBlockMessage.Text = "Заполните все обязательные поля";
                    WrongPassOrLogin.IsOpen = true;
                }
            }
            catch(Exception ex)
            {
                TextBlockMessage.Text = $"Ошибка: {ex.Message}, Источник: {ex.Source}";
                WrongPassOrLogin.IsOpen = true;
            }
        }

        private void ContinueReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyDataContext dboperator = new MyDataContext(connectionString);

                if (_RepitPasswordBox.Password == _TextBoxPasswordReg.Password)
                {
                    Operator operatornew = new Operator { FirstName = _TextBoxFirstName.Text, SecondName = _TextBoxSecondName.Text, Login = _TextBoxLoginReg.Text, Password = _TextBoxPasswordReg.Password, MiddleName = _TextBoxMiddleName.Text.Length == 0 ? null : _TextBoxMiddleName.Text };
                    dboperator.GetTable<Operator>().InsertOnSubmit(operatornew);
                    dboperator.SubmitChanges();

                    RepitPassword.IsOpen = false;
                    TextBlockMessage.Text = "Регистрация прошла успешно";
                    WrongPassOrLogin.IsOpen = true;

                    _TextBoxFirstName.Clear();
                    _TextBoxSecondName.Clear();
                    _TextBoxMiddleName.Clear();
                    _TextBoxLoginReg.Clear();
                    _TextBoxPasswordReg.Clear();
                    FlipperAcc.IsFlipped = false;
                }
                else
                {
                    TextBlockChangedPass.Text = "Неверный пароль";
                }
            }
            catch (Exception)
            {
                //TextBlockMessage.Text = $"Ошибка: {ex.Message}, Источник: {ex.Source}";
                RepitPassword.IsOpen = false;
                TextBlockMessage.Text = "Пользователь с таким логином уже зарегестрирован";
                WrongPassOrLogin.IsOpen = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
