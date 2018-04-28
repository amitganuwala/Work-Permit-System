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

namespace WorkPermitSystem
{
    class TimePickerFragment : DialogFragment,
                                   TimePickerDialog.IOnTimeSetListener
    {
        public static readonly string TAG = "X:" + typeof(TimePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<TimeSpan> _timeSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<TimeSpan> onDateSelected)
        {
            TimePickerFragment frag = new TimePickerFragment();
            frag._timeSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            TimePickerDialog dialog = new TimePickerDialog(Activity,
                                                           this,
                                                           currently.Hour,
                                                           currently.Minute,
                                                           false);
            return dialog;
        }

        public void OnTimeSet(TimePicker view, int hour, int minute)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            TimeSpan selectedDate = new TimeSpan(hour, minute, 0);
            //Log.Debug(TAG, selectedDate.ToString("HH:mm:ss"));
            _timeSelectedHandler(selectedDate);
        }
    }
}