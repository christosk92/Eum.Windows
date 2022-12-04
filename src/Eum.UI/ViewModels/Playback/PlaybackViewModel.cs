﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Eum.Artwork;
using Eum.Connections.Spotify;
using Eum.UI.Items;
using ReactiveUI;

namespace Eum.UI.ViewModels.Playback
{
    [INotifyPropertyChanged]
    public abstract partial class PlaybackViewModel
    {
        [ObservableProperty]
        private CurrentlyPlayingHolder _item;

        [ObservableProperty] private double _timestamp;

        private IDisposable _disposable;
        protected PlaybackViewModel()
        {
        }
        public abstract ServiceType Service { get; }

        public virtual void Deconstruct()
        {
            StopTimer();
        }

        protected void StopTimer()
        {
            _disposable?.Dispose();
        }

        protected void StartTimer(long atPosition)
        {
            StopTimer();
            Timestamp = atPosition;

            _disposable =  Observable.Interval(TimeSpan.FromMilliseconds(200), RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(l =>
                {
                    Timestamp += 200;
                });
        }
    }


    public record CurrentlyPlayingHolder
    {
        public string BigImage { get; init; }
        public string SmallImage { get; init; }
        public ItemId Context { get; init; }
        public IdWithTitle Title { get; init; }
        public IdWithTitle[] Artists { get; init; }
        public double Duration { get; init; }
    }

    public class IdWithTitle
    {
        public ItemId Id { get; init; }
        public string Title { get; init; }
    }
}