namespace Orders.Api.models.dtos
{
    public sealed class StudentInfo
    {
        public const string SectionName = "Student";
        public required string Name { get; set; } = "";
        public required string Password { get; set; } = "";
    }
}