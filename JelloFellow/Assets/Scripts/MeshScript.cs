using System.Collections.Generic;
using DelaunayTriangulator;
using UnityEngine;

public class MeshScript : MonoBehaviour
{
    private GameObject[] _nodes;

    private Mesh _mesh;

    private readonly Triangulator _angulator = new Triangulator();
    private readonly List<Vertex> _points = new List<Vertex>();
    private List<Triad> _triads;

    private void Start()
    {
        _nodes = GameObject.FindGameObjectsWithTag("Node");

        foreach (var node in _nodes)
        {
            _points.Add(new Vertex(node.transform.position.x, node.transform.position.y));
        }


        _triads = _angulator.Triangulation(_points);


        var verts = new Vector3[_nodes.Length];

        for (var i = 0; i < verts.Length; i++)
        {
            verts[i] = transform.position;
        }

        var tris = new int[_triads.Count * 3];

        var n = 0;
        foreach (var triad in _triads)
        {
            tris[n++] = triad.c;
            tris[n++] = triad.b;
            tris[n++] = triad.a;
        }

        Debug.Log(_triads.Count);

//        tris[0] = 0;
//        tris[1] = 1;
//        tris[2] = 2;

        _mesh = new Mesh
        {
            vertices = verts,
            triangles = tris
        };


        var mf = GetComponent<MeshFilter>();
        mf.mesh = _mesh;
    }

    private void LateUpdate()
    {
        for (var i = 0; i < _nodes.Length; i++)
        {
            _points[i] = new Vertex(_nodes[i].transform.position.x, _nodes[i].transform.position.y);
        }


        _triads = _angulator.Triangulation(_points);

        foreach (var triad in _triads)
        {
            var a = new Vector2(_points[triad.a].x, _points[triad.a].y);
            var b = new Vector2(_points[triad.b].x, _points[triad.b].y);
            var c = new Vector2(_points[triad.c].x, _points[triad.c].y);
            Debug.DrawLine(a, b, Color.green);
            Debug.DrawLine(b, c, Color.green);
            Debug.DrawLine(c, a, Color.green);
        }

        var verts = new Vector3[_nodes.Length];

        for (var i = 0; i < verts.Length; i++)
        {
            verts[i] = _nodes[i].transform.position;
        }

        var tris = new int[_triads.Count * 3];

        var n = 0;
        foreach (var triad in _triads)
        {
            if (Orientation(_points[triad.a], _points[triad.b], _points[triad.c]) < 0)
            {
                tris[n++] = triad.c;
                tris[n++] = triad.b;
                tris[n++] = triad.a;
            }
            else
            {
                tris[n++] = triad.a;
                tris[n++] = triad.b;
                tris[n++] = triad.c;
            }
        }

        _mesh.vertices = verts;
        _mesh.triangles = tris;
        _mesh.RecalculateBounds();
    }


    int Orientation(Vertex p1, Vertex p2, Vertex p3)
    {
        // See 10th slides from following link for derivation
        // of the formula
        var val = (p2.y - p1.y) * (p3.x - p2.x) -
                  (p2.x - p1.x) * (p3.y - p2.y);

        if (val == 0) return 0; // colinear

        return val > 0 ? 1 : -1; // clock or counterclock wise
    }
}