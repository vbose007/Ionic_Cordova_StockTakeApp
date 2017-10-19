namespace MillProApp.API.Models
{
    public interface IDataTransferObject<TE,T>
    {
        T FromEntity(TE entity);
        TE ToEntity();
    }

}