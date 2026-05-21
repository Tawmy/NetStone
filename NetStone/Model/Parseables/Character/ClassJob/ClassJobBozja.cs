using AngleSharp.Dom;
using NetStone.Definitions.Model.Character;

namespace NetStone.Model.Parseables.Character.ClassJob;

/// <summary>
/// Entry for Bozja Field Operation of a character
/// </summary>
public class ClassJobBozja : LodestoneParseable, IOptionalParseable<ClassJobBozja>
{
	private readonly ClassJobBozjaDefinition _definition;

	/// <summary>
	/// Constructs a new class entry
	/// </summary>
	/// <param name="rootNode">Root node of this entry</param>
	/// <param name="definition">Parser definition</param>
	public ClassJobBozja(IElement rootNode, ClassJobBozjaDefinition definition) : base(rootNode)
	{
		this._definition = definition;
	}

	/// <summary>
	/// The name of this class and job combo.
	/// </summary>
	public string Name => Parse(this._definition.Name);

	/// <summary>
	/// The level this class or job is at.
	/// </summary>
	public int Level => int.TryParse(Parse(this._definition.Level), out var levelOut) ? levelOut : 0 ;
	
	/// <summary>
	/// The amount of current achieved EXP on this level.
	/// </summary>
	public int MettleCurrent => int.TryParse(Parse(this._definition.Mettle, "Mettle").Replace(",", ""), out var mettleCurrent)
		? mettleCurrent
		: 0;
	
	/// <summary>
	/// The amount of EXP to be reached to gain the next level.
	/// </summary>
	public int MettleMax => int.TryParse(Parse(this._definition.Mettle, "MettleNextRank").Replace(",", ""), out var mettleMax)
		? mettleMax
		: 0;

	/// <summary>
	/// The outstanding amount of EXP to go to the next level.
	/// </summary>
	public int MettleToGo => this.MettleMax - this.MettleCurrent;

	/// <summary>
	/// Value indicating if this class is unlocked.
	/// </summary>
	public bool Exists => this.Level != 0;

	/// <summary>
	/// The string representation of this object.
	/// </summary>
	/// <returns>Name (Level)</returns>
	public override string ToString() => $"{this.Name} ({this.Level})";
}