using UnityEngine;
using System.Collections;
using System;

namespace MyRPGGame.PathFinding
{
    public class Unit : MonoBehaviour
    {
        
        const float minPathUpdateTime = .2f;
        const float pathUpdateMoveThreshold = .5f;
        const float turnDst = 5;
        const float stoppingDst = 10;
        Path path;
        public Vector3 currentDirection;
        public bool active=false;
        public bool pathPossible = false;
        Func<Vector3> obtainTargetPosition;
        Func<Vector3> obtainThisPosition;
        private const bool showPath = false;
        public void Inicialize( Func<Vector3> _obtainTargetPosition, Func<Vector3> _obtainThisPosition)
        {
            obtainTargetPosition = _obtainTargetPosition;
            StartCoroutine(UpdatePath());
            obtainThisPosition = _obtainThisPosition;
        }

        void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = new Path(waypoints, obtainThisPosition(), turnDst, stoppingDst);
                pathPossible = true;
                StopCoroutine(nameof(FollowPath));
                StartCoroutine(nameof(FollowPath));
            }
            else
            {
                pathPossible = false;
                currentDirection = Vector3.zero;
            }
        }
        IEnumerator UpdatePath()
        {
            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = obtainTargetPosition();
            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                if(active)
                {
                    if ((obtainTargetPosition() - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                    {
                        PathRequestManager.RequestPath(new PathRequest(obtainThisPosition(), obtainTargetPosition(), OnPathFound));
                        targetPosOld = obtainTargetPosition();
                    }
                }
            }
        }
        IEnumerator FollowPath()
        {
            int pathIndex = 0;
            while (true)
            {
                if(active)
                {
                    Vector2 pos2D = new Vector2(obtainThisPosition().x, obtainThisPosition().y);
                    if(Vector3.Distance(path.lookPoints[pathIndex], obtainThisPosition())<0.5f)
                    {
                        break;
                    }
                    currentDirection = (path.lookPoints[pathIndex] - obtainThisPosition()).normalized;
                }
                yield return null;
            }
        }
        public void OnDrawGizmos()
        {
            if (path != null&&showPath)
            {
                path.DrawWithGizmos();
            }
        }
    }
}