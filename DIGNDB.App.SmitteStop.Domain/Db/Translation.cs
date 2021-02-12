namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public class Translation
    {
        public long Id { get; set; }
        public string Value { get; set; }

        public string EntityName { get; set; }
        public long EntityId { get; set; }

        public virtual Country LanguageCountry { get; set; }
    }
}