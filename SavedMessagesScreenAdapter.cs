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
using SMSApp1.DAL;

namespace SMSApp1
{
    public class SavedMessagesScreenAdapter : BaseAdapter<SimpleMessage>
    {
        IEnumerable<SimpleMessage> messages;
        Activity context;

        public SavedMessagesScreenAdapter(Activity ctx, List<SimpleMessage> _msgs)
            :base()
        {
            context = ctx;
            messages = _msgs;
        }

        public override SimpleMessage this[int position] => messages.ElementAtOrDefault(position);

        public override int Count => messages.Count();

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Resource.Layout.message_item, null);
                //convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            }
            var msg = messages.ElementAtOrDefault(position);
            convertView.FindViewById<TextView>(Resource.Id.msgItem_textMessage).Text = msg.MessageContent;
            convertView.FindViewById<TextView>(Resource.Id.msgItem_phoneNumber).Text = msg.RecieverNumber;
            convertView.FindViewById<TextView>(Resource.Id.msgItem_date).Text = msg.Date.ToString("dd-MM-yyyy hh:mm");
            //convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = msg.RecieverNumber;
            //convertView.FindViewById<TextView>(Android.Resource.Id.Text2).Text = msg.MessageContent;

            return convertView;
        }
    }
}