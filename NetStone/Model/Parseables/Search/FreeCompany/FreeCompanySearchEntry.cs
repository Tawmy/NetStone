using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetStone.Definitions.Model.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Search.FreeCompany;

namespace NetStone.Model.Parseables.Search.FreeCompany;

/// <summary>
/// Models on entry of the free company search results
/// </summary>
public class FreeCompanySearchEntry : LodestoneParseable
{
    private readonly LodestoneClient _client;
    private readonly FreeCompanySearchEntryDefinition _definition;

    ///
    public FreeCompanySearchEntry(LodestoneClient client, HtmlNode rootNode,
                                  FreeCompanySearchEntryDefinition definition) : base(rootNode)
    {
        this._client = client;
        this._definition = definition;
    }

    /// <summary>
    /// Free Company name
    /// </summary>
    public string Name => Parse(this._definition.Name);

    /// <summary>
    /// Free company Id
    /// </summary>
    public string Id => Parse(this._definition.Id);

    /// <summary>
    /// Home world of the FC
    /// </summary>
    public string Server => Parse(this._definition.Server, "World");

    /// <summary>
    /// Data center of the FC
    /// </summary>
    public string Datacenter => Parse(this._definition.Server, "DC");

    /// <summary>
    /// FC crest/icon
    /// </summary>
    public IconLayers CrestLayers => new(this.RootNode, this._definition.CrestLayers);

    /// <summary>
    /// Formation date
    /// </summary>
    public DateTime Formed => ParseTime(this._definition.Formed);

    /// <summary>
    /// Active status
    /// </summary>
    public ActiveTimes Active => this.ActiveText switch
    {
        "Always"        => ActiveTimes.Always,
        "Weekends"      => ActiveTimes.WeekendsOnly,
        "Weekdays"      => ActiveTimes.WeekdaysOnly,
        "Not specified" => ActiveTimes.All,
        { } s           => throw new ArgumentOutOfRangeException(s),
    };

    /// <summary>
    /// Full text of active times
    /// </summary>
    public string ActiveText => Parse(this._definition.Active);

    /// <summary>
    /// Active member count
    /// </summary>
    public int ActiveMembers => int.Parse(Parse(this._definition.ActiveMembers));

    /// <summary>
    /// Recruitment status
    /// </summary>
    public bool RecruitmentOpen => Parse(this._definition.RecruitmentOpen) == "Open";

    /// <summary>
    /// Affiliated grand company
    /// </summary>
    public string GrandCompany => Parse(this._definition.GrandCompany);

    /// <summary>
    /// Estate status
    /// </summary>
    public Housing EstateBuild => Parse(this._definition.EstateBuilt) switch
    {
        "No Estate or Plot" => Housing.NoEstateOrPlot,
        "Estate Built"      => Housing.EstateBuilt,
        "Plot Only"         => Housing.PlotOnly,
        _                   => throw new ArgumentOutOfRangeException(),
    };

    /// <summary>
    /// Retrieve Free Company profile 
    /// </summary>
    /// <returns>Full FC profile</returns>
    public async Task<LodestoneFreeCompany?> GetFreeCompany() => await this._client.GetFreeCompany(this.Id);

    ///<inheritdoc />
    public override string ToString() => this.Name;
}