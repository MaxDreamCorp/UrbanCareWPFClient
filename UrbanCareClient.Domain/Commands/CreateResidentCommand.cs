namespace UrbanCareClient.Domain.Commands
{
    public record CreateResidentCommand(int UserId,
        int ApartmentId,
        DateOnly MovingIntoDate);
}
