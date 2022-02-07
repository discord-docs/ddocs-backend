using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    public enum EventTypes
    {
        None = 1 << 0,
        DraftModified = 1 << 1,
        DraftCreated = 1 << 2,
        SummaryCreated = 1 << 3,
        SummaryDeleted = 1 << 4,
        SummaryModifed = 1 << 5,
    }
}
