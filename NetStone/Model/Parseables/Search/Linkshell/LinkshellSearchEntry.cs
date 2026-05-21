using System.Threading.Tasks;
using AngleSharp.Dom;
using NetStone.Definitions.Model.Linkshell;
using NetStone.Model.Parseables.Linkshell;

namespace NetStone.Model.Parseables.Search.Linkshell;

/// <summary>
/// Models one entry in the linkshell search results list
/// </summary>
public class LinkshellSearchEntry : LodestoneParseable
{
    private readonly LodestoneClient _client;
    private readonly LinkshellSearchEntryDefinition _definition;

    ///
    public LinkshellSearchEntry(LodestoneClient client, IElement rootNode, LinkshellSearchEntryDefinition definition) :
        base(rootNode)
    {
        this._client = client;
        this._definition = definition;
    }

    /// <summary>
    /// Character name
    /// </summary>
    public string Name => Parse(this._definition.Name);

    /// <summary>
    /// Lodestone Id
    /// </summary>
    public string Id => Parse(this._definition.Id);

    /// <summary>
    /// Homeworld / Server
    /// </summary>
    public string HomeWorld => Parse(this._definition.Server);
    
    /// <summary>
    /// Number of active members
    /// </summary>
    public int ActiveMembers =>  int.TryParse(Parse(this._definition.ActiveMembers), out var parsed) ? parsed : -1;

    /// <summary>
    /// Fetch character profile
    /// </summary>
    /// <returns>Task of retrieving character</returns>
    public async Task<LodestoneLinkshell?> GetLinkshell() => await this._client.GetLinkshell(this.Id);

    ///<inheritdoc />
    public override string ToString() => this.Name;
}