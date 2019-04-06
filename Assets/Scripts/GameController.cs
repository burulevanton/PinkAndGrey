using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{

  private Vector3 fp; //Первая позиция касания
  private Vector3 lp; //Последняя позиция касания
  private float dragDistance; //Минимальная дистанция для определения свайпа
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

  public static event Action<EDirection> OnSwipe;
  public Text Text;

  //public static GameController instance;
  public PlayerController PlayerController;
  public GameUIController GameUiController;

  [SerializeField] private Movable[] _movables;

  void Awake()
  {
    dragDistance = Screen.height * 5 / 100;
  }

  private void Start()
  {
    StartCoroutine(StartLevel());
  }

  private IEnumerator StartLevel()
  {
    Text.text = "Десериализация";
    yield return StartCoroutine(LevelController.Instance.Deserialize());
    Text.text = "Десериализация закончена";
    yield return StartCoroutine(GameUiController.StartScene());
  }

  public void LevelPassed()
  {
    GameData.Instance.CurrentLevel++;
    StartCoroutine(StartLevel());
  }

  // Update is called once per frame
  void Update()
  {
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
    movable.MovableDirection.Horizontal =
    movable.IsReverseMove ? _movableDirection.Horizontal * -1 : _movableDirection.Horizontal;
    movable.MovableDirection.Vertical =
    movable.IsReverseMove ? _movableDirection.Vertical * -1 : _movableDirection.Vertical;
  }
}
