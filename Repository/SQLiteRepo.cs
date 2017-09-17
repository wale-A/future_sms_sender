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
using System.IO;
using SQLite;
using SMSApp1.DAL;

namespace SMSApp1.Repository
{
    public class SQLiteRepo
    {
        static SQLiteConnection db;
        object locker;
        //public SQLiteRepo(SQLiteConnection _db)
        //{
        //    db = _db;
        //}

        public SQLiteRepo()
        {
            locker = new object();
            SQLiteRepo.createDb();
            //populateDbWithTestData();
        }

        private static void createDb()
        {
            if (db == null)
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "smsAppDatabase.db3");
                //if (!File.Exists(dbPath))
                //{
                    db = new SQLiteConnection(dbPath);
                db.CreateTable<SimpleMessage>();
                //}
            }
        }

        public SimpleMessage insertMessage(SimpleMessage msg)
        {
            try
            {
                lock (locker)
                {
                    var result = db.Insert(msg);
                    if (result > 0)
                    {
                        return msg;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
                //throw;
            }
        }

        public SimpleMessage getMessage(int id)
        {
            try
            {
                lock (locker)
                {
                    return db.Get<SimpleMessage>(id);
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public void deleteMessage(int id)
        {
            try
            {
                lock (locker)
                {
                    db.Delete<SimpleMessage>(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void deleteAllMessages()
        {
            try
            {
                lock (locker)
                {
                    //db.Delete("", 1, null)
                    db.Execute("delete from Messages", new object[] { });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<SimpleMessage> getMessages()
        {
            lock (locker)
            {
                return db.Table<SimpleMessage>().AsQueryable();
            }
        }

        private void populateDbWithTestData()
        {
            try
            {
                lock (locker)
                {
                    if (getMessages().Count() == 0)
                    //db.DeleteAll<SimpleMessage>();
                    {
                        IEnumerable<SimpleMessage> msgs = new List<SimpleMessage>()
                {
                    new SimpleMessage() { MessageContent = "lorem ipsum", RecieverNumber = "08081658689", Date = DateTime.Now.AddDays(-1234)},
                    new SimpleMessage() { MessageContent = "lorem ipsum dolot sit amen", RecieverNumber = "+2348081658689", Date = DateTime.Now.AddDays(-4123)},
                    new SimpleMessage() { MessageContent = "quos quia harum excepturi soluta consequatur temporibus rerum.", RecieverNumber = "1281658689", Date = DateTime.Now.AddDays(34)},
                    new SimpleMessage() { MessageContent = "unt nostrum dolore voluptates. Possimus ", RecieverNumber = "+892689", Date = DateTime.Now.AddDays(-14)},
                    new SimpleMessage() { MessageContent = "quasi quia asperiores et. Praesentium labore consequatur architecto qui", RecieverNumber = "5120020", Date = DateTime.Now.AddDays(-92)},
                    new SimpleMessage() { MessageContent = "Quis doloremque blanditiis corporis est sit vitae provident rerum. Ut quo quo qui ut harum.", RecieverNumber = "5048618689", Date = DateTime.Now.AddDays(34)},
                    new SimpleMessage() { MessageContent = "nim quos quia harum excepturi soluta cons", RecieverNumber = "513201651", Date = DateTime.Now.AddDays(94)},
                    new SimpleMessage() { MessageContent = "Perspiciatis sed id est provident ullam", RecieverNumber = "81658689", Date = DateTime.Now.AddDays(3)},
                    new SimpleMessage() { MessageContent = "raesentium labore consequatur architecto qui", RecieverNumber = "6520113100", Date = DateTime.Now.AddDays(54)},
                    new SimpleMessage() { MessageContent = "lorem sgfvb s jsdfjlm dolot sit amen", RecieverNumber = "46656559",Date = DateTime.Now.AddDays(1234)},
                    new SimpleMessage() { MessageContent = "lo0874 eghbdv 08784rem ipsum", RecieverNumber = "650415655", Date = DateTime.Now.AddDays(-854)},
                    new SimpleMessage() { MessageContent = "lorem ipsum dolot sit amen", RecieverNumber = "+23210569", Date = DateTime.Now.AddDays(934)},
                };

                        var result = msgs.Select(x => insertMessage(x));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}