using UnityEngine;

public class JellyEffect : MonoBehaviour
{
    private JellySprite _jelly;
    public float MaxStretchDist = 5f;


    void Start()
    {
        _jelly = GetComponent<JellySprite>();
    }


    private void Update()
    {
        FixPoints();
        
        //Debug.DrawLine(_jelly.ReferencePoints[1].transform.position, _jelly.ReferencePoints[1].transform.position + Vector3.one.normalized * MaxStretchDist);
    }

    private void FixPoints()
    {
        for (var i = 1; i < _jelly.ReferencePoints.Count; i++)
        {
            var n = i + 1 == _jelly.ReferencePoints.Count ? 1 : i + 1;

            var cur = _jelly.ReferencePoints[i];
            var next = _jelly.ReferencePoints[n];

            var dist = Vector2.Distance(cur.transform.position, next.transform.position);

            if (dist > MaxStretchDist)
            {
                _jelly.ResetPoint(cur);
            }
        }

        for (var i = _jelly.ReferencePoints.Count - 1; i > 0; i--)
        {
            var n = i - 1 == 0 ? _jelly.ReferencePoints.Count - 1 : i - 1;

            var cur = _jelly.ReferencePoints[i];
            var next = _jelly.ReferencePoints[n];

            var dist = Vector2.Distance(cur.transform.position, next.transform.position);

            if (dist > MaxStretchDist)
            {
                _jelly.ResetPoint(cur);
            }
        }
    }
}