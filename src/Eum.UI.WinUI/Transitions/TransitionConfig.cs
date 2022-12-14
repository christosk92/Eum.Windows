// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml.Media.Animation;

namespace Eum.UI.WinUI.Transitions
{
    /// <summary>
    /// Configuration used for the transition between UI elements.
    /// </summary>
    public class TransitionConfig
    {
        /// <summary>
        /// Gets or sets an id to indicate the target UI elements.
        /// </summary>
        public string Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the scale strategy of the transition.
        /// The default value is <see cref="ScaleMode.None"/>.
        /// </summary>
        public ScaleMode ScaleMode { get; set; } = ScaleMode.None;

        /// <summary>
        /// Gets or sets the center point used to calculate the element's translation or scale when animating.
        /// Value is normalized with respect to the size of the animated element.
        /// For example, a value of (0.0, 0.5) means that this point is at the leftmost point of the element horizontally and the center of the element vertically.
        /// The default value is (0, 0).
        /// </summary>
        public Point NormalizedCenterPoint { get; set; } = default;

        /// <summary>
        /// Gets or sets the transition strategy.
        /// The default value is <see cref="TransitionMode.Normal"/>.
        /// </summary>
        public TransitionMode TransitionMode { get; set; } = TransitionMode.Normal;

        /// <summary>
        /// Gets or sets the easing function type for the transition.
        /// The default value is <see cref="EasingType.Default"/>.
        /// </summary>
        public EasingType EasingType { get; set; } = EasingType.Default;

        /// <summary>
        /// Gets or sets the easing function mode for the transition.
        /// The default value is <see cref="EasingMode.EaseInOut"/>.
        /// </summary>
        public EasingMode EasingMode { get; set; } = EasingMode.EaseInOut;
    }
}
