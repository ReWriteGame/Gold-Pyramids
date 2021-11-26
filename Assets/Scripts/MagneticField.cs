using UnityEngine;


[RequireComponent(typeof(Collider))]
public class MagneticField : MonoBehaviour
{
    public float force = 1;
    public float dragFill = 0;


    public AnimationCurve forceMagneticCurve;
    public Vector2 forceMagnetic2;
    public bool isActive = true;
    private SphereCollider currentCollider;


    float dampingObj;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        dampingObj = other.GetComponent<Rigidbody>().drag;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive)
        {
            other.GetComponent<Rigidbody>().drag = dragFill;
            if (other.GetComponent<Magnetable>())
            {

                if (!other.GetComponent<Magnetable>().ignoreMagneticField)
                {
                    float distance = Vector3.Distance(transform.position, other.transform.position);
                    float percent = 1 - distance / gameObject.GetComponent<SphereCollider>().radius;


                    Vector3 dirictionMove = (other.transform.position - transform.position) * (force * forceMagneticCurve.Evaluate(percent));
                    //other.GetComponent<Rigidbody>().AddForce(-dirictionMove);


                    other.GetComponent<Rigidbody>().velocity += -dirictionMove;

                }
            }
        }
    }


    //TODO:
    //��������� ����������  ��� ����� � ������
    //����� ������ ��������� � ���� ���������� 
    // ������ � ������� �� ��������� ���� � ����� ����� 
    // �������� �� �����? 
}
