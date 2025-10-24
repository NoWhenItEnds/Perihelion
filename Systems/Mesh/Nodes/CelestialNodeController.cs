using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Perihelion.Types;
using Perihelion.Types.Singletons;

namespace Perihelion.Mesh.Nodes
{
    /// <summary> The main controller of the celestial nodes within the game world. </summary>
    public partial class CelestialNodeController : SingletonNode3D<CelestialNodeController>
    {
        /// <summary> The celestial node prefab to use when spawning them. </summary>
        [ExportGroup("Prefabs")]
        [Export] private PackedScene _nodePrefab;


        /// <summary> A reference to the world's camera. </summary>
        [Export] private Camera3D _camera;


        /// <summary> The object pool representing the celestial objects within the game world. </summary>
        private ObjectPool<CelestialNode> _objectPool;

        /// <summary> An array of the celestial objects that currently exist within the game world. </summary>
        private HashSet<CelestialObject> _celestialObjects = new HashSet<CelestialObject>();


        private CelestialNode? _currentSelection = null;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _objectPool = new ObjectPool<CelestialNode>(this, _nodePrefab);

            // Populate the solar system.
            _celestialObjects.Add(CelestialObject.SOL);
            _celestialObjects.Add(CelestialObject.MERCURY);
            _celestialObjects.Add(CelestialObject.VENUS);
            _celestialObjects.Add(CelestialObject.EARTH);
            _celestialObjects.Add(CelestialObject.MARS);
            _celestialObjects.Add(CelestialObject.JUPITER);
            _celestialObjects.Add(CelestialObject.SATURN);
            _celestialObjects.Add(CelestialObject.URANUS);
            _celestialObjects.Add(CelestialObject.NEPTUNE);
            _celestialObjects.Add(CelestialObject.PLUTO);

            foreach (CelestialObject data in _celestialObjects)
            {
                CelestialNode current = _objectPool.GetAvailableObject();
                current.Initialise(_objectPool, data);
            }

            _currentSelection = GetCelestialNodeById("Sol");
        }


        /// <summary> Get a reference to the celestial object from its unique id. </summary>
        /// <param name="id"> The unique id of the celestial object to search for. </param>
        /// <returns> A reference to the data object, or a null if one couldn't be found. </returns>
        public CelestialObject? GetCelestialObjectById(String id) => _celestialObjects.FirstOrDefault(x => x.Id.ToUpper() == id.ToUpper()) ?? null;


        /// <summary> Get a reference to the celestial node from its unique id. </summary>
        /// <param name="id"> The unique id of the celestial object to search for. </param>
        /// <returns> A reference to the node in world space, or a null if one couldn't be found. </returns>
        public CelestialNode? GetCelestialNodeById(String id) => _objectPool.GetActiveObjects().FirstOrDefault(x => x.Data.Id.ToUpper() == id.ToUpper()) ?? null;


        public override void _Input(InputEvent @event)
        {
            if(@event is InputEventKey key && key.IsReleased() && key.Keycode == Key.Space)
            {
                CelestialNode[] allNodes = _objectPool.GetActiveObjects();
                Int32 index = Array.FindIndex(allNodes, x => x == _currentSelection) + 1;
                if (index >= allNodes.Length)
                {
                    index = 0;
                }
                _currentSelection = allNodes[index];

                GD.Print(_currentSelection.Data.Id);
                _camera.GlobalPosition = _currentSelection.GetCameraPosition();
            }
        }
    }
}
