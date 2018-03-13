public static class UniqueSorting
{
    private static int _counter;

    public static int GetNextSorting()
    {
        return --_counter;
    }
}
