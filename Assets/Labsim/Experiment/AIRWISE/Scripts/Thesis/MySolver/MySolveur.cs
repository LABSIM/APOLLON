using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RungeKutta_Explicite
{
    private SteeringDynamics m_SteeringDynamics;
    public static readonly string m_logPath = "C:\\Users\\yalel\\Documents\\3_Manip\\1_Test data\\5_Unity";

    public int order;
    public float t0, tf;
    private float dt, tSolver;

    private int N;

    // Initial conditions
    public Vector4 X0;

    private Vector4 YnEstNext;
    private Vector4 YnNext;
    private float tnNext;

    public List<Vector4> Y = new List<Vector4>();
    public List<float> t = new List<float>();

    private Func<float, Vector4, Vector4> function;

    public RungeKutta_Explicite(int order, Func<float, Vector4, Vector4> function, float dt, float tSolver)
    {
        this.order = order;
        this.tSolver = tSolver;
        this.dt = dt;
        this.function = function;

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.SolverKey, Logger.Utilities.OrderKey, this.order);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.SolverKey, Logger.Utilities.TSolverKey, this.tSolver);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.SolverKey, Logger.Utilities.DtKey, this.dt);
    }

    public void Initialize(float t0, float tf, Vector4 X0)
    {
        this.t0 = t0;
        this.tf = tf;
        this.X0 = X0;

        this.N = (int) Math.Floor((tf - t0) / dt);
    }

    // Set new initial conditions
    public void Reset(float t0, Vector4 X0)
    {
        this.Initialize(t0, t0 + this.tSolver, X0);
    }

    public void Solve()
    {
        for (int i = 0; i< N; i=i+5)
        {
            //Je résouds mon éuqua diff
            //Toujours décr&menter plutôt qu'incrémenter
        }
        this.Y = new List<Vector4>();
        this.t = new List<float>();

        // Compute first values
        float tn = this.t0;
        Vector4 Yn = this.X0;

        // Save first values
        this.Y.Add(Yn);
        this.t.Add(tn);

        for (int n = 1; n < N + 1; n++)
        {
            // y_{n+1}
            if (order == 1)
            {
                YnNext = RK1(tn, Yn);
            }
            else if (order == 2)
            {
                YnNext = RK2(tn, Yn);
            }
            else if (order == 4)
            {
                YnNext = RK4(tn, Yn);
            }

            // t_{n+1} = t_n + dt
            tnNext = tn + dt;

            // Save all current values
            this.Y.Add(YnNext);
            this.t.Add(tnNext);

            // t_{n+1}
            tn = tnNext;

            // Y_{n+1}
            Yn = YnNext;
        }
    }

    private Vector4 RK1(float tn, Vector4 Yn)
    {
        // y(t_{n+1}) = y(t_n) + (t_{n+1} - t_n) * f(t_n, y(t_n))
        return Yn + dt * this.function(tn, Yn);
    }

    // TODO: fix time scale
    private Vector4 RK2(float tn, Vector4 Yn)
    {
        // \tilde{y_{n+1}} = y_n + dt f(t_n, y(t_n))
        YnEstNext = Yn + dt * this.function(tn, Yn);

        // t_{n+1} = t_n + dt
        tnNext = tn + dt;

        // y(t_{n+1}) = y(t_n) + dt\frac{f(t_n, y(t_n)) + f(t_{n+1}, \tilde{y}(t_{n+1}))}{2}
        return Yn + dt * (this.function(tn, Yn) + this.function(tnNext, YnEstNext) / 2);
    }

    private Vector4 RK4(float tn, Vector4 Yn)
    {
        // k_1 = f(t_n;y_n)
        Vector4 k1 = this.function(tn, Yn);
        // k_2 = f(t_n + \frac{dt}{2}, y_n + dt\frac{k_1}{2})
        Vector4 k2 = this.function(tn + dt / 2, Yn + dt * k1 / 2);
        // k_3 = f(t_n + \frac{dt}{2}, y_n + dt\frac{k_2}{2})
        Vector4 k3 = this.function(tn + dt / 2, Yn + dt * k2 / 2);
        // k_4 = f(t_n + dt, y_n + dt k_3)
        Vector4 k4 = this.function(tn + dt / 2, Yn + dt * k3);

        // y(t_{n+1}) = y(t_n) + \frac{dt}{6} (k_1 + 2 k_2 + 2 k_3 + k_4)
        return Yn + dt / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
    }
}
