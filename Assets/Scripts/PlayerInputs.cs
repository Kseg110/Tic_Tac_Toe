using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInputs : MonoBehaviour
{
    private int moveCounter = 0;
    public int MoveCounter => moveCounter;

    private void Awake()
    {
        moveCounter = 0;
    }



}
