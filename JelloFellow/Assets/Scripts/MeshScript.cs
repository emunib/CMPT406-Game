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

        tri[0] = 1;
        tri[1] = 0;
        tri[2] = 2;

        tri[3] = 2;
        tri[4] = 0;
        tri[5] = 3;

        tri[6] = 3;
        tri[7] = 0;
        tri[8] = 4;

        tri[9] = 4;
        tri[10] = 0;
        tri[11] = 5;

        tri[12] = 5;
        tri[13] = 0;
        tri[14] = 6;

        tri[15] = 6;
        tri[16] = 0;
        tri[17] = 7;

        tri[18] = 7;
        tri[19] = 0;
        tri[20] = 8;

        tri[21] = 8;
        tri[22] = 0;
        tri[23] = 9;

        tri[24] = 9;
        tri[25] = 0;
        tri[26] = 10;

        tri[27] = 10;
        tri[28] = 0;
        tri[29] = 11;

        tri[30] = 11;
        tri[31] = 0;
        tri[32] = 12;

        tri[33] = 12;
        tri[34] = 0;
        tri[35] = 1;

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