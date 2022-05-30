namespace Haystack;

public static class Int32Extensions
{
    /// <summary>
    /// Rounds the value up to the nearest multiple of toNearest. For example:
    /// - 150 to the nearest 10 would return 150
    /// - 151 to the nearest 10 would return 160
    /// </summary>
    /// <param name="???"></param>
    /// <param name="toNearestMultipleOf"></param>
    /// <returns></returns>
    public static int RoundUp(this int round, int toNearestMultipleOf)
        => round % toNearestMultipleOf == 0
            ? round
            : (toNearestMultipleOf - round % toNearestMultipleOf) + round;

    /// <summary>
    /// Rounds the value down to the nearest multiple of toNearest. For example:
    /// - 150 to the nearest 10 would return 150
    /// - 151 to the nearest 10 would return 150
    /// </summary>
    /// <param name="???"></param>
    /// <param name="toNearestMultipleOf"></param>
    /// <returns></returns>
    public static int RoundDown(this int incoming, int toNearestMultipleOf)
        => incoming - incoming % toNearestMultipleOf;
}