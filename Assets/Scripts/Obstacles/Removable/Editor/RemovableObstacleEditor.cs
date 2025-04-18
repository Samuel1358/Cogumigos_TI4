using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[CustomEditor(typeof(RemovableObstacle))]
public class RemovableObstacleEditor : Editor
{
    RemovableObstacle _removableObstacle;

    private void OnEnable()
    {
        _removableObstacle = target as RemovableObstacle;
    }

    private void OnSceneGUI()
    {
        if (Application.isPlaying)
            return;

        HandleSceneTools.PositionalHandle(_removableObstacle, _removableObstacle.transform, ref _removableObstacle.destiny);

        NavMeshHit hit;
        Vector3 pos = _removableObstacle.transform.TransformPoint(_removableObstacle.destiny);
        _removableObstacle.positionated = NavMesh.SamplePosition(pos, out hit, 1f, NavMesh.AllAreas);

        if (_removableObstacle.positionated)
        {
            if (pos.y > hit.position.y)
            {
                Handles.DrawWireDisc(pos, Vector3.up, 0.8f);
                Handles.DrawLine(pos, hit.position);
            }
            else
            {
                _removableObstacle.positionated = false;
            }                     
        }
    }
}
