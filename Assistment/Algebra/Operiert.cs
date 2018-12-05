namespace Assistment.Algebra
{
    public interface Operiert<in A, in B>
    {
        void Add(A Preimage, B Wert);
    }
}
