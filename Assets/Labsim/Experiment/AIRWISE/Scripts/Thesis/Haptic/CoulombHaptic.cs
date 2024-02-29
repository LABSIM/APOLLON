using System;
using UnityEngine;

class CoulombHapticCreator : HapticCreator
{
    public CoulombHapticCreator(AbstractHaptic.HapticAxisModes axisMode): base(axisMode) {}
    public override AbstractHaptic Create(AbstractHapticConfig abstractHapticConfig, Rigidbody rb, Manager manager)
    {
        CoulombHapticConfig CoulombHapticConfig = (abstractHapticConfig as CoulombHapticConfig);
        if (CoulombHapticConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert haptic config of type " + abstractHapticConfig.GetType() + " to CoulombHapticConfig.");
        }
        return new CoulombHaptic(CoulombHapticConfig, rb, manager);
    }
}

[Serializable]
public class CoulombHapticConfig : AbstractHapticConfig
{
    public float DistanceDanger, DistanceWarning;
    public float SecurityFactor;
    public float ReactionTime;
    public float kX, kY;
}

public class CoulombHaptic : AbstractHaptic
{
    // CoulombHaptic-specific members
    private float DistanceDanger, DistanceWarning, SecurityFactor, ReactionTime, kX, kY;
    private float[] Distances, SecuredDistances;
    private float ForceXToSend, ForceYToSend;
    private bool sent = false;

    private int DistanceDangerLoggerIdx, DistanceWarningLoggerIdx, 
        Distance0LoggerIdx, Distance1LoggerIdx, Distance2LoggerIdx, Distance3LoggerIdx, 
        SecuredDistance0LoggerIdx, SecuredDistance1LoggerIdx, SecuredDistance2LoggerIdx, SecuredDistance3LoggerIdx;

