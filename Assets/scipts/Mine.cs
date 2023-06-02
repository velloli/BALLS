using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    //THIS SCRIPT IS RESPONSIBLE FOR DROPPING THE MINE OBJECT

    //inmplement a cooldown and when user presses left alt, drop a mine if cooldown is over
    public GameObject mineObject;

    private GameObject player;//can be accessed through Gamemanager
    // Start is called before the first frame update
    void Start()
    {
        //always a good idea to check if mineObject is null in the start
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            // CONTROL STARTS HERE

            //TODO: drop mineObject at the player's location
        }
    }
}
