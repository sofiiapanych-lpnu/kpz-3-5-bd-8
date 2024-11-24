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
            var deliveries = await _deliveryRepository.GetAllDeliveriesAsync();

            // ?????? ?? ????????
            if (!string.IsNullOrEmpty(status))
            {
                deliveries = deliveries.Where(d => d.Status !=null && d.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            // ?????? ?? ?????????? ?????
            if (startDate.HasValue)
            {
                deliveries = deliveries.Where(d => d.StartTime >= startDate.Value);
            }

            // ?????? ?? ???????? ?????
            if (endDate.HasValue)
            {
                deliveries = deliveries.Where(d => d.EndTime <= endDate.Value);
            }

            //// ?????? ?? ????????? ???????
            //if (!string.IsNullOrEmpty(query))
            //{
            //    string lowerQuery = query.ToLower();
            //    deliveries = deliveries.Where(d =>
            //        d.Status.ToLower().Contains(lowerQuery) ||
            //        (d.Description != null && d.Description.ToLower().Contains(lowerQuery))
            //    );
            //}

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
            //reloadContext();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            deliveryDTO.EndTime ??= null;
            deliveryDTO.ActualDuration ??= null;
            var updatedDelivery = new DeliveryDTO
            {
                DeliveryId = id,
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

            var updated = await _deliveryRepository.UpdateDeliveryAsync(id, updatedDelivery);
            if (!updated) return BadRequest();
            return Ok(deliveryDTO);
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
                current_transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
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
    }
}
