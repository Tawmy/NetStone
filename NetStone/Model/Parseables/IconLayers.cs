using System;
using HtmlAgilityPack;
using NetStone.Definitions.Model;

namespace NetStone.Model.Parseables;

/// <summary>
/// Container class holding information about a social group's icon.
/// </summary>
public class IconLayers : LodestoneParseable
{
    private readonly IconLayersDefinition definition;

    ///<inheritdoc />
    public IconLayers(HtmlNode rootNode, IconLayersDefinition definition) : base(rootNode)
    {
        this.definition = definition;
    }

    /// <summary>
    /// Link to the top layer image of the icon.
    /// </summary>
    public Uri? TopLayer => Parse(this.definition.Top) is { Length: > 0 } topLayer ? new Uri(topLayer) : null;

    /// <summary>
    /// Link to the top layer image of the icon.
    /// </summary>
    public Uri? MiddleLayer => Parse(this.definition.Middle) is { Length: > 0 } middleLayer ? new Uri(middleLayer) : null;

    /// <summary>
    /// Link to the top layer image of the icon.
    /// </summary>
    public Uri? BottomLayer => Parse(this.definition.Bottom) is { Length: > 0 } bottomLayer ? new Uri(bottomLayer) : null;
}