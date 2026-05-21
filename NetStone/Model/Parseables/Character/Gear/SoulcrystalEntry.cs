using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;
using System;

namespace NetStone.Model.Parseables.Character.Gear;

/// <summary>
/// Represents data about a character's soul crystal
/// </summary>
public class SoulcrystalEntry : LodestoneParseable, IOptionalParseable<SoulcrystalEntry>
{
    private readonly SoulcrystalEntryDefinition _definition;

    ///<inheritdoc />
    public SoulcrystalEntry(IElement rootNode, SoulcrystalEntryDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    //public Uri ItemDatabaseLink => ParseHrefOld(this.definition.Name);

    /// <summary>
    /// Name of the item
    /// </summary>
    public string ItemName => Parse(this._definition.Name);
    
    /// <summary>
    /// Icon of the item
    /// </summary>
    public Uri IconLink => new(Parse(this._definition.IconLink));

    /// <inheritdoc />
    public bool Exists => HasNode(this._definition.Name);

    ///<inheritdoc />
    public override string ToString() => this.ItemName;
}