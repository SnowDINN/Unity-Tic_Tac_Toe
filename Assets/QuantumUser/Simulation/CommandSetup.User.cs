using System.Collections.Generic;
using Photon.Deterministic;
using Redbean.Network;

namespace Quantum
{
    public static partial class DeterministicCommandSetup
    {
        static partial void AddCommandFactoriesUser(ICollection<IDeterministicCommandFactory> factories, RuntimeConfig gameConfig, SimulationConfig simulationConfig)
        {
            factories.Add(new QCommandStoneMatch());
            factories.Add(new QCommandNextTurn());
            factories.Add(new QCommandStoneCreate());
            factories.Add(new QCommandStoneDestroy());
        }
    }
}