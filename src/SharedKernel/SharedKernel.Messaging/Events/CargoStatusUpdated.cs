using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Messaging.Events
{
    public record CargoStatusUpdated(Guid CargoId, string TrackingId, string NewStatus, string CurrentLocation) : BaseEvent;
    
}
