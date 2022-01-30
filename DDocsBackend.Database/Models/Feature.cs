using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    /// <summary>
    ///     An object representing a mentioned feature.
    /// </summary>
    public class Feature
    {
        /// <summary>
        ///     Gets or sets the name of the feature.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets or sets the optional description of the feature.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        ///     Gets or sets the status of the feature.
        /// </summary>
        public FeatureType Status { get; set; }
    }
}
