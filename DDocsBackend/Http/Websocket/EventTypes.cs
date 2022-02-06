using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    public enum EventTypes
    {
        DraftModified = 1 << 0,
        DraftCreated = 1 << 1,
        SummaryCreated = 1 << 2,
        SummaryDeleted = 1 << 3,
        SummaryModifed = 1 << 4,
    }
}
