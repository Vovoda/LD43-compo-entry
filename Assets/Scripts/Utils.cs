using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

	public static Quaternion RandomRotation2D()
    {
        return Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f));
    }

    public static Quaternion RandomRotation2D(Transform parent)
    {
        return Quaternion.Euler(0.0f, 0.0f, Random.Range(parent.rotation.eulerAngles.z + 90.0f, parent.rotation.eulerAngles.z - 90.0f));
    }



    public static void SetLine(GameObject myLine, Vector2 start, Vector2 end, float duration = 0.2f)
    {
        Line line = myLine.GetComponent<Line>();
        LineRenderer lr = line.lineRenderer;
        EdgeCollider2D edgeCol = line.edgeCol;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.2f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
        edgeCol.points = new[] {start, end}; 
    }

    public static GameObject InstantiateChild(GameObject prefab, Vector3 relativePos, Quaternion relativeRot, Transform _parent)
    {
        GameObject child = GameObject.Instantiate(prefab);
        child.transform.parent = _parent;
        child.transform.localPosition = relativePos;
        child.transform.localRotation = relativeRot;
        child.transform.localScale = Vector3.one;
        return child;
    }
    
}
