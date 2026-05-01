using HtmlAgilityPack;
using NetStone.Definitions.Model.FreeCompany;

namespace NetStone.Model.Parseables.FreeCompany;

/// <summary>
/// Models reputation of a Free Company with on Grand Company
/// </summary>
public class FreeCompanyReputationEntry : LodestoneParseable
{
    private readonly FreeCompanyReputationEntryDefinition _definition;

    /// <summary>
    /// Constructs FC reputation with on GC
    /// </summary>
    /// <param name="rootNode"></param>
    /// <param name="definition"></param>
    public FreeCompanyReputationEntry(HtmlNode rootNode, FreeCompanyReputationEntryDefinition definition) :
        base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// Name of the Grand Company
    /// </summary>
    public string Name => Parse(this._definition.Name);

    /// <summary>
    /// Progress to next rank
    /// </summary>
    public int Progress => int.Parse(Parse(this._definition.Progress));

    /// <summary>
    /// Current rank
    /// </summary>
    public string Rank => Parse(this._definition.Rank);
}