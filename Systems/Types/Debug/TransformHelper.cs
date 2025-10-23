using Godot;
using System;

namespace Perihelion.Types.Debug
{
    /// <summary> A helper to visualise a position and rotation in 3D space. </summary>
    public partial class TransformHelper : Node3D
    {
        /// <summary> A custom string to display as part of the helper's label. </summary>
        private String _customText = String.Empty;

        /// <summary> The format to use for displaying the text. </summary>
        private const String TEXT_FORMAT = "POS: <{0:F2}, {1:F2}, {2:F2}>\nROT: <{3:F2}, {4:F2}, {5:F2}>\n{6}";


        /// <summary> Add a piece of custom text to the helper for its display. </summary>
        /// <param name="text"> The line of text to display on the helper. </param>
        public void SetCustomText(String text)
        {
            _customText = text;
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(Double delta)
        {
            DebugDraw3D.DrawText(GlobalPosition + Vector3.One,
                String.Format(TEXT_FORMAT, GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z, GlobalRotationDegrees.X, GlobalRotationDegrees.Y, GlobalRotationDegrees.Z, _customText));
        }
    }
}
