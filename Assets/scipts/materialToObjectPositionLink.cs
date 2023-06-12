using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialToObjectPositionLink : MonoBehaviour
{
    public GameObject player;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        //material = GetComponent<Renderer>().materials[0];
        //GetComponent<Terrain>().materialTemplate.SetVector(
        //    "player position",
        //    new Vector4(player.transform.position.x, player.transform.position.y, player.transform.position.z, 0));
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector4 siu = new Vector4(player.transform.position.x, player.transform.position.y, player.transform.position.z, 0);
        material.SetVector("_player_position", siu);
        //Debug.Log(GetComponent<Renderer>().material.GetVector("_player_position"));
        //GetComponent<Renderer>().material = material;
        //Debug.Log(material.ToString());
        //GetComponent<Terrain>().materialTemplate = null;
    }
}
