public interface ITurret
{
    void SetTurretParameters();
    void ChangeTurretAutoFire(bool turretAutoFire);

    void AutomaticTurretControl();
    void ManualTurretControl();

    bool AimedAtEnemy();
    bool AimedAtOwner();
    void SearchTheNearestTargetWithParameter();

    void RotateBase();
    void RotateCannons();
    void RotateToDefault();

    void CooldownDecrease();
    bool CooldownIsZero();

    void Shoot();
}
