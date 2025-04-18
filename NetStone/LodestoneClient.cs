﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetStone.Definitions;
using NetStone.GameData;
using NetStone.Model;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.CWLS;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;
using NetStone.Model.Parseables.Linkshell;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Model.Parseables.Search.CWLS;
using NetStone.Model.Parseables.Search.FreeCompany;
using NetStone.Model.Parseables.Search.Linkshell;
using NetStone.Search.Character;
using NetStone.Search.FreeCompany;
using NetStone.Search.Linkshell;

namespace NetStone;

/// <summary>
/// The main Lodestone client class offering parsed information and search.
/// </summary>
public class LodestoneClient : IDisposable
{
    // TODO: Switch to URLs in meta.json

    /// <summary>
    /// Container holding information about the current Lodestone layout, needed to parse responses.
    /// </summary>
    public DefinitionsContainer Definitions { get; }

    /// <summary>
    /// The service providing game data to the Lodestone parser.
    /// </summary>
    public IGameDataProvider? Data { get; set; }

    private readonly HttpClient client;

    /// <summary>
    /// Initialize a new Lodestone client with default options.
    /// </summary>
    private LodestoneClient(DefinitionsContainer definitions, IGameDataProvider? gameData = null,
        string lodestoneBaseAddress = Constants.LodestoneBase)
    {
        this.client = new HttpClient
        {
            BaseAddress = new Uri(lodestoneBaseAddress),
        };

        this.Definitions = definitions;
        this.Data = gameData;
    }

    /// <summary>
    /// Initializes and returns a new Lodestone client with custom definitions provided<br/>
    /// To get a client with standard definitions use <see cref="GetClientAsync"/>
    /// </summary>
    /// <param name="definitions">A set of custom definitions </param>
    /// <param name="gameData">Service providing game data for parsing</param>
    /// <param name="lodestoneBaseAddress">Base address for Lodestone access (defaults to EU Lodestone)</param>
    /// <returns></returns>
    public static LodestoneClient GetCustomClient(DefinitionsContainer definitions,IGameDataProvider? gameData = null,
                                                             string lodestoneBaseAddress = Constants.LodestoneBase)
    {
        return new LodestoneClient(definitions, gameData, lodestoneBaseAddress);
    }
    
    /// <summary>
    /// Initializes and returns a new Lodestone client with current definitions loaded from xivapi/lodestone-css-selectors
    /// </summary>
    /// <param name="gameData">Service providing game data for parsing</param>
    /// <param name="lodestoneBaseAddress">Base address for Lodestone access (defaults to EU Lodestone)</param>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <returns></returns>
    public static async Task<LodestoneClient> GetClientAsync(IGameDataProvider? gameData = null,
        string lodestoneBaseAddress = Constants.LodestoneBase)
    {
        var definitions = new XivApiDefinitionsContainer();
        await definitions.Reload();

        return new LodestoneClient(definitions, gameData, lodestoneBaseAddress);
    }

    #region Character

    /// <summary>
    /// Get a character by its Lodestone ID.
    /// </summary>
    /// <param name="id">The ID of the character.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="LodestoneCharacter"/> class containing information about the character.</returns>
    public async Task<LodestoneCharacter?> GetCharacter(string id) => await GetParsed($"/lodestone/character/{id}/",
        node => new LodestoneCharacter(this, node, this.Definitions, id));

    /// <summary>
    /// Get a characters' class/job information by its Lodestone ID.
    /// You can also get this from the character directly by calling <see cref="LodestoneCharacter.GetClassJobInfo()"/>.
    /// </summary>
    /// <param name="id">The ID of the character.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="CharacterClassJob"/> class containing information about the characters' classes and jobs.</returns>
    public async Task<CharacterClassJob?> GetCharacterClassJob(string id) => await GetParsed(
        $"/lodestone/character/{id}/class_job/", node => new CharacterClassJob(node, this.Definitions.ClassJob));

