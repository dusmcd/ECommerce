namespace ECommerceAPI.Models
{
    public abstract class DTO
    {
        public virtual void MapToModel<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
