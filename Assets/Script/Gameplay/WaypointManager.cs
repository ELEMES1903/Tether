using UnityEngine;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public float cellSize = 1f;
    public LayerMask obstacleLayer;

    public List<Transform> waypoints = new List<Transform>();

    void Start()
    {
        GenerateWaypoints();
    }

    void GenerateWaypoints()
    {
        waypoints.Clear();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 cellCenter = GetCellCenter(x, y);

                // Check if there is an obstacle at the cell center
                if (!IsObstacle(cellCenter))
                {
                    // Instantiate a waypoint at the cell center
                    Transform waypoint = new GameObject("Waypoint").transform;
                    waypoint.position = cellCenter;
                    waypoints.Add(waypoint);
                }
            }
        }
    }

    Vector3 GetCellCenter(int x, int y)
    {
        float centerX = transform.position.x + x * cellSize + cellSize / 2f;
        float centerY = transform.position.y + y * cellSize + cellSize / 2f;
        return new Vector3(centerX, centerY, transform.position.z);
    }

    bool IsObstacle(Vector3 position)
    {
        // Use raycasting to check for obstacles at the specified position
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 0f, obstacleLayer);
        return hit.collider != null;
    }
}
//use the unity ai tool to create behaviors for the enemy prefab. for now, try to make a behavior of the enemy prefab finding a way to get to the player as close as possible in the confines of tilemap grid cells.