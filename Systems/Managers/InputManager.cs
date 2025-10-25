using System;
using Godot;
using Perihelion.Mesh.Nodes;
using Perihelion.Types.Singletons;

namespace Perihelion.Managers
{
    /// <summary> The game's central manager of player input during gameplay. </summary>
    public partial class InputManager : SingletonNode<InputManager>
    {
        /// <summary> A reference to the celestial controller singleton. </summary>
        private CelestialNodeController _celestialController;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _celestialController = CelestialNodeController.Instance;
        }


        /// <inheritdoc/>
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey key && key.IsReleased())
            {
                if (key.Keycode == Key.Right)
                {
                    _celestialController.SetCurrentFocus(1);
                }
                else if (key.Keycode == Key.Left)
                {
                    _celestialController.SetCurrentFocus(-1);
                }
                else if (key.Keycode == Key.Down)
                {
                    _celestialController.CurrentSelection.AlterCameraZoom(0.1f);
                }
                else if (key.Keycode == Key.Up)
                {
                    _celestialController.CurrentSelection.AlterCameraZoom(-0.1f);
                }
            }
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(Double delta)
        {
            base._PhysicsProcess(delta);
        }
    }
}
