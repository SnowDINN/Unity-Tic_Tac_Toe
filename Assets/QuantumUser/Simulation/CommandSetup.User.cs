using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum
{
    public static partial class DeterministicCommandSetup
    {
        static partial void AddCommandFactoriesUser(ICollection<IDeterministicCommandFactory> factories, RuntimeConfig gameConfig, SimulationConfig simulationConfig)
        {
            factories.Add(new QCommandGameResult());
            factories.Add(new QCommandGameTurn());
            factories.Add(new QCommandGameVote());
        }
    }
}