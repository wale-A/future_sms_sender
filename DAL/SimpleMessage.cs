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
using SQLite;

namespace SMSApp1.DAL
{
    [Table("Messages")]
    public class SimpleMessage
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(14)]
        [NotNull]
        public string RecieverNumber { get; set; }

        [NotNull]
        public string MessageContent { get; set; }

        [NotNull]
        public DateTime Date { get; set; }
    }
}