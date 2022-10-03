using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace IslandEscape.Map
{
    public class WaterManager : MonoBehaviour
    {
        public UnityEvent<TerrainEventArgs> WaterRose { get; } = new UnityEvent<TerrainEventArgs>();

        public const float RISE_INTERVAL = 10f;

        public float waterLevel = 0f;
        public float rateOfRise = 1f;
        public float rateMultiplier = 0.25f;
        public float lastRise;

        public int SizeOfNextRise { get { return (int)Math.Floor(rateOfRise); } }
        public int TimeToNextRise { get { return (int)Math.Abs(Math.Round(Time.timeSinceLevelLoad - lastRise - RISE_INTERVAL)); } }
        public bool WaterRising { get => levelsToRise > 0; }

        public Tile waterTile;
        public Tilemap terrain;

        private Tilemap tilemap;
        private List<Vector3Int> lastFlooded;

        private int levelsToRise = 0;

        public void Awake()
        {
            lastRise = Time.timeSinceLevelLoad;
            tilemap = GetComponent<Tilemap>();
        }

        public void Start()
        {
            // get a starting list of all island tiles that are adjacent to water
            // these will be the first tiles to flood, but we're just using it as a way of getting
            // the boundaries of the water against the island.
            List<Vector3Int> terrainEdge = new List<Vector3Int>();
            foreach (var position in terrain.cellBounds.allPositionsWithin)
            {
                if (TileAdjacentToWater(position) && !tilemap.HasTile(position))
                    terrainEdge.Add(position);
            }

            // Get that list of the water tiles adjacent to those island tiles
            // we'll use this to figure out which tiles to flood, and keep it up to date as a list
            // of the last tiles flooded.
            lastFlooded = new List<Vector3Int>();
            foreach (var position in terrainEdge)
            {
                // TODO: don't add dupes
                lastFlooded.AddRange(GetAdjacentWater(position));
            }
        }

        public void Update()
        {
            // TODO: what if it takes more than 10 seconds to rise all levels? Doubt that would ever happen, but who knows
            if (!WaterRising && Time.timeSinceLevelLoad - lastRise >= RISE_INTERVAL)
            {
                levelsToRise = SizeOfNextRise;
                rateOfRise += rateOfRise * rateMultiplier;
            }

            // only rise one level at a time
            if (WaterRising)
            {
                RaiseWaterLevel();
            }
        }

        public void RaiseWaterLevel()
        {
            // TODO: add some sort of perceptible buildup/delay?
            Debug.Log("raising water level");

            // keep track of flooded tiles to reset lastFlooded
            List<Vector3Int> flood = new List<Vector3Int>();

            // loop over each of the last flooded tiles, find adjacent empty tiles, add water.
            foreach (var position in lastFlooded)
            {
                foreach (var empty in GetAdjacentEmpty(position))
                {
                    tilemap.SetTile(empty, waterTile);
                    flood.Add(empty);
                }
            }

            // TODO: deal with player being in a flooding tile. (oo maybe could use OnTriggerEnter2D)

            lastFlooded = flood;
            // TODO: do we really need a list of the tiles that rose in the event?
            // although I think I did the TerrainEventArgs like that so it could be useable for other things (like,
            // I dunno, if for some reason we add fire mechanics and want to catch terrain tiles on fire? lol)
            WaterRose?.Invoke(new TerrainEventArgs(this, flood));

            waterLevel++;
            levelsToRise--;
            if (levelsToRise == 0)
            {
                lastRise = Time.timeSinceLevelLoad;
            }
        }

        private bool TileAdjacentToWater(Vector3Int position)
        {
            bool adjacent = false;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // don't care about current position
                    if (i == 0 && j == 0)
                        continue;

                    if (tilemap.HasTile(new Vector3Int(position.x + i, position.y + j, position.z)))
                    {
                        adjacent = true;
                    }
                }
            }
            return adjacent;
        }

        /// <summary>
        /// Helper function that gets tiles adjacent to the given position that match the supplied condition.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private List<Vector3Int> GetAdjacent(Vector3Int position, Func<Vector3Int, bool> condition)
        {
            List<Vector3Int> selected = new List<Vector3Int>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // don't care about current position
                    if (i == 0 && j == 0)
                        continue;

                    var tmpPos = new Vector3Int(position.x + i, position.y + j, position.z);
                    if (condition(tmpPos))
                        selected.Add(tmpPos);
                }
            }
            return selected;
        }

        /// <summary>
        /// Get adjacent tiles with water in them.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private List<Vector3Int> GetAdjacentWater(Vector3Int position)
        {
            return GetAdjacent(position, (pos) => { return tilemap.HasTile(pos); });
        }

        /// <summary>
        /// Get adjacent empty tiles.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private List<Vector3Int> GetAdjacentEmpty(Vector3Int position)
        {
            return GetAdjacent(position, (pos) => { return !tilemap.HasTile(pos); });
        }
    }
}

