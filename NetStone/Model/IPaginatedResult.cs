using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using NetStone.Definitions.Model;
using NetStone.Search;

namespace NetStone.Model;

/// <summary>
/// Models data that is presented over multiple pages
/// </summary>
/// <typeparam name="T">Type of data presented</typeparam>
public interface IPaginatedResult<T> where T : LodestoneParseable
{
    /// <summary>
    /// Currently handled page
    /// </summary>
    int CurrentPage { get; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    int NumPages { get; }

    /// <summary>
    /// Gets the next page of results
    /// </summary>
    /// <returns>Task of retrieving next page</returns>
    Task<T?> GetNextPage();
}

/// <summary>
/// Container class holding paginated information
/// </summary>
public abstract class PaginatedIdResult<TPage, TEntry, TEntryDef> 
    : PaginatedResult<TPage, TEntry, TEntryDef,string> where TPage : LodestoneParseable 
                                                       where TEntry : LodestoneParseable 
                                                       where TEntryDef : PagedEntryDefinition
{
    ///<inheritdoc />
    protected PaginatedIdResult(IElement rootNode, PagedDefinition<TEntryDef> pageDefinition, 
                                Func<string, int, Task<TPage?>> nextPageFunc, string id) 
        : base(rootNode, pageDefinition, nextPageFunc, id)
    {
    }
}
/// <summary>
/// Container class holding paginated information
/// </summary>
public abstract class PaginatedSearchResult<TPage, TEntry, TEntryDef, TQuery> 
    : PaginatedResult<TPage, TEntry, TEntryDef,TQuery> where TPage : LodestoneParseable 
                                                       where TEntry : LodestoneParseable 
                                                       where TEntryDef : PagedEntryDefinition
                                                       where TQuery : ISearchQuery
{
    ///<inheritdoc />
    protected PaginatedSearchResult(IElement rootNode, PagedDefinition<TEntryDef> pageDefinition, 
                                    Func<TQuery, int, Task<TPage?>> nextPageFunc, 
                                    TQuery query) 
        : base(rootNode, pageDefinition, nextPageFunc, query)
    {
    }
}


/// <summary>
    /// Container class holding paginated information
    /// </summary>
    public abstract class PaginatedResult<TPage, TEntry, TEntryDef,TRequest> : LodestoneParseable, IPaginatedResult<TPage> where TPage : LodestoneParseable where TEntry : LodestoneParseable where TEntryDef : PagedEntryDefinition
    {
    /// <summary>
    /// Definition for the paginated type
    /// </summary>
    protected readonly PagedDefinition<TEntryDef> PageDefinition;

    private readonly TRequest _request;
    
    private readonly Func<TRequest, int, Task<TPage?>> _nextPageFunc;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rootNode">The root document node of the page</param>
    /// <param name="pageDefinition">CSS definitions for the paginated type</param>
    /// <param name="nextPageFunc">Function to retrieve a page of this type</param>
    /// <param name="request">The input used to request further pages.</param>
    protected PaginatedResult(IElement rootNode, PagedDefinition<TEntryDef> pageDefinition,Func<TRequest, int, Task<TPage?>> nextPageFunc, TRequest request) : base(rootNode)
    {
        this.PageDefinition = pageDefinition;
        this._request = request;
        this._nextPageFunc = nextPageFunc;
    }

    /// <summary>
    /// If there is any data
    /// </summary>
    public bool HasResults => this.PageDefinition.NoResultsFound is null || !HasNode(this.PageDefinition.NoResultsFound);
    
    private TEntry[]? _parsedResults;
    
    /// <summary>
    /// List of members
    /// </summary>
    protected IEnumerable<TEntry> Results
    {
        get
        {
            if (!this.HasResults) return Array.Empty<TEntry>();
            this._parsedResults ??= ParseResults();
            return this._parsedResults;
        }
    }

    /// <summary>
    /// Creates the array of all entries on this page>
    /// </summary>
    protected abstract TEntry[] ParseResults();
    
    private int? _currentPageVal;

    ///<inheritdoc />
    public int CurrentPage
    {
        get
        {
            if (!this.HasResults)
                return 0;
            if (!this._currentPageVal.HasValue)
                ParsePagesCount();

            return this._currentPageVal!.Value;
        }
    }

    private int? _numPagesVal;

    /// <inheritdoc/>
    public int NumPages
    {
        get
        {
            if (!this.HasResults)
                return 0;
            if (!this._numPagesVal.HasValue)
                ParsePagesCount();

            return this._numPagesVal!.Value;
        }
    }
    private void ParsePagesCount()
    {
        this._currentPageVal = int.Parse(Parse(this.PageDefinition.PageInfo, "CurrentPage"));
        this._numPagesVal = int.Parse(Parse(this.PageDefinition.PageInfo, "NumPages"));
    }
    
    /// <inheritdoc />
    public async Task<TPage?> GetNextPage()
    {
        if (!this.HasResults)
            return null;
        
        if (this.CurrentPage == this.NumPages)
            return null;

        return await this._nextPageFunc(this._request, this.CurrentPage + 1);
    }
}