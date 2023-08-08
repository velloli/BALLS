using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    //public game
    public WeaponItem weapon;
    public MeshFilter displayMesh;
    // Start is called before the first frame update
    void Start()
    {
        displayMesh = GetComponent<MeshFilter>();
        displayMesh.mesh = weapon.displayMesh;
    }

    // Update is called once per frame
    void Update()
    {
        //displayMesh.mesh = weapon.displayMesh;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Build>().AddWeaponItem(weapon);
        }
        Debug.Log("Player picked up object");
        Destroy(this.gameObject);
    }
}
