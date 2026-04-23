using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NetStone.Definitions;
using NetStone.Definitions.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NetStone.Model;

/// <summary>
/// Main superclass for parsed lodestone nodes.
/// </summary>
public abstract class LodestoneParseable
{
    /// <summary>
    /// The HTML document's root node.
    /// </summary>
    protected readonly HtmlNode RootNode;

    /// <summary>
    /// Constructs an instance of parseable data for given node
    /// </summary>
    /// <param name="rootNode"></param>
    protected LodestoneParseable(HtmlNode rootNode)
    {
        this.RootNode = rootNode;
    }

    /// <summary>
    /// Query a <see cref="HtmlNode"/> via pack selector.
    /// </summary>
    /// <param name="pack">Definition of the node.</param>
    /// <returns>The needed node.</returns>
    protected HtmlNode? QueryNode(DefinitionsPack pack) => this.RootNode.QuerySelector(pack.Selector);

    /// <summary>
    /// Query all ChildNodes of a <see cref="HtmlNode"/> via pack selector.
    /// Removes unneeded "#text" nodes.
    /// </summary>
    /// <param name="pack">Definition of the node.</param>
    /// <returns>All ChildNodes.</returns>
    protected HtmlNode[] QueryChildNodes(DefinitionsPack pack) => this.RootNode
        .QuerySelectorAll(pack.Selector)
        .Where(x => x.Name != "#text")
        .ToArray();

    /// <summary>
    /// Get a list of root nodes for entries of this paged list.
    /// Throws <see cref="ArgumentException"/> if definition does not contain a entry definition
    /// </summary>
    /// <param name="pagedDefinition">Parser definition</param>
    /// <returns>List of nodes</returns>
    /// <exception cref="ArgumentException"></exception>
    protected HtmlNode[] QueryContainer<TEntry>(PagedDefinition<TEntry> pagedDefinition) where TEntry : PagedEntryDefinition
    {
        var entryDef = pagedDefinition.Entry;

        if (entryDef == null)
            throw new ArgumentException("Could not get entry definition");

        return QueryNode(pagedDefinition.Root)
            ?.QuerySelectorAll(entryDef.Root.Selector).ToArray() ?? Array.Empty<HtmlNode>();
    }

    /// <summary>
    /// Indicates if a node for that definition exists.
    /// </summary>
    /// <param name="pack">Definition of the node.</param>
    /// <returns>True the node existence, false otherwise</returns>
    protected bool HasNode(DefinitionsPack pack) => QueryNode(pack) != null;

    /// <summary>
    /// Parse via selector. Attribute and regex from selector will be used.
    /// </summary>
    /// <param name="pack">Definition of the node.</param>
    /// <param name="regexGroup">Group to select from regex with named groups.</param>
    /// <returns>InnerText of the node or empty string on parse error.</returns>
    protected string Parse(DefinitionsPack pack, string? regexGroup = null)
    {
        var result = ParseInternal(pack);

        if (!string.IsNullOrEmpty(pack.Regex))
        {
            result = ParseRegex(pack, result, regexGroup) ?? "";
        }
        
        return result;
    }

    /// <summary>
    /// Parse a Lodestone Uri. Parsed Uris are relative and will have the Lodestone base URL prepended.
    /// </summary>
    /// <param name="pack"></param>
    /// <returns></returns>
    protected Uri ParseLodestoneUri(DefinitionsPack pack)
    {
        var href = ParseInternal(pack);
        
        if (!href.StartsWith("http://", StringComparison.InvariantCulture) &&
            !href.StartsWith("https://", StringComparison.InvariantCulture))
        {
            var prefix = !href.StartsWith('/') ? "/" : string.Empty;
            href = $"{Constants.LodestoneBase}{prefix}{href}";
        }

        return new Uri(href);
    }

    /// <summary>
    /// Try to parse a Lodestone Uri. Parsed Uris are relative and will have the Lodestone base URL prepended.
    /// </summary>
    /// <returns></returns>
    protected bool TryParseLodestoneUri(DefinitionsPack pack, out Uri? result)
    {
        result = null;
        try
        {
            result = ParseLodestoneUri(pack);
        }
        catch
        {
            // ignored
        }

        return result is not null;
    }

    private string ParseInternal(DefinitionsPack pack)
    {
        string? result;
        if (!string.IsNullOrEmpty(pack.Attribute))
        {
            result = ParseAttribute(pack);
        }
        else if (!string.IsNullOrEmpty(pack.Regex))
        {
            result = ParseInnerHtml(pack);
        }
        else
        {
            result = ParseInnerText(pack);
        }

        return result ?? string.Empty;
    }

    /// <summary>
    /// Get the inner text of a node
    /// </summary>
    /// <param name="pack">Definition of node</param>
    /// <returns>Text inside node</returns>
    private string ParseInnerText(DefinitionsPack pack)
    {
        var node = QueryNode(pack);

        // Handle default attribute parsing
        var text = node?.InnerText;

        return !string.IsNullOrEmpty(text) ? HttpUtility.HtmlDecode(text) : string.Empty;
    }

    /// <summary>
    /// Get the inner html of a node
    /// </summary>
    /// <param name="pack">Definition of node</param>
    /// <returns>Text inside node</returns>
    private string ParseInnerHtml(DefinitionsPack pack)
    {
        var node = QueryNode(pack);

        // Handle default attribute parsing
        var text = node?.InnerHtml;

        return !string.IsNullOrEmpty(text) ? HttpUtility.HtmlDecode(text) : string.Empty;
    }

    /// <summary>
    /// Parse attribute from pack.
    /// </summary>
    /// <param name="pack">Definition of the node.</param>
    /// <returns>Parsed attribute.</returns>
    private string? ParseAttribute(DefinitionsPack pack) =>
        pack.Attribute == null ? null : ParseAttribute(pack, pack.Attribute);

    /// <summary>
    /// Parse specified attribute via selector from pack.
    /// </summary>
    /// <param name="pack">Definition of the node.</param>
    /// <param name="attribute">Attribute to parse.</param>
    /// <returns>Parsed attribute.</returns>
    private string? ParseAttribute(DefinitionsPack pack, string attribute)
    {
        var node = QueryNode(pack);

        return node?.Attributes.FirstOrDefault(x => x.Name == attribute)?.Value;
    }

    /// <summary>
    /// Parse a timestamp
    /// </summary>
    /// <param name="pack">Selector definition for timestamp</param>
    /// <returns>Parsed Unix timestamp</returns>
    protected DateTime ParseTime(DefinitionsPack pack)
    {
        var res = Parse(pack);
        return DateTimeOffset.FromUnixTimeSeconds(long.Parse(res)).UtcDateTime;
    }

    private static string? ParseRegex(DefinitionsPack pack, string text, string? regexSelector = null)
    {
        var regex = new Regex(pack.Regex ?? "");
        var match = regex.Match(text);

        if (match.Groups.Count < 2)
        {
            return null;
        }

        if (pack.Type?.Equals("boolean", StringComparison.OrdinalIgnoreCase) == true)
        {
            return "true";
        }

        return regexSelector is not null 
            ? match.Groups[regexSelector].Value 
            : match.Groups[1].Value;
    }
}