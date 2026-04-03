using System.ComponentModel.DataAnnotations;

namespace UrbanCareClient.Domain.Enums
{
    public enum OrderStatusEnum
    {
        [Display(Name = "Новый")]
        New = 1,

        [Display(Name = "Назначен исполнитель")]
        ExecutorAppointed = 2,

        [Display(Name = "В работе")]
        InProgress = 3,

        [Display(Name = "Отмечен выполненным исполнителем")]
        MarkedAsCompletedByExecutor = 4,

        [Display(Name = "Ожидает оплаты")]
        PendingPayment = 5,

        [Display(Name = "Завершен")]
        Completed = 6,

        [Display(Name = "Отменен")]
        Canceled = 7
    }
}
