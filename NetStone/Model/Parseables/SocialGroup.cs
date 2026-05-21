using System;
using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables;

/// <summary>
/// Models a group of players/characters
/// </summary>
public class SocialGroup : LodestoneParseable, IOptionalParseable<SocialGroup>
{
    private readonly ICharacterSocialGroupDefinition _definition;

    ///<inheritdoc />
    public SocialGroup(IElement rootNode, ICharacterSocialGroupDefinition socialGroupDefinition) : base(rootNode)
    {
        this._definition = socialGroupDefinition;
    }

    /// <summary>
    /// Indicating whether this social group exists or not.
    /// </summary>
    public bool Exists => !string.IsNullOrEmpty(Id);

    /// <summary>
    /// Name of this social group.
    /// </summary>
    public string Name => Parse(this._definition.Name);

    /// <summary>
    /// ID of this social group.
    /// </summary>
    public string Id => Parse(this._definition.Id);

    /// <summary>
    /// Link to this social group's page.
    /// </summary>
    public Uri? Link => TryParseLodestoneUri(this._definition.Id, out var link) ? link : null;

    /// <summary>
    /// <see cref="IconLayers"/> of this social group's icon.
    /// </summary>
    public IconLayers IconLayers => new(this.RootNode, this._definition.IconLayers);

    /// <summary>
    /// String representation of the gear slot.
    /// </summary>
    /// <returns>The name of the item.</returns>
    public override string ToString() => this.Name;
}