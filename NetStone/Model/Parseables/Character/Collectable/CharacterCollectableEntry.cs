using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.Collectable;

/// <summary>
/// Entry for collectable collection
/// </summary>
public class CharacterCollectableEntry : LodestoneParseable
{
    private readonly ICharacterCollectableDefinition _definition;

    /// <summary>
    /// Constructs one collectable entry
    /// </summary>
    /// <param name="rootNode">Root node for entry</param>
    /// <param name="definition">Parse definition</param>
    public CharacterCollectableEntry(IElement rootNode, ICharacterCollectableDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// The name of this collectable.
    /// </summary>
    public string Name => Parse(this._definition.GetDefinitions().Name);

    /// <summary>
    /// The string representation of this collectable.
    /// </summary>
    /// <returns><see cref="Name"/></returns>
    public override string ToString() => this.Name;
}