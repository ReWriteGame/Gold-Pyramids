using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum MovementType// добавит в инспекоре  
    {
        Moveing,
        Lerping,
        Teleporting
    }

    public MovementType type = MovementType.Moveing;// вид движения 
    public MovementPath myPath;// используемый путь 
    public float speed = 1;
    public float maxDistance = .1f; // на какое растоение должен подойти объект что бы перейти на следующую точку 

    public bool usePathOneTime = false;// использует путь 1 раз 
    public bool turnOffAfterPassing = false;

    private Transform currentPos;// текущая точка движения
    private IEnumerator<Transform> pointInPath;// следущая точка движения (проверка точек)

    private void Start()
    {
        currentPos = GetComponent<Transform>();

        if (myPath == null)// debug проверка 
        {
            Debug.Log("Путь null");
            return;
        }


        pointInPath = myPath.GetNextPathPoint();// получаем данные из другова скрипта вызываем метод из MovementPath другова объекта  это точка 0

        pointInPath.MoveNext();// получение следующей точки в пути 1

        if (pointInPath == null)// debug проверка
        {
            Debug.Log("Точки не найдены");
            return;
        }

        currentPos.position = pointInPath.Current.position;// перемещаем обект на старотовую точку 
    }

    private void Update()
    {
        if (pointInPath == null || pointInPath.Current == null)// debug проверка если путь удалили и тд...
            return;


        if(type == MovementType.Moveing)
            currentPos.position = Vector3.MoveTowards(currentPos.position, pointInPath.Current.position, Time.deltaTime * speed);
        

        if (type == MovementType.Lerping)
            currentPos.position = Vector3.Lerp(currentPos.position, pointInPath.Current.position, Time.deltaTime * speed);


        if (type == MovementType.Teleporting)
            currentPos.position = pointInPath.Current.position;




        float distanceSqure = (currentPos.position - pointInPath.Current.position).sqrMagnitude;

        if (usePathOneTime)
        {
            //линейное движение 
            if (myPath.startPoint == MovementPath.StartPoint.end && myPath.moveingTo == 0 ||
                myPath.startPoint == MovementPath.StartPoint.start && myPath.moveingTo == myPath.PathElements.Length - 1)
            {
                if (turnOffAfterPassing && currentPos.position == pointInPath.Current.position) GetComponent<FollowPath>().enabled = false;// добавил проверку позиции что бы избавиться от остановки на предпоследних точках
                return;
            }
            // зациклинное движение (дописать на зацикленное движение)
        }

        if (distanceSqure < maxDistance * maxDistance)// достаточно ли мы близко по т. пифагора 
            pointInPath.MoveNext();// двигаемся к след точке 

    }
}

// нашел альтернативную телепортацию можно просто поставить одну точку и при активации скрипта мы автоматически туда приедем а потом просто выключить скрипт
