using HtmlAgilityPack;
using NetStone.Definitions.Model.CWLS;

namespace NetStone.Model.Parseables.CWLS.Members;

/// <summary>
/// Container class holding information about a cross-world linkshell member.
/// </summary>
public class CrossworldLinkshellMemberEntry : LodestoneParseable
{
    private readonly CrossworldLinkshellMemberEntryDefinition _definition;
    /// <summary>
    /// Create instance of member entry for a given node
    /// </summary>
    /// <param name="rootNode">Root html node of this entry</param>
    /// <param name="definition">Css and regex definition</param>
    public CrossworldLinkshellMemberEntry(HtmlNode rootNode, CrossworldLinkshellMemberEntryDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// Avatar
    /// </summary>
    public string Avatar => Parse(this._definition.Avatar);

    /// <summary>
    /// ID
    /// </summary>
    public string Id => Parse(this._definition.Id);

    /// <summary>
    /// Name
    /// </summary>
    public string Name => Parse(this._definition.Name);

    /// <summary>
    /// Rank
    /// </summary>
    public string Rank => Parse(this._definition.Rank);

    /// <summary>
    /// Rank Icon
    /// </summary>
    public string RankIcon => Parse(this._definition.RankIcon);

    /// <summary>
    /// Linkshell rank
    /// </summary>
    public string LinkshellRank => Parse(this._definition.LinkshellRank);

    /// <summary>
    /// Linkshell rank Icon
    /// </summary>
    public string LinkshellRankIcon => Parse(this._definition.LinkshellRankIcon);

    /// <summary>
    /// Server
    /// </summary>
    public string Server => Parse(this._definition.Server);
}