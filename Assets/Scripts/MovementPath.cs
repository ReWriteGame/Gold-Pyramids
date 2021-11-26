using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPath : MonoBehaviour
{
    public enum PathTypes// добавит в инспекоре  
    {
        linear,
        loop
    }
    public enum MovementDirection// добавит в инспекоре  
    {
        right,
        left,
        stopAtPoint
    }
    public enum StartPoint
    {
        start,
        end
    }


    public PathTypes pathType;// тип пути
    public StartPoint startPoint = StartPoint.start;// стартовая точка
    public MovementDirection movementDirection = MovementDirection.right;// направление движения вперед назад

    public int moveingTo;// точка к которой движемся 
    public Transform[] PathElements;// могу быть проблемы изза открытости мы имеем доступ к массиву 

    private int direction = 1;// -1 0 1

    private void Awake()
    {
        if (startPoint == StartPoint.start)
            moveingTo = 0;

        if (startPoint == StartPoint.end)
            moveingTo = PathElements.Length - 1;
    }

    public IEnumerator<Transform> GetNextPathPoint()// возвращает Transform положение текщей точки 
    {
        if (PathElements == null || PathElements.Length < 1)// выход если мало элементов или их нет
            yield break;// выход из корутина return тут не работате

       

        while (true)// отрисовываем все точки в пути
        {
            yield return PathElements[moveingTo];// возвращает ТЕКУЩУЮ точку

            if (PathElements.Length == 1 /*|| !startMoving*/)// проверка если точка одна 
                continue;


            if (pathType == PathTypes.linear)// разворачивает линейное движение
            {
                if (movementDirection == MovementDirection.right && moveingTo >= PathElements.Length - 1)// двигаемся по наростающей
                    movementDirection = MovementDirection.left;

                else if (movementDirection == MovementDirection.left && moveingTo <= 0)// двигаемся по убывающей
                    movementDirection = MovementDirection.right;
            }


            if (pathType == PathTypes.loop)
            { 
                if (movementDirection == MovementDirection.right && moveingTo >= PathElements.Length - 1)// дошли до конца с начала
                    moveingTo = 0 - 1;// идем в начало 

                else if (movementDirection == MovementDirection.left && moveingTo <= 0)// дошли до начала с конца 
                    moveingTo = PathElements.Length;// идем в конец
            }


            if (movementDirection == MovementDirection.right) direction = 1;
            if (movementDirection == MovementDirection.left) direction = -1;
            if (movementDirection == MovementDirection.stopAtPoint) direction = 0;


            moveingTo += direction;// направление движения
        }   
    }
    public void OnDrawGizmos()
    {
        if (PathElements == null || PathElements.Length < 2) return;// проверка минимум 2 элемента в пути

        for (int i = 1; i < PathElements.Length; i++)
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);

        if (pathType == PathTypes.loop)
            Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length - 1].position);
    }
}

// IEnumerator - колекция это не много поток он вызываеться из обетка на котором весит но выполняется тип паралельно 
// MoveNext() - метод возвражает следующий елемент колекции и переходит на него
// Current возвращет текущий елемент последовательности 
// Метод Reset() сбрасывает указатель позиции в начальное положение.

// добавить прохождение через несколько секунд

// Руководство: useOneAtStart и usePathOneTime могут конфликтовать желательно использовать по раздельнности 
//usePathOneTime нельзяиспользовать со старта игры
// путь не должен принадлажать объекту движения из-за дочерности он идет вместе с обектом либо попробовать раздочерить пути
// возможно будет конфликтовать с rb

//    using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MovementPath : MonoBehaviour
//{
//    public enum PathTypes// добавит в инспекоре  
//    {
//        linear,
//        loop
//    }
//    public enum MovementDirection// добавит в инспекоре  
//    {
//        right,
//        left,
//        stopAtPoint
//    }




//    //public bool startpoint;// start end(дописать)

