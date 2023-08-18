using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//need to change this later, figure out how to do weapon spawning preoperly
public class WeaponSpawn : MonoBehaviour
{
    //public game
    public WeaponItem weapon;
    public MeshFilter displayMeshFilter;
    // Start is called before the first frame update
    void Start()
    {
        displayMeshFilter = GetComponent<MeshFilter>();
        displayMeshFilter.mesh = weapon.displayMesh;
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
            other.gameObject.GetComponent<Build>().AddItemToBackpack(weapon);
        }
        Debug.Log("Player picked up object");
        Destroy(this.gameObject);
    }
}
