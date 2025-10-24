using System;
using Godot;
using Perihelion.Managers;
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
            Single scale = (Single)(MathExtensions.KmToAu(Data.Radius) * 1000.0);
            _mesh.Scale = Vector3.One * scale;
            //_cameraPosition.Position = new Vector3(0f, 0f, scale);
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(Double delta)
        {
            if (Data != null)
            {
                Position = Data.CalculateCartesianPosition(_gameManager.CurrentTime) * 1000f;
            }
        }


        /// <summary> Get the position of the camera in world space. </summary>
        /// <returns> The global position of the celestial node's camera point. </returns>
        public Vector3 GetCameraPosition() => _cameraPosition.GlobalPosition;


        /// <inheritdoc/>
        public void FreeObject()
        {
            Data = null;
            _objectPool?.FreeObject(this);
        }
    }
}
