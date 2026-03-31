namespace UrbanCareClient.Domain.DTOs
{
    public record ResidentResponseDTO(
        int Id,
        UserDataResponseDTO UserData,
        ApartmentResponseDTO Apartment,
        DateOnly MovingIntoDate,
        DateOnly? MovingOutDate,
        bool IsLiving);
}
