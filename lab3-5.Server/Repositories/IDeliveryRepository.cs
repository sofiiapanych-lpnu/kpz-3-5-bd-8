using lab3_5.Server.Models;

namespace lab3_5.Server.Repositories
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<DeliveryDTO>> GetAllDeliveriesAsync();
        Task<DeliveryDTO?> GetDeliveryByIdAsync(int id);
        Task<DeliveryDTO> CreateDeliveryAsync(DeliveryDTO delivery);
        Task<bool> UpdateDeliveryAsync(int id, DeliveryDTO updatedDelivery);
        Task<bool> DeleteDeliveryAsync(int id);
        Task AssignAvailableCouriersToDeliveriesAsync();
        void setContext(DeliverycourierserviceContext context);
    }
}
