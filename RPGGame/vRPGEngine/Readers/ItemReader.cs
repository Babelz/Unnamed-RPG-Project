using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items;

namespace vRPGEngine.Readers
{
    public sealed class ItemReader : GenericElementReader<Item>
    {
    }

    public sealed class QuestItemReader : GenericElementReader<QuestItem>
    {
    }

    public sealed class MaterialItemReader : GenericElementReader<Material>
    {
    }
}
