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
        Vector3 siu = player.transform.position;
        //material.SetVector("_player_position_prev", siu);
        material.SetVector("_player_position", siu);
        siu = player.GetComponent<Schmovement>().positions.GetLastElement();

        //Debug.Log(GetComponent<Renderer>().material.GetVector("_player_position"));
        //GetComponent<Renderer>().material = material;
        //Debug.Log(material.ToString());
        //GetComponent<Terrain>().materialTemplate = null;
    }
}
