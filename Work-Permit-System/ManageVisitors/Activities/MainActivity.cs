using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Firebase.Iid;
using WorkPermitSystem;

namespace WorkPermitSystem
{
    [Activity(Label = "ManageVendors",  Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

        }
    }
}

