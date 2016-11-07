using System.Text;
using PrimitiveObsession.Config;

namespace PrimitiveObsession
{
    public interface IBuilder
    {
        string Build();
    }

    public interface IEngineBuilder : IBuilder
    {
    }

    public class EngineBuilder : IEngineBuilder
    {
        private readonly ITireBuilder _tireBuilder;
        private readonly PistonCount _pistonCount;

        public EngineBuilder(ITireBuilder tireBuilder, PistonCount pistonCount)
        {
            _tireBuilder = tireBuilder;
            _pistonCount = pistonCount;
        }

        public string Build()
        {
            var sb = new StringBuilder();

            sb.Append("Pistons: ");
            for (var i = 0; i < _pistonCount.Value; i++) sb.Append("||");

            sb.AppendLine();
            sb.Append(_tireBuilder.Build());

            return sb.ToString();
        }
    }
}