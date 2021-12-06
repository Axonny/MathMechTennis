using System;

namespace App
{
    public static class BugReporter
    {
        private static Action<Exception> reportSendingMethods;
        
        public static event Action<Exception> OnReportSend
        {
            add => reportSendingMethods += value;
            remove => reportSendingMethods -= value;
        }

        public static void SendReport(Exception exception)
        {
            reportSendingMethods(exception);
        }
    }
}