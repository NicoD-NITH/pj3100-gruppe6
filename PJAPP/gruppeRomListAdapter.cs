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

namespace PJAPP
{
    public class gruppeRomListAdapter : BaseAdapter<PJAPP.RomBeacon>
    {
        private List<PJAPP.RomBeacon> mItems;
        private Context mContext;

        public gruppeRomListAdapter(Context context, List<PJAPP.RomBeacon> items)
        {
            mItems = items;
            mContext = context;
        }
        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override PJAPP.RomBeacon this[int position]
        {
            get
            {
                return mItems[position];
            }
        }
        public override bool IsEnabled(int position)
        {
            return true;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(PJAPP.Resource.Layout.gruppeRomRow, null, false);
            }

            TextView txtDistance = row.FindViewById<TextView>(Resource.Id.txtDistance);
            txtDistance.Text = mItems[position].distance.ToString() + "m";

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            txtName.Text = mItems[position].RomNavn;

            return row;
        }
    }
}