namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById
{
    public class GetFormByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }
    }
}