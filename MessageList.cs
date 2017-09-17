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
using SMSApp1.Repository;

namespace SMSApp1
{
    [Activity(Label = "SMSApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MessageList : ListActivity 
    {
        IEnumerable<SimpleMessage> msgs;
        SQLiteRepo db;

        public MessageList()
        {
            db = new SQLiteRepo();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return true;
            //return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.msgMenu_newMessage:
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    return true;
                case Resource.Id.msgMenu_clearMessages:
                    ConfirmDelete();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void ConfirmDelete()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm Delete");
            alert.SetMessage("Are you sure you want to delete all the messages ?");
            alert.SetPositiveButton("Delete", (senderAlert, args) =>
            {
                db.deleteAllMessages();
                Finish();
                StartActivity(typeof(MessageList));
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var allMessages = db.getMessages().ToList();
            //db.insertMessage(new SimpleMessage() {
            //    MessageContent = "this is a test message to check the database", RecieverNumber = "080833202783"
            //});
            ListAdapter = new SavedMessagesScreenAdapter(this, allMessages);

            ListView.TextFilterEnabled = true;
            ListView.ItemClick += MessageItemClicked;
        }

        private void MessageItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Toast.MakeText(Application, (e.View))
        }
    }
}