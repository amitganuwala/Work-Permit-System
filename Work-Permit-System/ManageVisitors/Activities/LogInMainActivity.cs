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
using WorkPermitSystem.Models;
using System.Threading.Tasks;
using Firebase.Iid;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "WPS", MainLauncher = true, Icon = "@drawable/mahindralogo", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LogInMainActivity : Activity
    {
        Button btnDepartmentLogIn, btnVendorLogIn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LogInMainlayout);

            btnDepartmentLogIn = FindViewById<Button>(Resource.Id.btnDepartmentLogIn);
            btnVendorLogIn = FindViewById<Button>(Resource.Id.btnVendorLogIn);

            btnDepartmentLogIn.Click += BtnDepartmentLogIn_Click;
            btnVendorLogIn.Click += BtnVendorLogIn_Click;
        }

        private void BtnVendorLogIn_Click(object sender, EventArgs e)
        {
            StatusModel.LoginUserStatus = 1;
            Intent intent = new Intent(this, typeof(LogInActivity));
            this.StartActivity(intent);
        }

        private void BtnDepartmentLogIn_Click(object sender, EventArgs e)
        {
            StatusModel.LoginUserStatus = 0;
            Intent intent = new Intent(this, typeof(LogInActivity));
            this.StartActivity(intent);
        }
    }
}