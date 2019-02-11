using System;
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

    void FixedUpdate()
    {
        UpdateGroundStatus();
        if(isMoving)
            body.AddForce(PlayerDirection.Direction * 200.0f);
        if (isGrounded)
            isMoving = false;
        
    }

    private void Update()
    {
//        vertical = 0;
//        horizontal = 0;
        
        if (isMoving) return;
        PlayerDirection.Vertical = 0;
        PlayerDirection.Horizontal = 0;
        
        
//        if (Input.GetKey(KeyCode.UpArrow)) vertical = 1;
//        else if (Input.GetKey(KeyCode.DownArrow)) vertical = -1;
//        else vertical = 0;
//
//        if (vertical == 0)
//        {
//            if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1;
//            else if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1;
//            else horizontal = 0;
//        }
        foreach (Touch touch in Input.touches) //используем цикл для отслеживания больше одного свайпа
        {
            //должны быть закоментированы, если вы используете списки 
            /*if (touch.phase == TouchPhase.Began) //проверяем первое касание
            {
                fp = touch.position;
                lp = touch.position;
         
            }*/

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
        if(isMoving)
            transform.rotation = PlayerDirection.RotateAngleZ;
        Debug.Log($"isGrounded = {isGrounded}, isMoving = {isMoving}");

    }

//    private void OnCollisionEnter2D(Collision2D other)
//    {
//        isMoving = false;
//    }

//    private UnityEngine.Quaternion GetRotateAngleZ()
//    {
//        if(direction == Vector2.up)
//            return Quaternion.Euler(0,0,180f);
//        if(direction == Vector2.down)
//            return Quaternion.Euler(0,0,0f);
//        if (direction == Vector2.left)
//            return Quaternion.Euler(0, 0, -90f);
//        if (direction == Vector2.right)
//            return Quaternion.Euler(0, 0, 90f);
//        return new Quaternion();
//    }

    void UpdateGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
    }
    
    
}
