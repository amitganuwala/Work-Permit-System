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
using WorkPermitSystem.Adapter;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "CheckVendorRequestActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CheckVendorRequestActivity : AppCompatActivity
    {
        ListView lvVendorRequestList;
        TextView tvlblVendorRequestList, lblTokenNo, lblDepartment, lblEmployeeName;
        ListView mListView;
        ListProcessRequestByDepartmentEmployeeModel RPSRNO = new ListProcessRequestByDepartmentEmployeeModel();
        List<ListProcessRequestByDepartmentEmployeeModel> ResultListProcessRequestByDepartmentEmployeeModel;
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CheckVendorRequestlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            
            lblTokenNo = FindViewById<TextView>(Resource.Id.lblTokenNumber);
            lblDepartment = FindViewById<TextView>(Resource.Id.lblEmployeeDepartment);
            lblEmployeeName = FindViewById<TextView>(Resource.Id.lblEmployeeName);
            mListView = FindViewById<ListView>(Resource.Id.RPlistView);

            mListView.ItemClick += MListView_ItemClick;
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
             RPSRNO = (ResultListProcessRequestByDepartmentEmployeeModel.ElementAt(e.Position));

            StatusModel.RequestProcessSrNo = RPSRNO.RequestProcessSrNo.ToString();
            Intent intent = new Intent(this, typeof(CheckVendorRequestDetailsActivity));
            this.StartActivity(intent);
        }

        //private void TvlblVendorRequestList_Click(object sender, EventArgs e)
        //{
        //    Intent intent = new Intent(this, typeof(CheckVendorRequestDetailsActivity));
        //    this.StartActivity(intent);
        //}


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

        public async void GetAllEmployeeDepartmentInfoByTokenNo()
        {
            try
            {
                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);
                string Url = StatusModel.Url + "GetDepartmentEmployeeInformationByTokenNo";
                WebHelpper _objHelper = new WebHelpper();
                GetAllDepartmentEmployeeNameModel _objGetAllDepartmentEmployeeNameModel = new GetAllDepartmentEmployeeNameModel();

                _objGetAllDepartmentEmployeeNameModel.EmployeeTokenNo = StatusModel.LoginUserName;

                var PostString = JsonConvert.SerializeObject(_objGetAllDepartmentEmployeeNameModel);
                var request = await _objHelper.MakePostRequest(Url, PostString, true);

                GetAllDepartmentEmployeeNameModel ResultGetAllDepartmentEmployeeNameModel = JsonConvert.DeserializeObject<GetAllDepartmentEmployeeNameModel>(request);

                lblTokenNo.Text = ResultGetAllDepartmentEmployeeNameModel.EmployeeTokenNo;
                lblEmployeeName.Text = ResultGetAllDepartmentEmployeeNameModel.EmployeeName;
                lblDepartment.Text = ResultGetAllDepartmentEmployeeNameModel.EmployeeDepartmentName;
                StatusModel.EmployeeDesignationName = ResultGetAllDepartmentEmployeeNameModel.EmployeeDesignationName;

                
                string Url1 = StatusModel.Url + "GetProcessRequestByUser";
                WebHelpper _objHelper1 = new WebHelpper();
               
                var PostString1 = JsonConvert.SerializeObject(ResultGetAllDepartmentEmployeeNameModel);
                var request1 = await _objHelper.MakePostRequest(Url1, PostString1, true);

                ResultListProcessRequestByDepartmentEmployeeModel = JsonConvert.DeserializeObject<List<ListProcessRequestByDepartmentEmployeeModel>>(request1);


                mListView.Adapter = new RPItemListForDeptEmployeeAdapter(this, ResultListProcessRequestByDepartmentEmployeeModel);
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
            SupportActionBar.SetTitle(Resource.String.VendorsRequestList);
            GetAllEmployeeDepartmentInfoByTokenNo();
            base.OnResume();
        }
    }
}