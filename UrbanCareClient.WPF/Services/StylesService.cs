using System.Windows.Media;

namespace UrbanCareClient.WPF.Services
{
    public static class StylesService
    {
        private static SolidColorBrush GetBrush(string key)
        {
            return System.Windows.Application.Current.TryFindResource(key) as SolidColorBrush
                   ?? new SolidColorBrush(Colors.Transparent);
        }

        public static SolidColorBrush NewBgBrush => GetBrush("NewBgBrush");
        public static SolidColorBrush NewBrush => GetBrush("NewBrush");
        public static SolidColorBrush ExecutorAppointedBgBrush => GetBrush("ExecutorAppointedBgBrush");
        public static SolidColorBrush ExecutorAppointedBrush => GetBrush("ExecutorAppointedBrush");
        public static SolidColorBrush MarkedAsCompletedBgBrush => GetBrush("MarkedAsCompletedBgBrush");
        public static SolidColorBrush MarkedAsCompletedBrush => GetBrush("MarkedAsCompletedBrush");
        public static SolidColorBrush InProgressBgBrush => GetBrush("InProgressBgBrush");
        public static SolidColorBrush InProgressBrush => GetBrush("InProgressBrush");
        public static SolidColorBrush PendingPaymentBgBrush => GetBrush("PendingPaymentBgBrush");
        public static SolidColorBrush PendingPaymentBrush => GetBrush("PendingPaymentBrush");
        public static SolidColorBrush CompletedBgBrush => GetBrush("CompletedBgBrush");
        public static SolidColorBrush CompletedBrush => GetBrush("CompletedBrush");
        public static SolidColorBrush CanceledBgBrush => GetBrush("CanceledBgBrush");
        public static SolidColorBrush CanceledBrush => GetBrush("CanceledBrush");

        // Critical
        public static SolidColorBrush PriorityCriticalBrush => GetBrush("PriorityCriticalBrush");
        public static SolidColorBrush PriorityCriticalBgBrush => GetBrush("PriorityCriticalBgBrush");

        // High
        public static SolidColorBrush PriorityHighBrush => GetBrush("PriorityHighBrush");
        public static SolidColorBrush PriorityHighBgBrush => GetBrush("PriorityHighBgBrush");

        // Medium
        public static SolidColorBrush PriorityMediumBrush => GetBrush("PriorityMediumBrush");
        public static SolidColorBrush PriorityMediumBgBrush => GetBrush("PriorityMediumBgBrush");

        // Low
        public static SolidColorBrush PriorityLowBrush => GetBrush("PriorityLowBrush");
        public static SolidColorBrush PriorityLowBgBrush => GetBrush("PriorityLowBgBrush");

        // Planned
        public static SolidColorBrush PriorityPlannedBrush => GetBrush("PriorityPlannedBrush");
        public static SolidColorBrush PriorityPlannedBgBrush => GetBrush("PriorityPlannedBgBrush");

    }
}
