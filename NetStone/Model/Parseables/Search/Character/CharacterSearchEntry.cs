using System.Threading.Tasks;
using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;
using NetStone.Model.Parseables.Character;

namespace NetStone.Model.Parseables.Search.Character;

/// <summary>
/// Models one entry in the character search results list
/// </summary>
public class CharacterSearchEntry : LodestoneParseable
{
    private readonly LodestoneClient _client;
    private readonly CharacterSearchEntryDefinition _definition;

    ///
    public CharacterSearchEntry(LodestoneClient client, IElement rootNode, CharacterSearchEntryDefinition definition) :
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
    /// Fetch character profile
    /// </summary>
    /// <returns>Task of retrieving character</returns>
    public async Task<LodestoneCharacter?> GetCharacter() => await this._client.GetCharacter(this.Id);

    ///<inheritdoc />
    public override string ToString() => this.Name;
}