    public CoulombHaptic(CoulombHapticConfig config, Rigidbody rb, Manager manager) : base(config as AbstractHapticConfig, rb, manager) {
        this.DistanceDanger = config.DistanceDanger;
        this.DistanceWarning = config.DistanceWarning;
        this.SecurityFactor = config.SecurityFactor;
        this.ReactionTime = config.ReactionTime;
        this.kX = config.kX;
        this.kY = config.kY;
        this.sent = false;
        this.Distances = new float[Manager.Instance.QuadController.NbRaycast];
        this.SecuredDistances = new float[Manager.Instance.QuadController.NbRaycast];
        
        this.DistanceDangerLoggerIdx = Logger.Instance.GetEntry("DistanceDanger");
        this.DistanceWarningLoggerIdx = Logger.Instance.GetEntry("DistanceWarning");
        this.Distance0LoggerIdx = Logger.Instance.GetEntry("Distance0");
        this.Distance1LoggerIdx = Logger.Instance.GetEntry("Distance1");
        this.Distance2LoggerIdx = Logger.Instance.GetEntry("Distance2");
        this.Distance3LoggerIdx = Logger.Instance.GetEntry("Distance3");
        this.SecuredDistance0LoggerIdx = Logger.Instance.GetEntry("SecuredDistance0");
        this.SecuredDistance1LoggerIdx = Logger.Instance.GetEntry("SecuredDistance1");
        this.SecuredDistance2LoggerIdx = Logger.Instance.GetEntry("SecuredDistance2");
        this.SecuredDistance3LoggerIdx = Logger.Instance.GetEntry("SecuredDistance3");

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.SecurityFactorKey, this.SecurityFactor);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.ReactionTimeKey, this.ReactionTime);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.kXKey, this.kX);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.kYKey, this.kY);
    }

    // Apply to current RigidBody provided force and torque expressed in aero frame using Euler-Cardan ZYX angles
    public override void FetchCriterion(float tElapsed)
    {
        base.FetchCriterion(tElapsed);
        for (int i = 0; i < Manager.Instance.QuadController.NbRaycast; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(Manager.Instance.QuadController.transform.position, Utilities.ToUnityFromAeroFrame(Manager.Instance.QuadController.RayDirections[i]), out hit))
            {
                // float DistanceDanger = Mathf.Max(Mathf.Abs(this.ReactionTime * Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Utilities.ToUnityFromAeroFrame(Manager.Instance.QuadController.RayDirections[i]))), this.DistanceDanger);
                // float DistanceWarning = Mathf.Max(this.SecurityFactor * DistanceDanger, this.DistanceWarning);
                // float DistanceDanger = this.DistanceDanger + Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[i]) * this.ReactionTime;
                // float DistanceWarning = this.DistanceWarning + Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[i]) * this.ReactionTime;
                Logger.Instance.AddEntry(this.DistanceDangerLoggerIdx, DistanceDanger);
                Logger.Instance.AddEntry(this.DistanceWarningLoggerIdx, DistanceWarning);

                if (hit.distance < DistanceDanger)
                {
                    Debug.DrawRay(Manager.Instance.QuadController.transform.position, Manager.Instance.QuadController.transform.TransformDirection(Utilities.ToUnityFromAeroFrame(Manager.Instance.QuadController.RayDirections[i])) * hit.distance, Color.red);
                    this.Distances[i] = hit.distance;
                    this.SecuredDistances[i] = this.SecurityFactor * this.Distances[i];
                    // this.SecuredDistances[i] =  this.SecurityFactor * (this.Distances[i] - Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[i]) * this.ReactionTime);
                }
                else if (hit.distance < DistanceWarning)
                {
                    Debug.DrawRay(Manager.Instance.QuadController.transform.position, Manager.Instance.QuadController.transform.TransformDirection(Utilities.ToUnityFromAeroFrame(Manager.Instance.QuadController.RayDirections[i])) * hit.distance, Color.yellow);
                    this.Distances[i] = hit.distance;
                    this.SecuredDistances[i] = this.Distances[i];
                    // this.SecuredDistances[i] = (this.Distances[i] - Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[i]) * this.ReactionTime);
                }
                else 
                { 
                    Debug.DrawRay(Manager.Instance.QuadController.transform.position, Manager.Instance.QuadController.transform.TransformDirection(Utilities.ToUnityFromAeroFrame(Manager.Instance.QuadController.RayDirections[i])) * hit.distance, Color.green);
                    this.Distances[i] = Mathf.Infinity;
                    this.SecuredDistances[i] = this.Distances[i];
                }
            }
            else
            {
                this.Distances[i] = Mathf.Infinity;
                this.SecuredDistances[i] = this.Distances[i];
            }
        }
        Logger.Instance.AddEntry(this.Distance0LoggerIdx, this.Distances[0]);
        Logger.Instance.AddEntry(this.Distance1LoggerIdx, this.Distances[1]);
        Logger.Instance.AddEntry(this.Distance2LoggerIdx, this.Distances[2]);
        Logger.Instance.AddEntry(this.Distance3LoggerIdx, this.Distances[3]);
        Logger.Instance.AddEntry(this.SecuredDistance0LoggerIdx, this.SecuredDistances[0]);
        Logger.Instance.AddEntry(this.SecuredDistance1LoggerIdx, this.SecuredDistances[1]);
        Logger.Instance.AddEntry(this.SecuredDistance2LoggerIdx, this.SecuredDistances[2]);
        Logger.Instance.AddEntry(this.SecuredDistance3LoggerIdx, this.SecuredDistances[3]);
    }
    public override void ComputeForce()
    {
        // TODO: � nettoyer pour avoir une expression plus g�n�rale
        this.ForceXToSend = -this.kX * 1 / (this.Distances[0] * this.Distances[0]) + this.kX * 1 / (this.Distances[2] * this.Distances[2]);
        this.ForceYToSend = -this.kY * 1 / (this.Distances[1] * this.Distances[1]) + this.kY * 1 / (this.Distances[0] * this.Distances[0]);

        // float gainXPos = this.kX * Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[0]);
        // float gainXNeg = this.kX * Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[2]);
        // this.ForceXToSend = -gainXPos * 1 / (this.SecuredDistances[0] * this.SecuredDistances[0]) + gainXNeg * 1 / (this.SecuredDistances[2] * this.SecuredDistances[2]);
        // float gainYPos = this.kX * Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[1]);
        // float gainYNeg = this.kX * Utilities.ScalarProduct(AeroFrame.GetAbsoluteVelocity(this.m_rb), Manager.Instance.QuadController.RayDirections[3]);
        // this.ForceYToSend = -gainYPos * 1 / (this.SecuredDistances[1] * this.SecuredDistances[1]) + gainYNeg * 1 / (this.SecuredDistances[3] * this.SecuredDistances[3]);
        // this.ForceXToSend = -gainXPos * 1 / (this.SecuredDistances[0] ) + gainXNeg * 1 / (this.SecuredDistances[2]);
        // this.ForceYToSend = -gainYPos * 1 / (this.SecuredDistances[1] ) + gainYNeg * 1 / (this.SecuredDistances[3]);
    }
    public override void Actuate(float elapsed)
    {
        this.profileX = this.profileLat;
        this.profileY = this.profileLongi;
        UInt16[] profile1 = this.profileX;
        UInt16[] profileXNeg = this.profileX;
        UInt16[] profile2 = this.profileY;
        // Debug.LogWarning("Actuate from CoulombHaptic");
        // Debug.Log(this.forceX);
        // Debug.Log(this.forceY);
        // BrunnerHandle.Instance.WriteForceProfile(this.forceX, this.forceY);
        // Debug.Log(this.Distances[0] + " m, " + this.Distances[1] + " m, " + this.Distances[2] + " m, " + this.Distances[3] + " m / " 
        //     + this.forceX + " N, " + this.forceY + " N"
        // );
        BrunnerHandle.Instance.WriteTrimPositionXY(this.ForceXToSend, this.ForceYToSend);
        Logger.Instance.AddEntry(this.ForceXToSendLoggerIdx, this.ForceXToSend);
        Logger.Instance.AddEntry(this.ForceYToSendLoggerIdx, this.ForceYToSend);

        // Tentative par force-displacement profile
        // for (int i = Mathf.Abs(Mathf.FloorToInt(this.MeasuredX/12.5f)); i < this.profileX.Length; i++) {
        //     Debug.Log(i);
        //     profile1[i] = (UInt16)Mathf.Max(Mathf.FloorToInt(this.profileX[i] + this.forceX), 0.0f);
        //     profile1[i] = (UInt16)Mathf.FloorToInt(this.profileX[i] + Mathf.Abs(this.forceX));
        //     Debug.Log((UInt16)Mathf.Max(Mathf.FloorToInt(profileX[i] + this.forceX), 0.0f));
        // }
        // // Debug.Log(profile1[0] + ", " + profile1[1] + ", " + profile1[2] + ", " + profile1[3] + ", " + profile1[4] + ", " + profile1[5] + ", " + profile1[6] + ", " + profile1[7] + ", " + profile1[8]);
        // BrunnerHandle.Instance.WriteForceProfile(this.profileLat[0], this.profileLat[1], this.profileLat[2], this.profileLat[3], this.profileLat[4], this.profileLat[5], this.profileLat[6], this.profileLat[7], this.profileLat[8], 
        //     profile1[0], profile1[1], profile1[2], profile1[3], profile1[4], profile1[5], profile1[6], profile1[7], profile1[8]);

        // Tentative par force scale factor
        // BrunnerHandle.Instance.WriteForceScaleFactor((UInt16)Mathf.FloorToInt(Mathf.Abs(this.forceX * )),(UInt16)Mathf.FloorToInt(Mathf.Abs(this.forceX)));
        
        // Mesure de réponse indicielle en consigne de position
        // if (!this.sent && elapsed > 5.0f) {
        //     BrunnerHandle.Instance.WriteTrimPositionXY(-50.0f,0.0f);
        //     Debug.LogWarning(elapsed);
        //     this.sent = true;
        // }       
        // Debug.Log("sent: "+ this.sent + " - elapsed = " + (elapsed > 5.0f));
        // Debug.Log(elapsed + ": x = " + (this.MeasuredX) + "; y = " + (-this.MeasuredY));

        // TODO : Mesure de réponse indicielle en consigne de raideur
        // if (!this.sent && elapsed > 5.0f) {
        //     this.profileLongi = new UInt16[9] {10, 100, 200, 300, 400, 500, 600, 700, 800};
        //     BrunnerHandle.Instance.WriteForceProfile(this.profileLat[0], this.profileLat[1], this.profileLat[2], this.profileLat[3], this.profileLat[4], this.profileLat[5], this.profileLat[6], this.profileLat[7], this.profileLat[8], 
        //         this.profileLongi[0], this.profileLongi[1], this.profileLongi[2], this.profileLongi[3], this.profileLongi[4], this.profileLongi[5], this.profileLongi[6], this.profileLongi[7], this.profileLongi[8]);
        //     // Debug.LogWarning(elapsed);
        //     this.sent = true;
        // }
        // Debug.Log(elapsed + ";" + (BrunnerHandle.Instance.GetForceLongitudinal()) + ";" + (-BrunnerHandle.Instance.GetForceLateral()) + ";" + (BrunnerHandle.Instance.GetForceLongitudinal()) + ";" + (-BrunnerHandle.Instance.GetForceLateral()));
        Logger.Instance.AddEntry(base.ForceProfileLat0LoggerIdx, this.profileLat[0].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat1LoggerIdx, this.profileLat[1].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat2LoggerIdx, this.profileLat[2].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat3LoggerIdx, this.profileLat[3].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat4LoggerIdx, this.profileLat[4].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat5LoggerIdx, this.profileLat[5].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat6LoggerIdx, this.profileLat[6].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat7LoggerIdx, this.profileLat[7].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLat8LoggerIdx, this.profileLat[8].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi0LoggerIdx, this.profileLongi[0].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi1LoggerIdx, this.profileLongi[1].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi2LoggerIdx, this.profileLongi[2].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi3LoggerIdx, this.profileLongi[3].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi4LoggerIdx, this.profileLongi[4].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi5LoggerIdx, this.profileLongi[5].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi6LoggerIdx, this.profileLongi[6].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi7LoggerIdx, this.profileLongi[7].ToString("#"));
        Logger.Instance.AddEntry(base.ForceProfileLongi8LoggerIdx, this.profileLongi[8].ToString("#"));
    }
}