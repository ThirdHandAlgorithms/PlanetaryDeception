using System;
using PlanetaryDeception;

namespace DialogRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var mode = args.Length > 0 ? args[0] : "help";

            switch (mode.ToLower())
            {
                case "flowershop":
                    RunFlowershop();
                    break;
                case "interrogation":
                    RunInterrogation();
                    break;
                case "console":
                    var network = args.Length > 1 ? args[1] : "VenusHome";
                    RunConsole(network);
                    break;
                default:
                    Console.WriteLine("Usage: DialogRunner <mode> [options]");
                    Console.WriteLine();
                    Console.WriteLine("Modes:");
                    Console.WriteLine("  flowershop              Run the flowershop dialog");
                    Console.WriteLine("  interrogation           Run the interrogation dialog");
                    Console.WriteLine("  console [network]       Run the SolarOS console");
                    Console.WriteLine();
                    Console.WriteLine("Networks: VenusHome, Venus, Venref, Space, EarthMoon, Mars, Ceres, Europa");
                    break;
            }
        }

        static void RunFlowershop()
        {
            var console = new CliDialogConsole();
            var controller = new LevelController_2();

            controller.FlowershopQA1(console, () => { console.End(); });
            console.Run();

            var inventory = PlayerInventory.Instance();
            if (inventory.ContainsItem(KnownItem.PinkRoses))
                Console.WriteLine("\n  [Purchased: Pink Roses]");
            else if (inventory.ContainsItem(KnownItem.RedRoses))
                Console.WriteLine("\n  [Purchased: Red Roses]");

            Console.WriteLine($"  [Wallet: {PlayerWallet.Instance().GetAmount()} solarbits]");
        }

        static void RunInterrogation()
        {
            var console = new CliDialogConsole();
            var controller = new Level_2_interrogation();

            controller.InitQuestions(console, () => { console.End(); });
            console.Run();

            var inventory = PlayerInventory.Instance();
            if (inventory.ContainsItem(KnownItem.VenusLaunchAssistanceTicket))
                Console.WriteLine("\n  [Received: Free Launch Assistance Ticket]");
            Console.WriteLine("  [Interrogation complete]");
        }

        static void RunConsole(string networkName)
        {
            SolarOSNetwork network;
            switch (networkName.ToLower())
            {
                case "venushome": network = SolarOSNetwork.VenusHome; break;
                case "venus": network = SolarOSNetwork.Venus; break;
                case "venref": network = SolarOSNetwork.Venref; break;
                case "space": network = SolarOSNetwork.Space; break;
                case "earthmoon": network = SolarOSNetwork.EarthMoon; break;
                case "mars": network = SolarOSNetwork.Mars; break;
                case "ceres": network = SolarOSNetwork.Ceres; break;
                case "europa": network = SolarOSNetwork.Europa; break;
                default:
                    Console.WriteLine($"Unknown network: {networkName}");
                    return;
            }

            var runner = new CliConsoleRunner(network);
            runner.Run();
        }
    }
}
