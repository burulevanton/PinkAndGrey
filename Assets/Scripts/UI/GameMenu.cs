using System;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private Animator _animator;
    public Action action;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Close(Action action)
    {
        this.action = action;
        _animator.SetTrigger("Close");
    }

    public void OnAnimationEnd()
    {
        action();
        gameObject.SetActive(false);
    }
}
