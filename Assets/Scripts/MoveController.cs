using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    
    private Vector3 fp;   //Первая позиция касания
    private Vector3 lp;   //Последняя позиция касания
    private float dragDistance;  //Минимальная дистанция для определения свайпа
    private List<Vector3> touchPositions = new List<Vector3>(); //Храним все позиции касания в списке
    
    private MovableDirection _movableDirection = new MovableDirection();

    [SerializeField] private Movable[] _movables;
    // Start is called before the first frame update
    void Start()
    {
        dragDistance = Screen.height*5/100;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movables.Any(movable => movable.IsMoving))
        {
            return;
        }
        _movableDirection.Vertical = 0;
        _movableDirection.Horizontal = 0;
        foreach (Touch touch in Input.touches) //используем цикл для отслеживания больше одного свайпа
        {

            if (touch.phase == TouchPhase.Moved) //добавляем касания в список, как только они определены
            {
                touchPositions.Add(touch.position);
            }

            if (touch.phase == TouchPhase.Ended) //проверяем, если палец убирается с экрана
            {
                //lp = touch.position;  //последняя позиция касания. закоментируйте если используете списки
                fp = touchPositions[0]; //получаем первую позицию касания из списка касаний
                lp = touchPositions[touchPositions.Count - 1]; //позиция последнего касания

                //проверяем дистанцию перемещения больше чем 20% высоты экрана
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    //это перемещение
                    //проверяем, перемещение было вертикальным или горизонтальным 
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        //Если горизонтальное движение больше, чем вертикальное движение ...
                        if ((lp.x > fp.x)) //Если движение было вправо
                        {
                            //Свайп вправо
                            Debug.Log("Right Swipe");
                            _movableDirection.Horizontal = 1;
                        }
                        else
                        {
                            //Свайп влево
                            Debug.Log("Left Swipe");
                            _movableDirection.Horizontal = -1;
                        }
                    }
                    else
                    {
                        //Если вертикальное движение больше, чнм горизонтальное движение
                        if (lp.y > fp.y) //Если движение вверх
                        {
                            //Свайп вверх
                            Debug.Log("Up Swipe");
                            _movableDirection.Vertical = 1;
                        }
                        else
                        {
                            //Свайп вниз
                            Debug.Log("Down Swipe");
                            _movableDirection.Vertical = -1;
                        }
                    }
                }

                touchPositions.Clear();
            }
        }

        foreach (var movable in _movables)
        {
            SetDirection(movable);
        }
    }

    public void SetDirection(Movable movable)
    {
            if (!movable.gameObject.activeSelf) return;
            movable.MovableDirection.Horizontal = movable.IsReverseMove ? _movableDirection.Horizontal*-1 : _movableDirection.Horizontal;
            movable.MovableDirection.Vertical = movable.IsReverseMove ? _movableDirection.Vertical*-1 : _movableDirection.Vertical;
    }
}
