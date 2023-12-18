using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteeringDynamics : MonoBehaviour
{
    private Rigidbody rb;

    // Constant parameters
    private float b, kG, c1, c2, kO, c3, c4;
    private float errD;

    // Parameters to depend on trajectory portion
    public float xG, yG;
    public float[] xO, yO;
    //private float t0, tf;
    //private Vector4 X0;
    private float vNorm;

    // Solver members
    public RungeKutta_Explicite m_RK;
    private static int iMax;
    private int iCurr;
    public Dictionary<float, Vector4> solverRes = new Dictionary<float, Vector4>();

    public void Configure(float b, float kG, float c1, float c2, float kO, float c3, float c4, Rigidbody rb, int order, float tSolver, float dt, int iMax)
    {
        this.b = b;
        this.kG = kG;
        this.c1 = c1;
        this.c2 = c2;
        this.kO = kO;
        this.c3 = c3;
        this.c4 = c4;
        this.rb = rb;

        SteeringDynamics.iMax = iMax;
        this.iCurr = iMax;

        this.m_RK = new RungeKutta_Explicite(order, this.FajenDynamics, dt, tSolver);

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.BKey, this.b);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.KGKey, this.kG);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.C1Key, this.c1);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.C2Key, this.c2);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.KOKey, this.kO);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.C3Key, this.c3);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.HapticKey, Logger.Utilities.C4Key, this.c4);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.SolverKey, Logger.Utilities.NStepSolverKey, iMax);
    }

    public void SetGoalAndObstacles(float xG, float yG, float[] xO, float[] yO, float errD)
    {
        this.xG = xG;
        this.yG = yG;
        this.xO = xO;
        this.yO = yO;
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.TrajectoryKey, Logger.Utilities.XGKey, this.xG);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.TrajectoryKey, Logger.Utilities.YGKey, this.yG);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.TrajectoryKey, Logger.Utilities.XOKey, this.xO.ToList());
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.TrajectoryKey, Logger.Utilities.YOKey, this.yO.ToList());
    }

    public void Initialize(float t0, float tf, Vector4 X0, float vNorm)
    {
        //this.t0 = t0;
        //this.tf = tf;

        //this.X0 = X0;

        this.m_RK.Initialize(t0, tf, X0);
        this.vNorm = vNorm;
    }

    private void OnEnable()
    {
        if (this.m_RK != null)
        {
            this.Solve();
        } else
        {
            UnityEngine.Debug.Log("");
        }

        // TODO: vérifier si Faen et enregistré via manager, alors s'activer
        // Sinon, penser à se désactiver
    }

    private void onDisable() {
        // TODO
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UnityEngine.Debug.Log(iCurr);
        if (this.iCurr <= 0)
        {
            //TODO: r�cup�rer tElapsed...
            this.ResetSolver(
                Manager.Instance.GetElapsedTime(), 
                new Vector4(AeroFrame.ComputePhi(this.rb), AeroFrame.ComputePhiDot(this.rb), AeroFrame.GetPosition(this.rb).x, AeroFrame.GetPosition(this.rb).y), 
                Utilities.Norm(AeroFrame.GetAbsoluteVelocity(this.rb)));
            this.Solve();
            
            //linePos = new List<Vector3>();
            //lineX = new List<float>();
            //lineY = new List<float>();

            //List<float> dG = new List<float>();
            //List<float> dO = new List<float>();

            //lineX.Add(Y0.z);
            //lineY.Add(Y0.w);

            //dG.Add(Utilities.Distance(new Vector3(this.m_SteeringDynamics.xG, this.m_SteeringDynamics.yG, 0.0f), new Vector3(Y0.z, Y0.w, 0.0f)));
            //dO.Add(Utilities.Distance(new Vector3(this.m_SteeringDynamics.xO, this.m_SteeringDynamics.yO, 0.0f), new Vector3(Y0.z, Y0.w, 0.0f)));


            //Debug.Log("n=" + n + ", t=" + tn + ", phi=" + Y[n].x + ", phiDot=" + Y[n].y + ", x=" + Y[n].z + ", y=" + Y[n].w);
            //lineX.Add(Y[n].z);
            //lineY.Add(Y[n].w);
            //linePos.Add(new Vector3(lineX[n], lineY[n], 0.0f));
            //dG.Add(Mathf.Sqrt(Mathf.Pow((m_RK.xG - lineX[n]), 2) + Mathf.Pow((m_RK.yG - lineY[n]), 2)));
            //dO.Add(Mathf.Sqrt(Mathf.Pow((m_RK.xO - lineX[n]), 2) + Mathf.Pow((m_RK.yO - lineY[n]), 2)));


            this.iCurr = iMax;
        }
        else
        {
            this.iCurr--;
        }

        this.CheckDistanceCriterion();

    }
    private void ResetSolver(float t0, Vector4 X0, float vNorm)
    {
        this.solverRes.Clear();
        this.vNorm = vNorm;
        // Set new initial conditions to solver
        this.m_RK.Reset(t0, X0);
    }

    private void Solve()
    {
        this.m_RK.Solve();
        this.FillResolutionResults();
    }

    private void FillResolutionResults()
    {
        for (int i = 0; i < this.m_RK.t.Count; i ++)
        {
            this.solverRes.Add(this.m_RK.t[i], this.m_RK.Y[i]);
        }
    }

    public Vector4 GetResolutionResultAtTime(float t)
    {
        try
        {
            float tSolver = t - this.m_RK.t0;
            List<float> keys = new List<float>(this.solverRes.Keys);
            float closest = keys.Aggregate((x, y) => Math.Abs(x - tSolver) < Math.Abs(y - tSolver) ? x : y);
            int index = keys.BinarySearch(closest);

            return (this.solverRes[keys[index + 1]] - this.solverRes[keys[index]]) / (keys[index + 1] - keys[index]) * (tSolver - keys[index]) + this.solverRes[keys[index]];
        } 
        catch (ArgumentOutOfRangeException e)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".GetResolutionResultAtTime(): Cannot find trajectory resolution at time " + t + ".");
            return new Vector4();
        }
    }

    public Vector3 GetResolutionPositionAtTime(float t)
    {
        Vector4 res = this.GetResolutionResultAtTime(t);
        return new Vector3(res.z, res.w, AeroFrame.GetPosition(this.rb).z);
    }

    public Vector4 FajenDynamics(float tn, Vector4 X)
    {
        // Fetch current vector-related values
        float phi = X.x;
        float phiDot = X.y;
        float x = X.z;
        float y = X.w;

        // Compute intermediate goal-related values
        float psiG = Mathf.Atan((this.yG - y) / (this.xG - x));
        float dG = Utilities.Distance(new Vector3(this.xG, this.yG, 0.0f), new Vector3(x, y, 0.0f));

        // Compute intermediate obstacle-related values
        float[] psiO = new float[xO.Length];
        float[] dO = new float[xO.Length];
        float[] factorO = new float[xO.Length];
        for (int i = 0; i < this.xO.Length; i++)
        {
            psiO[i]  = Mathf.Atan((this.yO[i] - y) / (this.xO[i] - x));
            dO[i] = Utilities.Distance(new Vector3(this.xO[i], this.yO[i], 0.0f), new Vector3(x, y, 0.0f));
            factorO[i] = this.kO * (phi - psiO[i]) * Mathf.Exp(-this.c3 * Mathf.Abs(phi - psiO[i])) * Mathf.Exp(-this.c4 * dO[i]);
        }
        
        // Compute differential
        float phiDDot = -this.b * phiDot - this.kG * (phi - psiG) * (Mathf.Exp(-this.c1 * dG) + this.c2) + factorO.Sum();
        float xDot = Mathf.Cos(phi) * this.vNorm;
        float yDot = Mathf.Sin(phi) * this.vNorm;

        return new Vector4(phiDot, phiDDot, xDot, yDot);
    }

    private void CheckDistanceCriterion()
    {
        //TODO: update goal if distance < errD
    }
}
