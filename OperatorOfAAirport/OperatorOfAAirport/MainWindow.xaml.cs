using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace OperatorOfAAirport
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

          
        }

        //Закрытие программы
        private void Click_ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Авторизация в программе
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

            try
            {
                var entry = dboperator.ExecuteQuery<Operator>("SELECT * FROM Operator WHERE OperatorLogin COLLATE Latin1_General_CS_AS = {0} AND OperatorPassword COLLATE Latin1_General_CS_AS = {1}", LoginTextBox.Text, PasswordBox.Password).ToList();

                if (entry.Count() == 0)
                {
                    TextBlockMessage.Text = "Неверный логин или пароль, повторите попытку";
                    WrongPassOrLogin.IsOpen = true;
                }
                else
                {
                    CurrentUser.CurrentID = entry.Single().OperatorID;
                    CurrentUser.FirstName = entry.Single().FirstName;
                    CurrentUser.SecondName = entry.Single().SecondName;

                    Window1 window1 = new Window1();
                    window1.Show();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                TextBlockMessage.Text = "Не удалось подключится к базе данных";
                WrongPassOrLogin.IsOpen = true;
                return;
            }
        }

        //Регистрация
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
                MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

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
                RepitPassword.IsOpen = false;
                TextBlockMessage.Text = "Пользователь с таким логином уже зарегестрирован";
                WrongPassOrLogin.IsOpen = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ConnectionStringTextBox.Text = CurrentUser.connectionString;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentUser.ChangeConStr(_ConnectionStringTextBox.Text);
            ConnectionStringDialogHost.IsOpen = false;
        }
    }
}
