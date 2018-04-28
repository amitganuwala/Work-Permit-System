using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WorkPermitSystem.Models;
using Android.Graphics.Drawables;
using Newtonsoft.Json;
using Android.Views.InputMethods;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using WorkPermitSystem;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "VendorCreateNewRequestActivity", Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VendorCreateNewRequestActivity : AppCompatActivity
    {
        tbl_RequestProcess _ObjRequestProcess = new tbl_RequestProcess();
        Button btnSendRequest, btnRequestCancel;
        EditText txt_Associries, txt_NoOFVendors, txt_Reasons;
        TextView dtp_RequestStartTime, dtp_RequestEndTime, txt_EmployeeDepartmentID;
        AutoCompleteTextView txt_EmployeeName;
        List<GetAllDepartmentEmployeeNameModel> ResultGetAllDepartmentEmployeeNameModel = new List<GetAllDepartmentEmployeeNameModel>();
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VendorCreateNewRequestlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            View header = navigationView.GetHeaderView(0);

            btnSendRequest = FindViewById<Button>(Resource.Id.btnSendRequest);
            btnRequestCancel = FindViewById<Button>(Resource.Id.btnCancelSendRequest);

            txt_EmployeeName = FindViewById<AutoCompleteTextView>(Resource.Id.txt_RequestEmployeeName);
            txt_EmployeeDepartmentID = FindViewById<TextView>(Resource.Id.txt_RequestDepartment);
            dtp_RequestStartTime = FindViewById<TextView>(Resource.Id.txt_RequestStartTime);
            dtp_RequestEndTime = FindViewById<TextView>(Resource.Id.txt_SelectRequestEndTime);
            txt_Associries = FindViewById<EditText>(Resource.Id.txt_VendorsAssociries);
            txt_NoOFVendors = FindViewById<EditText>(Resource.Id.txt_SelectNoofVendors);
            txt_Reasons = FindViewById<EditText>(Resource.Id.txt_RequestReason);

            dtp_RequestEndTime.Click += Dtp_RequestEndTime_Click;
            dtp_RequestStartTime.Click += Dtp_RequestStartTime_Click;
            btnSendRequest.Click += BtnSendRequest_Click;
            btnRequestCancel.Click += BtnRequestCancel_Click;
            txt_EmployeeName.ItemClick += Txt_EmployeeName_ItemClick;
        }

        private void Txt_EmployeeName_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            foreach (var item in ResultGetAllDepartmentEmployeeNameModel)
            {
                if (item.EmployeeName.ToString().Trim() == (txt_EmployeeName.Text.Trim()))
                {
                    txt_EmployeeDepartmentID.Text = item.EmployeeDepartmentName.ToString();
                }
            }
        }

        private void Dtp_RequestEndTime_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimeSpan time)
            {
                String aMpM = "AM";
                if (time.Hours > 11)
                {
                    aMpM = "PM";
                }

                //Make the 24 hour time format to 12 hour time format
                int currentHour;
                if (time.Hours > 11)
                {
                    currentHour = time.Hours - 12;
                }
                else
                {
                    currentHour = time.Hours;
                }

                dtp_RequestEndTime.Text = currentHour + ":" + time.Minutes + " " + aMpM;
            });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void Dtp_RequestStartTime_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimeSpan time)
            {
                String aMpM = "AM";
                if (time.Hours > 11)
                {
                    aMpM = "PM";
                }

                //Make the 24 hour time format to 12 hour time format
                int currentHour;
                if (time.Hours > 11)
                {
                    currentHour = time.Hours - 12;
                }
                else
                {
                    currentHour = time.Hours;
                }

                dtp_RequestStartTime.Text = currentHour + ":" + time.Minutes + " " + aMpM;
            });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void BtnRequestCancel_Click(object sender, EventArgs e)
        {
           
        }

        private void BtnSendRequest_Click(object sender, EventArgs e)
        {
            RequestProcess();
        }

        private async void RequestProcess()
        {
            try
            {
                Drawable icon_error = Resources.GetDrawable(Resource.Drawable.alert);
                icon_error.SetBounds(0, 0, 40, 30);

                if (txt_EmployeeName.Text != "")
                {
                    if (txt_EmployeeDepartmentID.Text != "")
                    {
                        if (dtp_RequestStartTime.Text != "")
                        {
                            if (dtp_RequestEndTime.Text != "")
                            {
                                if (txt_Associries.Text != "")
                                {
                                    if (txt_NoOFVendors.Text != "")
                                    {
                                        if (txt_Reasons.Text != "")
                                        {

                                            foreach (var item in ResultGetAllDepartmentEmployeeNameModel)
                                            {
                                                if (item.EmployeeName.ToString().Trim() == (txt_EmployeeName.Text.Trim()))
                                                {
                                                    _ObjRequestProcess.EmployeeDepartmentID = item.EmployeeDepartmentID;
                                                    _ObjRequestProcess.EmployeeId = item.EmployeeSrNo;
                                                }
                                            }

                                            progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Creating New Request...</font>"), true);


                                            string Url = StatusModel.Url + "GetVendorUserInformationByVendorUserID";
                                            WebHelpper _objHelper = new WebHelpper();
                                            VendorUserRegistrationModel _objVendorUserRegistrationModel = new VendorUserRegistrationModel();

                                            _objVendorUserRegistrationModel.VendorUserID = StatusModel.LoginUserName;

                                            var PostString = JsonConvert.SerializeObject(_objVendorUserRegistrationModel);
                                            var request = await _objHelper.MakePostRequest(Url, PostString, true);

                                            VendorUserRegistrationModel ResultVendorUserRegistrationModel = JsonConvert.DeserializeObject<VendorUserRegistrationModel>(request);



                                            _ObjRequestProcess.VendorSrNo = ResultVendorUserRegistrationModel.VendorSrNo;
                                            _ObjRequestProcess.VisitStartTime = Convert.ToDateTime(dtp_RequestStartTime.Text);
                                            _ObjRequestProcess.VisitEndTime = Convert.ToDateTime(dtp_RequestEndTime.Text);
                                            _ObjRequestProcess.VendorAccessories = txt_Associries.Text;
                                            _ObjRequestProcess.NoOfVendors = Convert.ToInt32(txt_NoOFVendors.Text);
                                            _ObjRequestProcess.VendorVisitResons = txt_Reasons.Text;
                                            _ObjRequestProcess.RequestProcessDate  = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                                            _ObjRequestProcess.ActivityOwnerStatus = "None";
                                            _ObjRequestProcess.AreaOwnerStatus = "None";
                                            _ObjRequestProcess.SafetyStatus = "None";
                                            _ObjRequestProcess.ContractorStatus = "None";
                                            // ADD Insert Code Here

                                            WebHelpper _objHelper1 = new WebHelpper();

                                            string Url1 = StatusModel.Url + "AddRequestProcess";

                                            var PostString1 = JsonConvert.SerializeObject(_ObjRequestProcess);
                                            var requestTemp1 = await _objHelper1.MakePostRequest(Url1, PostString1, true);
                                            ResultModel ResultgetRequest1 = JsonConvert.DeserializeObject<ResultModel>(requestTemp1);

                                            if (ResultgetRequest1.success == 1)
                                            {
                                                progressDialog.Hide();
                                                Toast.MakeText(this, ResultgetRequest1.msg, ToastLength.Short).Show();
                                            }

                                            else
                                            {
                                                progressDialog.Hide();
                                                Toast.MakeText(this, ResultgetRequest1.msg, ToastLength.Short).Show();
                                                return;
                                            }

                                            clear();
                                        }
                                        else
                                        {
                                            txt_Reasons.RequestFocus();
                                            txt_Reasons.SetError("Please Enter Visit Reason First", icon_error);
                                        }
                                    }
                                    else
                                    {
                                        txt_NoOFVendors.RequestFocus();
                                        txt_NoOFVendors.SetError("Please Select No of Vendors First", icon_error);
                                    }
                                }
                                else
                                {
                                    txt_Associries.RequestFocus();
                                    txt_Associries.SetError("Please Enter Associries First", icon_error);
                                }
                            }

                            else
                            {
                                dtp_RequestEndTime.RequestFocus();
                                dtp_RequestEndTime.SetError("Please Select End Time First", icon_error);
                            }
                        }
                        else
                        {
                            dtp_RequestEndTime.RequestFocus();
                            dtp_RequestEndTime.SetError("Please Select Start Time First", icon_error);
                        }
                    }
                    else
                    {
                        txt_EmployeeDepartmentID.RequestFocus();
                        txt_EmployeeDepartmentID.SetError("Please Select Employee Department First", icon_error);
                    }
                }
                else
                {
                    txt_EmployeeName.RequestFocus();
                    txt_EmployeeName.SetError("Please Enter Full Name First", icon_error);
                }
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        public void clear()
        {
            txt_EmployeeName.Text = "";
            txt_EmployeeDepartmentID.Text = "";
            dtp_RequestStartTime.Text = "";
            dtp_RequestEndTime.Text = "";
            txt_Associries.Text = "";
            txt_NoOFVendors.Text = "";
            txt_Reasons.Text = "";
        }

        public String[] AllDepartmentEmployeeName;
        public async void GetAllDepartmentEmployeeName()
        {
            try
            {
                string Url = StatusModel.Url + "GetAllDepartmentEmployeeName";
                WebHelpper _objHelper = new WebHelpper();
                GetAllDepartmentEmployeeNameModel _objGetAllDepartmentEmployeeNameModel = new GetAllDepartmentEmployeeNameModel();


                var request = await _objHelper.MakeGetRequest(Url);

                ResultGetAllDepartmentEmployeeNameModel = JsonConvert.DeserializeObject<List<GetAllDepartmentEmployeeNameModel>>(request);

                AllDepartmentEmployeeName = new string[ResultGetAllDepartmentEmployeeNameModel.Count + 1];

                int i = 1;
                AllDepartmentEmployeeName[0] = "--Select Department Employee Name--";
                foreach (var item in ResultGetAllDepartmentEmployeeNameModel)
                {
                    AllDepartmentEmployeeName[i] = item.EmployeeName;
                    i++;
                }

                ArrayAdapter _adapterAllDepartmentEmployeeName = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, AllDepartmentEmployeeName);
                txt_EmployeeName.Adapter = _adapterAllDepartmentEmployeeName;
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_send_request):
                    StartActivity(new Intent(this, typeof(CheckVendorRequestForVendorsActivity)));
                    break;
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.CreateNewRequest);
            GetAllDepartmentEmployeeName();
            base.OnResume();
        }
    }
}