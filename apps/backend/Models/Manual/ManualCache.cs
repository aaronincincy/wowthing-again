﻿using Newtonsoft.Json.Linq;
using Wowthing.Backend.Models.Data.Collections;
using Wowthing.Backend.Models.Data.Dragonriding;
using Wowthing.Backend.Models.Data.Heirlooms;
using Wowthing.Backend.Models.Data.Illusions;
using Wowthing.Backend.Models.Data.ItemSets;
using Wowthing.Backend.Models.Data.Progress;
using Wowthing.Backend.Models.Manual.Transmog;
using Wowthing.Backend.Models.Manual.TransmogSets;
using Wowthing.Backend.Models.Manual.Vendors;
using Wowthing.Backend.Models.Manual.ZoneMaps;

namespace Wowthing.Backend.Models.Manual;

public class ManualCache
{
    public List<DataDragonridingCategory> Dragonriding { get; set; }

    [JsonProperty("rawHeirloomGroups")]
    public DataHeirloomGroup[] HeirloomSets { get; set; }

    [JsonProperty("rawIllusionGroups")]
    public DataIllusionGroup[] IllusionSets { get; set; }

    [JsonProperty("rawMountSets")]
    public List<List<OutCollectionCategory>> MountSets { get; set; }

    [JsonProperty("rawPetSets")]
    public List<List<OutCollectionCategory>> PetSets { get; set; }

    //[JsonProperty("rawProgressSets")]
    public List<List<OutProgress>> ProgressSets { get; set; }

    [JsonProperty("rawToySets")]
    public List<List<OutCollectionCategory>> ToySets { get; set; }

    [JsonProperty("rawTransmogSets")]
    public List<List<ManualTransmogCategory>> TransmogSets { get; set; }

    [JsonProperty("rawTransmogSetsV2")]
    public List<List<ManualTransmogSetCategory>> TransmogSetsV2 { get; set; }

    [JsonProperty("rawVendorSets")]
    public List<List<ManualVendorCategory>> VendorSets { get; set; }

    [JsonProperty("rawZoneMapSets")]
    public List<List<ManualZoneMapCategory>> ZoneMapSets { get; set; }

    // Shared
    [JsonProperty("rawSharedItemSets")]
    public List<ManualSharedItemSet> SharedItemSets { get; set; }

    [JsonProperty("rawSharedVendors")]
    public List<ManualSharedVendor> SharedVendors { get; set; }

    // Tags
    [JsonProperty("rawTags")]
    public List<JArray> Tags { get; set; }
}
