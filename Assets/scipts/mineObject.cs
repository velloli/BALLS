using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineObject : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float fuseTime = 5.0f;
    public float DMG = 10.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void triggerBomb()
    {
        //once this method is called a timer should start with fuseTime duration.
        //at the end of the duration, the mine should "explode" and check within a certain radius for enemy objects
        //destroy this gameobject once exploded

        //STAGE 1: check for enemy turrets within given radius and deal constant DMG to all objects with certain radius
        //STAGE 2: destroy all bullets within certain radius
        //STAGE 3: damage fall-off for far away objects( not really required but it would be coll to have a proof of concept)
        //ALL ENEMIES HAVE "enemy" tag on them

    }
}

//LINKS
//https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html USEFUL FOR RADIUS CHECKING


