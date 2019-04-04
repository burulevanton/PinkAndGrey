using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using UI;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    
    private Vector3 fp;   //Первая позиция касания
    private Vector3 lp;   //Последняя позиция касания
    private float dragDistance;  //Минимальная дистанция для определения свайпа
    private List<Vector3> touchPositions = new List<Vector3>(); //Храним все позиции касания в списке
    
    private MovableDirection _movableDirection = new MovableDirection();
    
    private float swipeTimeout;
    private float swipeTime = 0.3f;
    private int touchId = -1;
    private Vector2 touchBeginPoint = Vector2.zero;
    private bool swiped;
  
    private float swipeSqrMagnitude = 0.0015f;
  
    private float dblTouchTimeout;
    private float dblTouchTime = 0.19f;

    public int CurrentLevel { get; set; } = 1;
    
    public static event Action<EDirection> OnSwipe;
  
    //public static GameController instance;
    public PlayerController PlayerController;
    public GameUIController GameUiController;

    [SerializeField] private Movable[] _movables;
    void Awake()
    {
      
//        if (instance == null)
//          instance = this;
//        else 
//        if (instance != this)
//          Destroy(gameObject);
//        DontDestroyOnLoad(this);
        DontDestroyOnLoad(this);
        dragDistance = Screen.height*5/100;
    }

    private void Start()
    {
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return StartCoroutine(LevelController.Instance.Deserialize());
        GameUiController.StartScene();
    }

    // Update is called once per frame
    void Update()
    {
        ReadTouches();
//        if (_movables.Any(movable => movable.IsMoving))
//        {
//            return;
//        }
//        _movableDirection.Vertical = 0;
//        _movableDirection.Horizontal = 0;
//        foreach (Touch touch in Input.touches) //используем цикл для отслеживания больше одного свайпа
//        {
//
//            if (touch.phase == TouchPhase.Moved) //добавляем касания в список, как только они определены
//            {
//                touchPositions.Add(touch.position);
//            }
//
//            if (touch.phase == TouchPhase.Ended) //проверяем, если палец убирается с экрана
//            {
//                //lp = touch.position;  //последняя позиция касания. закоментируйте если используете списки
//                fp = touchPositions[0]; //получаем первую позицию касания из списка касаний
//                lp = touchPositions[touchPositions.Count - 1]; //позиция последнего касания
//
//                //проверяем дистанцию перемещения больше чем 20% высоты экрана
//                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
//                {
//                    //это перемещение
//                    //проверяем, перемещение было вертикальным или горизонтальным 
//                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
//                    {
//                        //Если горизонтальное движение больше, чем вертикальное движение ...
//                        if ((lp.x > fp.x)) //Если движение было вправо
//                        {
//                            //Свайп вправо
//                            Debug.Log("Right Swipe");
//                            _movableDirection.Horizontal = 1;
//                        }
//                        else
//                        {
//                            //Свайп влево
//                            Debug.Log("Left Swipe");
//                            _movableDirection.Horizontal = -1;
//                        }
//                    }
//                    else
//                    {
//                        //Если вертикальное движение больше, чнм горизонтальное движение
//                        if (lp.y > fp.y) //Если движение вверх
//                        {
//                            //Свайп вверх
//                            Debug.Log("Up Swipe");
//                            _movableDirection.Vertical = 1;
//                        }
//                        else
//                        {
//                            //Свайп вниз
//                            Debug.Log("Down Swipe");
//                            _movableDirection.Vertical = -1;
//                        }
//                    }
//                }
//
//                touchPositions.Clear();
//            }
//        }
//
//        foreach (var movable in _movables)
//        {
//            SetDirection(movable);
//        }
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
                  edirection = (double) Mathf.Abs(touch.deltaPosition.y) < (double) num ? ((double) touch.deltaPosition.x <= 0.0 ? EDirection.Left : EDirection.Right) : ((double) touch.deltaPosition.y <= 0.0 ? EDirection.Down : EDirection.Up);
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
    if (edirection != EDirection.None && GameController.OnSwipe != null)
    {
        GameController.OnSwipe(edirection);
    }
    if (!flag1)
      return;
    //this.ProcessDblTap(); //todo функция даблтапа
  }

    public void SetDirection(Movable movable)
    {
            if (!movable.gameObject.activeSelf) return;
            movable.MovableDirection.Horizontal = movable.IsReverseMove ? _movableDirection.Horizontal*-1 : _movableDirection.Horizontal;
            movable.MovableDirection.Vertical = movable.IsReverseMove ? _movableDirection.Vertical*-1 : _movableDirection.Vertical;
    }
}
