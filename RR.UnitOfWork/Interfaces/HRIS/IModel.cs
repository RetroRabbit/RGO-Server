namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IModel<T>
{
    int Id { get; set; }
    T ToDto();
}