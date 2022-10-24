using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Messaging
{
    //Insieme al CorrelationId ci può essere qualunque metadato
    public record MessageEnvelope<T>(T Message, string CorrelationId) where T : IFeedrMessage;
}
