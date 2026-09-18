namespace Yamaha.AccountStatement.Core.Models
{
    public class FileData
    {
        public string FileName { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
        public long Length { get; init; }
        public byte[] Data { get; init; } = Array.Empty<byte>();
    }
}
