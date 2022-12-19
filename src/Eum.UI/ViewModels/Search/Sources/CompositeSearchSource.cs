using System.Reactive.Linq;
using DynamicData;
using Eum.UI.ViewModels.Search.Patterns;
using Eum.UI.ViewModels.Search.SearchItems;
using MoreLinq;

namespace Eum.UI.ViewModels.Search.Sources;

public class CompositeSearchSource : ISearchSource
{
	private readonly ISearchSource[] _sources;

	public CompositeSearchSource(params ISearchSource[] sources)
	{
		_sources = sources;
	}

	public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes => _sources.Select(r => r.Changes).Merge();
    public IObservable<IChangeSet<SearchGroup>> GroupChanges => _sources.Select(a => a.GroupChanges).Merge();
}
