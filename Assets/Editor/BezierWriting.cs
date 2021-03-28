using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BezierExample))]
public class BezierWriting : Editor
{

    BezierExample be0;
    private Vector3[] points;

    public void OnSceneGUI()
    {
        BezierExample be = target as BezierExample;

        //be.startPoint = Handles.PositionHandle(be.startPoint, Quaternion.identity);
        //be.endPoint = Handles.PositionHandle(be.endPoint, Quaternion.identity);
        //be.startTangent = Handles.PositionHandle(be.startTangent, Quaternion.identity);
        //be.endTangent = Handles.PositionHandle(be.endTangent, Quaternion.identity);

        if (be.points == null) return;

        int numPoints = be.points.Count;

        for (int i = 0; i < numPoints; i++)
        {

            Vector3 start = be.points[i];
            Vector3 end = be.points[(i + 1) % numPoints];
            Vector3 startTang = be.points[i] + (be.points[(i + 1) % numPoints] - be.points[(i -1 + numPoints) % numPoints]) * be.cornerVelocity;
            Vector3 endTang = be.points[(i + 1) % numPoints] + (be.points[i] - be.points[(i + 2) % numPoints]) * be.cornerVelocity;

            Handles.DrawBezier(start, end, startTang, endTang, Color.red, null, 2f);
        }
    }

}

