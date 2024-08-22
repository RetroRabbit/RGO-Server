using HRIS.Services.Services;

namespace HRIS.Services.Interfaces
{
    public interface IDataTypeProvider
    {
        List<BaseDataType> GetDataTypes();
    }
}
