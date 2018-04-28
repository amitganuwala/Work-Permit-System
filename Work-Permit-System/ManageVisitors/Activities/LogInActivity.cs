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
using Android.Graphics.Drawables;
using WorkPermitSystem.Models;
using Android.Support.V7.App;
using Newtonsoft.Json;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "LogInActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class LogInActivity : AppCompatActivity
    {
        Button btnLogIN, btnSignUP;
        EditText txt_UserName, txt_Password;
        ProgressDialog progressDialog;
        MyFirebaseIIDService _MyFirebaseIIDService = new MyFirebaseIIDService();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Loginlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            btnLogIN = FindViewById<Button>(Resource.Id.btnLogIn);
            btnSignUP = FindViewById<Button>(Resource.Id.btnSignUP);

            txt_UserName = FindViewById<EditText>(Resource.Id.txtUsername);
            txt_Password = FindViewById<EditText>(Resource.Id.txtPassword);


            btnLogIN.Click += BtnLogIN_Click;
            btnSignUP.Click += BtnSignUP_Click;
            IsPlayServicesAvailable();
        }

       


        private void BtnSignUP_Click(object sender, EventArgs e)
        {
            if (StatusModel.LoginUserStatus == 0)
            {
                StatusModel.LoginUserStatus = 0;
                
                Intent intent = new Intent(this, typeof(DepartmentNewRegistrationActivity));
                this.StartActivity(intent);
            }
            else if (StatusModel.LoginUserStatus == 1)
            {
               
                StatusModel.LoginUserStatus = 1;
                Intent intent = new Intent(this, typeof(VendorNewRegistrationActivity));
                this.StartActivity(intent);
            }
           
        }

        string msgText = "";
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    msgText = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                    Toast.MakeText(this, msgText, ToastLength.Short).Show();
                }
                else
                {
                    msgText = "This device is not supported";
                    Toast.MakeText(this, msgText, ToastLength.Short).Show();
                    Finish();
                }
                return false;
            }
            else
            {
                msgText = "Google Play Services is available.";
                Toast.MakeText(this, msgText, ToastLength.Short).Show();
                return true;
            }
        }

        private async void BtnLogIN_Click(object sender, EventArgs e)
        {
            try
            {
                Drawable icon_error = Resources.GetDrawable(Resource.Drawable.alert);
                icon_error.SetBounds(0, 0, 40, 30);
                if (txt_UserName.Text != "")
                {
                    if (txt_Password.Text != "")
                    {
                        UserLoginModel _objUserLoginModel = new Models.UserLoginModel();
                        StatusModel.LoginUserName = txt_UserName.Text;
                        _objUserLoginModel.UserName = txt_UserName.Text;
                        _objUserLoginModel.Password = txt_Password.Text;

                        if (StatusModel.LoginUserStatus == 0)
                        {
                            WebHelpper _objHelper = new WebHelpper();

                            string Url = StatusModel.Url + "EmployeeLogIn";

                            progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);

                            var PostString = JsonConvert.SerializeObject(_objUserLoginModel);
                            var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                            ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(requestTemp);

                            if (ResultgetRequest.success == 1)
                            {
                                _MyFirebaseIIDService.OnTokenRefresh(txt_UserName.Text.Trim(), 0);
                                clear();
                                progressDialog.Hide();
                                Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                                Intent intent = new Intent(this, typeof(CheckVendorRequestActivity));
                                this.StartActivity(intent);
                            }

                            else
                            {
                                progressDialog.Hide();
                                Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                                clear();
                                return;
                            }
                        }
                        else if (StatusModel.LoginUserStatus == 1)
                        {
                            WebHelpper _objHelper = new WebHelpper();

                            string Url = StatusModel.Url + "VendorLogIn";

                            progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);

                            var PostString = JsonConvert.SerializeObject(_objUserLoginModel);
                            var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                            ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(requestTemp);

                            if (ResultgetRequest.success == 1)
                            {
                                _MyFirebaseIIDService.OnTokenRefresh(txt_UserName.Text.Trim(), 1);
                                clear();
                                progressDialog.Hide();
                                Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                                Intent intent = new Intent(this, typeof(VendorCreateNewRequestActivity));
                                this.StartActivity(intent);
                            }

                            else
                            {
                                progressDialog.Hide();
                                Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                                clear();
                                return;
                            }

                           
                        }
                        // ADD Insert Code Here
                    }
                    else
                    {
                        txt_Password.RequestFocus();
                        txt_Password.SetError("Please Enter Password First", icon_error);
                    }
                }
                else
                {

                    txt_UserName.RequestFocus();
                    txt_UserName.SetError("Please Enter UserName First", icon_error);
                }

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }
        public void clear()
        {
            txt_Password.Text = "";
            txt_UserName.Text = "M&M";
            txt_UserName.RequestFocus();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            //
            if (StatusModel.LoginUserStatus == 1)
            {
                StatusModel.LoginUserStatus = 1;
                txt_UserName.Text = "M&M";
                SupportActionBar.SetTitle(Resource.String.VendorLogIn);
            }
            else
            {
                StatusModel.LoginUserStatus = 0;
                SupportActionBar.SetTitle(Resource.String.DepartmentLogIn);
            }
            base.OnResume();
        }
    }
}