using System.Xml;
using UnityEngine;

public class MeshScript : MonoBehaviour
{
    public Transform Center;
    public Transform One;
    public Transform Two;
    public Transform Three;
    public Transform Four;
    public Transform Five;
    public Transform Six;

    private Transform[] _nodes;
    private int _size;
    private Vector3[] _vertices;
    private Mesh _mesh;

    private void Start()
    {
        _nodes = new[] {Center, One, Two, Three, Four, Five, Six};
        _size = _nodes.Length * 2 - 1;

        var mf = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        mf.mesh = _mesh;

        _vertices = new Vector3[_size];

        _mesh.vertices = _vertices;

        var tri = new int[(_size - 1) * 3];

        var n = 1;
        for (var i = 0; i < tri.Length; i++)
        {
            if (n >= _size) n = 1;

            if (i % 3 == 0) tri[i] = n++;
            else if (i % 3 == 1)
                tri[i] = 0;
            else
                tri[i] = n;
        }

        _mesh.triangles = tri;
    }

    private void Update()
    {
        _vertices[0] = Center.position;
        for (var i = 1; i < _nodes.Length; i++) // length must be odd ( even + 1 for center)
        {
            var x = i - 1 <= 0 ? _nodes.Length - 1 : i - 1;
            var y = i + 1 >= _nodes.Length ? 1 : i + 1;

            var prev = _nodes[x].position;
            var cur = _nodes[i].position;
            var next = _nodes[y].position;

            var p = prev - cur;
            p = new Vector2(-p.y, p.x).normalized / 2;

            var n = cur - next;
            n = new Vector2(-n.y, n.x).normalized / 2;

            _vertices[i * 2 - 1] = _nodes[i].position + p;
            _vertices[i * 2] = _nodes[i].position + n;
        }

        _mesh.vertices = _vertices;
    }
}