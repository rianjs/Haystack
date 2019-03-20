using System.Threading;
using System.Threading.Tasks;

namespace Haystack.Random
{
    using System;

    /// <inheritdoc />
    public class AsyncSafeRandom
        : IAsyncSafeRandom
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly Random _random;

        /// <inheritdoc />
        public AsyncSafeRandom(int seed)
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _random = new Random(seed);
        }

        /// <inheritdoc />
        public AsyncSafeRandom()
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _random = new Random();
        }

        /// <inheritdoc />
        public async Task<int> NextAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.Next();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<int> NextAsync(int maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.Next(maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<int> NextAsync(int minValue, int maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.Next(minValue, maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<double> NextDoubleAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.NextDouble(); ;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<double> NextDoubleAsync(double minValue, double maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.NextDouble(minValue, maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<double> NextDoubleAsync(double maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.NextDouble(maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task NextBytesAsync(byte[] buffer)
        {
            try
            {
                await _semaphore.WaitAsync();
                _random.NextBytes(buffer);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}