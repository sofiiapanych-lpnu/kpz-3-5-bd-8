using System;
using System.Threading.Tasks;

class Programm
{
    static async Task Main(string[] args)
    {
        var simulator = new PhantomTransactionConflictSimulator();

        Console.WriteLine("Starting Phantom Anomaly Simulation...");
        await simulator.SimulatePhantomAnomaly();
        Console.WriteLine("Simulation Completed.");
    }
}
