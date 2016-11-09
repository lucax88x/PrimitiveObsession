using PrimitiveObsession.Config;

namespace PrimitiveObsession
{
    public class EngineBuilder
    {
        private readonly TireBuilder _tirebuilder;
        public PistonCount PistonCount { get; }

        public EngineBuilder(TireBuilder tirebuilder, PistonCount pistonPistonCount)
        {
            _tirebuilder = tirebuilder;
            PistonCount = pistonPistonCount;
        }
    }
}