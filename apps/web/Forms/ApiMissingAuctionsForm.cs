﻿using Wowthing.Lib.Enums;

namespace Wowthing.Web.Forms;

public class ApiMissingAuctionsForm
{
    public bool AllRealms { get; set; }
    public bool MissingPetsMaxLevel { get; set; }
    public WowRegion Region { get; set; }
    public string Type { get; set; }
}
