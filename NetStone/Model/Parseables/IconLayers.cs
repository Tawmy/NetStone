using System;
using HtmlAgilityPack;
using NetStone.Definitions.Model;

namespace NetStone.Model.Parseables;

/// <summary>
/// Container class holding information about a social group's icon.
/// </summary>
public class IconLayers : LodestoneParseable
{
    private readonly IconLayersDefinition _definition;

    ///<inheritdoc />
    public IconLayers(HtmlNode rootNode, IconLayersDefinition definition) : base(rootNode)
    {
        this._definition = definition;
    }

    /// <summary>
    /// Link to the top layer image of the icon.
    /// </summary>
    public Uri? TopLayer => Parse(this._definition.Top) is { Length: > 0 } topLayer ? new Uri(topLayer) : null;

    /// <summary>
    /// Link to the top layer image of the icon.
    /// </summary>
    public Uri? MiddleLayer => Parse(this._definition.Middle) is { Length: > 0 } middleLayer ? new Uri(middleLayer) : null;

    /// <summary>
    /// Link to the top layer image of the icon.
    /// </summary>
    public Uri? BottomLayer => Parse(this._definition.Bottom) is { Length: > 0 } bottomLayer ? new Uri(bottomLayer) : null;
}