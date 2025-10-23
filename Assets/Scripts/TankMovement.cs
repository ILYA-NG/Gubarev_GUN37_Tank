using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;    // Колеса для привода
    public bool steering; // Колеса для поворота
}

public class TankMovement : MonoBehaviour
{
    public List<AxleInfo> axleInfos;  // Информация по каждой оси
    public float maxMotorTorque = 1500f;    // Максимальный крутящий момент
    public float maxSteeringAngle = 30f;    // Максимальный угол поворота колес
    public float fallThreshold = -10f; // Порог высоты падения

    void FixedUpdate()
    {
        // Получаем ввод
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    // Синхронизируем позицию визуальных колес с WheelCollider
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;

        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            // Перезагрузить текущую сцену
            FindObjectOfType<GameManager>().EndGame();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }    
}
