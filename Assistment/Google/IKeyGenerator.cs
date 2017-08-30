using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Google
{
    public interface IKeyGenerator
    {
        string ApiKey { get; }
        string SearchID { get; }
    }
}
