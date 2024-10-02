using System.Collections.Generic;
using Photon.Deterministic;
using Redbean.Network;

namespace Quantum
{
    public static partial class DeterministicCommandSetup
    {
        static partial void AddCommandFactoriesUser(ICollection<IDeterministicCommandFactory> factories, RuntimeConfig gameConfig, SimulationConfig simulationConfig)
        {
            factories.Add(new QCommandGameEnd());
            factories.Add(new QCommandNextTurn());
            factories.Add(new QCommandTurnEnd());
        }
    }
}