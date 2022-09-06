using EPiServer.Commerce.Catalog.ContentTypes;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Media;
using Foundation.Infrastructure.Commerce.Extensions;
using Geta.Optimizely.ProductFeed.Configuration;

namespace Foundation.Features.ProductFeed;

public class EntityMapper : IEntityMapper<MyCommerceProductRecord>
{
    public MyCommerceProductRecord Map(CatalogContentBase catalogContent)
    {
        if (catalogContent is GenericVariant item)
        {
            return new MyCommerceProductRecord
            {
                Code = item.Code,
                DisplayName = item.DisplayName,
                Description = item.Description.ToHtmlString(),
                IsAvailable = true,
                Url = item.GetUrl(),
                ImageLink = item.GetDefaultAsset<ImageMediaData>()
                //Brand = item.
            };
        }

        return null;
    }
}