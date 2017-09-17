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
using Android.Telephony;

namespace SMSApp1
{
    [BroadcastReceiver]
    public class SmsAlarmReciever : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //alarmIntent.PutExtra("recieverPhoneNumber", msg.RecieverNumber);
            //alarmIntent.PutExtra("recieverMessage", msg.MessageContent);
            //alarmIntent.PutExtra("messageSendDate", msg.Date.ToString());

            string phoneNumber = intent.GetStringExtra("recieverPhoneNumber");
            string messageContent = intent.GetStringExtra("recieverMessage");
            string sendTime = intent.GetStringExtra("messageSendDate");

            //SmsManager.Default.SendTextMessage(phoneNumber, null, messageContent, null, null);

            Toast.MakeText(context, $"reciever - {phoneNumber}, content - {messageContent}, time - {sendTime}", ToastLength.Long);

            //Vibrator v = (Vibrator)context.GetSystemService(Context.VibratorService);
            //v.Vibrate(3000);

            Intent messageListIntent = new Intent(context, typeof(MessageList));
            PendingIntent pi = PendingIntent.GetActivity(context, 0, messageListIntent, PendingIntentFlags.OneShot);

            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle("Saved Message Sent")
                .SetContentIntent(pi)
                .SetContentText($"message sent to: {phoneNumber}, at: {sendTime}")
                .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                .SetSmallIcon(Resource.Drawable.Icon);

            Notification notification = builder.Build();

            NotificationManager notificationMgr = context.GetSystemService(Context.NotificationService) as NotificationManager;

            const int notificationId = 0;
            notificationMgr.Notify(notificationId, notification);


        }
    }
}