using HtmlAgilityPack;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.Collectable;

/// <summary>
/// Models a collection of collectables for characters
/// </summary>
public class CharacterCollectable : LodestoneParseable
{
    private readonly ICharacterCollectableDefinition _definition;

    /// <summary>
    /// Constructs a collectable collection
    /// </summary>
    /// <param name="rootNode">Root node of list</param>
    /// <param name="definition">Parser definitions</param>
    public CharacterCollectable(HtmlNode rootNode, ICharacterCollectableDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    private CharacterCollectableEntry[]? _parsedResults;

    /// <summary>
    /// All collectables collected by the character.
    /// </summary>
    public CharacterCollectableEntry[] Collectables
    {
        get
        {
            if (this._parsedResults == null)
                ParseCollectables();

            return this._parsedResults!;
        }
    }

    private void ParseCollectables()
    {
        var nodes = QueryChildNodes(this._definition.GetDefinitions().Root);

        this._parsedResults = new CharacterCollectableEntry[nodes.Length];
        for (var i = 0; i < this._parsedResults.Length; i++)
        {
            this._parsedResults[i] = new CharacterCollectableEntry(nodes[i], this._definition);
        }
    }
}