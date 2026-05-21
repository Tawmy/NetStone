using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.ClassJob;

/// <summary>
/// Entry for Eureka Field Operation of a character
/// </summary>
public class ClassJobEureka : LodestoneParseable, IOptionalParseable<ClassJobEureka>
{
	private readonly ClassJobEurekaDefinition _definition;

	/// <summary>
	/// Constructs a new class entry
	/// </summary>
	/// <param name="rootNode">Root node of this entry</param>
	/// <param name="definition">Parser definition</param>
	public ClassJobEureka(IElement rootNode, ClassJobEurekaDefinition definition) : base(rootNode)
	{
		this._definition = definition;
	}

	/// <summary>
	/// The name of this class and job combo.
	/// </summary>
	public string Name => Parse(this._definition.Name);

	/// <summary>
	/// Value indicating whether this class has its job unlocked.
	/// </summary>
	public bool IsJobUnlocked => this.Name.Contains("/");

	/// <summary>
	/// The level this class or job is at.
	/// </summary>
	public int Level => int.TryParse(Parse(this._definition.Level), out var level)? level : 0;

	/// <summary>
	/// The amount of current achieved EXP on this level.
	/// </summary>
	public int ExpCurrent => int.TryParse(Parse(this._definition.Exp, "CurrentEXP").Replace(",", ""), out var expCurrent)
		? expCurrent
		: 0;

	/// <summary>
	/// The amount of EXP to be reached to gain the next level.
	/// </summary>
	public int ExpMax => int.TryParse(Parse(this._definition.Exp, "MaxEXP").Replace(",", ""), out var expMax)
		? expMax
		: 0;

	/// <summary>
	/// The outstanding amount of EXP to go to the next level.
	/// </summary>
	public int ExpToGo => this.ExpMax - this.ExpCurrent;

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