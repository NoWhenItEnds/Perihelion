using System;
using Godot;
using Perihelion.Managers;
using Perihelion.Mesh.Resources;
using Perihelion.Types;
using Perihelion.Types.Extensions;

namespace Perihelion.Mesh.Nodes
{
    /// <summary> A representation of a celestial object in the game world. </summary>
    public partial class CelestialNode : Node3D, IPoolable
    {
        /// <summary> A reference to the mesh this object represents. </summary>
        [ExportGroup("Nodes")]
        [Export] private MeshInstance3D _mesh;

        /// <summary> The node around which the camera rotates. </summary>
        [Export] private Node3D _cameraPivot;

        /// <summary> The node working as a placeholder for the camera. </summary>
        [Export] private Marker3D _cameraPosition;


        /// <summary> The data component this world node represents. </summary>
        public CelestialObject? Data { get; private set; } = null;


        /// <summary> A reference to the game manager. </summary>
        private GameManager _gameManager;

        /// <summary> A reference to the object pool that spawned this node. </summary>
        private ObjectPool<CelestialNode>? _objectPool = null;

        /// <summary> The object's radius / size in au. </summary>
        private Single _size;

        /// <summary> The minimum distance from the node the camera can be. </summary>
        private Single _minZoom => _size + 0.001f;

        /// <summary> The maximum distance from the node the camera can be. </summary>
        private Single _maxZoom => _size * 1.5f;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gameManager = GameManager.Instance;
        }


        /// <summary> Initialise the object. </summary>
        /// <param name="objectPool"> A reference to the pool that spawned this object. </param>
        /// <param name="data"> The data component attached to this node. </param>
        public void Initialise(ObjectPool<CelestialNode> objectPool, CelestialObject data)
        {
            _objectPool = objectPool;
            Data = data;
            _size = (Single)(MathExtensions.KmToAu(Data.Radius) * 10000.0);
            _mesh.Scale = Vector3.One * _size;
            _mesh.MaterialOverride = data.MeshMaterial;
            _cameraPosition.Position = new Vector3(0f, 0f, _size * 1.5f);
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(Double delta)
        {
            if (Data != null)
            {
                Position = Data.CalculateCartesianPosition(_gameManager.CurrentTime) * 100f;
                _mesh.RotationDegrees = Data.CalculateRotation(_gameManager.CurrentTime);
            }
        }


        /// <summary> Rotate the camera pivot by the given offset in degrees. </summary>
        /// <param name="rotation"> The rotation as euler degrees. </param>
        public void RotateCameraPosition(Vector3 rotation) => _cameraPivot.RotationDegrees += rotation;


        /// <summary> Alter how close the camera is to the node relative to its current position. </summary>
        /// <param name="amount"> The amount to alter the position by as a percentage. </param>
        public void AlterCameraZoom(Single amount)
        {
            Single actualAmount = (_maxZoom - _minZoom) * amount;
            Single trueAmount = Math.Clamp(_cameraPosition.Position.Z + actualAmount, _minZoom, _maxZoom);
            _cameraPosition.Position = new Vector3(0f, 0f, trueAmount);
        }


        /// <summary> Get the position of the camera in world space. </summary>
        /// <returns> The global position of the celestial node's camera point. </returns>
        public Vector3 GetCameraPosition() => _cameraPosition.GlobalPosition;


        /// <summary> Get the rotation of the camera in world space. </summary>
        /// <returns> The global rotation of the celestial node's camera point. </returns>
        public Vector3 GetCameraRotation() => _cameraPosition.GlobalRotation;


        /// <inheritdoc/>
        public void FreeObject()
        {
            Data = null;
            _mesh.MaterialOverride = null;
            _objectPool?.FreeObject(this);
        }
    }
}
