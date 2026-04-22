using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.ClassJob;

/// <summary>
/// Entry for one class/job of a character
/// </summary>
public class ClassJobEntry : LodestoneParseable, IOptionalParseable<ClassJobEntry>
{
    private readonly ClassJobEntryDefinition definition;

    /// <summary>
    /// Constructs a new class entry
    /// </summary>
    /// <param name="rootNode">Root node of this entry</param>
    /// <param name="definition">Parser definition</param>
    public ClassJobEntry(HtmlNode rootNode, ClassJobEntryDefinition definition) : base(rootNode)
    {
        this.definition = definition;
    }

    /// <summary>
    /// The name of this class or job.
    /// </summary>
    public string Name => Parse(this.definition.Name);

    /// <summary>
    /// The name of this class and job combo as shown in its tooltip.
    /// </summary>
    public string Tooltip => Parse(this.definition.Tooltip);

    /// <summary>
    /// Value indicating whether this class has its job unlocked.
    /// </summary>
    public bool IsJobUnlocked => this.Tooltip.Contains("/");

    /// <summary>
    /// The level this class or job is at.
    /// </summary>
    public int Level
    {
        get
        {
            var level = Parse(this.definition.Level);
            return level == "-" ? 0 : int.Parse(level);
        }
    }

    /// <summary>
    /// The amount of current achieved EXP on this level.
    /// </summary>
    public long ExpCurrent => long.TryParse(Parse(this.definition.Exp, "CurrentEXP").Replace(",", ""), out var expCurrent)
        ? expCurrent
        : 0;

    /// <summary>
    /// The amount of EXP to be reached to gain the next level.
    /// </summary>
    public long ExpMax => long.TryParse(Parse(this.definition.Exp, "MaxEXP").Replace(",", ""), out var expMax)
        ? expMax 
        : 0;

    /// <summary>
    /// The outstanding amount of EXP to go to the next level.
    /// </summary>
    public long ExpToGo => this.ExpMax - this.ExpCurrent;

    /// <summary>
    /// Value indicating whether this job, if DoH or DoL, is specialized.
    /// </summary>
    public bool IsSpecialized => bool.TryParse(Parse(this.definition.IsSpecialized, "class"), out var isSpecialized) && isSpecialized;

    /// <summary>
    /// Value indicating if this class is unlocked.
    /// </summary>
    public bool Exists => this.Level != 0;

    /// <summary>
    /// Value indicating if this class is unlocked.
    /// </summary>
    public bool IsUnlocked => this.Exists;

    /// <summary>
    /// The string representation of this object.
    /// </summary>
    /// <returns>Name (Level)</returns>
    public override string ToString() => $"{this.Name} ({this.Level})";
}