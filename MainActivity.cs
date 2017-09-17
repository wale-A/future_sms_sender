using Android.App;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using System.IO;

namespace SMSApp1
{
    [Activity(Label = "SMSApp", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button saveButton;
        EditText numberEditText;
        EditText messageEditText;
        EditText dateEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            numberEditText = FindViewById<EditText>(Resource.Id.createMessage_number);
            messageEditText = FindViewById<EditText>(Resource.Id.createMessage_message);
            dateEditText = FindViewById<EditText>(Resource.Id.createMessage_date);

            saveButton = FindViewById<Button>(Resource.Id.createMessage_saveButton);
            saveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            var msg = saveMessage();

            Android.Content.Intent alarmIntent = new Android.Content.Intent(this, typeof(SmsAlarmReciever));
            alarmIntent.PutExtra("recieverPhoneNumber", msg.RecieverNumber);
            alarmIntent.PutExtra("recieverMessage", msg.MessageContent);
            alarmIntent.PutExtra("messageSendDate", msg.Date.ToString());



            //var triggerTime = msg.Date.AddHours(1).Subtract(System.DateTime.UtcNow).TotalMilliseconds;
            //var x = System.Convert.ToInt64(triggerTime);
            //Java.Lang.JavaSystem.tim
            var timeDifference = System.Convert.ToInt64(msg.Date.Subtract(System.DateTime.Now).TotalMilliseconds);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);
            alarmManager.SetExact(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis() + timeDifference, pendingIntent);

            //showNotification(msg);

            var intent = new Android.Content.Intent(this, typeof(MessageList));
            StartActivity(intent);
        }

        private void showNotification(DAL.SimpleMessage msg)
        {
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("Saved Message Sent")
                .SetContentText($"message sent to: {msg.RecieverNumber}, at; {msg.Date.ToString("dd-MM-yyyy")}")
                .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                .SetSmallIcon(Resource.Drawable.Icon);

            Notification notification = builder.Build();

            NotificationManager notificationMgr = GetSystemService(NotificationService) as NotificationManager;

            const int notificationId = 0;
            notificationMgr.Notify(notificationId, notification);
        }

        private DAL.SimpleMessage saveMessage()
        {
            var repo = new Repository.SQLiteRepo();
            var msg = repo.insertMessage(new DAL.SimpleMessage()
            {
                MessageContent = messageEditText.Text,
                RecieverNumber = numberEditText.Text,
                Date = System.DateTime.Parse(dateEditText.Text)
            });
            return msg;
        }

        private void clearTextAndChangeFocus()
        {
            numberEditText.Text = "";
            messageEditText.Text = "";
            Toast.MakeText(Application, "text sent !", ToastLength.Short).Show();
            numberEditText.RequestFocus();
        }

        private void createDb()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smsAppDatabase.db3");

        }
    }
}

