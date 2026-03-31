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
        public static SolidColorBrush InProgressBgBrush => GetBrush("InProgressBgBrush");
        public static SolidColorBrush InProgressBrush => GetBrush("InProgressBrush");
        public static SolidColorBrush WaitingForPaymentBgBrush => GetBrush("WaitingForPaymentBgBrush");
        public static SolidColorBrush WaitingForPaymentBrush => GetBrush("WaitingForPaymentBrush");
        public static SolidColorBrush FinishedBgBrush => GetBrush("FinishedBgBrush");
        public static SolidColorBrush FinishedBrush => GetBrush("FinishedBrush");
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
