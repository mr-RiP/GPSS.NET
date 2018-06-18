using GPSS;
using GPSS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new Model()
                .Storage("ReqQueue", 10)
                .Generate(3, 1)
                .Label("Process")
                .Gate(
                    (sna => GateCondition.StorageNotFull),
                    (sna => "ReqQueue"),
                    (sna => "Overload"))
                .Enter(sna => "ReqQueue")
                .Seize(sna => "Processor")
                .Leave(sna => "ReqQueue")
                .Advance(0.5, 0.5)
                .Release(sna => "Processor")
                .Transfer(
                    (sna => TransferMode.Fractional),
                    (sna => 0.1),
                    (sna => "Finish"),
                    (sna => "Process"),
                    null)
                .Label("Finish")
                .Terminate(1)
                .Label("Overload")
                .Terminate(0);

            var simulation = new Simulation(model);
            var report = simulation.Start(1000);

            Console.ReadLine();
        }
    }
}
