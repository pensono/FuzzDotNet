namespace FuzzDotNet.Core.Simplification
{
    /// <summary>
    /// Uses a binary search to simplify integers towards zero.
    /// </summary>
    public class BinarySearchIntegerSimplifier : BinarySearchSimplifier<int>
    {
        public override int DomainMinimum => 0;

        public override int Midpoint(int low, int high)
        {
            // Be sure not to overflow
            var sum = (long)low + high;

            // (low + high) / 2 rounds towards zero
            return (int)(sum / 2);
        }
    }
}
