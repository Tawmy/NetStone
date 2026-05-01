using HtmlAgilityPack;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.Gear;

/// <summary>
/// Container class holding information about a character's equipped gear.
/// </summary>
public class CharacterGear : LodestoneParseable
{
    private readonly LodestoneClient _client;
    private readonly CharacterGearDefinition _definition;

    /// <summary>
    /// Constructs parser for character gear
    /// </summary>
    /// <param name="client"></param>
    /// <param name="rootNode"></param>
    /// <param name="definition"></param>
    public CharacterGear(LodestoneClient client, HtmlNode rootNode, CharacterGearDefinition definition) : base(rootNode)
    {
        this._client = client;
        this._definition = definition;
    }

    /// <summary>
    /// Information about the characters' weapon. Null if none equipped.
    /// </summary>
    public GearEntry? Mainhand => new GearEntry(this._client, this.RootNode, this._definition.Mainhand).GetOptional();

    /// <summary>
    /// Information about the characters' shield/offhand. Null if none equipped.
    /// </summary>
    public GearEntry? Offhand => new GearEntry(this._client, this.RootNode, this._definition.Offhand).GetOptional();

    /// <summary>
    /// Information about the characters' headgear. Null if none equipped.
    /// </summary>
    public GearEntry? Head => new GearEntry(this._client, this.RootNode, this._definition.Head).GetOptional();

    /// <summary>
    /// Information about the characters' body gear. Null if none equipped.
    /// </summary>
    public GearEntry? Body => new GearEntry(this._client, this.RootNode, this._definition.Body).GetOptional();

    /// <summary>
    /// Information about the characters' gloves. Null if none equipped.
    /// </summary>
    public GearEntry? Hands => new GearEntry(this._client, this.RootNode, this._definition.Hands).GetOptional();

    /// <summary>
    /// Information about the characters' pants. Null if none equipped.
    /// </summary>
    public GearEntry? Legs => new GearEntry(this._client, this.RootNode, this._definition.Legs).GetOptional();

    /// <summary>
    /// Information about the characters' shoes. Null if none equipped.
    /// </summary>
    public GearEntry? Feet => new GearEntry(this._client, this.RootNode, this._definition.Feet).GetOptional();

    /// <summary>
    /// Information about the characters' facewear. Null if none equipped.
    /// </summary>
    public FacewearEntry? Facewear => new FacewearEntry(this.RootNode, this._definition.Facewear).GetOptional();

    /// <summary>
    /// Information about the characters' earrings. Null if none equipped.
    /// </summary>
    public GearEntry? Earrings => new GearEntry(this._client, this.RootNode, this._definition.Earrings).GetOptional();

    /// <summary>
    /// Information about the characters' necklace. Null if none equipped.
    /// </summary>
    public GearEntry? Necklace => new GearEntry(this._client, this.RootNode, this._definition.Necklace).GetOptional();

    /// <summary>
    /// Information about the characters' bracelets. Null if none equipped.
    /// </summary>
    public GearEntry? Bracelets => new GearEntry(this._client, this.RootNode, this._definition.Bracelets).GetOptional();

    /// <summary>
    /// Information about the characters' first ring. Null if none equipped.
    /// </summary>
    public GearEntry? Ring1 => new GearEntry(this._client, this.RootNode, this._definition.Ring1).GetOptional();

    /// <summary>
    /// Information about the characters' second ring. Null if none equipped.
    /// </summary>
    public GearEntry? Ring2 => new GearEntry(this._client, this.RootNode, this._definition.Ring2).GetOptional();

    /// <summary>
    /// Information about the characters' soul crystal. Null if none equipped.
    /// </summary>
    public SoulcrystalEntry? Soulcrystal =>
        new SoulcrystalEntry(this.RootNode, this._definition.Soulcrystal).GetOptional();
}