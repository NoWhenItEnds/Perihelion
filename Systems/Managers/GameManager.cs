using Godot;
using System;
using Perihelion.Types.Singletons;

namespace Perihelion.Managers
{
    /// <summary> The main manager for the game scene. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        /// <summary> How quickly time is moving. </summary>
        /// <remarks> One in game day takes two hours. </remarks>
        [ExportGroup("Settings")]
        [Export] private Single _timescale = 12f;


        /// <summary> The world's current time. </summary>
        /// <remarks> The game starts on Sunday the 5th of February, 2012. </remarks>
        public DateTimeOffset CurrentTime { get; private set; } = new DateTimeOffset(2012, 2, 5, 0, 0, 0, TimeSpan.FromHours(8));

        /// <summary> The amount of time (in seconds) that has passed in game time since the previous physics frame. </summary>
        public Double GameTimeDelta { get; private set; }

        [Export] private PackedScene _helper;


        /// <inheritdoc/>
        public override void _Ready()
        {
            /*
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
            */
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(Double delta)
        {
            DateTimeOffset previousTime = CurrentTime;
            CurrentTime = CurrentTime.AddSeconds(delta * _timescale);
            GameTimeDelta = (CurrentTime.ToUnixTimeMilliseconds() - previousTime.ToUnixTimeMilliseconds()) * 0.001f;
        }
    }
}
