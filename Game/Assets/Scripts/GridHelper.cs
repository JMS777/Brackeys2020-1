using UnityEngine;
using UnityEngine.AI;

public static class GridHelper
{
    public static Vector3 GetNearestTile(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x / 5) * 5, 0.1f, Mathf.Round(position.z / 5) * 5);
    }

    public static int GetActionPointsRequired(NavMeshPath path)
    {
        Vector3[] points = path.corners;
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; ++i)
        {
            distance += Vector3.Distance(points[i], points[i + 1]);
        }

        return (int)((0.95f * distance) / 5) + 1;
    }
}