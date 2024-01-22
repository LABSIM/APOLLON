using System.IO;
using UnityEngine;

public static class Constants
{
    // General config file path
    public const string streamingAssetsPath = @"Assets/StreamingAssets/AIRWISE/";
    public const string ConfigFile = @"ConfigFile.json";

    public const string ConfigFilePath = Constants.streamingAssetsPath + Constants.ConfigFile;
    public const string ForcingFunctionConfigFile = @"ForcingFunctionConfig.json";
    public const string MappingConfigFile = @"MappingConfig.json";
    public const string ControlConfigFile = @"ControlConfig.json";
    public const string ActuationConfigFile = @"ActuationConfig.json";
    public const string HapticConfigFile = @"HapticConfig.json";
    public const string ErrorDisplayConfigFile = @"ErrorDisplayConfig.json";

    // Simulation constants
    /* 
     * 10% error on estimations
     */
    public const float err = 0.00f;
    /* 
     * 5% dead zone on joystick
     */
    public const float epsilon = 5.0f;

    /*
     * Directly contol and actuate force and torque at CoM
     */
    //public const int directMode = 0;

    // Mapping constants
    /*
     * Map sidestick output to desired position expressed in inertial frame
     */
    //public const int position = 1;
    /*
     * Map sidestick output to desired altitude expressed in inertial frame + attitude expressed in body frame
     */
    //public const int altitudeAttitude = 2;
    /*
     * Map sidestick output to desired altitude expressed in inertial frame + velocity expressed in inertial frame
     */
    //public const int altitudeTranslationalVelocity = 3;

    // Filter constants
    /*
     * Apply pure gain filter on sidestick output
     */
    public const int gain = 0;
    /*
     * Apply first-order filter on sidestick output
     */
    public const int firstOrder = 1;
    /*
     * Apply second-order filter on sidestick output
     */
    public const int secondOrder = 2;

    // Control constants
    /*
     * Define position and attitude to apply without considering any objective
     */
    //public const int openLoop = 1;
    /*
     * Control altitude + attitude + yaw - position and velocity follow
     */
    //public const int altitudeAttitudeYaw = 2;
    /*
     * Control altitude + translational velocity - attitude and yaw follow
     */
    //public const int altitudeTranslationalVelocity = 3;
    /*
     * TODO: Control altitude + position - attitude and yaw follow
     */
    public const int positionYaw = 4;

    // Actuation constants
    /*
     * Actuate 4 rotors of balanced X4-flyer in thrust
     */
    public const int X4Mode = 1;

    // Haptic constants
    /*
     * Trim position based on current velocity
     */
    public const int velocityTrim = 1;

    // Vector constants
    public static readonly Vector3 e1 = new Vector3(1.0f, 0.0f, 0.0f);
    public static readonly Vector3 e2 = new Vector3(0.0f, 1.0f, 0.0f);
    public static readonly Vector3 e3 = new Vector3(0.0f, 0.0f, 1.0f);

