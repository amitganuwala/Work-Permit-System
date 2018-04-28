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
using System.Threading;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "CheckVendorRequestForVendorsActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CheckVendorRequestForVendorsActivity : AppCompatActivity
    {
        ListView lvVendorRequestList;
        TextView tvlblVendorRequestList, lblTokenNo, lblDepartment, lblEmployeeName;
        ListView mListView;
        ListProcessRequestByVendorUserModel RPSRNO = new ListProcessRequestByVendorUserModel();
        List<ListProcessRequestByVendorUserModel> ResultListProcessRequestByVendorUserModel;
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CheckVendorRequestForVendorlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            
            mListView = FindViewById<ListView>(Resource.Id.RPVendorlistView);

            mListView.ItemClick += MListView_ItemClick;
            mListView.ItemLongClick += MListView_ItemLongClick;
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            RPSRNO = (ResultListProcessRequestByVendorUserModel.ElementAt(e.Position));

            StatusModel.RequestProcessSrNo = RPSRNO.RequestProcessSrNo.ToString();
            Intent intent = new Intent(this, typeof(RequestProcessStatusFlowActivity));
            this.StartActivity(intent);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RPSRNO = (ResultListProcessRequestByVendorUserModel.ElementAt(e.Position));

            StatusModel.RequestProcessSrNo = RPSRNO.RequestProcessSrNo.ToString();
            Intent intent = new Intent(this, typeof(CheckVendorRequestDetailsForVendorActivity));
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

                string Url = StatusModel.Url + "GetVendorUserInformationByVendorUserID";
                WebHelpper _objHelper = new WebHelpper();
                VendorUserRegistrationModel _objVendorUserRegistrationModel = new VendorUserRegistrationModel();

                _objVendorUserRegistrationModel.VendorUserID = StatusModel.LoginUserName;

                var PostString = JsonConvert.SerializeObject(_objVendorUserRegistrationModel);
                var request = await _objHelper.MakePostRequest(Url, PostString, true);

                VendorUserRegistrationModel ResultVendorUserRegistrationModel = JsonConvert.DeserializeObject<VendorUserRegistrationModel>(request);
                


                string Url1 = StatusModel.Url + "GetProcessRequestByVendorUserSrNo";
                WebHelpper _objHelper1 = new WebHelpper();
               
                var PostString1 = JsonConvert.SerializeObject(ResultVendorUserRegistrationModel);
                var request1 = await _objHelper1.MakePostRequest(Url1, PostString1, true);

                ResultListProcessRequestByVendorUserModel = JsonConvert.DeserializeObject<List<ListProcessRequestByVendorUserModel>>(request1);


                mListView.Adapter = new RPItemListForVendorAdapter(this, ResultListProcessRequestByVendorUserModel);
                
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