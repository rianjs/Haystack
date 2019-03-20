using System.Threading.Tasks;

namespace Haystack.Random
{
    /// <summary>
    /// A thread-safe wrapper around the Random class that uses SemaphorSlim for synchronization safety. If you need deterministic behavior, you specify the
    /// random  seed, OR you can mock the IAsyncSafeRandom interface, which is a bit more convenient.
    /// - This class is designed to be created once and reused everywhere in your application
    /// - Creating two of these objects in quick succession can result in the same seed being reused
    /// </summary>
    public interface IAsyncSafeRandom
    {
        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        Task<int> NextAsync();

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        Task<int> NextAsync(int maxValue);

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        Task<int> NextAsync(int minValue, int maxValue);

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        Task<double> NextDoubleAsync();

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to minValue, and less than maxValue.
        /// </summary>
        Task<double> NextDoubleAsync(double minValue, double maxValue);

        /// <summary>
        /// Returns a random floating-point number that is less than maxValue.
        /// </summary>
        /// <param name="maxValue"></param>
        Task<double> NextDoubleAsync(double maxValue);

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        Task NextBytesAsync(byte[] buffer);
    }
}