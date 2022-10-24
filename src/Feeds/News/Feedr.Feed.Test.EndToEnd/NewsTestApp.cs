using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedr.Feed.News.Test.EndToEnd
{
    [ExcludeFromCodeCoverage]
    internal sealed class NewsTestApp : WebApplicationFactory<Program>
    {
    }
}
