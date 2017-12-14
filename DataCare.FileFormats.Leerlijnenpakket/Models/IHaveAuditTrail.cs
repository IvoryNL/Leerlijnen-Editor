namespace DataCare.Model
{
    public interface IHaveAuditTrail<T> where T : IHaveAuditTrail<T>
    {
        AuditTrail<T> AuditTrail { get; }
    }
}