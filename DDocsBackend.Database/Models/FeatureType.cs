using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     Represents a type for a <see cref="Feature"/>
    /// </summary>
    public enum FeatureType
    {
        /// <summary>
        ///     A feature planned for release in Q1.
        /// </summary>
        PlannedQ1,

        /// <summary>
        ///     A feature planned for release in Q2.
        /// </summary>
        PlannedQ2,

        /// <summary>
        ///     A feature planned for release in Q3.
        /// </summary>
        PlannedQ3,

        /// <summary>
        ///     A feature planned for release in Q4.
        /// </summary>
        PlannedQ4,

        /// <summary>
        ///     A feature lacking details on the release.
        /// </summary>
        Unknown,

        /// <summary>
        ///     A feature currently in closed beta testing.
        /// </summary>
        ClosedBeta,

        /// <summary>
        ///     A feature currently work-in-progress.
        /// </summary>
        WorkInProgress,

        /// <summary>
        ///     A feature that has been released.
        /// </summary>
        Released
    }
}
