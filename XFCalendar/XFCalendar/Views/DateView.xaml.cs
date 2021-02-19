using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateView : Frame
    {
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(string), typeof(DateView), default(string), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnDateChanged);

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public DateView()
        {
            InitializeComponent();
        }

        private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateView).Date = (string)newValue;
        }
    }
}