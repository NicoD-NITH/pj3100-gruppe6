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
    public class datePicker : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        Action<DateTime> _dateSelectHandler = delegate { };

        public static datePicker NewInstance(Action<DateTime> onDateSelected)
        {
            datePicker fragment = new datePicker();
            fragment._dateSelectHandler = onDateSelected;
            return fragment;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime now = DateTime.Now;
            DatePickerDialog dPicker = new DatePickerDialog(Activity, this, now.Year, now.Month, now.Day);
            return dPicker;
        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            DateTime date = new DateTime(year, monthOfYear, dayOfMonth);
            _dateSelectHandler(date);
        }
    }
}