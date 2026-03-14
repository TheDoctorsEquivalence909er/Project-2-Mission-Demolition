using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILE = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;

    public bool awake {
        get { return _awake; }
        private set { _awake = value; }
    }

    private Vector3 prevPos;
    private List<float> deltas = new List<float>();
    private Rigidbody rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        awake = true;
        prevPos = transform.position;
        deltas.Add(0);

        PROJECTILE.Add(this);
    }

    void FixedUpdate()
    {
        if (rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while (deltas.Count > LOOKBACK_COUNT)
            deltas.RemoveAt(0);

        float maxDelta = 0;

        foreach (float f in deltas)
            if (f > maxDelta) maxDelta = f;

        if (maxDelta <= Physics.sleepThreshold)
        {
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy() {
        PROJECTILE.Remove(this);
    }

    static public void DESTROY_PROJECTILES() {
        for (int i = PROJECTILE.Count - 1; i >= 0; i--) {
            Destroy(PROJECTILE[i].gameObject);
        }
    }
}