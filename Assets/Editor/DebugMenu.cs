
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public static class DebugMenu
{
    [MenuItem("Debug/Print global position")]
    public static void PrintGlobalPosition()
    {
        if(Selection.activeGameObject != null)
        {
            Debug.Log(
                Selection.activeGameObject.name
                + " is at world_pos["
                + Selection.activeGameObject.transform.position
                + "] world_rot["
                + Selection.activeGameObject.transform.rotation.eulerAngles
                + "] world_scale["
                + Selection.activeGameObject.transform.lossyScale
                + "]"
            );
        }
    }
}

#endif