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
using System.Threading.Tasks;
using Firebase.Iid;
using Android.Preferences;
using WorkPermitSystem;
using Firebase;

namespace WorkPermitSystem.Activities
{
    [Activity(Label = "VendorNewRegistrationActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VendorNewRegistrationActivity : AppCompatActivity
    {
        AutoCompleteTextView txt_VendorContractorName;
        VendorUserRegistrationModel _ObjVendorUserRegistration = new VendorUserRegistrationModel();
        List<ContractorMasterModel> ResultAllContractorMaster = new List<ContractorMasterModel>();
        Button btnRegisterVendor, btnRegisterVendorCancel;
        EditText txt_VendorUserID, txt_VendorName, txt_VendorAddress, txt_VendorContactNo, txt_VendorEmailID, txt_VendorNatureOfWork, txt_VendorContractorContactNo, txt_VendorPassword;
        ProgressDialog progressDialog;
        String DTI="";
        public static String SENT_TOKEN_TO_SERVER = "sentTokenToServer";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VendorNewRegistrationlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            btnRegisterVendor = FindViewById<Button>(Resource.Id.btnRegisterDepartment);
            btnRegisterVendorCancel = FindViewById<Button>(Resource.Id.btnCancelRegisterDepartment);

            txt_VendorUserID = FindViewById<EditText>(Resource.Id.txt_VendorUserID);
            txt_VendorName = FindViewById<EditText>(Resource.Id.txt_VendorName);
            txt_VendorAddress = FindViewById<EditText>(Resource.Id.txt_VendorAddress);
            txt_VendorContactNo = FindViewById<EditText>(Resource.Id.txt_VendorContactNumber);
            txt_VendorEmailID = FindViewById<EditText>(Resource.Id.txt_VendorEmailID);
            txt_VendorNatureOfWork = FindViewById<EditText>(Resource.Id.txt_VendorNatureOfWork);
            txt_VendorContractorName = FindViewById<AutoCompleteTextView>(Resource.Id.txt_VendorContractor);
            txt_VendorContractorContactNo = FindViewById<EditText>(Resource.Id.txt_VendorContractorContactNo);
            txt_VendorPassword = FindViewById<EditText>(Resource.Id.txt_VendorPassword);


            btnRegisterVendor.Click += BtnRegisterVendor_Click;
            btnRegisterVendorCancel.Click += BtnRegisterVendorCancel_Click;
            txt_VendorContractorName.ItemClick += Txt_VendorContractorName_ItemClick;
            TokenNo();
        }

        ContractorMasterModel ContractorMaster = new ContractorMasterModel();
        private void Txt_VendorContractorName_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ContractorMaster = ResultAllContractorMaster.ElementAt(e.Position);

            if (ContractorMaster != null)
            {
                txt_VendorContractorContactNo.Text = ContractorMaster.ContractorContactNo.ToString().Trim();
            }
        }

        private void BtnRegisterVendorCancel_Click(object sender, EventArgs e)
        {

        }

        private void BtnRegisterVendor_Click(object sender, EventArgs e)
        {
            VendorRegistration();
        }


        public async void GetVendorUserMaxSrNo()
        {
            try
            {
                WebHelpper _objHelper = new WebHelpper();

                string Url = StatusModel.Url + "GetVendorUserMaxSrNo";

                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Checking...</font>"), true);

                var request = await _objHelper.MakeGetRequest(Url);

                var _objVendorUserRegistrationModel = JsonConvert.DeserializeObject<VendorUserRegistrationModel>(request);

                txt_VendorUserID.Text = "M&M" + _objVendorUserRegistrationModel.VendorSrNo;
                progressDialog.Hide();
            }
            catch (Exception e)
            {
                progressDialog.Hide();
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        public String[] Contractor;
        public async void GetAllContractor()
        {
            try
            {
                string Url = StatusModel.Url + "GetContractorMaster";
                WebHelpper _objHelper = new WebHelpper();
                ContractorMasterModel _objContractorMaster = new ContractorMasterModel();


                var request = await _objHelper.MakeGetRequest(Url);

                ResultAllContractorMaster = JsonConvert.DeserializeObject<List<ContractorMasterModel>>(request);

                Contractor = new string[ResultAllContractorMaster.Count + 1];

                int i = 1;
                Contractor[0] = "--Select Contractor--";
                foreach (var item in ResultAllContractorMaster)
                {
                    Contractor[i] = item.ContractorName;
                    i++;
                }

                ArrayAdapter _adapterContractor = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Contractor);
                txt_VendorContractorName.Adapter = _adapterContractor;
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        private async void VendorRegistration()
        {
            TokenNo();
            try
            {
                Drawable icon_error = Resources.GetDrawable(Resource.Drawable.alert);
                icon_error.SetBounds(0, 0, 40, 30);

                if (txt_VendorName.Text != "")
                {
                    if (txt_VendorAddress.Text != "")
                    {
                        if (txt_VendorContactNo.Text != "")
                        {
                            if (txt_VendorEmailID.Text != "")
                            {
                                if (txt_VendorNatureOfWork.Text != "")
                                {
                                    if (txt_VendorContractorName.Text != "")
                                    {
                                        if (txt_VendorContractorContactNo.Text != "")
                                        {
                                            if (txt_VendorUserID.Text != "")
                                            {
                                                if (txt_VendorPassword.Text != "")
                                                {
                                                    foreach (var item in ResultAllContractorMaster)
                                                    {
                                                        if (item.ContractorName.ToString().Trim() == (txt_VendorContractorName.Text.Trim()))
                                                        {
                                                            _ObjVendorUserRegistration.VendorContractorSrNo = item.ContractorSrNo;
                                                            _ObjVendorUserRegistration.VendorContractorCoNo = item.ContractorContactNo;
                                                        }
                                                    }



                                                    _ObjVendorUserRegistration.DeviceTokenId = DTI.ToString();
                                                    _ObjVendorUserRegistration.VendorName = txt_VendorName.Text;
                                                    _ObjVendorUserRegistration.VendorAddress = txt_VendorAddress.Text;
                                                    _ObjVendorUserRegistration.VendorContactNo = txt_VendorContactNo.Text;
                                                    _ObjVendorUserRegistration.VendorEmailID = txt_VendorEmailID.Text;
                                                    _ObjVendorUserRegistration.VendorNatureOfWork = txt_VendorNatureOfWork.Text;
                                                    _ObjVendorUserRegistration.VendorUserID = txt_VendorUserID.Text;
                                                    _ObjVendorUserRegistration.VendorPassword = txt_VendorPassword.Text;
                                                    _ObjVendorUserRegistration.VendorRegistrationDate =Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));

                                                    // ADD Insert Code Here

                                                    WebHelpper _objHelper = new WebHelpper();

                                                    string Url = StatusModel.Url + "AddVendorUserRegistration";

                                                    var progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Inserting...</font>"), true);

                                                    var PostString = JsonConvert.SerializeObject(_ObjVendorUserRegistration);
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
                                                txt_VendorPassword.RequestFocus();
                                                txt_VendorPassword.SetError("Please Enter Password First", icon_error);
                                            }
                                        }
                                        else
                                        {
                                            txt_VendorUserID.RequestFocus();
                                            txt_VendorUserID.SetError("Please Enter Vendor User ID First", icon_error);
                                        }
                                    }
                                    else
                                    {
                                        txt_VendorContractorContactNo.RequestFocus();
                                        txt_VendorContractorContactNo.SetError("Please Enter Contractor Contact Number First", icon_error);
                                    }
                                }
                                else
                                {
                                    txt_VendorContractorName.RequestFocus();
                                    txt_VendorContractorName.SetError("Please Enter Contractor Name First", icon_error);
                                }
                            }
                            else
                            {
                                txt_VendorNatureOfWork.RequestFocus();
                                txt_VendorNatureOfWork.SetError("Please Enter Nature Of Work First", icon_error);
                            }
                        }
                        else
                        {
                            txt_VendorEmailID.RequestFocus();
                            txt_VendorEmailID.SetError("Please Enter Email ID First", icon_error);
                        }
                    }
                    else
                    {
                        txt_VendorContactNo.RequestFocus();
                        txt_VendorContactNo.SetError("Please Enter Contact Number First", icon_error);
                    }
                }
                else
                {
                    txt_VendorAddress.RequestFocus();
                    txt_VendorAddress.SetError("Please Enter Address First", icon_error);
                }
            }
                else
                {
                txt_VendorName.RequestFocus();
                txt_VendorName.SetError("Please Enter Full Name First", icon_error);
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
            txt_VendorUserID.Text = "";
            txt_VendorName.Text = "";
            txt_VendorAddress.Text = "";
            txt_VendorContactNo.Text = "";
            txt_VendorEmailID.Text = "";
            txt_VendorNatureOfWork.Text = "";
            txt_VendorContractorName.Text = "";
            txt_VendorContractorContactNo.Text = "";
            txt_VendorPassword.Text = "";
            GetVendorUserMaxSrNo();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.VendorRegistration);
           
            GetVendorUserMaxSrNo();

            GetAllContractor();
            TokenNo();
            base.OnResume();
        }

    }
}