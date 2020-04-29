using System;
using System.IO;
using System.Threading;

namespace ProgrammationEvent
{
    class Program
    {
        public event EventHandler<SendLogEventArgs> OnSendLog;
        static void Main(string[] args)
        {
            var program = new Program();
            var logger = new StandardOutputLogger();
            var fileLog = new FileStreamOutputLogger();

            logger.Subscribe(program);
            fileLog.Subscribe(program);

            var eventArgs = new SendLogEventArgs("LogEvent published", DateTime.Now);
   
            if (program.OnSendLog != null)
            {
                program.OnSendLog(program, eventArgs);
            }
        }


    }

    class StandardOutputLogger
    {
        public void Subscribe(Program program)
        {
            program.OnSendLog += OnLogSent;
        }

        public void OnLogSent(object sender, SendLogEventArgs args)
        {
            Write(args.Message, args.DateTime);
        }


        public void Write(String message, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }
            String formattedMessage = String.Format("{0} - {1}", dateTime, message);
            Console.WriteLine(formattedMessage);
        }
    }

    class FileStreamOutputLogger
    {

        public void Subscribe(Program program)
        {
            program.OnSendLog += OnLogSent;
        }

        public void OnLogSent(object sender, SendLogEventArgs args)
        {
            Write(args.Message, args.DateTime);
        }


        public void Write(String message, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }
            String formattedMessage = String.Format("{0} - {1}", dateTime, message);
            TextWriter textWrite = new StreamWriter(@"..\netcoreapp3.1\logFile.txt");
            textWrite.Write(formattedMessage);
            textWrite.Flush();
            textWrite.Close();
            textWrite = null;
        }

    }

    public class LogSender
    {
        public event EventHandler LogSended;

        public void StartLogSend()
        {
            Console.WriteLine("Start : Log send");
            OnLogSended(EventArgs.Empty);
        }

        protected virtual void OnLogSended(EventArgs e)
        {
            LogSended?.Invoke(this, e);
        }
    }

    class SendLogEventArgs : EventArgs
    {
        public String Message;
        public DateTime DateTime;

        public SendLogEventArgs(String message, DateTime dateTime)
        {
            Message = message;
            DateTime = dateTime;
        }
    }
}
