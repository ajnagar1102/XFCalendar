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
    public partial class DragToSelect : ContentPage
    {
        public DragToSelect()
        {
            InitializeComponent();
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch(e.StatusType)
                {
                case GestureStatus.Started:
                    break;

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    //Content.TranslationX =
                    //  Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(Content.Width - App.ScreenWidth));
                    //Content.TranslationY =
                    //  Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - App.ScreenHeight));
                    mybox.HeightRequest = mybox.Height + e.TotalX;
                    break;

                case GestureStatus.Completed:
                    break;

            }
        }
    }
}