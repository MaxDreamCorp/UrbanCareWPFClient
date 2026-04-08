using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.WPF.Views.UserControls.ViewModels
{
    public class MaterialControlViewModel
    {
        public MaterialResponseDTO Material { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