    /// <summary>
    /// Get a characters' unlocked achievement information by its Lodestone ID.
    /// You can also get this from the character directly by calling <see cref="LodestoneCharacter.GetAchievement()"/>.
    /// </summary>
    /// <param name="id">The ID of the character.</param>
    /// <param name="page">The number of the page that should be fetched.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="CharacterAchievementPage"/> class containing information about the characters' achievements.</returns>
    public async Task<CharacterAchievementPage?> GetCharacterAchievement(string id, int page = 1) =>
        await GetParsed(
            $"/lodestone/character/{id}/achievement/?page={page}",
            node => new CharacterAchievementPage(this, node, this.Definitions.Achievement, id));

    /// <summary>
    /// Get a characters' unlocked mount information by its Lodestone ID.
    /// You can also get this from the character directly by calling <see cref="LodestoneCharacter.GetMounts()"/>.
    /// </summary>
    /// <param name="id">The ID of the character.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="CharacterCollectable"/> class containing information about the characters' mounts.</returns>
    public async Task<CharacterCollectable?> GetCharacterMount(string id) => await GetParsed(
        $"/lodestone/character/{id}/mount/",
        node => new CharacterCollectable(node, this.Definitions.Mount),
        UserAgent.Mobile);

    /// <summary>
    /// Get a characters' unlocked minion information by its Lodestone ID.
    /// You can also get this from the character directly by calling <see cref="LodestoneCharacter.GetMinions()"/>.
    /// </summary>
    /// <param name="id">The ID of the character.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="CharacterCollectable"/> class containing information about the characters' minions.</returns>
    public async Task<CharacterCollectable?> GetCharacterMinion(string id) => await GetParsed(
        $"/lodestone/character/{id}/minion/",
        node => new CharacterCollectable(node, this.Definitions.Minion),
        UserAgent.Mobile);

    /// <summary>
    /// Search lodestone for a character with the specified query.
    /// </summary>
    /// <param name="query"><see cref="CharacterSearchQuery"/> object detailing search parameters</param>
    /// <param name="page">The page of search results to fetch.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="CharacterSearchPage"/> containing search results.</returns>
    public async Task<CharacterSearchPage?> SearchCharacter(CharacterSearchQuery query, int page = 1) =>
        await GetParsed($"/lodestone/character/{query.BuildQueryString()}&page={page}",
            node => new CharacterSearchPage(this, node, this.Definitions.CharacterSearch, query));
    
    #endregion
    
    #region Linkshells
    /// <summary>
    /// Gets a cross world link shell by its id.
    /// </summary>
    /// <param name="id">The ID of the cross world linkshell.</param>
    /// <param name="page"></param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="LodestoneCrossworldLinkshell"/> class containing information about the cross world link shell</returns>
    public async Task<LodestoneCrossworldLinkshell?> GetCrossworldLinkshell(string id, int page = 1) => 
        await GetParsed($"/lodestone/crossworld_linkshell/{id}?page={page}",
                        node => new LodestoneCrossworldLinkshell(this, node, this.Definitions,id));
    
    /// <summary>
    /// Search lodestone for a character with the specified query.
    /// </summary>
    /// <param name="query"><see cref="CharacterSearchQuery"/> object detailing search parameters</param>
    /// <param name="page">The page of search results to fetch.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="CharacterSearchPage"/> containing search results.</returns>
    public async Task<CrossworldLinkshellSearchPage?> SearchCrossworldLinkshell(CrossworldLinkshellSearchQuery query, int page = 1) =>
        await GetParsed($"/lodestone/crossworld_linkshell/{query.BuildQueryString()}&page={page}",
                        node => new CrossworldLinkshellSearchPage(this, node, this.Definitions.CrossworldLinkshellSearch, query));
    
