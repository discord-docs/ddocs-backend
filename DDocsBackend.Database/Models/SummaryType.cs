using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     Represents a type for a <see cref="Summary"/>
    /// </summary>
    internal enum SummaryType
    {
        /// <summary>
        ///     A feature summary.
        /// </summary>
        Feature,

        /// <summary>
        ///     A QNA summary.
        /// </summary>
        QNAAnswer,
    }
}
