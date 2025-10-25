using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Perihelion.Managers;
using Perihelion.Mesh.Resources;
using Perihelion.Types;
using Perihelion.Types.Extensions;
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


        /// <summary> The node that the player currently has selected. </summary>
        public CelestialNode CurrentSelection { get; private set; }


        /// <summary> A reference to the resource manager singleton. </summary>
        private ResManager _resourceManager;

        /// <summary> The object pool representing the celestial objects within the game world. </summary>
        private ObjectPool<CelestialNode> _objectPool;

        /// <summary> An array of the celestial objects that currently exist within the game world. </summary>
        private HashSet<CelestialObject> _celestialObjects = new HashSet<CelestialObject>();


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

            CurrentSelection = GetCelestialNodeById("sol");
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(double delta)
        {
            _camera.GlobalPosition = CurrentSelection.GetCameraPosition();
            _camera.GlobalRotation = CurrentSelection.GetCameraRotation();
        }


        /// <summary> Sets the node the controller is currently focused upon. This is the one the player is observing. </summary>
        /// <param name="relativeIndex"> The index relative to the current node to change focus to. </param>
        public void SetCurrentFocus(Int32 relativeIndex)
        {
            CelestialNode[] allNodes = _objectPool.GetActiveObjects();
            Int32 index = (Int32)MathExtensions.WrapValue(Array.FindIndex(allNodes, x => x == CurrentSelection) + relativeIndex, allNodes.Length);
            SetCurrentFocus(allNodes[index]);
        }


        /// <summary> Sets the node the controller is currently focused upon. This is the one the player is observing. </summary>
        /// <param name="node"> The node to change focus to. </param>
        public void SetCurrentFocus(CelestialNode node)
        {
            CurrentSelection = node;
        }


        /// <summary> Get a reference to the celestial object from its unique id. </summary>
        /// <param name="id"> The unique id of the celestial object to search for. </param>
        /// <returns> A reference to the data object, or a null if one couldn't be found. </returns>
        public CelestialObject? GetCelestialObjectById(String id) => _celestialObjects.FirstOrDefault(x => x.Id.ToLower() == id.ToLower()) ?? null;


        /// <summary> Get a reference to the celestial node from its unique id. </summary>
        /// <param name="id"> The unique id of the celestial object to search for. </param>
        /// <returns> A reference to the node in world space, or a null if one couldn't be found. </returns>
        public CelestialNode? GetCelestialNodeById(String id) => _objectPool.GetActiveObjects().FirstOrDefault(x => x.Data?.Id.ToLower() == id.ToLower()) ?? null;
    }
}
