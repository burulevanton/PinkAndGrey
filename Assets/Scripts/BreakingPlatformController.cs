using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatformController : MonoBehaviour
{
    public void BreakPlatform()
    {
        gameObject.SetActive(false);
    }
}
