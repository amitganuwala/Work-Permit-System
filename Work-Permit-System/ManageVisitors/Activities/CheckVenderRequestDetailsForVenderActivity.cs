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
using Android.Support.V7.App;
using WorkPermitSystem.Models;
using Newtonsoft.Json;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "CheckVendorRequestDetailsForVendorActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CheckVendorRequestDetailsForVendorActivity : AppCompatActivity
    {
        TextView tvRequestID, tvEmployeeName, tvDepartment, tvVendorNAme, tvContractor, tvNatureOfWork, tvStartTime, tvEndTime, tvNoOfPerson, tvReasons;
        ProcessRequestDetailsByRequestIDModel ResultProcessRequestDetailsByRequestIDModel;
        string ButtonAcceptOrDecline = "";
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CheckVendorRequestDetailsForVendorslayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            
            tvRequestID = FindViewById<TextView>(Resource.Id.lblViewVendorRequestID);
            tvEmployeeName = FindViewById<TextView>(Resource.Id.lblViewVendorEmployeeName);
            tvDepartment = FindViewById<TextView>(Resource.Id.lblViewVendorDepartment);
            tvVendorNAme = FindViewById<TextView>(Resource.Id.lblViewVendorVendorName);
            tvContractor = FindViewById<TextView>(Resource.Id.lblViewVendorContractor);
            tvNatureOfWork = FindViewById<TextView>(Resource.Id.lblViewVendorNatureOfWork);
            tvStartTime = FindViewById<TextView>(Resource.Id.lblViewVendorStartTime);
            tvEndTime = FindViewById<TextView>(Resource.Id.lblViewVendorEndTime);
            tvNoOfPerson = FindViewById<TextView>(Resource.Id.lblViewVendorNoofPersons);
            tvReasons = FindViewById<TextView>(Resource.Id.lblViewVendorReasons);
            
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

        public async void RequestDetailsByRequestID()
        {
            try
            {
                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);
                string Url = StatusModel.Url + "GetProcessRequestDetailsByRequestID";
                WebHelpper _objHelper = new WebHelpper();
                ProcessRequestDetailsByRequestIDModel _objProcessRequestDetailsByRequestIDModel = new ProcessRequestDetailsByRequestIDModel();

                _objProcessRequestDetailsByRequestIDModel.RequestProcessSrNo = Convert.ToInt32(StatusModel.RequestProcessSrNo);

                var PostString = JsonConvert.SerializeObject(_objProcessRequestDetailsByRequestIDModel);
                var request = await _objHelper.MakePostRequest(Url, PostString, true);

                ResultProcessRequestDetailsByRequestIDModel = JsonConvert.DeserializeObject<ProcessRequestDetailsByRequestIDModel>(request);

                tvRequestID.Text = Convert.ToString(ResultProcessRequestDetailsByRequestIDModel.RequestProcessSrNo);
                tvEmployeeName.Text = ResultProcessRequestDetailsByRequestIDModel.EmployeeName;
                tvDepartment.Text = ResultProcessRequestDetailsByRequestIDModel.EmployeeDepartmentName;
                tvVendorNAme.Text = ResultProcessRequestDetailsByRequestIDModel.VendorName;
                tvContractor.Text = ResultProcessRequestDetailsByRequestIDModel.ContractorName;
                tvNatureOfWork.Text = ResultProcessRequestDetailsByRequestIDModel.NatureOfWork;
                tvStartTime.Text = ResultProcessRequestDetailsByRequestIDModel.VisitStartTime.ToString();
                tvEndTime.Text = ResultProcessRequestDetailsByRequestIDModel.VisitEndTime.ToString();
                tvNoOfPerson.Text = ResultProcessRequestDetailsByRequestIDModel.NoOfVendors.ToString();
                tvReasons.Text = ResultProcessRequestDetailsByRequestIDModel.VendorVisitResons;
                progressDialog.Hide();
            }
            catch (Exception e)
            {
                progressDialog.Hide();
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.VendorsRequestListDetails);
            RequestDetailsByRequestID();
            base.OnResume();
        }
    }
}