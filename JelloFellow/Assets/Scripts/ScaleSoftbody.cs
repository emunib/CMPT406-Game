using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public class ScaleSoftbody : MonoBehaviour
{
    [Range(0.3f, 2f)] public float Scale = 1;
    public bool RevertToDefaults;
    public bool UpdateSize;

    private const float DefaultInnerNodeSize = 1f;
    private const float DefaultOuterNodeSize = 1.5f;
    private const float DefaultSpringDistance = 1f;

    private void Start()
    {
        Resize();
    }

    private void Update()
    {
        if (RevertToDefaults) Scale = 1;
        
        if (UpdateSize || RevertToDefaults) Resize();
        
    }

    private void Resize()
    {
        UpdateSize = false;
        RevertToDefaults = false;
            
        List<Transform> transforms = new List<Transform>();
            
        transforms.Add(GameObject.Find("Softbody/Centre").transform);
        for (var i = 1; i <= 6; i++)
        {
            transforms.Add(GameObject.Find("Softbody/C" + i).transform);
        }

        foreach (var tf in transforms)
        {
            tf.localScale = new Vector3(DefaultInnerNodeSize * Scale, DefaultInnerNodeSize * Scale, 1);
        }
            
        transforms.Clear();
            
        for (var i = 1; i <= 9; i++)
        {
            transforms.Add(GameObject.Find("Softbody/O" + i).transform);
        }

        foreach (var tf in transforms)
        {
            tf.localScale = new Vector3(DefaultOuterNodeSize * Scale, DefaultOuterNodeSize * Scale, 1);
        }
            
        var springs = GetComponentsInChildren<SpringJoint2D>();

        foreach (var sp in springs)
        {
            sp.distance = DefaultSpringDistance * Scale;
        }
    } 
}