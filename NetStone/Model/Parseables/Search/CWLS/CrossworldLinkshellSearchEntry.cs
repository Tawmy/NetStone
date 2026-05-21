using System.Threading.Tasks;
using AngleSharp.Dom;
using NetStone.Definitions.Model.CWLS;
using NetStone.Model.Parseables.CWLS;

namespace NetStone.Model.Parseables.Search.CWLS;

/// <summary>
/// Models one entry in the cwls search results list
/// </summary>
public class CrossworldLinkshellSearchEntry : LodestoneParseable
{
    private readonly LodestoneClient _client;
    private readonly CrossworldLinkshellSearchEntryDefinition _definition;

    ///
    public CrossworldLinkshellSearchEntry(LodestoneClient client, IElement rootNode, CrossworldLinkshellSearchEntryDefinition definition) :
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
    /// Datacenter
    /// </summary>
    public string DataCenter => Parse(this._definition.Dc);
    
    
    /// <summary>
    /// Number of active members
    /// </summary>
    public int ActiveMembers =>  int.TryParse(Parse(this._definition.ActiveMembers), out var parsed) ? parsed : -1;

    /// <summary>
    /// Fetch cross world link shell
    /// </summary>
    /// <returns>Task of retrieving cwls</returns>
    public async Task<LodestoneCrossworldLinkshell?> GetCrossworldLinkshell() =>
        await this._client.GetCrossworldLinkshell(this.Id);

    ///<inheritdoc />
    public override string ToString() => this.Name;
}