using UnityEngine;

public class HeatCooling : MonoBehaviour
{
    enum State
    {
        None,
        Heat,
        SuperHeat
    }

    float heat;
    State currentState;
    [SerializeField] private float heatIncreaseSpeed = 25f;
    [SerializeField] private float heatDecreaseSpeed = 15f;
    bool overheat = false;
    float overheatTimer = 0f;

    Vector3 originScale;
    Quaternion originRotation;

    void Start()
    {
        originScale = transform.localScale;
        originRotation = transform.localRotation;
    }

    void Update()
    {
        if (overheat)
        {
            overheatTimer += Time.deltaTime;

            if(overheatTimer >= 2f)
            {
                heat -= heatDecreaseSpeed * Time.deltaTime;
            }

            if(heat <= 20f)
            {
                overheat = false;
                overheatTimer = 0f;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                heat += heatIncreaseSpeed * Time.deltaTime;
                transform.localScale = originScale * Mathf.Lerp(1f, 1.5f, heat / 100);

                float angle = Mathf.Sin(Time.time * 30f) * 5f;
                transform.localRotation = originRotation * Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                heat -= heatDecreaseSpeed * Time.deltaTime;
                transform.localScale = originScale * Mathf.Lerp(1f, 1.5f, heat / 100f);
                transform.localRotation = originRotation;
            }

            if(heat >= 80f)
            {
                overheat = true;
                overheatTimer = 0f;
            }
        }


        heat = Mathf.Clamp(heat, 0, 100);

        HeatState();
    }

    void HeatState()
    {

        if (heat <= 39f)
            currentState = State.None;
        else if (heat <= 79f)
            currentState = State.Heat;
        else
            currentState = State.SuperHeat;

        if (currentState == State.None)
            this.GetComponent<MeshRenderer>().material.color = Color.blue;
        else if (currentState == State.Heat)
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
        else
            this.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
