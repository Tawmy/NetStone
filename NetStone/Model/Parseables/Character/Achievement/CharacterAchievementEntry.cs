using System;
using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.Achievement;

/// <summary>
/// Models data for one Achievement this character earned
/// </summary>
public class CharacterAchievementEntry : LodestoneParseable
{
    private readonly CharacterAchievementEntryDefinition _definition;

    /// <summary>
    /// Create instance of achievement entry fpr given node
    /// </summary>
    /// <param name="rootNode">Root html node of this entry</param>
    /// <param name="definition">Css and regex definition</param>
    public CharacterAchievementEntry(IElement rootNode, CharacterAchievementEntryDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// The Name of this achievement
    /// </summary>
    public string Name => Parse(this._definition.Name, "Name");

    /// <summary>
    /// ID of this achievement
    /// </summary>
    public ulong Id => ulong.Parse(Parse(this._definition.Id));

    /// <summary>
    /// Link to the Eorzean Database
    /// </summary>
    public Uri DatabaseLink => ParseLodestoneUri(this._definition.Id);

    /// <summary>
    /// Time when this character earned this achievement
    /// </summary>
    public DateTime TimeAchieved => ParseTime(this._definition.Time);

    /// <inheritdoc />
    public override string ToString() => this.Name;
}