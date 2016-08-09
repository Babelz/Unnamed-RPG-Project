using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "Item processor")]
    public sealed class ItemProcessor : GenericElementProcessor<Item>
    {
    }

    [ContentProcessor(DisplayName = "Quest item processor")]
    public sealed class QuestItemProcessor : GenericElementProcessor<QuestItem>
    {
    }

    [ContentProcessor(DisplayName = "Material item processor")]
    public sealed class MaterialItemProcessor : GenericElementProcessor<Material>
    {
    }
}
