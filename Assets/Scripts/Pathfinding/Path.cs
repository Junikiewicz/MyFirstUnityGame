using UnityEngine;
namespace MyRPGGame.PathFinding
{
    public class Path
    {

        public readonly Vector3[] lookPoints;
        public Path(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
        {
            lookPoints = waypoints;
        }

        Vector2 V3ToV2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public void DrawWithGizmos()
        {

            Gizmos.color = Color.black;
            foreach (Vector3 p in lookPoints)
            {
                Gizmos.DrawCube(p + Vector3.up, Vector3.one);
            }

            Gizmos.color = Color.white;

        }

    }
}