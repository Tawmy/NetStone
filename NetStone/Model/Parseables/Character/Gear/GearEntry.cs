using System;
using System.Linq;
using HtmlAgilityPack;
using NetStone.Definitions.Model.Character;
using NetStone.GameData;

namespace NetStone.Model.Parseables.Character.Gear;

/// <summary>
/// Container class holding information about a gear slot.
/// </summary>
public class GearEntry : LodestoneParseable, IOptionalParseable<GearEntry>
{
    /// <summary>
    /// Character representing the high quality symbol
    /// </summary>
    public const char HqChar = '\uE03C';

    private readonly LodestoneClient client;
    private readonly GearEntryDefinition definition;

    private NamedGameData? cachedGameData;

    /// <summary>
    /// Construct a new gear entry
    /// </summary>
    /// <param name="client">Lodestone client</param>
    /// <param name="rootNode">Entry node</param>
    /// <param name="definition">Parser definition</param>
    public GearEntry(LodestoneClient client, HtmlNode rootNode, GearEntryDefinition definition) : base(rootNode)
    {
        this.client = client;
        this.definition = definition;
    }

    /// <summary>
    /// Link to this piece's Eorzea DB page.
    /// </summary>
    public Uri? ItemDatabaseLink => ParseLodestoneUri(this.definition.DbLink);

    /// <summary>
    /// Name of this item.
    /// </summary>
    public string ItemName => Parse(this.definition.Name);
    
    /// <summary>
    /// Icon of this item.
    /// </summary>
    public Uri IconLink => new(Parse(this.definition.IconLink));

    /// <summary>
    /// Indicates if this item is high quality
    /// </summary>
    public bool IsHq => this.ItemName.EndsWith(HqChar);

    /// <summary>
    /// Returns the name of this item without high quality icon
    /// </summary>
    public string StrippedItemName => this.IsHq ? this.ItemName.Remove(this.ItemName.Length - 1) : this.ItemName;

    /// <summary>
    /// Link to the glamoured item's Eorzea DB page.
    /// </summary>
    public Uri GlamourDatabaseLink => ParseLodestoneUri(this.definition.MirageDbLink);

    /// <summary>
    /// Name of the glamoured item.
    /// </summary>
    public string GlamourName => Parse(this.definition.MirageName);

    /// <summary>
    ///     Link to the glamoured item's icon.
    /// </summary>
    public Uri? GlamourIconLink => Parse(this.definition.MirageIconLink) is { Length: > 0 } glamourItemLink
        ? new Uri(glamourItemLink)
        : null;

    /// <summary>
    /// Name of the dye applied to this item in slot 1.
    /// </summary>
    public string? Dye1Name => !string.IsNullOrEmpty(Dye1Color) ? Parse(this.definition.Stain1) : null;

    /// <summary>
    /// Link to the Eorzea DB page of the dye applied to this item in slot 1.
    /// </summary>
    public Uri? Dye1DatabaseLink => !string.IsNullOrEmpty(Dye1Color) 
        ? ParseLodestoneUri(this.definition.Stain1DbLink) 
        : null;

    /// <summary>
    /// Hex color code of the dye applied to this item in slot 1.
    /// </summary>
    public string Dye1Color => Parse(this.definition.Stain1Color);

    /// <summary>
    /// Name of the dye applied to this item in slot 2.
    /// </summary>
    public string Dye2Name
    {
        get
        {
            if (string.IsNullOrEmpty(Dye2Color))
            {
                // if dye 2 not set, return nothing
                return string.Empty;
            }

            // Check whether name for dye 2 is set. If dye 1 doesn't exist, this will falsely parse as dye 1's name
            var stain2 = Parse(this.definition.Stain2);
            return !string.IsNullOrEmpty(stain2)
                ? stain2
                : Parse(this.definition.Stain1);
        }
    }

    /// <summary>
    /// Link to the Eorzea DB page of the dye applied to this item in slot 2.
    /// </summary>
    public Uri? Dye2DatabaseLink
    {
        get
        {
            if (string.IsNullOrEmpty(Dye2Color))
            {
                // if dye 2 not set, return nothing
                return null;
            }

            // Check whether link for dye 2 is set. If dye 1 doesn't exist, this will falsely parse as dye 1's link
            var stain2Db = ParseLodestoneUri(this.definition.Stain2DbLink);
            return stain2Db ?? ParseLodestoneUri(this.definition.Stain1DbLink);
        }
    }

    /// <summary>
    /// Hex color code of the dye applied to this item in slot 2.
    /// </summary>
    public string Dye2Color => Parse(this.definition.Stain2Color);

    /// <summary>
    /// Materia applied to this item.
    /// </summary>
    public string[] Materia => new[]
    {
        Parse(this.definition.Materia1),
        Parse(this.definition.Materia2),
        Parse(this.definition.Materia3),
        Parse(this.definition.Materia4),
        Parse(this.definition.Materia5),
    };

    /// <summary>
    /// Name of this item's crafter.
    /// </summary>
    public string CreatorName => Parse(this.definition.CreatorName);
    
    /// <summary>
    /// Item level of this item.
    /// </summary>
    public int ItemLevel => int.TryParse(Parse(this.definition.ItemLevel).Split(' ').LastOrDefault(), out var itemLevel)
        ? itemLevel
        : 0;
    
    /// <summary>
    /// Rarity of this item.
    /// </summary>
    public string Rarity => Parse(this.definition.Rarity);

    /// <summary>
    /// Indicating whether the item slot has an item equipped or not.
    /// </summary>
    public bool Exists => HasNode(this.definition.Name);

    /// <summary>
    /// Get game data representing this item
    /// </summary>
    /// <returns>Item data</returns>
    public NamedGameData? GetGameData()
    {
        this.cachedGameData ??= !this.Exists ? null : this.client.Data?.GetItem(this.ItemName);
        return this.cachedGameData;
    }

    /// <summary>
    /// String representation of the gear slot.
    /// </summary>
    /// <returns>The name of the item.</returns>
    public override string ToString() => this.ItemName;
}