using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Eum.UI.Items;
using Eum.UI.Services.Tracks;
using Eum.UI.ViewModels.Artists;
using Eum.UI.ViewModels.Search.SearchItems;
using Eum.Users;

namespace Eum.UI.ViewModels.Playlists
{
    [INotifyPropertyChanged]
    public partial  class PlaylistTrackViewModel : IIsPlaying, IIsSaved
    {
        [ObservableProperty] private int _index;
        [ObservableProperty] private bool _isSaved;
        public PlaylistTrackViewModel(EumTrack eumTrack, int index, ICommand playCommand)
        {
            Index = index;
            Track = eumTrack;
            PlayCommand = playCommand;
        }

        public PlaylistTrackViewModel(SpotifyTrackSearchItem searchTrackItem, int index)
        {
            Index = index;

            Track = new EumTrack(searchTrackItem);

        }

        public ICommand PlayCommand { get; }

        public EumTrack Track { get; }
        public ItemId Id => Track.Id;

        public bool IsPlaying()
        {
            return Ioc.Default.GetRequiredService<MainViewModel>()
                .PlaybackViewModel?.Item?.Id == Track.Id; 
        }

        public bool WasPlaying => _wasPlaying;

        private bool _wasPlaying;

        public event EventHandler<bool>? IsPlayingChanged;
        public void ChangeIsPlaying(bool isPlaying)
        {
            if(_wasPlaying == isPlaying) return;
            
            _wasPlaying = isPlaying;
            IsPlayingChanged?.Invoke(this, isPlaying);
        }
    }

    public interface IIsPlaying
    {
        ItemId Id { get; }
        bool IsPlaying();
        bool WasPlaying { get; }
        event EventHandler<bool> IsPlayingChanged;

        void ChangeIsPlaying(bool isPlaying);
        // void RegisterEvents();
        // void UnregisterEvents();
    }
}
