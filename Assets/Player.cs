using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D _collider2D;
//    private int vertical;
//    private int horizontal;
//    private Vector2 direction;
    private bool isMoving;
    private bool isGrounded;

    public PlayerDirection PlayerDirection;
    
    public LayerMask groundCheckLayerMask;
    public Transform groundCheckTransform;
    
    private Vector3 fp;   //Первая позиция касания
    private Vector3 lp;   //Последняя позиция касания
    private float dragDistance;  //Минимальная дистанция для определения свайпа
    private List<Vector3> touchPositions = new List<Vector3>(); //Храним все позиции касания в списке
    

    void Start()
    {
        PlayerDirection = new PlayerDirection();
        body = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        isMoving = false;
        isGrounded = false;
        dragDistance = Screen.height*5/100;
    }

    private void Update()
    {
        Debug.Log($"isGrounded = {isGrounded}, isMoving = {isMoving}");
        if (isMoving) return;
        PlayerDirection.Vertical = 0;
        PlayerDirection.Horizontal = 0;
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
                            PlayerDirection.Horizontal = 1;
                        }
                        else
                        {
                            //Свайп влево
                            Debug.Log("Left Swipe");
                            PlayerDirection.Horizontal = -1;
                        }
                    }
                    else
                    {
                        //Если вертикальное движение больше, чнм горизонтальное движение
                        if (lp.y > fp.y) //Если движение вверх
                        {
                            //Свайп вверх
                            Debug.Log("Up Swipe");
                            PlayerDirection.Vertical = 1;
                        }
                        else
                        {
                            //Свайп вниз
                            Debug.Log("Down Swipe");
                            PlayerDirection.Vertical = -1;
                        }
                    }
                }

                touchPositions.Clear();
            }
        }
        isMoving = PlayerDirection.Vertical != 0 || PlayerDirection.Horizontal != 0;
        if (isMoving)
        {
            transform.rotation = PlayerDirection.RotateAngleZ;
            body.velocity = PlayerDirection.Direction * 40.0f;
            StartCoroutine(WaitForLoseGround());
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
        isMoving = false;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    IEnumerator WaitForLoseGround()
    {
        yield return new WaitForSeconds(3*Time.deltaTime);
        isMoving = !isGrounded;
        yield return null;
    }
}
