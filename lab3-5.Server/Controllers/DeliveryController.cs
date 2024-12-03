using lab3_5.Server.Models;
using lab3_5.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace lab3_5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private static IDeliveryRepository _deliveryRepository;

        private static DeliverycourierserviceContext _context = new DeliverycourierserviceContext();
        private static IDbContextTransaction current_transaction;

        private void reloadContext()
        {
            if (current_transaction != null)
            {
                try
                {
                    current_transaction.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        current_transaction.Dispose();
                        current_transaction = null;
                    }
                    catch (Exception ex2)
                    {
                        current_transaction = null;
                    }

                    try
                    {
                        _context.Dispose();
                    }
                    catch (Exception e)
                    {

                    }


                }
            }
            _context = new DeliverycourierserviceContext();
            _deliveryRepository.setContext(_context);
        }

        public DeliveryController(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryDTO>>> GetDeliveries(
    [FromQuery] string? status,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate)
        {
            reloadContext();
            var query = _context.Deliveries.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                var normalizedStatus = status.ToLower();
                query = query.Where(d => d.Status != null && d.Status.ToLower() == normalizedStatus);
            }

            if (startDate.HasValue)
            {
                query = query.Where(d => d.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(d => d.EndTime <= endDate.Value);
            }

            var deliveries = await query
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

            return Ok(deliveries);
        }

        //// GET: api/Delivery
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<DeliveryDTO>>> GetDeliveries()
        //{
        //    var deliveriesDTO = await _deliveryRepository.GetAllDeliveriesAsync();
        //    return Ok(deliveriesDTO);
        //}

        // GET: api/Delivery/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryDTO>> GetDelivery(int id)
        {
            //reloadContext();
            _deliveryRepository.setContext(_context);
            var deliveryDTO = await _deliveryRepository.GetDeliveryByIdAsync(id);
            if (deliveryDTO == null)
            {
                return NotFound();
            }
            return Ok(deliveryDTO);
        }

        // POST: api/Delivery
        [HttpPost]
        public async Task<ActionResult<DeliveryDTO>> PostDelivery(DeliveryDTO deliveryDTO)
        {
            reloadContext();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                deliveryDTO.EndTime ??= null;
                deliveryDTO.ActualDuration ??= null;
                deliveryDTO.CourierId ??= null;

                var delivery = new DeliveryDTO
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

                var createdDelivery = await _deliveryRepository.CreateDeliveryAsync(delivery);

                deliveryDTO.DeliveryId = createdDelivery.DeliveryId;
                return CreatedAtAction(nameof(GetDelivery), new { id = createdDelivery.DeliveryId }, deliveryDTO);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(401, new { message = "Error: " + e.Message, details = e.InnerException?.Message });
            }
        }

        // PUT: api/Delivery/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DeliveryDTO>> PutDelivery(int id, DeliveryDTO deliveryDTO)
        {
            try
            {
                    //reloadContext();
                    if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                deliveryDTO.EndTime ??= null;
                deliveryDTO.ActualDuration ??= null;
                //var updatedDelivery = new DeliveryDTO
                //{
                //    DeliveryId = id,
                //    OrderId = deliveryDTO.OrderId,
                //    CourierId = deliveryDTO.CourierId,
                //    StartTime = deliveryDTO.StartTime,
                //    EndTime = deliveryDTO.EndTime,
                //    DesiredDuration = deliveryDTO.DesiredDuration,
                //    ActualDuration = deliveryDTO.ActualDuration,
                //    WarehouseId = deliveryDTO.WarehouseId,
                //    AddressId = deliveryDTO.AddressId,
                //    Status = deliveryDTO.Status
                //};
                deliveryDTO.DeliveryId = id;
                //var updated = await _deliveryRepository.UpdateDeliveryAsync(id, updatedDelivery);
                var updated = await UpdateDeliveryAsync_(id, deliveryDTO);

                return Ok(updated);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(401, new { message = "Error: " + e.Message, details = e.InnerException?.Message });
            }
        }

        // DELETE: api/Delivery/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            reloadContext();
            var deleted = await _deliveryRepository.DeleteDeliveryAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("assign-couriers")]
        public async Task<IActionResult> AssignAvailableCouriersToDeliveries()
        {
            reloadContext();
            try
            {
                await _deliveryRepository.AssignAvailableCouriersToDeliveriesAsync();
                return Ok(new { message = "Couriers assigned to pending deliveries successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while assigning couriers.", details = ex.Message });
            }
        }

        [HttpPost("start-transaction")]
        public async Task<IActionResult> StartTransaction()
        {
            return Ok(await StartTransactionAsync_());
        }


        [HttpPost("commit")]
        public async Task<IActionResult> Commit()
        {
            return Ok(await Commit_());
        }

        private async Task<string> StartTransactionAsync_()
        {
            string msg = "";
            if (current_transaction != null)
            {
                try
                {
                    await current_transaction.CommitAsync();
                    msg += "\nprevious transaction commited\n";
                }
                catch (InvalidOperationException ex)
                {
                    current_transaction.Dispose();
                    msg += "\nprevious transaction disposed\n";
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }

            try
            {
                current_transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
                return msg + "\nTransaction started.\n";
            }
            catch (InvalidOperationException ex)
            {
                return "Failed to start transaction: " + ex.Message;
            }
            catch (DbUpdateException ex)
            {
                return "Database update error: " + ex.Message;
            }
        }

        private async Task<string> Commit_()
        {
            if (current_transaction != null)
            {
                try
                {
                    await current_transaction.CommitAsync();
                    return "Committed successfully.";
                }
                catch (InvalidOperationException ex)
                {
                    return "Invalid operation during commit: " + ex.Message;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await current_transaction.RollbackAsync();
                    current_transaction.Dispose();
                    return "Concurrency error during commit: " + ex.Message;
                }
                catch (Exception e)
                {
                    try
                    {
                        await current_transaction.RollbackAsync();
                        current_transaction.Dispose();
                    }
                    catch (Exception rollbackEx)
                    {
                        return "Rollback failed: " + rollbackEx.Message;
                    }

                    return "Unexpected error during commit: " + e.Message;
                }
            }
            else
            {
                return "No active transaction.";
            }
        }

        private async Task<string> UpdateDeliveryAsync_(int id, DeliveryDTO updatedDeliveryDTO)
        {
            if (id != updatedDeliveryDTO.DeliveryId)
                return "Params not valid";

            var existingDelivery = await _context.Deliveries.FindAsync(id);
            if (existingDelivery == null)
                return "Delivery is not found";

            existingDelivery.OrderId = updatedDeliveryDTO.OrderId;
            existingDelivery.CourierId = updatedDeliveryDTO.CourierId;
            existingDelivery.StartTime = updatedDeliveryDTO.StartTime;
            existingDelivery.EndTime = updatedDeliveryDTO.EndTime;
            existingDelivery.DesiredDuration = updatedDeliveryDTO.DesiredDuration;
            existingDelivery.ActualDuration = updatedDeliveryDTO.ActualDuration;
            existingDelivery.WarehouseId = updatedDeliveryDTO.WarehouseId;
            existingDelivery.AddressId = updatedDeliveryDTO.AddressId;
            existingDelivery.Status = updatedDeliveryDTO.Status;

            //_context.Entry(existingDelivery).State = EntityState.Modified;

            try
            {
                _context.Database.SetCommandTimeout(2);
                _context.SaveChanges();
                return "Changes saved.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return "Concurrency error while saving: " + ex.Message;
            }
            catch (DbUpdateException ex)
            {
                return "Database update error: " + ex.Message;
            }
            catch (Exception ex)
            {
                if (current_transaction != null)
                {
                    try
                    {
                        current_transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            current_transaction.Dispose();
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                        }
                    }
                }
                reloadContext();
                return "Unexpected error: " + ex.Message;
            }
        }
    }
}
