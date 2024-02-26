using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.IntegrationTests
{
    [CollectionDefinition("DataabseCollection")]
    public class DatabaseCollection : ICollectionFixture<SharedDbFixture>
    {
    }
}
