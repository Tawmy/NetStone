using AngleSharp.Dom;
using NetStone.Definitions.Model.FreeCompany;

namespace NetStone.Model.Parseables.FreeCompany;

/// <summary>
/// Information about the Free CCompany's estate
/// </summary>
public class FreeCompanyEstate : LodestoneParseable, IOptionalParseable<FreeCompanyEstate>
{
    private readonly EstateDefinition _definition;

    ///<inheritdoc />
    public FreeCompanyEstate(IElement rootNode, EstateDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// Name of the estate
    /// </summary>
    public string Name => Parse(this._definition.Name);

    /// <summary>
    /// The greeting phrase for this estate
    /// </summary>
    public string Greeting => Parse(this._definition.Greeting);

    /// <summary>
    /// The plot where the estate is built
    /// </summary>
    public string Plot => Parse(this._definition.Plot);

    ///<inheritdoc />
    public bool Exists => !HasNode(this._definition.NoEstate);
}