//    public PathTypes pathType;// тип пути
//    public MovementDirection movementDirection = MovementDirection.right;// направление движения вперед назад

//    //public bool startMoving = true;// начинать движение или нет
//    // public bool useOneAtStart = false;// пройти путь 1 раз с начала игры (возможно нужно что-то допилить )
//    // public bool usePathOneTime = false;// использует путь 1 раз (работает НО есть БАГИ с работой)
//    public int moveingTo = 0;// точка к которой движемся 
//    public Transform[] PathElements;

//    private int direction = 1;







//    /*private void Awake()
//    {
//        if (usePathOneTime == true) usePathOneTime = false;// убирает баг что объект не двигаеться при старте возник изза того что объект находиться на координатах которые есть в исключениях
//    }*/

//    public IEnumerator<Transform> GetNextPathPoint()// возвращает Transform положение текщей точки 
//    {
//        if (PathElements == null || PathElements.Length < 1)// выход если мало элементов или их нет
//            yield break;// выход из корутина return тут не работате



//        while (true)// отрисовываем все точки в пути
//        {
//            yield return PathElements[moveingTo];// возвращает ТЕКУЩУЮ точку

//            if (PathElements.Length == 1 /*|| !startMoving*/)// проверка если точка одна 
//                continue;




//            if (pathType == PathTypes.linear)// разворачивает линейное движение
//            {
//                /*if ((moveingTo == PathElements.Length - 1 || moveingTo == 0) /*&& usePathOneTime*)// пройти путь 1 раз
//                {
//                    //seOneAtStart = false;
//                    continue;
//                }*/


//                if (movementDirection == MovementDirection.right && moveingTo >= PathElements.Length - 1)// двигаемся по наростающей
//                    movementDirection = MovementDirection.left;

//                else if (movementDirection == MovementDirection.left && moveingTo <= 0)// двигаемся по убывающей
//                    movementDirection = MovementDirection.right;
//            }


//            if (pathType == PathTypes.loop)
//            {
//                /*if (moveingTo == 0/* && usePathOneTime)// пройти путь 1 раз (будет всегда заканчиваться в начальной точке)
//                {
//                    //useOneAtStart = false;
//                    continue;// выход из ф
//                }*/
//                if (movementDirection == MovementDirection.right && moveingTo >= PathElements.Length - 1)// дошли до конца с начала
//                    moveingTo = 0 - 1;// идем в начало 

//                else if (movementDirection == MovementDirection.left && moveingTo <= 0)// дошли до начала с конца 
//                    moveingTo = PathElements.Length;// идем в конец
//            }


//            if (movementDirection == MovementDirection.right) direction = 1;
//            if (movementDirection == MovementDirection.left) direction = -1;
//            if (movementDirection == MovementDirection.stopAtPoint) direction = 0;


//            moveingTo += direction;// направление движения

//            //if (useOneAtStart) usePathOneTime = true;
//        }
//    }
//    public void OnDrawGizmos()
//    {
//        if (PathElements == null || PathElements.Length < 2) return;// проверка минимум 2 элемента в пути

//        for (int i = 1; i < PathElements.Length; i++)
//            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);


//        if (pathType == PathTypes.loop)
//            Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length - 1].position);
//    }
//}

// IEnumerator - колекция это не много поток он вызываеться из обетка на котором весит но выполняется тип паралельно 
// MoveNext() - метод возвражает следующий елемент колекции и переходит на него
// Current возвращет текущий елемент последовательности 
// Метод Reset() сбрасывает указатель позиции в начальное положение.

// добавить прохождение через несколько секунд

// Руководство: useOneAtStart и usePathOneTime могут конфликтовать желательно использовать по раздельнности 
//usePathOneTime нельзяиспользовать со старта игры
// путь не должен принадлажать объекту движения из-за дочерности он идет вместе с обектом либо попробовать раздочерить пути
// возможно будет конфликтовать с rb

