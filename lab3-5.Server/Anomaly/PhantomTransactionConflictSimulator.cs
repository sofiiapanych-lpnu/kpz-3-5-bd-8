using Npgsql;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PhantomTransactionConflictSimulator
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=12345678;Database=deliverycourierservice;";

    public async Task SimulatePhantomAnomaly()
    {
        var connection1 = new NpgsqlConnection(_connectionString);
        var connection2 = new NpgsqlConnection(_connectionString);

        await connection1.OpenAsync();
        await connection2.OpenAsync();

        var transaction1 = await connection1.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);
        var transaction2 = await connection2.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        var task1 = Task.Run(async () =>
        {
            try
            {
                var command1 = new NpgsqlCommand("SELECT COUNT(*) FROM delivery WHERE status = 'pending'", connection1, transaction1);
                var count1 = (long)await command1.ExecuteScalarAsync();
                Console.WriteLine($"Transaction 1 - Initial Pending Deliveries: {count1}");

                Thread.Sleep(300);

                var command2 = new NpgsqlCommand("SELECT COUNT(*) FROM delivery WHERE status = 'pending'", connection1, transaction1);
                var count2 = (long)await command2.ExecuteScalarAsync();
                Console.WriteLine($"Transaction 1 - Final Pending Deliveries: {count2}");

                await transaction1.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in Transaction 1: {e.Message}");
            }
        });

        var task2 = Task.Run(async () =>
        {
            try
            {
                Thread.Sleep(100); 
                var insertCommand = new NpgsqlCommand(
                    "INSERT INTO delivery (delivery_id, order_id, start_time, status) VALUES (999, 102, '2024-11-02 14:00:00', 'pending')",
                    connection2,
                    transaction2
                );
                await insertCommand.ExecuteNonQueryAsync();
                Console.WriteLine("Transaction 2: Inserted new Pending delivery.");

                await transaction2.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in Transaction 2: {e.Message}");
            }
        });

        await Task.WhenAll(task1, task2);

        try
        {
            var finalCommand = new NpgsqlCommand("SELECT COUNT(*) FROM delivery WHERE status = 'pending'", connection1);
            var finalCount = (long)await finalCommand.ExecuteScalarAsync();
            Console.WriteLine($"Final Pending Deliveries in Database: {finalCount}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching final data: {e.Message}");
        }
    }
}
