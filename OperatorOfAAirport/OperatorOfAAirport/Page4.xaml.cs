using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();

            CurrentUser.GetOperators(ref _ComboBoxNames);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           // if (ButtonCheked.IsChecked == true) { AllOperators(); return; }
           // if (ButtonCheked.IsChecked == false) { CurrentOperators(); return; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            /*Word.Application word = new Word.Application(); 
            Word.Document doc = word.Documents.Add(Visible: true);
            Word.Range range = doc.Range();

            //doc.Select();

            // word.Selection.TypeText("Hello, World!");
            Word.Table table = doc.Tables.Add(range, DataGridAll.Items.Count+1, DataGridAll.Columns.Count);
            table.Borders.Enable = 1;

            int i = 0;
            //word.Selection.Tables[1].Rows.Aligment = Word.WdRowAlignment.wdAlignRowCenter;


            foreach (Word.Row row in table.Rows)
            {
                if (row.Index == 1)
                {
                  
                    row.Cells[1].Range.Text = "Рейс";
                    row.Cells[2].Range.Text = "Авиакомпания";
                    row.Cells[3].Range.Text = "Вылет";
                    row.Cells[4].Range.Text = "Направление";
                    row.Cells[5].Range.Text = "Время вылета";
                    row.Cells[6].Range.Text = "Время прилета";
                    row.Cells[7].Range.Text = "Самолет";
                    row.Cells[8].Range.Text = "Номер";
                    row.Cells[9].Range.Text = "Имя оператора";
                    row.Cells[10].Range.Text = "Фамилия оператора";

                }
                else
                {
                    dynamic items = DataGridAll.Items[i];

                    row.Cells[1].Range.Text = items.FlightNumber.ToString();
                    row.Cells[2].Range.Text = items.AirlineName.ToString();
                    row.Cells[3].Range.Text = items.DepatureCity.ToString();
                    row.Cells[4].Range.Text = items.ArrivalCity.ToString();
                    row.Cells[5].Range.Text = items.DepartureTime.ToString();
                    row.Cells[6].Range.Text = items.ArrivalTime.ToString();
                    row.Cells[7].Range.Text = items.AircraftModel.ToString();
                    row.Cells[8].Range.Text = items.SideNumber.ToString();
                    row.Cells[9].Range.Text = items.FirstName.ToString();
                    row.Cells[10].Range.Text = items.SecondName.ToString();

                    i++;
                }
                row.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            }

            foreach (Word.Row row in table.Rows)
            {
                foreach (Word.Cell cell in row.Cells)
                {
                    if(cell.RowIndex == 1)
                    {
                        cell.Range.Bold = 1;
                        cell.Range.Font.Size = 12;
                    }


                    cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    //cell.Width = ;
                }
            }


            try
            {
                doc.Save();
                doc.Close();
                word.Quit();
                return;
            }
            catch(Exception)
            {
                word.Quit(SaveChanges: null);
                return;
            }*/

            try
            {
                Excel.Application application = new Excel.Application();
                application.Application.Workbooks.Add(Type.Missing);
            

                for (int i = 1; i < DataGridAll.Columns.Count + 1; i++)
                {
                    application.Cells[1, i] = DataGridAll.Columns[i - 1].Header.ToString();
                    application.Cells[1, i].Font.Bold = true;
                }

                //application.Rows[1].Style

                for (int i = 0; i < DataGridAll.Items.Count; i++)
                {
                    int j = 0;
                    dynamic items = DataGridAll.Items[i];

                    application.Cells[i + 2, 1] = items.FlightNumber.ToString();
                    application.Cells[i + 2, 2] = items.AirlineName.ToString();
                    application.Cells[i + 2, 3] = items.DepatureCity.ToString();
                    application.Cells[i + 2, 4] = items.ArrivalCity.ToString();
                    application.Cells[i + 2, 5] = items.DepartureTime.ToString();
                    application.Cells[i + 2, 6] = items.ArrivalTime.ToString();
                    application.Cells[i + 2, 7] = items.AircraftModel.ToString();
                    application.Cells[i + 2, 8] = items.SideNumber.ToString();
                    application.Cells[i + 2, 9] = items.FirstName.ToString();
                    application.Cells[i + 2, 10] = items.SecondName.ToString();
                    application.Cells[i + 2, 11] = items.Login.ToString();

                    application.Rows[i + 1].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                }

                application.Columns.AutoFit();
                application.Visible = true;

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }          

        }

        private void ButtonCheked_Checked(object sender, RoutedEventArgs e)
        {
            AllOperators();
        }

        private void ButtonCheked_Unchecked(object sender, RoutedEventArgs e)
        {
            CurrentOperators();
        }

        void AllOperators()
        {
            DataGridAll.Items.Clear();
            MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

            var report = from fl in dboperator.flights
                         join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                         join aircrf in dboperator.aircrafts on fl.AircraftID equals aircrf.AircraftID
                         join op in dboperator.operators on fl.OperatorID equals op.OperatorID
                         select new
                         {
                             fl.ArrivalCity,
                             fl.ArrivalTime,
                             fl.DepartureTime,
                             fl.DepatureCity,
                             fl.FlightNumber,
                             airc.AirlineName,
                             aircrf.AircraftModel,
                             aircrf.SideNumber,
                             op.FirstName,
                             op.SecondName
                         };

            foreach (var item in report)
            {
                DataGridAll.Items.Add(item);
            }
        }

        void CurrentOperators()
        {
            DataGridAll.Items.Clear();
            MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

            var report = from fl in dboperator.flights
                         join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                         join aircrf in dboperator.aircrafts on fl.AircraftID equals aircrf.AircraftID
                         join op in dboperator.operators on fl.OperatorID equals op.OperatorID
                         where op.OperatorID == CurrentUser.CurrentID
                         select new
                         {
                             fl.ArrivalCity,
                             fl.ArrivalTime,
                             fl.DepartureTime,
                             fl.DepatureCity,
                             fl.FlightNumber,
                             airc.AirlineName,
                             aircrf.AircraftModel,
                             aircrf.SideNumber,
                             op.FirstName,
                             op.SecondName
                         };

            foreach (var item in report)
            {
                DataGridAll.Items.Add(item);
            }
        }

        private void _CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _ComboBoxNames.IsEnabled = false;
        }

        private void _CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _ComboBoxNames.IsEnabled = true;

        }

        private void Button_Click_CreateReport(object sender, RoutedEventArgs e)
        {
            DateTime res;

            if (DateTime.TryParse(_DataPiker_First.Text, out res) && DateTime.TryParse(_DataPiker_Second.Text, out res))
            {
                _CreateReportErrorMessage.Visibility = Visibility.Hidden;
                DataGridAll.Items.Clear();
                MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

                if (_CheckBoxAllOperators.IsChecked == true)
                {
                    var report = from fl in dboperator.flights
                                 join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                                 join aircrf in dboperator.aircrafts on fl.AircraftID equals aircrf.AircraftID
                                 join op in dboperator.operators on fl.OperatorID equals op.OperatorID
                                 where fl.DepartureTime >= _DataPiker_First.SelectedDate && fl.DepartureTime <= _DataPiker_Second.SelectedDate
                                 select new
                                 {
                                     fl.ArrivalCity,
                                     fl.ArrivalTime,
                                     fl.DepartureTime,
                                     fl.DepatureCity,
                                     fl.FlightNumber,
                                     airc.AirlineName,
                                     aircrf.AircraftModel,
                                     aircrf.SideNumber,
                                     op.FirstName,
                                     op.SecondName,
                                     op.Login
                                 };

                    foreach (var item in report)
                    {
                        DataGridAll.Items.Add(item);
                    }
                }
                else if(_CheckBoxAllOperators.IsChecked == false)
                {
                    var report = from fl in dboperator.flights
                                 join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                                 join aircrf in dboperator.aircrafts on fl.AircraftID equals aircrf.AircraftID
                                 join op in dboperator.operators on fl.OperatorID equals op.OperatorID
                                 where fl.DepartureTime >= _DataPiker_First.SelectedDate && fl.DepartureTime <= _DataPiker_Second.SelectedDate && op.Login == _ComboBoxNames.SelectedItem.ToString()
                                   select new
                                 {
                                     fl.ArrivalCity,
                                     fl.ArrivalTime,
                                     fl.DepartureTime,
                                     fl.DepatureCity,
                                     fl.FlightNumber,
                                     airc.AirlineName,
                                     aircrf.AircraftModel,
                                     aircrf.SideNumber,
                                     op.FirstName,
                                     op.SecondName,
                                     op.Login
                                 };

                    foreach (var item in report)
                    {
                        DataGridAll.Items.Add(item);
                    }
                }

            }
            else
            {
                _CreateReportErrorMessage.Text = "Заполните все поля";
                _CreateReportErrorMessage.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _DataPiker_First.Text = "";
            _DataPiker_Second.Text = "";
        }
    }
}
