using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Godot;
using Perihelion.Managers;
using Perihelion.Mesh.Resources;
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


        /// <summary> A reference to the resource manager singleton. </summary>
        private ResManager _resourceManager;

        /// <summary> The object pool representing the celestial objects within the game world. </summary>
        private ObjectPool<CelestialNode> _objectPool;

        /// <summary> An array of the celestial objects that currently exist within the game world. </summary>
        private HashSet<CelestialObject> _celestialObjects = new HashSet<CelestialObject>();


        private CelestialNode? _currentSelection = null;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _resourceManager = ResManager.Instance;
            _objectPool = new ObjectPool<CelestialNode>(this, _nodePrefab);

            // Populate the solar system.
            _celestialObjects.Add(_resourceManager.GetCelestialObject("sol"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("mercury"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("venus"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("earth"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("mars"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("jupiter"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("saturn"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("uranus"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("neptune"));
            _celestialObjects.Add(_resourceManager.GetCelestialObject("pluto"));

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
        public CelestialObject? GetCelestialObjectById(String id) => _celestialObjects.FirstOrDefault(x => x.Id.ToLower() == id.ToLower()) ?? null;


        /// <summary> Get a reference to the celestial node from its unique id. </summary>
        /// <param name="id"> The unique id of the celestial object to search for. </param>
        /// <returns> A reference to the node in world space, or a null if one couldn't be found. </returns>
        public CelestialNode? GetCelestialNodeById(String id) => _objectPool.GetActiveObjects().FirstOrDefault(x => x.Data?.Id.ToLower() == id.ToLower()) ?? null;


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
