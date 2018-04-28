using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WorkPermitSystem.Models;
using WorkPermitSystem;

namespace WorkPermitSystem.Adapter
{
    public class RPItemListForVendorAdapter : BaseAdapter<ListProcessRequestByVendorUserModel>
    {
        Activity context;
        List<ListProcessRequestByVendorUserModel> list;
        int SrNo;

        public RPItemListForVendorAdapter(Activity _context, List<ListProcessRequestByVendorUserModel> _list)
                : base()
        {
            this.context = _context;
            this.list = _list;

        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ListProcessRequestByVendorUserModel this[int index]
        {
            get { SrNo = 1; return list[index]; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CheckVendorRequestForVendorListItem, parent, false);

            ListProcessRequestByVendorUserModel item = this[position];
            if (item != null)
            {
                view.FindViewById<TextView>(Resource.Id.lblRPVendorRequestID).Text = item.RequestProcessSrNo == null ? "" : item.RequestProcessSrNo.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVendorEmployeeName).Text = item.EmployeeName == null ? "" : item.EmployeeName.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVendorStartTime).Text = item.VisitStartTime == null ? "" : item.VisitStartTime.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVendorEndTime).Text = item.VisitEndTime == null ? "" : item.VisitEndTime.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVendorReason).Text = item.VendorVisitResons == null ? "" : item.VendorVisitResons.ToString();

                SrNo++;
            }
            return view;

        }
    }
}