

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.TimeLine
{
    public class UniformTimeLine : ITimeLine
    {
        private readonly double _timeInterval;
        private readonly double _step;

        public UniformTimeLine(double timeInterval, double step)
        {
            _timeInterval = timeInterval;
            _step = step;
        }

        public int CountOfLayers() => (int)(_timeInterval / _step);

        public double TimeOnLayerByNumber(int number)
        {
            if (number < 0 || number >= CountOfLayers())
            {
                //throw new ArgumentOutOfRangeException(nameof(number));
            }

            return _step * number;
        }
    }
}
