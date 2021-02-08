using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public partial class Setting
    {
        public long Id { get; set; }
        [StringLength(96)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
