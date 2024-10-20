﻿using Newtonsoft.Json;

namespace NetStone.Definitions.Model.FreeCompany;

/// <summary>
/// Definition for one FC member
/// </summary>
public class FreeCompanyMembersEntryDefinition : PagedEntryDefinition
{
    /// <summary>
    /// Avatar of member character
    /// </summary>
    [JsonProperty("AVATAR")]
    public DefinitionsPack Avatar { get; set; }

    /// <summary>
    /// ID of character
    /// </summary>
    [JsonProperty("ID")]
    public DefinitionsPack Id { get; set; }

    /// <summary>
    /// Name of character
    /// </summary>
    [JsonProperty("NAME")]
    public DefinitionsPack Name { get; set; }

    /// <summary>
    /// Grand company rank
    /// </summary>
    [JsonProperty("RANK")]
    public DefinitionsPack Rank { get; set; }

    /// <summary>
    /// GC rank icon
    /// </summary>
    [JsonProperty("RANK_ICON")]
    public DefinitionsPack RankIcon { get; set; }

    /// <summary>
    /// Free company rank
    /// </summary>
    [JsonProperty("FC_RANK")]
    public DefinitionsPack FreeCompanyRank { get; set; }

    /// <summary>
    /// FC rank icon
    /// </summary>
    [JsonProperty("FC_RANK_ICON")]
    public DefinitionsPack FreeCompanyRankIcon { get; set; }

    /// <summary>
    /// Homeworld
    /// </summary>
    [JsonProperty("SERVER")]
    public DefinitionsPack Server { get; set; }
}