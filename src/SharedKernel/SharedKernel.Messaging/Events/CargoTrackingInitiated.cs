using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Messaging.Events
{
    public record CargoTrackingInitiated(Guid CargoId, string TrackingId) : BaseEvent;
    
}
