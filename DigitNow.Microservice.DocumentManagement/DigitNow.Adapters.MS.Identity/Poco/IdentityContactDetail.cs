
namespace DigitNow.Adapters.MS.Identity.Poco
{
    public class IdentityContactDetail
    {
        public int Id { get; set; }
        public string? IdentificationNumber { get; set; }
        public int CountryId { get; set; }
        public int CountyId { get; set; }
        public int CityId { get; set; }
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? Building { get; set; }
        public string? Entrance { get; set; }
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? PostCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
