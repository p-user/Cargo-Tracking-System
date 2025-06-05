using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Logging
{
    public sealed class SerilogOptions
    {
        public bool Enabled { get; set; }
        public bool UseConsole { get; set; } = true;

        public string? ElasticSearchUrl { get; set; }
        public string LogTemplate { get; set; } =  "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} {Level} - {Message:lj}{NewLine}{Exception}";
        public string? LogPath { get; set; }
    }
}
