namespace Haystack.Random
{
    using System;

    /// <inheritdoc />
    public class SafeRandom :
        ISafeRandom
    {
        private readonly object _lock;
        private readonly Random _random;

        /// <inheritdoc />
        public SafeRandom(int seed)
        {
            _lock = new object();
            _random = new Random(seed);
        }

        /// <inheritdoc />
        public SafeRandom()
        {
            _lock = new object();
            _random = new Random();
        }

        /// <inheritdoc />
        public int Next()
        {
            lock (_lock)
            {
                return _random.Next();
            }
        }

        /// <inheritdoc />
        public int Next(int maxValue)
        {
            lock (_lock)
            {
                return _random.Next(maxValue);
            }
        }

        /// <inheritdoc />
        public int Next(int minValue, int maxValue)
        {
            lock (_lock)
            {
                return _random.Next(minValue, maxValue);
            }
        }

        /// <inheritdoc />
        public double NextDouble()
        {
            lock (_lock)
            {
                return _random.NextDouble();
            }
        }

        /// <inheritdoc />
        public double NextDouble(double minValue, double maxValue)
        {
            lock (_lock)
            {
                return _random.NextDouble(minValue, maxValue);
            }
        }

        /// <inheritdoc />
        public double NextDouble(double maxValue)
        {
            lock (_lock)
            {
                return _random.NextDouble(maxValue);
            }
        }

        /// <inheritdoc />
        public void NextBytes(byte[] buffer)
        {
            lock (_lock)
            {
                _random.NextBytes(buffer);
            }
        }
    }
}