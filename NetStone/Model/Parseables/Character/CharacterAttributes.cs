﻿using HtmlAgilityPack;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character;

/// <summary>
/// Provides information about a characters attributes
/// </summary>
public class CharacterAttributes : LodestoneParseable
{
    private readonly CharacterAttributesDefinition definition;

    /// <summary>
    /// Creates an instance that provides Attributes for given node
    /// </summary>
    /// <param name="rootNode">Root HTML node of the character profile page on Lodestone</param>
    /// <param name="definition">Definitions on how to parse attributes from the HTML</param>
    public CharacterAttributes(HtmlNode rootNode, CharacterAttributesDefinition definition) : base(rootNode)
    {
        this.definition = definition;
    }

    /// <summary>
    /// This characters' Strength value.
    /// </summary>
    public int Strength => int.Parse(Parse(this.definition.Strength));

    /// <summary>
    /// This characters' Dexterity value.
    /// </summary>
    public int Dexterity => int.Parse(Parse(this.definition.Dexterity));

    /// <summary>
    /// This characters' Vitality value.
    /// </summary>
    public int Vitality => int.Parse(Parse(this.definition.Vitality));

    /// <summary>
    /// This characters' Intelligence value.
    /// </summary>
    public int Intelligence => int.Parse(Parse(this.definition.Intelligence));

    /// <summary>
    /// This characters' Mind value.
    /// </summary>
    public int Mind => int.Parse(Parse(this.definition.Mind));

    /// <summary>
    /// This characters' Critical Hit Rate value.
    /// </summary>
    public int CriticalHitRate => int.Parse(Parse(this.definition.CriticalHitRate));

    /// <summary>
    /// This characters' Determination value.
    /// </summary>
    public int Determination => int.Parse(Parse(this.definition.Determination));

    /// <summary>
    /// This characters' Direct Hit Rate value.
    /// </summary>
    public int DirectHitRate => int.Parse(Parse(this.definition.DirectHitRate));

    /// <summary>
    /// This characters' Defense value.
    /// </summary>
    public int Defense => int.Parse(Parse(this.definition.Defense));

    /// <summary>
    /// This characters' Magic Defense value.
    /// </summary>
    public int MagicDefense => int.Parse(Parse(this.definition.MagicDefense));

    /// <summary>
    /// This characters' Attack Power value.
    /// </summary>
    public int AttackPower => int.Parse(Parse(this.definition.AttackPower));

    /// <summary>
    /// This characters' Skill Speed value.
    /// </summary>
    public int SkillSpeed => int.Parse(Parse(this.definition.SkillSpeed));

    /// <summary>
    /// This characters' Attack Magic Potency value.
    /// </summary>
    /// <remarks>This value is only set for disciples of war/magic.</remarks>
    public int? AttackMagicPotency => MpGpCpParameterName == "MP" ? int.Parse(Parse(this.definition.AttackMagicPotency)) : null;

    /// <summary>
    /// This characters' Healing Magic Potency value.
    /// </summary>
    /// <remarks>This value is only set for disciples of war/magic.</remarks>
    public int? HealingMagicPotency => MpGpCpParameterName == "MP" ? int.Parse(Parse(this.definition.HealingMagicPotency)) : null;

    /// <summary>
    /// This characters' Spell Speed value.
    /// </summary>
    /// <remarks>This value is only set for disciples of war/magic.</remarks>
    public int? SpellSpeed => int.TryParse(Parse(this.definition.SpellSpeed), out var result) ? result : null;

    /// <summary>
    /// This characters' Tenacity value.
    /// </summary>
    public int? Tenacity => int.TryParse(Parse(this.definition.Tenacity), out var result) ? result : null;

    /// <summary>
    /// This characters' Piety value.
    /// </summary>
    public int? Piety => int.TryParse(Parse(this.definition.Piety), out var result) ? result : null;

    /// <summary>
    /// This characters' Craftmanship value.
    /// </summary>
    /// <remarks>This value is only set for disciples of the hand.</remarks>
    public int? Craftmanship => MpGpCpParameterName == "CP" ? AttackMagicPotencyValue : null;
    
    /// <summary>
    /// This characters' Control value.
    /// </summary>
    /// <remarks>This value is only set for disciples of the hand.</remarks>
    public int? Control => MpGpCpParameterName == "CP" ? HealingMagicPotencyValue : null;

    /// <summary>
    /// This characters' Gathering value.
    /// </summary>
    /// <remarks>This value is only set for disciples of the land.</remarks>
    public int? Gathering => MpGpCpParameterName == "GP" ? AttackMagicPotencyValue : null;
    
    /// <summary>
    /// This characters' Perception value.
    /// </summary>
    /// <remarks>This value is only set for disciples of the land.</remarks>
    public int? Perception => MpGpCpParameterName == "GP" ? HealingMagicPotencyValue : null;

    /// <summary>
    /// This characters' HP value.
    /// </summary>
    public int Hp => int.Parse(Parse(this.definition.Hp));

    /// <summary>
    /// This characters' MP, GP or CP value. Check the <see cref="MpGpCpParameterName"/> Property to find out which.
    /// </summary>
    public int MpGpCp => int.Parse(Parse(this.definition.MpGpCp));

    /// <summary>
    /// Value indicating which of MP, GP, or CP is indicated by <see cref="MpGpCp"/>.
    /// </summary>
    public string MpGpCpParameterName => Parse(this.definition.MpGpCpParameterName);
    
    private int AttackMagicPotencyValue => int.Parse(Parse(this.definition.AttackMagicPotency));
    
    private int HealingMagicPotencyValue => int.Parse(Parse(this.definition.HealingMagicPotency));
}