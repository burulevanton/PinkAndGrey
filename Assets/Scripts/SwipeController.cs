using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

public class SwipeController : Singleton<SwipeController>
{
    private Vector3 fp; //Первая позиция касания
    private Vector3 lp; //Последняя позиция касания
    private float dragDistance; //Минимальная дистанция для определения свайпа
    private List<Vector3> touchPositions = new List<Vector3>(); //Храним все позиции касания в списке

    private float swipeTimeout;
    private float swipeTime = 0.3f;
    private int touchId = -1;
    private Vector2 touchBeginPoint = Vector2.zero;
    private bool swiped;

    private float swipeSqrMagnitude = 0.0015f;

    private float dblTouchTimeout;
    private float dblTouchTime = 0.19f;
    private bool _tutorialPass;

    public static event Action<EDirection> OnSwipe;
    
    void Awake()
    {
        dragDistance = Screen.height * 5 / 100;
        _tutorialPass = GameData.Instance.TutorialPass;
    }
    
    void Update()
  {
    if (_tutorialPass && GameController.Instance.IsPaused)
      return;
    ReadTouches();
  }

  private void ReadTouches()
  {
    EDirection edirection = EDirection.None;
    bool flag1 = false; //doubletap
    float deltaTime = Time.deltaTime;
    if ((double) this.swipeTimeout > 0.0)
      this.swipeTimeout -= deltaTime;
    if ((double) this.dblTouchTimeout > 0.0)
      this.dblTouchTimeout -= deltaTime;
    if (Input.touchCount == 0)
    {
      this.touchId = -1;
    }
    else
    {
      for (int index = 0; index < Input.touchCount; ++index)
      {
        Touch touch = Input.GetTouch(index);
        switch (touch.phase)
        {
          case TouchPhase.Began:
            if (this.touchId < 0)
            {
              this.touchId = touch.fingerId;
              this.touchBeginPoint = (Vector2) Camera.main.ScreenToWorldPoint((Vector3) touch.position);
              this.swiped = false;
              this.swipeTimeout = this.swipeTime;
              if ((double) this.dblTouchTimeout > 0.0)
              {
                flag1 = true;
                break;
              }

              this.dblTouchTimeout = this.dblTouchTime;
              break;
            }

            break;
          case TouchPhase.Moved:
            if (touch.fingerId == this.touchId && !this.swiped)
            {
              Vector2 worldPoint = (Vector2) Camera.main.ScreenToWorldPoint((Vector3) touch.position);
              if ((double) this.swipeTimeout > 0.0)
              {
                if ((double) (worldPoint - this.touchBeginPoint).sqrMagnitude > (double) this.swipeSqrMagnitude)
                {
                  float num = Mathf.Abs(touch.deltaPosition.x);
                  edirection = (double) Mathf.Abs(touch.deltaPosition.y) < (double) num
                  ? ((double) touch.deltaPosition.x <= 0.0 ? EDirection.Left : EDirection.Right)
                  : ((double) touch.deltaPosition.y <= 0.0 ? EDirection.Down : EDirection.Up);
                  this.swiped = true;
                  this.dblTouchTimeout = 0.0f;
                  this.swipeTimeout = 0.0f;
                  break;
                }

                break;
              }

              this.touchBeginPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
              this.swipeTimeout = this.swipeTime;
              break;
            }

            break;
          case TouchPhase.Ended:
            if (touch.fingerId == this.touchId)
            {
              this.touchId = -1;
              this.swipeTimeout = 0.0f;
              break;
            }

            break;
          case TouchPhase.Canceled:
            if (touch.fingerId == this.touchId)
            {
              this.touchId = -1;
              this.dblTouchTimeout = 0.0f;
              this.swipeTimeout = 0.0f;
              break;
            }

            break;
        }
      }
    }

    if (edirection != EDirection.None && SwipeController.OnSwipe != null)
    {
      SwipeController.OnSwipe(edirection);
    }

    if (!flag1)
      return;
    //this.ProcessDblTap(); //todo функция даблтапа
  }
    
}