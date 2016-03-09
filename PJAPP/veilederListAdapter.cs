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
    public class veilederListAdapter : BaseAdapter<veileder>
    {
        private List<veileder> mItems;
        private Context mContext;

        public veilederListAdapter(Context context, List<veileder> items)
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
        public override veileder this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.veilederRow, null, false);
            }

            /*TextView txtMinor = row.FindViewById<TextView>(Resource.Id.txtMinor);
            txtMinor.Text = mItems[position].minor.ToString();

            TextView txtMajor = row.FindViewById<TextView>(Resource.Id.txtMajor);
            txtMajor.Text = mItems[position].major.ToString();
            */
            TextView txtFag= row.FindViewById<TextView>(Resource.Id.txtFag);
            txtFag.Text = mItems[position].FagID;

            TextView txtNavn = row.FindViewById<TextView>(Resource.Id.txtNavn);
            txtNavn.Text = mItems[position].VeilederNavn;

            return row;
        }
    }
}