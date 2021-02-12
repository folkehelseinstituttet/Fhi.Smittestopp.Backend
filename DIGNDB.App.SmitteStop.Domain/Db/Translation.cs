using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public class Translation
    {
        public long Id { get; set; }
        [StringLength(96)]
        public string Value { get; set; }
        [StringLength(96)]
        public string EntityName { get; set; }
        public long EntityId { get; set; }

        public virtual Country LanguageCountry { get; set; }
    }
}