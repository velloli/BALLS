using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect jumpSmoke;
    public GameObject jumpSmokeSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PopSmoke()
    {
        jumpSmoke.transform.position = jumpSmokeSpawnPoint.transform.position;
        jumpSmoke.Reinit();
        jumpSmoke.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
