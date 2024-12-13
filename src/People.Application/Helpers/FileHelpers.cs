namespace People.Application.Helpers;

public static class FileHelpers
{
    public static byte[] ToBytes(Stream stream)
    {
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
    
    public static Stream ToStream(byte[] bytes)
    {
        return new MemoryStream(bytes);
    }
}