using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PJAPP
{
    public class timePicker : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        Action<DateTime> _timeSelectHandler = delegate { };

        public static timePicker NewInstance(Action<DateTime> onTimeSelected)
        {
            timePicker fragment = new timePicker();
            fragment._timeSelectHandler = onTimeSelected;
            return fragment;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime now = DateTime.Now;
            TimePickerDialog tPicker = new TimePickerDialog(Activity, this, now.Hour, now.Minute, true);
            return tPicker;
        }
        
        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime currentTime = DateTime.Now;
            DateTime time = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 00);
            _timeSelectHandler(time);
        }

       
    }
}