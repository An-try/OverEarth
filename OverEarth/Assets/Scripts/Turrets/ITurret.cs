public interface ITurret
{
    void SetTurretParameters();
    void ChangeTurretAutoFire(bool turretAutoFire);
    void AutomaticTurretControl();
    void ManualTurretControl();

    void SearchTheNearestTargetWithParameter();
    void SetAimPoint();
    void RotateBase();
    void RotateCannon();
    bool AimedOnEnemy();
    bool AimedOnYourself();

    void Shoot();

    void CooldownDecrease();
    bool CooldownIsZero();

    void RotateToIdle();
}
