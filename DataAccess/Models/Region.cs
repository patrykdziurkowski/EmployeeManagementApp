namespace DataAccess.Models
{
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Region
    {
        public decimal? RegionId { get; set; }
        public string RegionName { get; set; }
    }
}
