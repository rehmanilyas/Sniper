using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem sparks;

    public static ParticleEffects Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void SparksAt(Vector3 pos, Transform refTransform=null)
    {
        sparks.transform.position = pos;

        if (refTransform != null)
        {

        }

        sparks.Emit(10);
    }
}
