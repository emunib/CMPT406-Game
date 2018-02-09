using System.Collections.Generic;
using UnityEngine;

public class MeshScript : MonoBehaviour
{
    private Mesh _mesh;

    private GameObject[] _nodes;
    private Vector3[] _vertices;
    private int[] _triangles;

    private void Awake()
    {
        transform.position = Vector3.zero;
    }
    
    private void Start()
    {
        _nodes = GameObject.FindGameObjectsWithTag("Node");

        _mesh = new Mesh();

        var mf = GetComponent<MeshFilter>();
        mf.mesh = _mesh;
    }

    private void LateUpdate()
    {
        var points = CircumPoints(_nodes, 0.75f, 12);
        points = ConvexHull(points);
        var triads = Triangulate(points);

        _mesh.Clear();
        _mesh.vertices = ToVector3S(points);
        _mesh.triangles = ToTriangles(triads);
        _mesh.RecalculateBounds();
    }

    private static Vertex PointOnCircle(float radius, float angleInDegrees, Vertex origin)
    {
        // Convert from degrees to radians via multiplication by PI/180        
        var x = radius * Mathf.Cos(angleInDegrees * Mathf.Deg2Rad) + origin.x;
        var y = radius * Mathf.Sin(angleInDegrees * Mathf.Deg2Rad) + origin.y;

        return new Vertex(x, y);
    }

    private List<Vertex> CircumPoints(GameObject[] nodes, float radius, uint deg)
    {
        var points = new List<Vertex>();

        foreach (var node in nodes)
        {
            var vert = ToVertex(node.transform);

            for (uint i = 0; i < 360; i += deg)
            {
                points.Add(PointOnCircle(radius, i, vert));
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
    
    private Vertex ToVertex(Transform trans)
    {
        return new Vertex(trans.position.x, trans.position.y);
    }
}