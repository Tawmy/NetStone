using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetStone.Definitions;
using NetStone.Definitions.Model.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Model.Parseables.FreeCompany;

/// <summary>
/// Provides information of a Free Company
/// </summary>
public class LodestoneFreeCompany : LodestoneParseable
{
    private readonly LodestoneClient _client;

    private readonly FreeCompanyDefinition _fcDefinition;
    private readonly FreeCompanyFocusDefinition _focusDefinition;
    private readonly FreeCompanyReputationDefinition _reputationDefinition;

    /// <summary>
    /// Constructs Free Company information parser
    /// </summary>
    /// <param name="client">Current client</param>
    /// <param name="rootNode">Root node of FC page</param>
    /// <param name="definitions">Parser definitions</param>
    /// <param name="id">Id of FC</param>
    public LodestoneFreeCompany(LodestoneClient client, HtmlNode rootNode, DefinitionsContainer definitions, string id)
        : base(rootNode)
    {
        this._client = client;
        this.Id = id;

        this._fcDefinition = definitions.FreeCompany;
        this._focusDefinition = definitions.FreeCompanyFocus;
        this._reputationDefinition = definitions.FreeCompanyReputation;
    }

    /// <summary>
    /// Name of this FC
    /// </summary>
    public string Name => Parse(this._fcDefinition.Name);

    /// <summary>
    /// Id of this FC
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Slogan
    /// </summary>
    public string Slogan => Parse(this._fcDefinition.Slogan);

    /// <summary>
    /// Tag
    /// </summary>
    public string Tag => Parse(this._fcDefinition.Tag);

    /// <summary>
    /// FC Icon/Crest
    /// </summary>
    public IconLayers CrestLayers => new(this.RootNode, this._fcDefinition.CrestLayers);

    /// <summary>
    /// Formation date
    /// </summary>
    public DateTime Formed => ParseTime(this._fcDefinition.Formed);

    /// <summary>
    /// Current GC
    /// </summary>
    public string GrandCompany => Parse(this._fcDefinition.GrandCompany).TrimEnd();

    /// <summary>
    /// Current Rank
    /// </summary>
    public int Rank => int.Parse(Parse(this._fcDefinition.Rank));

    /// <summary>
    /// Monthly ranking
    /// </summary>
    public int? RankingMonthly => int.TryParse(Parse(this._fcDefinition.Ranking.Monthly), out var result) ? result : null;

    /// <summary>
    /// Weekly ranking
    /// </summary>
    public int? RankingWeekly => int.TryParse(Parse(this._fcDefinition.Ranking.Weekly), out var result) ? result : null;

    /// <summary>
    /// Recruitment status
    /// </summary>
    public string Recruitment => Parse(this._fcDefinition.Recruitment);

    /// <summary>
    /// Number of active members
    /// </summary>
    public int ActiveMemberCount => int.Parse(Parse(this._fcDefinition.ActiveMemberCount));

    /// <summary>
    /// Activity status
    /// </summary>
    //todo: selector does not work
    public string ActiveState => Parse(this._fcDefinition.Activestate).Trim();


    /// <summary>
    /// Information about the estate
    /// </summary>
    public FreeCompanyEstate? Estate =>
        new FreeCompanyEstate(this.RootNode, this._fcDefinition.EstateDefinition).GetOptional();

    /// <summary>
    /// Information about focused gameplay
    /// </summary>
    public FreeCompanyFocus? Focus => new FreeCompanyFocus(this.RootNode, this._focusDefinition).GetOptional();

    /// <summary>
    /// Reputation with the Grand Companies
    /// </summary>
    public FreeCompanyReputation Reputation => new(this.RootNode, this._reputationDefinition);

    /// <summary>
    /// Home World 
    /// </summary>
    public string World => Parse(this._fcDefinition.Server);

    /// <summary>
    /// Fetches all members of this FC
    /// </summary>
    /// <returns>Members</returns>
    public async Task<FreeCompanyMembers?> GetMembers() => await this._client.GetFreeCompanyMembers(this.Id);
}