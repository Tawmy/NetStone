using System;
using HtmlAgilityPack;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.Gear;

/// <summary>
/// Represents data about a character's facewear
/// </summary>
public class FacewearEntry : LodestoneParseable, IOptionalParseable<FacewearEntry>
{
    private readonly FacewearEntryDefinition _definition;

    ///<inheritdoc />
    public FacewearEntry(HtmlNode rootNode, FacewearEntryDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// Name of the facewear
    /// </summary>
    public string ItemName => Parse(this._definition.Name);
    
    /// <summary>
    /// Name of the item this facewear is unlocked by
    /// </summary>
    public string UnlockedBy => Parse(this._definition.UnlockedBy);

    /// <summary>
    /// Link to this facewear's Eorzea DB page.
    /// </summary>
    public Uri? DbLink => TryParseLodestoneUri(this._definition.DbLink, out var dbLink) ? dbLink : null;
    
    /// <summary>
    /// Icon of this item.
    /// </summary>
    public Uri IconLink => new(Parse(this._definition.IconLink));
    
    /// <summary>
    /// Link to the glamoured item's icon.
    /// </summary>
    public Uri UnlockedByIconLink => new(Parse(this._definition.UnlockedByIconLink));
    
    ///<inheritdoc />
    public bool Exists => HasNode(this._definition.Name);
    
    ///<inheritdoc />
    public override string ToString() => this.ItemName;
}