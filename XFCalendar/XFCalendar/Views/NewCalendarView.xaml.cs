using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFCalendar.Model;

namespace XFCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCalendarView : Frame
    {
        int _year;
        int _month;
        CalendarViewType calendarViewType;
        DateView lastSaleted;
        List<Year> years = new List<Year>();
        List<DateView> dates = new List<DateView>();
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        public NewCalendarView()
        {
            InitializeComponent();

            _year = DateTime.Now.Year;
            _month = DateTime.Now.Month;
            calendarViewType = CalendarViewType.Dates;

            initMonthGrid();
            initDatesGrid();
            initYears();
            yearsCollectioview.ItemsSource = years;
            MapDatesForMonth(new DateTime(_year, _month, 1));

        }

        void initYears()
        {
            for (int i = 1900; i <= 2100; i++)
            {
                int fontSize = 18;
                string color = "Gray";
                if (i == _year)
                {
                    fontSize = 23;
                    color = "#AB4A5A";
                }
                years.Add(new Year() { year = i, yearColor = color, fontSize = fontSize });
            }
        }

        void initMonthGrid()
        {
            var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };

            MonthGrid.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef };
            MonthGrid.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef };

            SetupMonthTemplate();
        }

        void initDatesGrid()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)};
                var rowDef = new RowDefinition { Height = new GridLength(45) };

                DatesGrid.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef, columDef, columDef, columDef };
                DatesGrid.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef, rowDef, rowDef, rowDef };
                DatesGrid.RowSpacing = 2;

                DatesGrid.Children.Add(new Label() { Text = "Su", TextColor = Color.Red, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 0, 0);
                DatesGrid.Children.Add(new Label() { Text = "Mo", TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 1, 0);
                DatesGrid.Children.Add(new Label() { Text = "Tu", TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 2, 0);
                DatesGrid.Children.Add(new Label() { Text = "We", TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 3, 0);
                DatesGrid.Children.Add(new Label() { Text = "Th", TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 4, 0);
                DatesGrid.Children.Add(new Label() { Text = "Fr", TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 5, 0);
                DatesGrid.Children.Add(new Label() { Text = "Sa", TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 6, 0);

                tapGestureRecognizer.Tapped += DateCliekd_Tapped;
                for (int r = 1; r < 7; r++)
                {
                    for (int c = 0; c < 7; c++)
                    {
                        var dateView= new DateView
                        {
                            //BorderColor = Color.FromHex("#f0f0f0"),
                            //BackgroundColor = Color.White,
                            //HeightRequest= 45,
                            //WidthRequest= 45,
                        };
                        dateView.GestureRecognizers.Add(tapGestureRecognizer);
                        //bnt.Clicked += DateClickedEvent;
                        DatesGrid.Children.Add(dateView, c, r);
                        dates.Add(dateView);
                    }
                }
            });
        }

        private void DateCliekd_Tapped(object sender, EventArgs e)
        {

            (sender as DateView).BackgroundColor = Color.FromHex("#2627549c");
            if (lastSaleted == null)
                lastSaleted = (sender as DateView);
            else if(lastSaleted != (sender as DateView))
            {
                lastSaleted.BackgroundColor = Color.Transparent;
                lastSaleted = (sender as DateView);
            }
        }

        void SetupMonthTemplate()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    var b = new Button
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Text = DateTimeFormatInfo.CurrentInfo.MonthNames[(r * 3) + c].Substring(0, 3),
                        BackgroundColor = Color.White,
                        TextColor = Color.Black,
                        FontSize = 18,
                        BorderWidth = 1,
                        BorderColor = Color.WhiteSmoke,
                    };
                    b.Clicked += Month_Clicked;
                    MonthGrid.Children.Add(b, c, r);
                }
            }
            //this.Content = details;
        }

        private void Month_Clicked(object sender, EventArgs e)
        {
            DatesGrid.IsVisible = true;
            MonthGrid.IsVisible = false;
            calendarViewType = CalendarViewType.Dates;
            leftArrow.IsVisible = rightArrow.IsVisible = true;

            _month = monthToInt((sender as Button).Text);
            HeaderLbl.Text = $"{DateTimeFormatInfo.CurrentInfo.MonthNames[_month - 1]}, {_year}";
            var firstDateForSelectedMonth = new DateTime(_year, _month, 1);

            MapDatesForMonth(firstDateForSelectedMonth);
        }

        void MapDatesForMonth(DateTime firstDayOfMonth)
        {
            HeaderLbl.FadeTo(0, 4000, Easing.Linear);
            DatesGrid.FadeTo(0, 4000, Easing.Linear);
            Task.Delay(5000);
            Device.BeginInvokeOnMainThread(() =>
            {
                var dayOfWeek = (int)firstDayOfMonth.DayOfWeek;

                int offset = 0;
                //for start day is sunday
                if (dayOfWeek > 0)
                    offset = dayOfWeek;
                var loopStrtDate = firstDayOfMonth.AddDays(-offset);

                int count = 0;
                for (int r = 1; r < 7; r++)
                {
                    for (int c = 0; c < 7; c++)
                    {
                        dates[count++].Date = (loopStrtDate.Day).ToString();
                        loopStrtDate = loopStrtDate.AddDays(+1);
                    }
                }
                Task.Delay(3000);

            });
            HeaderLbl.FadeTo(1, 3000, Easing.Linear);
            DatesGrid.FadeTo(1, 3000, Easing.Linear);
        }

        int monthToInt(string monthStr)
        {
            List<string> months = new List<string>()
            {
                "january",
                 "february",
                 "march",
                 "april",
                 "may",
                 "june",
                 "july",
                 "august",
                 "september",
                 "october",
                 "november",
                 "december",
            };
            var m = months.Find(x => x.StartsWith(monthStr.ToLower()));
            return months.IndexOf(m) + 1;
        }

        private void leftArrow_Tapped(object sender, EventArgs e)
        {
            if (_month > 1)
                _month--;
            else
            {
                _month = 12;
                _year--;
            }
            HeaderLbl.Text = $"{DateTimeFormatInfo.CurrentInfo.MonthNames[_month - 1]}, { _year}";
            var firstDateForSelectedMonth = new DateTime(_year, _month, 1);
            MapDatesForMonth(firstDateForSelectedMonth);
        }

        private void RightArrow_Tapped(object sender, EventArgs e)
        {
            if (_month < 12)
                _month++;
            else
            {
                _month = 1;
                _year++;
            }
            HeaderLbl.Text = $"{DateTimeFormatInfo.CurrentInfo.MonthNames[_month - 1]}, {_year}";
            var firstDateForSelectedMonth = new DateTime(_year, _month, 1);
            MapDatesForMonth(firstDateForSelectedMonth);
        }

        private void Header_Tapped(object sender, EventArgs e)
        {
            if (calendarViewType == CalendarViewType.Dates)
            {
                MonthGrid.IsVisible = true;
                DatesGrid.IsVisible = false;
                leftArrow.IsVisible = false;
                rightArrow.IsVisible = false;
                calendarViewType = CalendarViewType.Months;

                HeaderLbl.Text = $"{_year}";
            }
            else if (calendarViewType == CalendarViewType.Months)
            {
                YearsGrid.IsVisible = true;
                MonthGrid.IsVisible = false;
                calendarViewType = CalendarViewType.Years;

                HeaderLbl.Text = $"Years";
                yearsCollectioview.ScrollTo(years.Find(x => x.year == _year));
            }
        }

        private void animation()
        {
            HeaderLbl.FadeTo(0.5, 25);
            DatesGrid.FadeTo(0.5, 25);

            HeaderLbl.FadeTo(1, 25);
            DatesGrid.FadeTo(1, 25);
        }

        private void Year_selectionChanegd(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
                _year = (e.CurrentSelection[0] as Year).year;
            YearsGrid.IsVisible = false;
            MonthGrid.IsVisible = true;
            calendarViewType = CalendarViewType.Months;
        }
   
    }
}