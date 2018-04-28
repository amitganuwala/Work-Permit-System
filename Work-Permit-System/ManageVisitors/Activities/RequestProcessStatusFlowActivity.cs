using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Firebase.Messaging;
using Firebase.Iid;
using System.Threading.Tasks;
using WorkPermitSystem.Models;
using Newtonsoft.Json;
using System;
using Android.Nfc;
using Android.Views;
using Android.Content;
using Android.Preferences;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "RequestProcessStatusFlowActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RequestProcessStatusFlowActivity : AppCompatActivity
    {
        TextView tvActivityOwner, tvAreaOwner, tvSafety, tvContractor;
        ProcessRequestDetailsByRequestIDModel ResultProcessRequestDetailsByRequestIDModel;
        string ButtonAcceptOrDecline = "";
        ProgressDialog progressDialog;
        Button btnViewRequestAccept, btnViewRequestDecline;
        public static String SENT_TOKEN_TO_SERVER = "sentTokenToServer";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RequestProcessStatusFlowlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            btnViewRequestAccept = FindViewById<Button>(Resource.Id.btnSubmitRequest);
            btnViewRequestDecline = FindViewById<Button>(Resource.Id.btnRejectRequest);

            tvActivityOwner = FindViewById<TextView>(Resource.Id.lblActivityOwnerStatus);
            tvAreaOwner = FindViewById<TextView>(Resource.Id.lblAreaOwnerStatus);
            tvSafety = FindViewById<TextView>(Resource.Id.lblSafetyStatus);
            tvContractor = FindViewById<TextView>(Resource.Id.lblContractor);


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

            if (tvActivityOwner.Text == "Accepted" && tvAreaOwner.Text == "Accepted" && tvSafety.Text == "Accepted")
            {
                ButtonAcceptOrDecline = "Accept";
                StatusUpdate();
            }

            else
            {
                Toast.MakeText(this, "You are Not Eligible TO Submut Request", ToastLength.Long).Show();
                return;
            }
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

                    if (StatusModel.LoginUserStatus == 1)
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

                    if (StatusModel.LoginUserStatus == 1)
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

                tvActivityOwner.Text = ResultProcessRequestDetailsByRequestIDModel.ActivityOwnerStatus;
                tvAreaOwner.Text = ResultProcessRequestDetailsByRequestIDModel.AreaOwnerStatus;
                tvSafety.Text = ResultProcessRequestDetailsByRequestIDModel.SafetyStatus;
                tvContractor.Text = ResultProcessRequestDetailsByRequestIDModel.ContractorStatus;

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
            SupportActionBar.SetTitle(Resource.String.RequestProcessStatusFlow);
            RequestDetailsByRequestID();
            base.OnResume();
        }
    }
}