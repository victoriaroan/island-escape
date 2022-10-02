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

        public Tile waterTile;
        public Tilemap terrain;

        private Tilemap tilemap;

        void Awake()
        {
            lastRise = Time.timeSinceLevelLoad;
            tilemap = GetComponent<Tilemap>();
        }

        void Update()
        {
            if (Time.timeSinceLevelLoad - lastRise >= RISE_INTERVAL)
            {
                RaiseWaterLevel();
            }
        }

        public void RaiseWaterLevel()
        {
            // TODO: add some sort of perceptible buildup/delay (while freezing TimeToNextRise count)?
            Debug.Log("raising water level");

            List<Vector3Int> flood = new List<Vector3Int>();

            // this is probably gonna start getting pretty laggy the more loops we have to make (already pretty laggy)
            // TODO: figure out how to do the calculations in the background before it's time to raise the water level.

            foreach (int i in Enumerable.Range(1, SizeOfNextRise))
            {
                // TODO: use last set of flooding tiles as starting position so we don't have to loop the entire island every time.
                foreach (var position in terrain.cellBounds.allPositionsWithin)
                {
                    // Debug.Log(position);
                    // Debug.Log(tilemap.HasTile(position));
                    if (TileAdjacentToWater(position) && !tilemap.HasTile(position))
                    {
                        flood.Add(position);
                    }
                }

                foreach (var position in flood)
                {
                    tilemap.SetTile(position, waterTile);
                }
            }

            // TODO: deal with player being in a flooding tile. (oo maybe could use OnTriggerEnter2D)

            lastRise = Time.timeSinceLevelLoad;
            WaterRose?.Invoke(new TerrainEventArgs(this, flood));

            waterLevel += SizeOfNextRise;
            rateOfRise += rateOfRise * rateMultiplier;
        }

        public bool TileAdjacentToWater(Vector3Int position)
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
    }
}

