using HRIS.Services.Interfaces;
using HRIS.Services.Services;

namespace HRIS.Services.Handler.Charts
{
    public class DataTypeProvider : IDataTypeProvider
    {
        public List<BaseDataType> GetDataTypes() => BaseDataType.Charts;
    }
}
