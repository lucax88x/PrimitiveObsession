using System.Text;
using PrimitiveObsession.Config;

namespace PrimitiveObsession
{
    public class EngineBuilder
    {
        private readonly TireBuilder _tireBuilder;
        private readonly PistonCount _pistonCount;

        public EngineBuilder(TireBuilder tireBuilder, PistonCount pistonCount)
        {
            _tireBuilder = tireBuilder;
            _pistonCount = pistonCount;
        }

        public string Build()
        {
            var sb = new StringBuilder();

            sb.Append("Pistons: ");
            for (var i = 0; i < _pistonCount; i++) sb.Append("||");

            sb.AppendLine();
            sb.Append(_tireBuilder.Build());

            return sb.ToString();
        }
    }
}