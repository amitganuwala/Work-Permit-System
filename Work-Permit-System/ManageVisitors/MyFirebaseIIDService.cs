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
using Firebase.Iid;
using Android.Preferences;
using Newtonsoft.Json;
using WorkPermitSystem.Models;
using Firebase;

namespace WorkPermitSystem
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public static String SENT_TOKEN_TO_SERVER = "sentTokenToServer";
        String USERID = "";
        /**
         * Called if InstanceID token is updated. This may occur if the security of
         * the previous token had been compromised. Note that this is called when the InstanceID token
         * is initially generated so this is where you would retrieve the token.
         */
        // [START refresh_token]
        public void OnTokenRefresh(string UserID, int LoginUserStatus)
        {
            // Get updated InstanceID token.
            USERID = UserID.Trim();
            FirebaseApp.InitializeApp(this);
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Android.Util.Log.Debug(TAG, "Refreshed token: " + refreshedToken);


            //ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            //sharedPreferences.Edit().PutBoolean(SENT_TOKEN_TO_SERVER, true).Apply();

            // TODO: Implement this method to send any registration to your app's servers.
            if (LoginUserStatus == 0)
            {
                SendRegistrationToServerForEmployee(refreshedToken, USERID);
            }
            else if (LoginUserStatus == 1)
            {
                SendRegistrationToServerForVendor(refreshedToken, USERID);

            }

        }

        // [END refresh_token]

        /**
         * Persist token to third-party servers.
         *
         * Modify this method to associate the user's FCM InstanceID token with any server-side account
         * maintained by your application.
         */
        async void SendRegistrationToServerForVendor(string token, string UserID)
        {
          
            VendorUserRegistrationModel _objVendorUserRegistrationModel = new VendorUserRegistrationModel();

            _objVendorUserRegistrationModel.DeviceTokenId = token.Trim();
            _objVendorUserRegistrationModel.VendorUserID = UserID.Trim();

            string Url = StatusModel.Url + "UpdateVendorDeviceTokenNumber";
            WebHelpper _objHelper = new WebHelpper();

            var PostString = JsonConvert.SerializeObject(_objVendorUserRegistrationModel);
            var request = await _objHelper.MakePostRequest(Url, PostString, true);

            ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(request);

        }

        async void SendRegistrationToServerForEmployee(string token, string UserID)
        {
          

            DepartmentEmployeeRegistrationModel _objDepartmentEmployeeRegistrationModel = new DepartmentEmployeeRegistrationModel();

            _objDepartmentEmployeeRegistrationModel.DeviceTokenId = token.Trim();
            _objDepartmentEmployeeRegistrationModel.EmployeeTokenNo = UserID.Trim();

            string Url = StatusModel.Url + "UpdateEmployeeDeviceTokenNumber";
            WebHelpper _objHelper = new WebHelpper();

            var PostString = JsonConvert.SerializeObject(_objDepartmentEmployeeRegistrationModel);
            var request = await _objHelper.MakePostRequest(Url, PostString, true);

            ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(request);
        }
    }
}