using System.Collections.Generic;
using UnityEngine;

public class MeshScript : MonoBehaviour
{
    private Mesh _mesh;

    private readonly List<Transform> _nodes = new List<Transform>();
    private float _nodeRadius;

    private Vector3[] _vertices;
    private int[] _triangles;

    private void Awake()
    {
        transform.position = Vector3.zero;
    }

    private void Start()
    {
        for (var i = 1; i <= 9; i++)
        {
            _nodes.Add(GameObject.Find("Softbody/O" + i).transform);
        }

        _nodeRadius = _nodes[0].localScale.x / 2;

        _mesh = new Mesh();

        var mf = GetComponent<MeshFilter>();
        mf.mesh = _mesh;
    }

    private void LateUpdate()
    {
        var points = CircumPoints(_nodes, _nodeRadius, 12);
        points = ConvexHull(points);
        var triads = Triangulate(points);

        _mesh.Clear();
        _mesh.vertices = ToVector3S(points);
        _mesh.triangles = ToTriangles(triads);
        _mesh.RecalculateBounds();
    }

    private List<Vertex> CircumPoints(List<Transform> nodes, float radius, uint deg)
    {
        var points = new List<Vertex>();

        for (var i = 0; i < nodes.Count; i++)
        {
            var p = i == 0 ? nodes.Count - 1 : i - 1;
            var n = i == nodes.Count - 1 ? 0 : i + 1;

            var prev = nodes[p].position;
            var cur = nodes[i].position;
            var next = nodes[n].position;

            var from = next - cur;
            var to = prev - cur;

            var dot = Vector2.Dot(from, to); // dot product between [x1, y1] and [x2, y2]
            var det = from.x * to.y - from.y * to.x; // determinant
            var angle = Mathf.Atan2(det, dot); // atan2(y, x) or atan2(sin, cos)

            angle *= Mathf.Rad2Deg;

            if (angle < 0) angle += 360;

            var dir = from;

            for (uint d = 0; d <= angle; d += deg)
            {
                dir = Quaternion.AngleAxis(deg, Vector3.forward) * dir;
                points.Add(ToVertex(cur + dir.normalized * radius));
            }
        }

        return points;
    }

    private List<Vertex> ConvexHull(List<Vertex> vertices)
    {
        var triangulator = new Triangulator();
        return triangulator.ConvexHull(vertices, true);
    }

    private List<Triad> Triangulate(List<Vertex> vertices)
    {
        var triangulator = new Triangulator();
        var triads = triangulator.Triangulation(vertices, true);

        foreach (var triad in triads)
        {
            triad.MakeClockwise(vertices);
        }

        return triads;
    }

    private int[] ToTriangles(List<Triad> triads)
    {
        var triangles = new int[triads.Count * 3];
        var i = 0;

        foreach (var triad in triads)
        {
            triangles[i++] = triad.a;
            triangles[i++] = triad.b;
            triangles[i++] = triad.c;
        }

        return triangles;
    }

    private Vector3[] ToVector3S(List<Vertex> vertices)
    {
        var vectors = new Vector3[vertices.Count];

        for (var i = 0; i < vertices.Count; i++)
        {
            vectors[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        return vectors;
    }

    private void DrawTriangles(int[] triangles, Vector3[] vertices, Color color)
    {
        for (var i = 0; i < triangles.Length;)
        {
            var a = vertices[triangles[i++]];
            var b = vertices[triangles[i++]];
            var c = vertices[triangles[i++]];

            DrawTriangle(a, b, c, color);
        }
    }

    private void DrawTriangles(List<Triad> triads, List<Vertex> vertices, Color color)
    {
        foreach (var triad in triads)
        {
            var a = new Vector2(vertices[triad.a].x, vertices[triad.a].y);
            var b = new Vector2(vertices[triad.b].x, vertices[triad.b].y);
            var c = new Vector2(vertices[triad.c].x, vertices[triad.c].y);

            DrawTriangle(a, b, c, color);
        }
    }

    private void DrawTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
    {
        Debug.DrawLine(a, b, color);
        Debug.DrawLine(b, c, color);
        Debug.DrawLine(c, a, color);
    }

    private Vertex ToVertex(Vector3 vector3)
    {
        return new Vertex(vector3.x, vector3.y);
    }
}