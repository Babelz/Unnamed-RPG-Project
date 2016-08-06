﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vRPGContent.Data.Attributes;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "Specialization processor")]
    public sealed class SpecializationProcessor : GenericElementProcessor<SpecializationData>
    {
    }
}
