using UnityEngine;
using UnityEditor;

public static class HandleSceneTools
{
    private static int selectedHandleIndex = -1;

    static Color oldHandlesColor = Handles.color;
    static Color transparentWhite = new Color(1f, 1f, 1f, 0.3f);
    static Color transparentRed = new Color(1f, 0f, 0f, 0.25f);
    static Color transparentYellow = new Color(1f, 0.92f, 0.016f, 0.4f);

    #region // PositionnalHandle

    public static void PositionalHandle(Object target, Transform transform, ref Vector3 position)
    {
        m_PositionalHandle(target, transform, ref position, Vector3.zero);
    }
    public static void PositionalHandle(Object target, Transform transform, ref Vector3 position, Vector3 handleOffset)
    {
        m_PositionalHandle(target, transform, ref position, handleOffset);
    }

    private static void m_PositionalHandle(Object target, Transform transform, ref Vector3 position, Vector3 handleOffset)
    {
        Handles.color = oldHandlesColor;

        Vector3 worldPos = transform.TransformPoint(position);

        float handleSize = HandleUtility.GetHandleSize(worldPos) * 0.1f;
        Handles.color = selectedHandleIndex == 0 ? Color.green : Color.gray;

        if (Handles.Button(worldPos, Quaternion.identity, handleSize, handleSize, Handles.SphereHandleCap))
        {
            selectedHandleIndex = (selectedHandleIndex == 0) ? -1 : 0;
            SceneView.RepaintAll();
        }

        if (selectedHandleIndex == 0)
        {
            m_DrawPositionalHandle(target, transform, ref position, handleOffset, worldPos);
        }

        Handles.color = oldHandlesColor;
    }

    private static void m_DrawPositionalHandle(Object target, Transform transform, ref Vector3 position, Vector3 handleOffset, Vector3 worldPos)
    {
        Handles.color = oldHandlesColor;

        EditorGUI.BeginChangeCheck();

        worldPos = Handles.PositionHandle(worldPos + handleOffset, Quaternion.identity) - handleOffset;


        Handles.color = transparentRed;
        Handles.DrawSolidDisc(worldPos, Vector3.up, 0.6f);

        Handles.color = transparentYellow;
        Handles.SphereHandleCap(0, worldPos, Quaternion.identity, 0.2f, EventType.Repaint);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Gizmo Point");

            position = transform.InverseTransformPoint(worldPos);
        }

        Handles.color = oldHandlesColor;
    }

    #endregion
}
