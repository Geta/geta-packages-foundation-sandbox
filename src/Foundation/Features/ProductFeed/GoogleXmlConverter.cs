using EPiServer.Web;
using Geta.Optimizely.ProductFeed;
using Geta.Optimizely.ProductFeed.Google.Models;

namespace Foundation.Features.ProductFeed;

public class GoogleXmlConverter : IProductFeedConverter<MyCommerceProductRecord>
{
    public object Convert(MyCommerceProductRecord entity, HostDefinition host)
    {
        return new Entry
        {
            Id = entity.Code,
            Description = entity.Description,
            Link = entity.Url,
            ImageLink = entity.ImageLink
        };
    }
}