using Eum.Connections.Spotify.JsonConverters;
using Eum.Connections.Spotify.Models.Users;
using Eum.UI.Items;
using Eum.UI.ViewModels.Search.Patterns;

namespace Eum.UI.ViewModels.Search.SearchItems;

public class SpotifySearchItem : ISearchItem
{
	public SpotifySearchItem(string title, string description, string image, SpotifyId id,  string category, int categoryOrder)
    {
        Description = description;
        Image = image;
        Id = new ItemId(id.Uri);
        Name = title;
        Category = category;
        CategoryOrder = categoryOrder;
    }
	public ItemId Id { get; }
	public string Name { get; }
	public string Description { get; }
	public ComposedKey Key => new($"{Id.Id}{CategoryOrder}");
	public string? Image { get; set; }
	public string Category { get; }
    public int CategoryOrder { get; }
}

public class SpotifyTrackSearchItem : SpotifySearchItem
{
    public SpotifyTrackSearchItem(MercurySearchTrack mercurySearchTrack,
        string category,
        int categoryOrder) : base(mercurySearchTrack.Title, mercurySearchTrack.Description, mercurySearchTrack.Image,
        mercurySearchTrack.Id, category, categoryOrder)
    {
        Track = mercurySearchTrack;
    }
    public MercurySearchTrack Track { get; }
}
