namespace ATA.Library.Shared.Dto
{
    public partial class SecurityResult<T>
    {
        public T Data { get; set; } = default!;

        public string? Message { get; set; }

        public bool IsSuccessful { get; set; }
    }
}