    // Unity from/to aero frame change constants
    public static readonly Matrix4x4 mIndirectDirect = new Matrix4x4(
        new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
        new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
        new Vector4(0.0f, 0.0f, -1.0f, 0.0f),
        new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
    public static readonly Matrix4x4 mDirectAero = new Matrix4x4(
        new Vector4(0.0f, 0.0f, -1.0f, 0.0f),
        new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
        new Vector4(0.0f, -1.0f, 0.0f, 0.0f),
        new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
    public static readonly Matrix4x4 mUnityAero = mIndirectDirect * mDirectAero;
    public static readonly Matrix4x4 mAeroUnity = mUnityAero.transpose;
}

public static class Parameters
{
    public const float DistMax = 1000.0f;
    public const float VMax = 10.0f;

    public const float JoystickToPosition = 0.2f;
}

// Define Utilities static methods
public static class Utilities
{
    // Read configuration from provided path and return one corresponding to provided type
    public static T Read<T>(string filePath)
    {
        // UnityEngine.Debug.Log(typeof(T).ToString());
        string text = File.ReadAllText(filePath);
        T res = JsonUtility.FromJson<T>(text);
        return res;
    }

    // Return norm of provided Vector3
    public static float Norm(Vector3 v)
    {
        return Mathf.Sqrt(Mathf.Pow(v.x,2) + Mathf.Pow(v.y,2) + Mathf.Pow(v.z,2));
    }

    // Return distance between two Vector3
    public static float Distance(Vector3 v1, Vector3 v2)
    {
        return Utilities.Norm(v1 - v2);
    }

    // Return scalar product between two Vector3
    public static float ScalarProduct(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    // Return rotation matrix based on provided ZYX Euler-Cardan angles
    public static Matrix4x4 ZYXToMatrix(float phi, float theta, float psi)
    {
        Matrix4x4 RPsi = Matrix4x4.Rotate(Quaternion.Euler(0, 0, psi));
        Matrix4x4 RTheta = Matrix4x4.Rotate(Quaternion.Euler(0, theta, 0));
        Matrix4x4 RPhi = Matrix4x4.Rotate(Quaternion.Euler(phi, 0, 0));

        return RPsi * RTheta * RPhi;
    }

    // Return rotation matrix based on provided ZYX Euler-Cardan angle vector
    public static Matrix4x4 ZYXToMatrix(Vector3 angles)
    {
        return ZYXToMatrix(angles.x, angles.y, angles.z);
    }

    // Return rotation matrix based on provided ZXY Tait-Bryan angles
    public static Matrix4x4 ZXYToMatrix(float alpha, float beta, float gamma)
    {
        Matrix4x4 RGamma = Matrix4x4.Rotate(Quaternion.Euler(0, 0, gamma));
        Matrix4x4 RAlpha = Matrix4x4.Rotate(Quaternion.Euler(alpha, 0, 0));
        Matrix4x4 RBeta = Matrix4x4.Rotate(Quaternion.Euler(0, beta, 0));

        return RGamma * RAlpha * RBeta;
    }

    // Return rotation matrix based on provided ZXY Tait-Bryan angle vector
    public static Matrix4x4 ZXYToMatrix(Vector3 angles)
    {
        return ZXYToMatrix(angles.x, angles.y, angles.z);
    }

    // Return Quaternion based on provided rotation matrix (independant of expression frame)
    public static Quaternion MatrixToQuaternion(Matrix4x4 mRotation)
    {
        return Quaternion.LookRotation(mRotation.GetColumn(2), mRotation.GetColumn(1));
    }

    // Convert Vector3 in Unity frame to Aero frame
    public static Vector3 ToAeroFromUnityFrame(Vector3 aeroVector)
    {
        return Constants.mAeroUnity * aeroVector;
    }

    // Convert Vector3 in Aero frame to Unity frame
    public static Vector3 ToUnityFromAeroFrame(Vector3 unityVector)
    {
        return Constants.mUnityAero * unityVector;
    }

    public static float FromTimeSpanToFloatElapsed(System.TimeSpan elapsed)
    {
        return elapsed.Minutes * 60 + elapsed.Seconds + elapsed.Milliseconds / 1000.0f;
    }
}

// Define static methods w.r.t. Unity frame
public static class UnityFrame
{
    // Project Vector3 in horizontal plane
    public static Vector3 ProjectOnHorizontalplane(Vector3 v)
    {
        return v - v.y * Constants.e2;
    }

    // Set current RigidBody in provided position expressed in Unity frame
    public static void SetPosition(Rigidbody rb, Vector3 position)
    {
        rb.MovePosition(position);
    }
    // Increment current RigidBody by provided position vector expressed in Unity frame
    public static void IncrementPosition(Rigidbody rb, Vector3 positionVector)
    {
        rb.position += positionVector;
    }

    // Return current RigidBody position expressed in Unity frame
    public static Vector3 GetPosition(Rigidbody rb)
    {
        return rb.position;
    }

    // Set current RigidBody in provided rotation matrix expressed in Unity frame
    public static void SetRotation(Rigidbody rb, Matrix4x4 mRotation)
    {
        rb.MoveRotation(Utilities.MatrixToQuaternion(mRotation));
    }

    // Set current RigidBody as provided angles expressed in Unity frame using Tait-Bryan ZXY angles
    public static void SetAngles(Rigidbody rb, Vector3 angles)
    {
        Matrix4x4 mRotation = Utilities.ZXYToMatrix(angles);
        SetRotation(rb, mRotation);
    }

    // Return rotation matrix of current RigidBody expressed in aero frame
    public static Matrix4x4 GetRotationMatrix(Rigidbody rb)
    {
        return Utilities.ZXYToMatrix(rb.rotation.eulerAngles);
    }

    // Return current RigidBody Tait-Bryan ZXY angles expressed in Unity frame
    public static Vector3 GetAngles(Rigidbody rb)
    {
        return rb.rotation.eulerAngles;
    }

    // Set current RigidBody to provided absolute velocity expressed in Unity frame
    public static void SetAbsoluteVelocity(Rigidbody rb, Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    // Return current RigidBody absolute velocity expressed in Unity frame
    public static Vector3 GetAbsoluteVelocity(Rigidbody rb)
    {
        return rb.velocity;
    }

    // Set current RigidBody to provided relative velocity expressed in Unity frame
    public static void SetRelativeVelocity(Rigidbody rb, Vector3 velocity)
    {
        UnityFrame.SetAbsoluteVelocity(rb, UnityFrame.GetRotationMatrix(rb) * velocity);
    }

    // Return current RigidBody relative velocity expressed in Unity frame
    public static Vector3 GetRelativeVelocity(Rigidbody rb)
    {
        return UnityFrame.GetRotationMatrix(rb).transpose * UnityFrame.GetAbsoluteVelocity(rb);
    }

    // Set current RigidBody to provided angular velocity expressed in Unity frame
    public static void SetAngularVelocity(Rigidbody rb, Vector3 angularVelocity)
    {
        rb.angularVelocity = angularVelocity;
    }

    // Return current RigidBody angular velocity expressed in Unity frame
    public static Vector3 GetAngularVelocity(Rigidbody rb)
    {
        return rb.angularVelocity;
    }

    // Return current Rigidbody propLever
    public static float GetPropLever(Rigidbody rb)
    {
        return rb.mass * (1.0f + Random.Range(-Constants.err, Constants.err));
    }

    // Return gravity vector expressed in Unity frame when applies to current Rigidbody and empty Vector4 otherwise
    public static Vector3 GetGravity(Rigidbody rb)
    {
        if (rb.useGravity)
        {
            return Physics.gravity;
        }
        else
        {
            return new Vector4();
        }
    }

    // Return current Rigidbody mass
    public static float GetMass(Rigidbody rb)
    {
        return rb.mass * (1.0f + Random.Range(-Constants.err, Constants.err));
    }

    // Return current Rigidbody inertia tensor in Unity frame
    public static Matrix4x4 GetInertiaTensor(Rigidbody rb)
    {
        Vector3 I = rb.inertiaTensor;
        return new Matrix4x4(
            new Vector4(I[0] * (1.0f + Random.Range(-Constants.err, Constants.err)), 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, I[1] * (1.0f + Random.Range(-Constants.err, Constants.err)), 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, I[2] * (1.0f + Random.Range(-Constants.err, Constants.err)), 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
    }

    // Apply to current Rigidbody provided force expressed in Unity frame
    public static void ApplyRelativeForce(Rigidbody rb, Vector3 force)
    {
        rb.AddRelativeForce(force);
    }

    // Apply to current Rigidbody provided torque expressed in Unity frame
    public static void ApplyRelativeTorque(Rigidbody rb, Vector3 torque)
    {
        rb.AddRelativeTorque(torque);
    }

    // Apply to current Rigidbody provided force and torque expressed in Unity frame
    public static void ApplyRelativeForceAndTorque(Rigidbody rb, Vector3 force, Vector3 torque)
    {
        ApplyRelativeForce(rb, force);
        ApplyRelativeTorque(rb, torque);
    }
}

// Define static methods w.r.t. aero frame
public static class AeroFrame
{
    // Project Vector3 in horizontal plane
    public static Vector3 ProjectOnHorizontalplane(Vector3 v)
    {
        return v - v.z * Constants.e3;
    }

    // Set current RigidBody in provided position expressed in aero frame
    public static void SetPosition(Rigidbody rb, Vector3 position)
    {
        UnityFrame.SetPosition(rb, Constants.mUnityAero * position);
    }
    // Increment current RigidBody by provided position vector expressed in aero frame
    public static void IncrementPosition(Rigidbody rb, Vector3 positionVector)
    {
        UnityFrame.IncrementPosition(rb, Constants.mUnityAero * positionVector);
    }

    // Return current RigidBody position expressed in aero frame
    public static Vector3 GetPosition(Rigidbody rb)
    {
        return Constants.mAeroUnity * UnityFrame.GetPosition(rb);
    }

    // Set current RigidBody in provided rotation matrix expressed in aero frame
    public static void SetRotation(Rigidbody rb, Matrix4x4 mRotation)
    {
        UnityFrame.SetRotation(rb, Constants.mUnityAero * mRotation * Constants.mAeroUnity);
    }

    // Set current RigidBody as provided angles expressed in aero frame using Euler-Cardan ZYX angles
    public static void SetAngles(Rigidbody rb, Vector3 angles)
    {
        Matrix4x4 mRotation = Utilities.ZYXToMatrix(angles);
        SetRotation(rb, mRotation);
    }

    // Return rotation matrix of current RigidBody expressed in aero frame
    public static Matrix4x4 GetRotationMatrix(Rigidbody rb)
    {
        return Constants.mAeroUnity * Matrix4x4.Rotate(rb.rotation) * Constants.mUnityAero;
    }

    // Return current RigidBody Euler-Cardan ZYX angles expressed in aero frame
    //TODO: explicit pitch angle not in [-pi/2; pi/2]
    public static Vector3 GetAngles(Rigidbody rb)
    {
        Matrix4x4 R = GetRotationMatrix(rb);
        Vector3 res = new Vector3();
        if (R[2, 0] != 1 && R[2, 0] != -1)
        {
            res[1] = Mathf.Asin(-R[2, 0]) * Mathf.Rad2Deg;
            res[0] = Mathf.Atan2(R[2, 1], R[2, 2]) * Mathf.Rad2Deg;
            res[2] = Mathf.Atan2(R[1, 0], R[0, 0]) * Mathf.Rad2Deg;

        }
        else if (R[2, 0] == -1)
        {
            res[1] = Mathf.PI / 2 * Mathf.Rad2Deg;
            res[2] = 0 * Mathf.Rad2Deg;
            res[0] = (Mathf.Atan2(R[0, 1], R[0, 2]) + res[2]) * Mathf.Rad2Deg;
        }
        else if (R[2, 0] == 1)
        {
            res[1] = -Mathf.PI / 2 * Mathf.Rad2Deg;
            res[2] = 0 * Mathf.Rad2Deg;
            res[0] = (Mathf.Atan2(-R[0, 1], -R[0, 2]) + res[2]) * Mathf.Rad2Deg;
        }
        return res;
    }

    // Set current RigidBody to provided absolute velocity expressed in aero frame
    public static void SetAbsoluteVelocity(Rigidbody rb, Vector3 velocity)
    {
        UnityFrame.SetAbsoluteVelocity(rb, Constants.mUnityAero * velocity);
    }

    // Return current RigidBody absolute velocity expressed in aero frame
    public static Vector3 GetAbsoluteVelocity(Rigidbody rb)
    {
        return Constants.mAeroUnity * UnityFrame.GetAbsoluteVelocity(rb);
    }

    // Set current RigidBody to provided relative velocity expressed in aero frame
    public static void SetRelativeVelocity(Rigidbody rb, Vector3 velocity)
    {
        SetAbsoluteVelocity(rb, AeroFrame.GetRotationMatrix(rb) * velocity);
    }

    // Return current RigidBody relative velocity expressed in aero frame
    public static Vector3 GetRelativeVelocity(Rigidbody rb)
    {
        return AeroFrame.GetRotationMatrix(rb).transpose * GetAbsoluteVelocity(rb);
    }

    // Set current RigidBody to provided angular velocity expressed in aero frame 
    public static void SetAngularVelocity(Rigidbody rb, Vector3 angularVelocity)
    {
        UnityFrame.SetAngularVelocity(rb, Constants.mUnityAero * -angularVelocity);
    }

    // Return current RigidBody angular velocity expressed in aero frame
    public static Vector3 GetAngularVelocity(Rigidbody rb)
    {
        return Constants.mAeroUnity * -UnityFrame.GetAngularVelocity(rb);
    }

    // Return gravity vector expressed in aero frame when applies to current Rigidbody and empty Vector4 otherwise
    public static Vector3 GetGravity(Rigidbody rb)
    {
        return Constants.mAeroUnity * UnityFrame.GetGravity(rb);
    }

    // Return current Rigidbody mass
    public static float GetMass(Rigidbody rb)
    {
        return UnityFrame.GetMass(rb);
    }

    // Return current Rigidbody inertia tensor in aero frame
    public static Matrix4x4 GetInertiaTensor(Rigidbody rb)
    {
        return Constants.mAeroUnity * UnityFrame.GetInertiaTensor(rb) * Constants.mUnityAero;
    }

    // Apply to current Rigidbody provided force expressed in aero frame
    public static void ApplyRelativeForce(Rigidbody rb, Vector3 force)
    {
        UnityFrame.ApplyRelativeForce(rb, Constants.mUnityAero * force);
    }

    // Apply to current Rigidbody provided torque expressed in aero frame
    public static void ApplyRelativeTorque(Rigidbody rb, Vector3 torque)
    {
        UnityFrame.ApplyRelativeTorque(rb, UnityFrame.GetRotationMatrix(rb).transpose * Constants.mUnityAero * AeroFrame.GetRotationMatrix(rb) * -torque);
    }

    // Apply to current Rigidbody provided force and torque expressed in Unity frame
    public static void ApplyRelativeForceAndTorque(Rigidbody rb, Vector3 force, Vector3 torque)
    {
        ApplyRelativeForce(rb, force);
        ApplyRelativeTorque(rb, torque);
    }

    public static float ComputePhi(Rigidbody rb)
    {
        return Utilities.ScalarProduct(Constants.e1, AeroFrame.GetAbsoluteVelocity(rb));
    }

    public static float ComputePhiDot(Rigidbody rb)
    {
        //TODO: estimer phiDot
        return 0.0f;
    }
}