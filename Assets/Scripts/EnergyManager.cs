using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float energyRecoveryRate = 5f;
    public float energyConsumptionPerJump = 20f;

    void Start()
    {
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        RecoverEnergy();
    }

    void RecoverEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRecoveryRate * Time.deltaTime;
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }
    }

    public bool ConsumeEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }
        return false;
    }
}
