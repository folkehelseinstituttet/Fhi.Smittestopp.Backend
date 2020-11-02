namespace DIGNDB.App.SmitteStop.Domain
{
    /// <summary>
    /// Put this interface on a class which has Id property.
    /// </summary>
    /// <typeparam name="TId">Type of Id field. Could be Guid or long or string or anything.</typeparam>
    public interface IIdentifiedEntity<TId>
    {
        public TId Id { get; set; }
    }
}