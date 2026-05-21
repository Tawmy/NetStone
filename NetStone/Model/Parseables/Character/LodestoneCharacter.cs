using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using NetStone.Definitions;
using NetStone.Definitions.Model.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.Character.Gear;

namespace NetStone.Model.Parseables.Character;

/// <summary>
/// Container class holding information about a character and facilitating retrieval of further information.
/// </summary>
public class LodestoneCharacter : LodestoneParseable
{
    /// <summary>
    /// Unicode character used to represent female characters
    /// </summary>
    public const char FemaleChar = '\u2640';
    
    /// <summary>
    /// Unicode character used to represent male characters
    /// </summary>
    public const char MaleChar = '\u2642';
    
    private readonly LodestoneClient _client;

    private readonly string _charId;

    private readonly CharacterDefinition _charDefinition;
    private readonly CharacterGearDefinition _gearDefinition;
    private readonly CharacterAttributesDefinition _attributesDefinition;

    /// <summary>
    /// Container class for a parseable character page.
    /// </summary>
    /// <param name="client">The <see cref="LodestoneClient"/> to be used to fetch further information.</param>
    /// <param name="rootNode">The root document node of the page.</param>
    /// <param name="container">The <see cref="DefinitionsContainer"/> holding definitions to be used to access data.</param>
    /// <param name="charId">The ID of the character.</param>
    public LodestoneCharacter(LodestoneClient client, IElement rootNode, DefinitionsContainer container, string charId)
        : base(rootNode)
    {
        this._client = client;
        this._charId = charId;

        this._charDefinition = container.Character;
        this._gearDefinition = container.Gear;
        this._attributesDefinition = container.Attributes;
    }

    #region Properties

    /// <summary>
    /// Icon of current active ClassJob.
    /// </summary>
    public string ActiveClassJobIcon => Parse(this._charDefinition.ActiveClassJob);

    /// <summary>
    /// Level of the current active ClassJob.
    /// </summary>
    public int ActiveClassJobLevel => int.Parse(Parse(this._charDefinition.ActiveClassJobLevel));

    /// <summary>
    /// An URI to the avatar of the character.
    /// </summary>
    public Uri Avatar => new(Parse(this._charDefinition.Avatar));

    /// <summary>
    /// The character bio/description.
    /// </summary>
    public string Bio => Parse(this._charDefinition.Bio);

    /// <summary>
    /// The character FreeCompany info.
    /// </summary>
    public SocialGroup? FreeCompany =>
        new FreeCompanySocialGroup(this._client, this.RootNode, this._charDefinition.FreeCompany).GetOptional();

    /// <summary>
    /// The grand company of the character.
    /// </summary>
    public string GrandCompanyName => Parse(this._charDefinition.GrandCompany);

    /// <summary>
    /// The grand company rank of the character.
    /// </summary>
    public string GrandCompanyRank => Parse(this._charDefinition.GrandCompany, "Rank");

    /// <summary>
    /// The name of the guardian deity of the character.
    /// </summary>
    public string GuardianDeityName => Parse(this._charDefinition.GuardianDeity.Name);

    /// <summary>
    /// The icon of the guardian deity of the character.
    /// </summary>
    public Uri GuardianDeityIcon => new(Parse(this._charDefinition.GuardianDeity.Icon));

    /// <summary>
    /// The name of the character.
    /// </summary>
    public string Name => Parse(this._charDefinition.Name);

    /// <summary>
    /// The nameday of the character.
    /// </summary>
    public string Nameday => Parse(this._charDefinition.Nameday);

    /// <summary>
    /// An URI to the avatar of the character.
    /// </summary>
    public Uri Portrait => new(Parse(this._charDefinition.Portrait));

    /// <summary>
    /// The character PvPTeam info.
    /// </summary>
    public SocialGroup? PvPTeam => new SocialGroup(this.RootNode, this._charDefinition.PvPTeam).GetOptional();
    
    /// <summary>
    /// Race of the character
    /// </summary>
    public string Race => Parse(this._charDefinition.RaceClanGender, "Race");
    
    /// <summary>
    /// Tribe this character belongs to
    /// </summary>
    public string Tribe => Parse(this._charDefinition.RaceClanGender, "Tribe");
    
    /// <summary>
    /// Character representing the characters gender <see cref="FemaleChar"/> and <see cref="MaleChar"/>
    /// </summary>
    public char Gender => Parse(this._charDefinition.RaceClanGender, "Gender")[0];

    /// <summary>
    /// The server/world of the character.
    /// </summary>
    public string Server => Parse(this._charDefinition.Server);

    /// <summary>
    /// The title of the character.
    /// </summary>
    public string Title => Parse(this._charDefinition.Title);

    /// <summary>
    /// The town of the character.
    /// </summary>
    public string TownName => Parse(this._charDefinition.Town.Name);

    /// <summary>
    /// The town of the character.
    /// </summary>
    public Uri TownIcon => new(Parse(this._charDefinition.Town.Icon));

    /// <summary>
    /// The character gear information.
    /// </summary>
    public CharacterGear Gear => new(this._client, this.RootNode, this._gearDefinition);

    /// <summary>
    /// The character attribute information.
    /// </summary>
    public CharacterAttributes Attributes => new(this.RootNode, this._attributesDefinition);

    #endregion

    /// <summary>
    /// Fetch more information about this character's classes and jobs(level, exp, unlocked, etc.).
    /// </summary>
    /// <returns><see cref="CharacterClassJob"/> object holding this information.</returns>
    public async Task<CharacterClassJob?> GetClassJobInfo() => await this._client.GetCharacterClassJob(this._charId);

    /// <summary>
    /// Fetch more information about this character's unlocked achievements.
    /// </summary>
    /// <returns><see cref="CharacterAchievementPage"/> object holding this information.</returns>
    public async Task<CharacterAchievementPage?> GetAchievement() =>
        await this._client.GetCharacterAchievement(this._charId);

    /// <summary>
    /// Fetch more information about this character's unlocked mounts.
    /// </summary>
    /// <returns><see cref="CharacterCollectable"/> object holding this information.</returns>
    public async Task<CharacterCollectable?> GetMounts() => await this._client.GetCharacterMount(this._charId);

    /// <summary>
    /// Fetch more information about this character's unlocked minions.
    /// </summary>
    /// <returns><see cref="CharacterCollectable"/> object holding this information.</returns>
    public async Task<CharacterCollectable?> GetMinions() => await this._client.GetCharacterMinion(this._charId);

    /// <summary>
    /// String representation of this character.
    /// </summary>
    /// <returns>"Name on World"</returns>
    public override string ToString() => $"{this.Name} on {this.Server}";
}