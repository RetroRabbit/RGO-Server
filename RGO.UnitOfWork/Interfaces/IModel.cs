namespace RGO.UnitOfWork.Interfaces;

public interface IModel<T>
{
    int Id { get; set; }
    T ToDto();
}