    /// <summary>
    /// Gets a link shell by its id.
    /// </summary>
    /// <param name="id">The ID of the linkshell.</param>
    /// <param name="page"></param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="LodestoneCrossworldLinkshell"/> class containing information about the cross world link shell</returns>
    public async Task<LodestoneLinkshell?> GetLinkshell(string id, int page = 1) =>
        await GetParsed($"/lodestone/linkshell/{id}?page={page}",
                        node => new LodestoneLinkshell(this, node, this.Definitions,id));
    
    /// <summary>
    /// Search lodestone for a linkshell with the specified query.
    /// </summary>
    /// <param name="query"><see cref="LinkshellSearchQuery"/> object detailing search parameters</param>
    /// <param name="page">The page of search results to fetch.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="LinkshellSearchPage"/> containing search results.</returns>
    public async Task<LinkshellSearchPage?> SearchLinkshell(LinkshellSearchQuery query, int page = 1) =>
        await GetParsed($"/lodestone/linkshell/{query.BuildQueryString()}&page={page}",
                        node => new LinkshellSearchPage(this, node, this.Definitions.LinkshellSearch, query));
    
    #endregion

    #region FreeCompany

    /// <summary>
    /// Get a character by its Lodestone ID.
    /// </summary>
    /// <param name="id">The ID of the character.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="LodestoneFreeCompany"/> class containing information about the character.</returns>
    public async Task<LodestoneFreeCompany?> GetFreeCompany(string id) => await GetParsed(
        $"/lodestone/freecompany/{id}/", node => new LodestoneFreeCompany(this, node, this.Definitions, id));

    /// <summary>
    /// Get the members of a Free Company
    /// </summary>
    /// <param name="id">The ID of the free company.</param>
    /// <param name="page">The page of members to fetch.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="FreeCompanyMembers"/> class containing information about FC members.</returns>
    public async Task<FreeCompanyMembers?> GetFreeCompanyMembers(string id, int page = 1) => await GetParsed(
        $"/lodestone/freecompany/{id}/member/?page={page}",
        node => new FreeCompanyMembers(this, node, this.Definitions.FreeCompanyMembers, id));

    /// <summary>
    /// Search lodestone for a free company with the specified query.
    /// </summary>
    /// <param name="query"><see cref="FreeCompanySearchPage"/> object detailing search parameters.</param>
    /// <param name="page">The page of search results to fetch.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns><see cref="FreeCompanySearchPage"/> containing search results.</returns>
    public async Task<FreeCompanySearchPage?> SearchFreeCompany(FreeCompanySearchQuery query, int page = 1) =>
        await GetParsed($"/lodestone/freecompany/{query.BuildQueryString()}&page={page}",
            node => new FreeCompanySearchPage(this, node, this.Definitions.FreeCompanySearch, query));

    #endregion

    /// <summary>
    /// Get the instantiated LodestoneParseable if the request succeeded, or null in case of StatusCode.NotFound.
    /// </summary>
    /// <typeparam name="T">The LodestoneParseable descendant to instantiate.</typeparam>
    /// <param name="url">The URL to fetch.</param>
    /// <param name="createParseable">Func creating the LodestoneParseable.</param>
    /// <param name="agent">The user agent to use for the request.</param>
    /// <exception cref="HttpRequestException"> The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <returns>The instantiated LodestoneParseable in case of success.</returns>
    private async Task<T?> GetParsed<T>(string url, Func<HtmlNode, T?> createParseable,
        UserAgent agent = UserAgent.Desktop) where T : LodestoneParseable
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        switch (agent)
        {
            case UserAgent.Desktop:
                request.Headers.UserAgent.ParseAdd(this.Definitions.Meta.UserAgentDesktop);
                break;
            case UserAgent.Mobile:
                request.Headers.UserAgent.ParseAdd(this.Definitions.Meta.UserAgentMobile);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(agent), agent, null);
        }

        var response = await this.client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(await response.Content.ReadAsStringAsync());

        return createParseable.Invoke(doc.DocumentNode);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.client.Dispose();
        this.Definitions.Dispose();
    }
}