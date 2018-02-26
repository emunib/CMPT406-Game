using UnityEngine;

public class MainScript : MonoBehaviour
{
    [CustomLabel("Gravity Force")] [Tooltip("Force at which gravity is applied to objects.")] [SerializeField]
    private float gravity_force = 9.81f;
    public static MainScript mainScript;

    void Awake()
    {
        if (!mainScript)
        {
            mainScript = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public float GravityForce()
    {
        return gravity_force;
    }
}