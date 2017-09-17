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
using System.Threading;
using Android.Util;

namespace SMSApp1
{
    [Service]
    public class SmsService : Android.App.Service
    {
        static readonly string TAG = "X:" + typeof(SmsService).Name;
        static readonly int TimeWait = 4000;
        Timer timer;
        DateTime startTime;
        bool isStarted = false;

        public override void OnCreate()
        {
            base.OnCreate();
        }


        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug(TAG, $"OnStartCommand called atr {startTime}, flags = {flags}, startid = {startId}");
            if (isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                Log.Debug(TAG, $"this server was already started, it's been running for {runtime:c}.");
            }
            else
            {
                startTime = DateTime.UtcNow;
                Log.Debug(TAG, $"starting the service, at {startTime}");
                timer = new Timer(HandleTimerCallback, startTime, 0, TimeWait);
                isStarted = true;
            }
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            //started service not bound service.....return null
            return null;
        }

        public override void OnDestroy()
        {
            timer.Dispose();
            timer = null;
            isStarted = false;

            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"simple service destroyed at {DateTime.UtcNow} after running for {runtime:c}");
            base.OnDestroy();
        }

        private void HandleTimerCallback(object state)
        {
            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"this service has been running for {runtime:c} (since ${state}).");
        }
    }
}