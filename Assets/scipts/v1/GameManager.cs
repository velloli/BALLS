using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // A static reference to the GameManager instance

    public delegate void BulletEventHandler();
    public static event BulletEventHandler playerPhase;
    public static event BulletEventHandler playerUnPhase;




    public bool phaseActive = false;
    public bool jumpUnlocked = false;
    public bool jumpStrafeUnlocked = false;
    public bool phaseUnlocked = false;
    public bool reverseUnlocked = false;

    public int bulletCount = 0;

    public TMP_Text bulletCountUI;

    public GameObject player;

    void Awake()
    {
        
        if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }

        Debug.Log("GM SET");
    }

    private void Update()
    {
        bulletCountUI.SetText("bullet count: " + bulletCount.ToString());
    }
    public void ExecutePhase()
    {
        StartCoroutine(InvokeRoutine());

    }

    public void EnableJump()
    {
        jumpUnlocked = true;   
    }
    public void EnableJumpStrafe()
    {
        jumpStrafeUnlocked=true;
    }
    public void EnablePhase()
    {
        phaseUnlocked = true;
    }
    public void EnableReverse()
    {
        reverseUnlocked = true;
    }
    private IEnumerator InvokeRoutine()
    {
        playerPhase.Invoke();
        phaseActive = true;
        yield return new WaitForSeconds(3);
        playerUnPhase.Invoke();
        phaseActive = false;
    }

}
