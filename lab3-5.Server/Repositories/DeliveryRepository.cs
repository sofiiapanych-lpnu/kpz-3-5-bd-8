using lab3_5.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_5.Server.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private static DeliverycourierserviceContext _context;

        public DeliveryRepository(DeliverycourierserviceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryDTO>> GetAllDeliveriesAsync()
        {
            return await _context.Deliveries
                .Include(d => d.Address)
                .Include(d => d.Courier)
                .Include(d => d.Order)
                .Include(d => d.Warehouse)
                .Select(d => new DeliveryDTO
                {
                    DeliveryId = d.DeliveryId,
                    OrderId = d.OrderId,
                    CourierId = d.CourierId,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    DesiredDuration = d.DesiredDuration,
                    ActualDuration = d.ActualDuration,
                    WarehouseId = d.WarehouseId,
                    AddressId = d.AddressId,
                    Status = d.Status
                })
                .ToListAsync();
        }

        public async Task<DeliveryDTO?> GetDeliveryByIdAsync(int id)
        {
            var delivery = await _context.Deliveries
                .Include(d => d.Address)
                .Include(d => d.Courier)
                .Include(d => d.Order)
                .Include(d => d.Warehouse)
                .FirstOrDefaultAsync(d => d.DeliveryId == id);

            if (delivery == null)
                return null;

            return new DeliveryDTO
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                CourierId = delivery.CourierId,
                StartTime = delivery.StartTime,
                EndTime = delivery.EndTime,
                DesiredDuration = delivery.DesiredDuration,
                ActualDuration = delivery.ActualDuration,
                WarehouseId = delivery.WarehouseId,
                AddressId = delivery.AddressId,
                Status = delivery.Status
            };
        }

        public async Task<DeliveryDTO> CreateDeliveryAsync(DeliveryDTO deliveryDTO)
        {
            var delivery = new Delivery
            {
                OrderId = deliveryDTO.OrderId,
                CourierId = deliveryDTO.CourierId,
                StartTime = deliveryDTO.StartTime,
                EndTime = deliveryDTO.EndTime,
                DesiredDuration = deliveryDTO.DesiredDuration,
                ActualDuration = deliveryDTO.ActualDuration,
                WarehouseId = deliveryDTO.WarehouseId,
                AddressId = deliveryDTO.AddressId,
                Status = deliveryDTO.Status
            };

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            return new DeliveryDTO
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                CourierId = delivery.CourierId,
                StartTime = delivery.StartTime,
                EndTime = delivery.EndTime,
                DesiredDuration = delivery.DesiredDuration,
                ActualDuration = delivery.ActualDuration,
                WarehouseId = delivery.WarehouseId,
                AddressId = delivery.AddressId,
                Status = delivery.Status
            };
        }

        public async Task<bool> UpdateDeliveryAsync(int id, DeliveryDTO updatedDeliveryDTO)
        {
            if (id != updatedDeliveryDTO.DeliveryId)
                return false;

            var existingDelivery = await _context.Deliveries.FindAsync(id);
            if (existingDelivery == null)
                return false;

            existingDelivery.OrderId = updatedDeliveryDTO.OrderId;
            existingDelivery.CourierId = updatedDeliveryDTO.CourierId;
            existingDelivery.StartTime = updatedDeliveryDTO.StartTime;
            existingDelivery.EndTime = updatedDeliveryDTO.EndTime;
            existingDelivery.DesiredDuration = updatedDeliveryDTO.DesiredDuration;
            existingDelivery.ActualDuration = updatedDeliveryDTO.ActualDuration;
            existingDelivery.WarehouseId = updatedDeliveryDTO.WarehouseId;
            existingDelivery.AddressId = updatedDeliveryDTO.AddressId;
            existingDelivery.Status = updatedDeliveryDTO.Status;

            _context.Entry(existingDelivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteDeliveryAsync(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
                return false;

            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool DeliveryExists(int id)
        {
            return _context.Deliveries.Any(d => d.DeliveryId == id);
        }

        public async Task AssignAvailableCouriersToDeliveriesAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("CALL AssignAvailableCouriersToDeliveries();");
        }

        public void setContext(DeliverycourierserviceContext context)
        {
            _context = context;
        }


    }
}
