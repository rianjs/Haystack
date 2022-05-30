namespace Haystack.Random;

/// <summary>
/// A thread-safe wrapper around the Random class that uses locks for synchronization safety. If you need deterministic behavior, you specify the random
/// seed, OR you can mock the ISafeRandom interface, which is a bit more convenient.
/// - Do NOT use this with async code. Use AsyncSafeRandom instead.
/// - This class is designed to be created once and reused everywhere in your application
/// - Creating two of these objects in quick succession can result in the same seed being reused
/// </summary>
public interface ISafeRandom
{
    /// <summary>
    /// Returns a non-negative random integer.
    /// </summary>
    int Next();

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    int Next(int maxValue);

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    int Next(int minValue, int maxValue);

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    double NextDouble();

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to minValue, and less than maxValue.
    /// </summary>
    double NextDouble(double minValue, double maxValue);

    /// <summary>
    /// Returns a random floating-point number that is less than maxValue.
    /// </summary>
    /// <param name="maxValue"></param>
    double NextDouble(double maxValue);

    /// <summary>
    /// Fills the elements of a specified array of bytes with random numbers.
    /// </summary>
    void NextBytes(byte[] buffer);
}