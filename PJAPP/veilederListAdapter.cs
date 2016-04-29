using System.Collections.Generic;
using Android.Content;
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

            TextView txtFag= row.FindViewById<TextView>(Resource.Id.txtFag);
            txtFag.Text = mItems[position].FagID;

            TextView txtNavn = row.FindViewById<TextView>(Resource.Id.txtVeilederNavn);
            txtNavn.Text = mItems[position].VeilederNavn;

            return row;
        }
    }
}