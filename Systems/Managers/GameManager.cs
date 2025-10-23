using Godot;
using System;
using Perihelion.Types.Singletons;
using Perihelion.Types.Debug;
using Perihelion.Types.Extensions;

namespace Perihelion.Managers
{
    /// <summary> The main manager for the game scene. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        [Export] private PackedScene _helper;


        /// <inheritdoc/>
        public override void _Ready()
        {
            Random random = new Random();

            // Generate coordinates.
            (Single latitude, Single longitude)[] coordinates = new (Single latitude, Single longitude)[1000];
            for (Int32 i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = new(random.NextSingle() * 180f - 90f, random.NextSingle() * 360f - 180f);
                TransformHelper instance = _helper.InstantiateOrNull<TransformHelper>();
                AddChild(instance);
                instance.GlobalPosition = MathExtensions.GeographicalToCartesian(coordinates[i].latitude, coordinates[i].longitude, 10);
                instance.SetCustomText($"LAT: {coordinates[i].latitude:F2}, LON: {coordinates[i].longitude:F2}");
            }
        }
    }
}
