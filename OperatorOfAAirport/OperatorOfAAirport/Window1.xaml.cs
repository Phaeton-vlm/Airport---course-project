using System;
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
using System.Windows.Shapes;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Page1 page1 = new Page1();
        Page2 page2 = new Page2();
        Page3 page3 = new Page3();
        Page4 page4 = new Page4();
        HelpPage5 helpPage5 = new HelpPage5();

        public Window1()
        {
            InitializeComponent();
            FrameCh.Navigate(page1);
            ChangeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Airport;
            ChangeHeader.Text = "РАСПИСАНИЕ";

            TextboxFI.Text = $"{CurrentUser.SecondName} {CurrentUser.FirstName}";
        }


        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ChangeHeader.Visibility = Visibility.Hidden;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ChangeHeader.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;

            switch(index)
            {
                case 0:
                    FrameCh.Navigate(page1);
                    ChangeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Airport;
                    ChangeHeader.Text = "РАСПИСАНИЕ";
                    break;
                case 1:
                    FrameCh.Navigate(page2);
                    ChangeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Aeroplane;
                    ChangeHeader.Text = "САМОЛЕТЫ";
                    break;
                case 2:
                    FrameCh.Navigate(page3);
                    ChangeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.UserSupervisorCircle;
                    ChangeHeader.Text = "АВИАКОМПАНИИ";
                    break;
                case 3:
                    FrameCh.Navigate(page4);
                    ChangeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.FileDocument;
                    ChangeHeader.Text = "ОТЧЕТ";
                    break;
            }

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_ClickLogout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // DragMove();
        }

        private void ButtonClick_help(object sender, RoutedEventArgs e)
        {
            FrameCh.Navigate(helpPage5);
            ChangeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.HelpOutline;
            ChangeHeader.Text = "СПРАВКА";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
