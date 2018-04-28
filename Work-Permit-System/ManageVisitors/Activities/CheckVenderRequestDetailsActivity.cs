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
    [Activity(Label = "CheckVendorRequestDetailsActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CheckVendorRequestDetailsActivity : AppCompatActivity
    {
        Button btnViewRequestAccept, btnViewRequestDecline;
        TextView tvRequestID, tvTokenNo, tvEmployeeName, tvDepartment, tvVendorNAme, tvContractor, tvNatureOfWork, tvStartTime, tvEndTime, tvNoOfPerson, tvReasons;
        ProcessRequestDetailsByRequestIDModel ResultProcessRequestDetailsByRequestIDModel;
        string ButtonAcceptOrDecline = "";
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CheckVendorRequestDetailslayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            btnViewRequestAccept = FindViewById<Button>(Resource.Id.btnViewAcceptRequest);
            btnViewRequestDecline = FindViewById<Button>(Resource.Id.btnViewDeclineRequest);

            tvRequestID = FindViewById<TextView>(Resource.Id.lblViewRequestID);
            tvTokenNo = FindViewById<TextView>(Resource.Id.lblViewTokenNumber);
            tvEmployeeName = FindViewById<TextView>(Resource.Id.lblViewEmployeeName);
            tvDepartment = FindViewById<TextView>(Resource.Id.lblViewDepartment);
            tvVendorNAme = FindViewById<TextView>(Resource.Id.lblViewVendorName);
            tvContractor = FindViewById<TextView>(Resource.Id.lblViewContractor);
            tvNatureOfWork = FindViewById<TextView>(Resource.Id.lblViewNatureOfWork);
            tvStartTime = FindViewById<TextView>(Resource.Id.lblViewStartTime);
            tvEndTime = FindViewById<TextView>(Resource.Id.lblViewEndTime);
            tvNoOfPerson = FindViewById<TextView>(Resource.Id.lblViewNoofPersons);
            tvReasons = FindViewById<TextView>(Resource.Id.lblViewReasons);


            btnViewRequestAccept.Click += BtnViewRequestAccept_Click;
            btnViewRequestDecline.Click += BtnViewRequestDecline_Click;
        }

        private void BtnViewRequestDecline_Click(object sender, EventArgs e)
        {
            ButtonAcceptOrDecline = "Decline";
            StatusUpdate();
        }

        private void BtnViewRequestAccept_Click(object sender, EventArgs e)
        {
            ButtonAcceptOrDecline = "Accept";
            StatusUpdate();
        }


        public async void StatusUpdate()
        {
            try
            {
                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);

                tbl_RequestProcess _objRequestProcessModel = new tbl_RequestProcess();

                _objRequestProcessModel.EmployeeId = ResultProcessRequestDetailsByRequestIDModel.EmployeeId;
                _objRequestProcessModel.VendorSrNo = ResultProcessRequestDetailsByRequestIDModel.VendorSrNo;
                _objRequestProcessModel.RequestProcessSrNo = ResultProcessRequestDetailsByRequestIDModel.RequestProcessSrNo;
                _objRequestProcessModel.EmployeeDepartmentID = ResultProcessRequestDetailsByRequestIDModel.EmployeeDepartmentID;
                _objRequestProcessModel.VisitStartTime = ResultProcessRequestDetailsByRequestIDModel.VisitStartTime;
                _objRequestProcessModel.VisitEndTime = ResultProcessRequestDetailsByRequestIDModel.VisitEndTime;
                _objRequestProcessModel.VendorAccessories = ResultProcessRequestDetailsByRequestIDModel.VendorAccessories;
                _objRequestProcessModel.NoOfVendors = ResultProcessRequestDetailsByRequestIDModel.NoOfVendors;
                _objRequestProcessModel.VendorVisitResons = ResultProcessRequestDetailsByRequestIDModel.VendorVisitResons;
                _objRequestProcessModel.RequestProcessDate = ResultProcessRequestDetailsByRequestIDModel.RequestProcessDate;

                if (ButtonAcceptOrDecline == "Decline")
                {
                    if (StatusModel.EmployeeDesignationName == "Activity Owner")
                    {
                        _objRequestProcessModel.ActivityOwnerStatus = "Decline";
                    }
                    else
                    {
                        _objRequestProcessModel.ActivityOwnerStatus = ResultProcessRequestDetailsByRequestIDModel.ActivityOwnerStatus;
                    }

                    if (StatusModel.EmployeeDesignationName == "Area Owner")
                    {
                        _objRequestProcessModel.AreaOwnerStatus = "Decline";
                    }
                    else
                    {
                        _objRequestProcessModel.AreaOwnerStatus = ResultProcessRequestDetailsByRequestIDModel.AreaOwnerStatus;
                    }

                    if (StatusModel.EmployeeDesignationName == "Safety")
                    {
                        _objRequestProcessModel.SafetyStatus = "Decline";
                    }
                    else
                    {
                        _objRequestProcessModel.SafetyStatus = ResultProcessRequestDetailsByRequestIDModel.SafetyStatus;
                    }

                    if (StatusModel.EmployeeDesignationName == "Contractor")
                    {
                        _objRequestProcessModel.ContractorStatus = "Decline";
                    }
                    else
                    {
                        _objRequestProcessModel.ContractorStatus = ResultProcessRequestDetailsByRequestIDModel.ContractorStatus;
                    }
                }
                else
                {
                    if (StatusModel.EmployeeDesignationName == "Activity Owner")
                    {
                        _objRequestProcessModel.ActivityOwnerStatus = "Accepted";
                    }
                    else
                    {
                        _objRequestProcessModel.ActivityOwnerStatus = ResultProcessRequestDetailsByRequestIDModel.ActivityOwnerStatus;
                    }

                    if (StatusModel.EmployeeDesignationName == "Area Owner")
                    {
                        _objRequestProcessModel.AreaOwnerStatus = "Accepted";
                    }
                    else
                    {
                        _objRequestProcessModel.AreaOwnerStatus = ResultProcessRequestDetailsByRequestIDModel.AreaOwnerStatus;
                    }

                    if (StatusModel.EmployeeDesignationName == "Safety")
                    {
                        _objRequestProcessModel.SafetyStatus = "Accepted";
                    }
                    else
                    {
                        _objRequestProcessModel.SafetyStatus = ResultProcessRequestDetailsByRequestIDModel.SafetyStatus;
                    }

                    if (StatusModel.EmployeeDesignationName == "Contractor")
                    {
                        _objRequestProcessModel.ContractorStatus = "Accepted";
                    }
                    else
                    {
                        _objRequestProcessModel.ContractorStatus = ResultProcessRequestDetailsByRequestIDModel.ContractorStatus;
                    }
                }
               
               
                
                string Url = StatusModel.Url + "ManageProcessRequestStatusUpdate";
                WebHelpper _objHelper = new WebHelpper();

                var PostString = JsonConvert.SerializeObject(_objRequestProcessModel);
                var request = await _objHelper.MakePostRequest(Url, PostString, true);

                ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(request);

                if (ResultgetRequest.success == 1)
                {
                    progressDialog.Hide();
                    Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                }

                else
                {
                    progressDialog.Hide();
                    Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                progressDialog.Hide();
                string ErrorMsg = ex.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
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
                tvTokenNo.Text = ResultProcessRequestDetailsByRequestIDModel.EmployeeTokenNo;
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