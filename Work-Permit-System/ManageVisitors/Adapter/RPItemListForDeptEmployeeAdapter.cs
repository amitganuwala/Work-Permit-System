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
    public class RPItemListForDeptEmployeeAdapter : BaseAdapter<ListProcessRequestByDepartmentEmployeeModel>
    {
        Activity context;
        List<ListProcessRequestByDepartmentEmployeeModel> list;
        int SrNo;

        public RPItemListForDeptEmployeeAdapter(Activity _context, List<ListProcessRequestByDepartmentEmployeeModel> _list)
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

        public override ListProcessRequestByDepartmentEmployeeModel this[int index]
        {
            get { SrNo = 1; return list[index]; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CheckVendorRequestListItem, parent, false);

            ListProcessRequestByDepartmentEmployeeModel item = this[position];
            if (item != null)
            {
                view.FindViewById<TextView>(Resource.Id.lblRPRequestID).Text = item.RequestProcessSrNo == null ? "" : item.RequestProcessSrNo.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPEmployeeName).Text = item.EmployeeName == null ? "" : item.EmployeeName.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPStartTime).Text = item.VisitStartTime == null ? "" : item.VisitStartTime.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPEndTime).Text = item.VisitEndTime == null ? "" : item.VisitEndTime.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPRequestStatus).Text = item.RequestStatus == null ? "" : item.RequestStatus.ToString();

                SrNo++;
            }
            return view;

        }
    }
}