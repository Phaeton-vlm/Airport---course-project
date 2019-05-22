using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Data.Linq;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using MaterialDesignThemes.Wpf;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";


        public Page4()
        {
            InitializeComponent();                     
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ButtonCheked.IsChecked == true) { AllOperators(); return; }
            if (ButtonCheked.IsChecked == false) { CurrentOperators(); return; }
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

            Excel.Application application = new Excel.Application();
            application.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < DataGridAll.Columns.Count+ 1; i++)
            {
                application.Cells[1, i] = DataGridAll.Columns[i - 1].Header.ToString();
                application.Cells[1, i].Font.Bold = true;
            }

            //application.Rows[1].Style

            for (int i = 0; i < DataGridAll.Items.Count ; i++)
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

                application.Rows[i+1].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

            }

            application.Columns.AutoFit();
           
            application.Visible = true;
            


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
            MyDataContext dboperator = new MyDataContext(connectionString);

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
            MyDataContext dboperator = new MyDataContext(connectionString);

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

       
    }
}
