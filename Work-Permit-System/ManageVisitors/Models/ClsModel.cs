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
using System.Threading.Tasks;
using Firebase.Iid;

namespace WorkPermitSystem.Models
{
    class ClsModel
    {
    }

    public static class StatusModel
    {
        public static long LoginUserStatus = 0;
        public static string LoginUserName = "";
        public static string RequestProcessSrNo = "";
        public static string EmployeeDesignationName = "";
        public static string Url = "http://softstudio.suvarnapp.com/Admin/AdminApiForMV/";
        //public static string Url = "http://192.168.0.10:81/Admin/AdminApiForMV/";
    }

    //public string TokenNo()
    //{
    //    if (GetString(Resource.String.google_storage_bucket).Equals("1:950100958925:android:acaf35dae135a1d3"))
    //        throw new System.Exception("Invalid google-services.json file.  Make sure you've downloaded your own config file and added it to your app project with the 'GoogleServicesJson' build action.");

    //    string Token = "";
    //    Task.Run(() =>
    //    {
    //        var InstantId = FirebaseInstanceId.Instance;

    //        InstantId.DeleteInstanceId();
    //        Android.Util.Log.Debug("TAG", "{0} {1}", InstantId.Token, InstantId.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope));
    //        Token = FirebaseInstanceId.Instance.Token.ToString();
    //        //sendRegistrationToServer(Token);
    //    });

    //    return Token;
    //}


    public class ResultModel
    {
        public long success { get; set; }
        public string msg { get; set; }
    }

    public class UserLoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class DepartmentEmployeeRegistrationModel
    {
        public string DeviceTokenId { get; set; }
        public long EmployeeSrNo { get; set; }
        public string EmployeeTokenNo { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeeContactNo { get; set; }
        public string EmployeeEmailID { get; set; }
        public Nullable<long> EmployeeDepartmentID { get; set; }
        public Nullable<long> EmployeeDesignationID { get; set; }
        public string EmployeePassword { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class VendorUserRegistrationModel
    {
        public string DeviceTokenId { get; set; }
        public long VendorSrNo { get; set; }
        public string VendorUserID { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorContactNo { get; set; }
        public string VendorEmailID { get; set; }
        public string VendorNatureOfWork { get; set; }
        public Nullable<long> VendorContractorSrNo { get; set; }
        public string VendorContractorCoNo { get; set; }
        public string VendorPassword { get; set; }
        public Nullable<System.DateTime> VendorRegistrationDate { get; set; }
    }

    public class tbl_RequestProcess
    {
        public Nullable<long> EmployeeId { get; set; }
        public long RequestProcessSrNo { get; set; }
        public Nullable<long> VendorSrNo { get; set; }
        public Nullable<long> EmployeeDepartmentID { get; set; }
        public Nullable<System.DateTime> VisitStartTime { get; set; }
        public Nullable<System.DateTime> VisitEndTime { get; set; }
        public string VendorAccessories { get; set; }
        public Nullable<long> NoOfVendors { get; set; }
        public string VendorVisitResons { get; set; }
        public Nullable<System.DateTime> RequestProcessDate { get; set; }
        public string ActivityOwnerStatus { get; set; }
        public string AreaOwnerStatus { get; set; }
        public string SafetyStatus { get; set; }
        public string ContractorStatus { get; set; }
    }

    public class tbl_DepartmentMaster
    {
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.DateTime> DepartmentCreateDate { get; set; }
    }

    public class tbl_DesignationMaster
    {
        public long DesignationID { get; set; }
        public long DepartmentID { get; set; }
        public string DesignationName { get; set; }
        public Nullable<System.DateTime> DesignationCreateDate { get; set; }
    }

    public class GetAllDepartmentEmployeeNameModel
    {
        public long EmployeeSrNo { get; set; }
        public string EmployeeTokenNo { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<long> EmployeeDepartmentID { get; set; }
        public Nullable<long> EmployeeDesignationID { get; set; }
        public string EmployeeDepartmentName { get; set; }
        public string EmployeeDesignationName { get; set; }
    }


    public class ListProcessRequestByDepartmentEmployeeModel
    {
        public long RequestProcessSrNo { get; set; }
        public string EmployeeTokenNo { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> VisitStartTime { get; set; }
        public Nullable<System.DateTime> VisitEndTime { get; set; }
        public string RequestStatus { get; set; }
    }

    public class ListProcessRequestByVendorUserModel
    {
        public Nullable<long> VendorSrNo { get; set; }
        public long RequestProcessSrNo { get; set; }
        public string EmployeeTokenNo { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> VisitStartTime { get; set; }
        public Nullable<System.DateTime> VisitEndTime { get; set; }
        public string VendorVisitResons { get; set; }
    }


    public class ProcessRequestDetailsByRequestIDModel
    {
        public long RequestProcessSrNo { get; set; }
        public string EmployeeTokenNo { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDepartmentName { get; set; }
        public string VendorName { get; set; }
        public string ContractorName { get; set; }
        public string NatureOfWork { get; set; }
        public Nullable<System.DateTime> VisitStartTime { get; set; }
        public Nullable<System.DateTime> VisitEndTime { get; set; }
        public Nullable<long> NoOfVendors { get; set; }
        public string VendorVisitResons { get; set; }
        public string RequestStatus { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<long> VendorSrNo { get; set; }
        public Nullable<long> EmployeeDepartmentID { get; set; }
        public string VendorAccessories { get; set; }
        public Nullable<System.DateTime> RequestProcessDate { get; set; }
        public string ActivityOwnerStatus { get; set; }
        public string AreaOwnerStatus { get; set; }
        public string SafetyStatus { get; set; }
        public string ContractorStatus { get; set; }
    }

    public class ContractorMasterModel
    {
        public long ContractorSrNo { get; set; }
        public string CompanyName { get; set; }
        public string ContractorName { get; set; }
        public string ContractorContactNo { get; set; }
        public Nullable<System.DateTime> ContractorCreateDate { get; set; }
    }
}