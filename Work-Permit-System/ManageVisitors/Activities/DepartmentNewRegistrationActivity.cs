using System;
using System.Collections.Generic;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using WorkPermitSystem.Models;
using Firebase.Iid;
using Android.Preferences;
using WorkPermitSystem;
using Firebase;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "DepartmentNewRegistrationActivity", Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DepartmentNewRegistrationActivity : AppCompatActivity
    {
        DepartmentEmployeeRegistrationModel _ObjDepartmentEmployeeRegistration = new DepartmentEmployeeRegistrationModel();
        Button btnRegisterDepartment, btnRegisterDepartmentCancel;
        AutoCompleteTextView txt_EmployeeDesignationID, txt_EmployeeDepartmentID;
        EditText txt_EmployeeTokenNo, txt_EmployeeName, txt_EmployeeAddress, txt_EmployeeContactNo, txt_EmployeeEmailID, txt_EmployeePassword;
        ProgressDialog progressDialog;
        String sDepartmentID = "", sDesignationID = "", DTI="";
        ArrayAdapter _adapterDepartment, _adapterDesignation;

        public static String SENT_TOKEN_TO_SERVER = "sentTokenToServer";
        List<tbl_DepartmentMaster> ResultAllDepartmentMaster = new List<tbl_DepartmentMaster>();
        List<tbl_DesignationMaster> ResultAllDesignationMaster = new List<tbl_DesignationMaster>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DepartmentNewRegistrationlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            btnRegisterDepartment = FindViewById<Button>(Resource.Id.btnRegisterDepartment);
            btnRegisterDepartmentCancel = FindViewById<Button>(Resource.Id.btnCancelRegisterDepartment);
            txt_EmployeeTokenNo = FindViewById<EditText>(Resource.Id.txt_TokenNumber);
            txt_EmployeeName = FindViewById<EditText>(Resource.Id.txt_EmployeeName);
            txt_EmployeeAddress = FindViewById<EditText>(Resource.Id.txt_EmployeeAddress);
            txt_EmployeeContactNo = FindViewById<EditText>(Resource.Id.txt_ContactNo);
            txt_EmployeeEmailID = FindViewById<EditText>(Resource.Id.txt_EmailID);
            txt_EmployeeDesignationID = FindViewById<AutoCompleteTextView>(Resource.Id.txt_Designation);
            txt_EmployeeDepartmentID = FindViewById<AutoCompleteTextView>(Resource.Id.txt_Department);
            txt_EmployeePassword = FindViewById<EditText>(Resource.Id.txt_Password);


            btnRegisterDepartment.Click += BtnRegisterDepartment_Click;
            btnRegisterDepartmentCancel.Click += BtnRegisterDepartmentCancel_Click;

            txt_EmployeeDepartmentID.ItemClick += Txt_EmployeeDepartmentID_ItemClick;
            TokenNo();
        }


        tbl_DepartmentMaster DepartmentMaster = new tbl_DepartmentMaster();
        private void Txt_EmployeeDepartmentID_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string DepartmentID = "";
            
            foreach (var item in ResultAllDepartmentMaster)
            {
                if (item.DepartmentName.ToString().Trim() == (txt_EmployeeDepartmentID.Text.Trim()))
                {
                    DepartmentID = item.DepartmentID.ToString().Trim();
                }
            }
            if (DepartmentID != "")
            {
                GetAllDesignationByDepartment(DepartmentID);
            }
        }

        public void LoadData()
        {

            //if (CheckInternetConnection())
            //{
                GetAllDepartment();
                GetAllDesignation();
            //}
        }


        public String[] Department;
        public async void GetAllDepartment()
        {
            try
            {
                string Url = StatusModel.Url + "GetDepartmentMaster";
                WebHelpper _objHelper = new WebHelpper();
                tbl_DepartmentMaster _objDepartmentMaster = new tbl_DepartmentMaster();


                //using (var client = new WebClient())
                //{
                //    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                //    var responce = client.UploadString(new Uri(Url), "GET");
                //    ResultAllDepartmentMaster = JsonConvert.DeserializeObject < List<tbl_DepartmentMaster>>(responce);

                //}

                var request = await _objHelper.MakeGetRequest(Url);

                ResultAllDepartmentMaster = JsonConvert.DeserializeObject<List<tbl_DepartmentMaster>>(request);

                Department = new string[ResultAllDepartmentMaster.Count + 1];

                int i = 1;
                Department[0] = "--Select Department--";
                foreach (var item in ResultAllDepartmentMaster)
                {
                    Department[i] = item.DepartmentName;
                    i++;
                }

                ArrayAdapter _adapterDepartment = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Department);
                txt_EmployeeDepartmentID.Adapter = _adapterDepartment;
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        public String[] Designation;
        public async void GetAllDesignation()
        {
            try
            {
                string Url = StatusModel.Url + "GetDesignationMaster";
                WebHelpper _objHelper = new WebHelpper();
                tbl_DesignationMaster _objDesignationMaster = new tbl_DesignationMaster();


                var request = await _objHelper.MakeGetRequest(Url);

                ResultAllDesignationMaster = JsonConvert.DeserializeObject<List<tbl_DesignationMaster>>(request);

                Designation = new string[ResultAllDesignationMaster.Count + 1];

                int i = 1;
                Designation[0] = "--Select Designation--";
                foreach (var item in ResultAllDesignationMaster)
                {
                    Designation[i] = item.DesignationName;
                    i++;
                }


                ArrayAdapter _adapterDesignation = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Designation);

                txt_EmployeeDesignationID.Adapter = _adapterDesignation;
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }


        public String[] DesignationByDepartment;
        public async void GetAllDesignationByDepartment(string sDepartmentID)
        {
            try
            {
                string Url = StatusModel.Url + "GetDesignationMasterByDepartment";
                WebHelpper _objHelper = new WebHelpper();
                tbl_DesignationMaster _objDesignationMaster = new tbl_DesignationMaster();
                tbl_DepartmentMaster _objDepartmentMaster = new tbl_DepartmentMaster();
                _objDepartmentMaster.DepartmentID = Convert.ToInt32(sDepartmentID);
                var PostString = JsonConvert.SerializeObject(_objDepartmentMaster);
                var request = await _objHelper.MakePostRequest(Url, PostString, true);
                
                ResultAllDesignationMaster = JsonConvert.DeserializeObject<List<tbl_DesignationMaster>>(request);

                DesignationByDepartment = new string[ResultAllDesignationMaster.Count + 1];

                int i = 1;
                DesignationByDepartment[0] = "--Select Designation--";
                foreach (var item in ResultAllDesignationMaster)
                {
                    DesignationByDepartment[i] = item.DesignationName;
                    i++;
                }


                ArrayAdapter _adapterDesignationByDepartment = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, DesignationByDepartment);

                txt_EmployeeDesignationID.Adapter = _adapterDesignationByDepartment;
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }


        private void BtnRegisterDepartmentCancel_Click(object sender, EventArgs e)
        {
          
        }

        private void BtnRegisterDepartment_Click(object sender, EventArgs e)
        {
            DepartmentEmployeeRegistration();
        }

        private async void DepartmentEmployeeRegistration()
        {
            try

            {

                TokenNo();

                Drawable icon_error = Resources.GetDrawable(Resource.Drawable.alert);
                icon_error.SetBounds(0, 0, 40, 30);
                if (txt_EmployeeTokenNo.Text != "")
                {
                    if (txt_EmployeeName.Text != "")
                    {
                        if (txt_EmployeeAddress.Text != "")
                        {
                            if (txt_EmployeeContactNo.Text != "")
                            {
                                if (txt_EmployeeEmailID.Text != "")
                                {
                                    if (txt_EmployeeDepartmentID.Text != "")
                                    {
                                        if (txt_EmployeeDesignationID.Text != "")
                                        {
                                            if (txt_EmployeePassword.Text != "")
                                            {

                                                foreach (var item in ResultAllDesignationMaster)
                                                {
                                                    if (item.DesignationName.ToString().Trim() == (txt_EmployeeDesignationID.Text.Trim()))
                                                    {
                                                        _ObjDepartmentEmployeeRegistration.EmployeeDepartmentID = item.DepartmentID;
                                                        _ObjDepartmentEmployeeRegistration.EmployeeDesignationID = item.DesignationID;
                                                    }
                                                }


                                                _ObjDepartmentEmployeeRegistration.DeviceTokenId = DTI.ToString();
                                                _ObjDepartmentEmployeeRegistration.EmployeeTokenNo = txt_EmployeeTokenNo.Text;
                                                _ObjDepartmentEmployeeRegistration.EmployeeName = txt_EmployeeName.Text;
                                                _ObjDepartmentEmployeeRegistration.EmployeeAddress = txt_EmployeeAddress.Text;
                                                _ObjDepartmentEmployeeRegistration.EmployeeContactNo = txt_EmployeeContactNo.Text;
                                                _ObjDepartmentEmployeeRegistration.EmployeeEmailID = txt_EmployeeEmailID.Text;
                                                //_ObjDepartmentEmployeeRegistration.EmployeeDepartmentID = Convert.ToInt32(txt_EmployeeDepartmentID.Text);
                                                //_ObjDepartmentEmployeeRegistration.EmployeeDesignationID = Convert.ToInt32(txt_EmployeeDesignationID.Text);
                                                _ObjDepartmentEmployeeRegistration.EmployeePassword = txt_EmployeePassword.Text;
                                                _ObjDepartmentEmployeeRegistration.Date = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                                                // ADD Insert Code Here

                                                WebHelpper _objHelper = new WebHelpper();

                                                string Url = StatusModel.Url + "AddDepartmentEmployeeRegistration";

                                                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Inserting...</font>"), true);

                                                var PostString = JsonConvert.SerializeObject(_ObjDepartmentEmployeeRegistration);
                                                var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                                                ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(requestTemp);

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


                                                clear();

                                            }
                                            else
                                            {
                                                txt_EmployeePassword.RequestFocus();
                                                txt_EmployeePassword.SetError("Please Enter Password First", icon_error);
                                            }
                                        }
                                        else
                                        {
                                            txt_EmployeeDesignationID.RequestFocus();
                                            txt_EmployeeDesignationID.SetError("Please Select Designation First", icon_error);
                                        }
                                    }
                                    else
                                    {
                                        txt_EmployeeDepartmentID.RequestFocus();
                                        txt_EmployeeDepartmentID.SetError("Please Select Department First", icon_error);
                                    }
                                }
                                else
                                {
                                    txt_EmployeeEmailID.RequestFocus();
                                    txt_EmployeeEmailID.SetError("Please Enter Email ID First", icon_error);
                                }
                            }
                            else
                            {
                                txt_EmployeeContactNo.RequestFocus();
                                txt_EmployeeContactNo.SetError("Please Enter Contact Number First", icon_error);
                            }
                        }
                        else
                        {
                            txt_EmployeeAddress.RequestFocus();
                            txt_EmployeeAddress.SetError("Please Enter Address First", icon_error);
                        }
                    }
                    else
                    {
                        txt_EmployeeName.RequestFocus();
                        txt_EmployeeName.SetError("Please Enter Full Name First", icon_error);
                    }
                }
                else
                {

                    txt_EmployeeTokenNo.RequestFocus();
                    txt_EmployeeTokenNo.SetError("Please Enter Token Number First", icon_error);
                }

            }
            catch (Exception e)
            {
                progressDialog.Hide();
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }
        const string TAG = "MyFirebaseIIDService";
        public void TokenNo()
        {
            FirebaseApp.InitializeApp(this);
            DTI = FirebaseInstanceId.Instance.Token;
            Android.Util.Log.Debug(TAG, "Refreshed token: " + DTI);

            // TODO: Implement this method to send any registration to your app's servers.
            sendRegistrationToServer(DTI);
        }


        private void sendRegistrationToServer(String token)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            sharedPreferences.Edit().PutBoolean(SENT_TOKEN_TO_SERVER, true).Apply();
        }


        public void clear()
        {
            txt_EmployeeTokenNo.Text = "";
            txt_EmployeeName.Text = "";
            txt_EmployeeAddress.Text = "";
            txt_EmployeeContactNo.Text = "";
            txt_EmployeeEmailID.Text = "";
            txt_EmployeeDesignationID.Text = "";
            txt_EmployeeDepartmentID.Text = "";
            txt_EmployeePassword.Text = "";
        }

        public bool CheckInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.DepartmentEmployeeRegistration);
            GetAllDepartment();
            TokenNo();
            base.OnResume();
        }
    }
}