public static class Zobrist
{
    public static readonly ulong[,] PieceKeys = new ulong[12, 64];
    public static readonly ulong BlackToMoveKey;
    public static readonly ulong[] CastlingKeys = new ulong[4];
    public static readonly ulong[] EnPassantFileKeys = new ulong[8];

    static Zobrist()
    {
        Random random = new Random(123456789);
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                PieceKeys[i, j] = NextUlong(random);
            }
        }

        BlackToMoveKey = NextUlong(random);

        for (int i = 0; i < 4; i++)
        {
            CastlingKeys[i] = NextUlong(random);
        }
        for (int i = 0; i < 8; i++)
        {
            EnPassantFileKeys[i] = NextUlong(random);
        }
    }

    private static ulong NextUlong(Random random)
    {
        byte[] buffer = new byte[8];
        random.NextBytes(buffer);
        return BitConverter.ToUInt64(buffer, 0);
    }
}
