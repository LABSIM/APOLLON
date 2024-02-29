using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class LoggerConfig
{
    public string Path, FallbackPath;
    public string NumSep;
    public string PythonPath;
    public string VarjoVideoPath;
    public string ScriptName;
    public string TextSep;
    public string TableExtension, StructureExtension;
    public bool RunPython;
    public bool CopyVarjoVideo;
    public string VarjoVideoFilenameSep;
    public string VarjoVideoFilenameSep2;
    public LoggerConfig(){}
}

public class Logger
{
    public static string m_rootPath;

    // Top-level log
    private StreamWriter m_writer;
    private string m_topLevelPath, m_trialConfigPath, m_fallbackPath;
    // private List<string> m_topLevelBuffer;

    // Simulation process-related log
    private List<string> m_keys, m_buffer;
    private Dictionary<string, Dictionary<string, List<string>>> m_trialConfigBuffer;
    
    // Simulation process-related members
    private Rigidbody m_rb;

    // Utilities members
    private static string csvExt = ".csv";
    private static string jsonExt = ".json";
    private bool loggerConfigured = false;
    private string m_path, m_trackersFilename, m_trackersPath,  m_pythonPath, m_scriptPath, m_varjoVideoPath;
    private string m_numSep, m_textSep, m_tableExtension = csvExt, m_structureExtension = jsonExt, m_varjoVideoFilenameSep, m_varjoVideoFilenameSep2;
    private DateTime m_timestamp;
    private bool m_runPython, m_copyVarjoVideo;
    private TimeSpan m_elapsed;

    // private bool m_isInitialized = false;

    #region singleton pattern
    private static readonly System.Lazy<Logger> _lazyLogger
        = new System.Lazy<Logger>(
            () =>
            {
                var logger = new Logger();
                return logger;
            }
        );

    public static Logger Instance => _lazyLogger.Value;

    private Logger() { }
    ~Logger()
    {
        this.Dispose();
    }

    private void Init(LoggerConfig config, DateTime timestamp, TimeSpan elapsed, Rigidbody rb)
    {
        UnityEngine.Debug.Log("Logger.Init()");
        // // Bail out early with error is already initialized
        // if (this.m_isInitialized) {
        //     UnityEngine.Debug.LogError(
        //         "<color=red>Error: </color> " + this.GetType() + " should not be initiated when entering initialization method."
        //     );
        //     return;
        // }

        this.m_rb = rb;

        this.loggerConfigured = false;
        this.m_numSep = config.NumSep;
        this.m_pythonPath = config.PythonPath;
        this.m_textSep = config.TextSep;
        this.m_tableExtension = (!string.IsNullOrEmpty(config.TableExtension)) ? config.TableExtension : csvExt;
        this.m_structureExtension = (!string.IsNullOrEmpty(config.StructureExtension)) ? config.StructureExtension : jsonExt;
        this.m_timestamp = timestamp;
        this.CreateTopLevelPath(config.Path);
        this.m_fallbackPath = config.FallbackPath;
        this.m_scriptPath = this.m_path + config.ScriptName;
        this.m_varjoVideoPath = config.VarjoVideoPath;
        this.m_runPython = config.RunPython;
        this.m_copyVarjoVideo = config.CopyVarjoVideo;
        this.m_varjoVideoFilenameSep = config.VarjoVideoFilenameSep;
        this.m_varjoVideoFilenameSep2 = config.VarjoVideoFilenameSep2;
        
        this.m_trialConfigBuffer = new Dictionary<string, Dictionary<string, List<string>>>();
        // this.m_topLevelBuffer = new List<string>();

        // Simulation process-related log
        this.m_keys = new List<string>();
        this.m_buffer = new List<string>();
        UnityEngine.Debug.Log("Logger.Init\t" + this.m_buffer);
        this.m_elapsed = elapsed;
        this.m_buffer.Add(String.Format("{0:00000}.{1:000}", this.m_elapsed.Seconds, this.m_elapsed.Milliseconds));
        UnityEngine.Debug.Log("Logger.Init - suite " + this.m_buffer);

        // this.m_isInitialized = true;
    }

    public void BuildConfig(Type forcingFunction, Type mapping, Type control, Type actuation, Type haptic, Type errorDisplay)
    {   
        UnityEngine.Debug.Log("Logger.BuildConfig()" + Manager.Instance.QuadController.Rb);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.DtKey, Time.fixedDeltaTime);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.ForcingFunctionKey, forcingFunction);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.MappingKey, mapping);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.ControlKey, control);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.ActuationKey, actuation);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.HapticKey, haptic);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.ConfigurationKey, Logger.Utilities.ErrorDisplayKey, errorDisplay);
    }

    public void Initialize()
    {
        UnityEngine.Debug.Log("Logger.Initialize()" + Manager.Instance.QuadController.Rb);
        // Start by creating log path based on current member values 
        this.CreateLogPath();

        // Instantiate log writer when needed
        if (this.m_writer != null) {
            FileStream tempWriter = (FileStream) this.m_writer.BaseStream;
            if (tempWriter == null || tempWriter.Name != this.m_trackersPath) {
                this.m_writer = this.TryOpeningStream(this.m_trackersPath);
            }
        } else {
            this.m_writer = this.TryOpeningStream(this.m_trackersPath);
        }

        // Add headers
        Logger.Instance.SetHeaders();
    }

    public void Dispose()
    {
        UnityEngine.Debug.Log("Logger.Dispose()");
        // Potentially, plot results in Python
        this.PlotInPython();

        // Potentially, copy video files generated by Varjo eye-tracking
        this.CopyVarjoVideo();

        // Flush
        this.Flush();

        // Top-level log
        if (this.m_writer != null) {
            this.m_writer.Close();
        };
    }

    public void Flush()
    {
        UnityEngine.Debug.Log("Logger.Flush()");

        // Flush remaining logger buffer
        this.FlushBuffer();
        this.FlushTrialConfigBuffer();
        // this.FlushTopLevelBuffer();
        // this.Dispose();

    }

    // public void Close() 
    // {



    // }

    #endregion 

    public void Configure(LoggerConfig config, DateTime timestamp, TimeSpan elapsed, Rigidbody rb)
    {
        this.Init(config, timestamp, elapsed, rb);
        this.loggerConfigured = true;
    }
    public string GetPath() { return this.m_path; }
    public string GetNumSep() { return this.m_numSep; }
    public string GetPythonPath() { return this.m_pythonPath; }
    public string GetScriptPath() { return this.m_scriptPath; }
    public string GetTextSep() { return this.m_textSep; }
    public string GetTableExtension() { return this.m_tableExtension; }
    public string GetStructureExtension() { return this.m_structureExtension; }
    public bool GetRunPython() { return this.m_runPython; }
    public string GetVarjoVideoFilenameSep() { return this.m_varjoVideoFilenameSep; }
    public string GetVarjoVideoFilenameSep2() { return this.m_varjoVideoFilenameSep2; }

    public void Reset() {
        UnityEngine.Debug.Log("Logger.Reset()");
        this.FlushBuffer();
        this.FlushTrialConfigBuffer();
        if (this.m_writer != null) {
            this.m_writer.Close();
        }
    }

    private void CreateTopLevelPath(string path)
    {
        // null rootPath means to unset externally (if set externally, consider folder created)
        // N.B.: StreamWriter does not accept disk root path -- falls back to current project top folder
        // N.B.: Folder may be created to protected paths but trials config file cannot directly be saved at protected path -- raises error message
        if (m_rootPath == null) {
            // Hence needs to be defualt-set based on timestamp
            m_rootPath = Path.Combine(path, this.m_timestamp.ToString("yyyyMMddTHHmmss"));
            this.m_topLevelPath = m_rootPath;

            // Create directory when needed
            try
            {
                Directory.CreateDirectory(m_rootPath);
            }
            catch
            {
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> " + this.GetType() + ".CreateTopLevelPath(): Could not create directory at '"
                    + m_rootPath
                    + "."
                );
            }
        }
    }

    private string GenerateSuffixFromTrial() {
        UnityEngine.Debug.Log("loggerConfigured.GenerateSuffixFromTrial()" + Manager.Instance.GetCurrTrial());
        return this.GetTextSep() + "T" + Manager.Instance.GetCurrTrial().ToString("D3");
    }

    private string GenerateTableFilenameFromTrial(string filename)
    {
        return filename + this.GenerateSuffixFromTrial() + this.GetTableExtension();
    }
    private string GenerateStructureFilenameFromTrial(string filename)
    {
        return filename + this.GenerateSuffixFromTrial() + this.GetStructureExtension();
    }

    public void CreateLogPath()
    {
        // Build path
        this.m_path = Path.Combine(m_rootPath, "trackers");
        this.m_trackersFilename = GenerateTableFilenameFromTrial("Thesis_Logger");
        this.m_trackersPath = Path.Combine(this.m_path, this.m_trackersFilename);
        this.m_trialConfigPath = Path.Combine(m_path, GenerateStructureFilenameFromTrial("Thesis_TrialConfig"));

        // Create directory when needed
        try {
            Directory.CreateDirectory(this.m_path);
        } catch {
            UnityEngine.Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".CreateLogPath(): Could not create directory at '"
                + this.m_path
                + "."
            );
        }
    }

    private StreamWriter TryOpeningStream(string path) {
        StreamWriter writer = null;
        try {
            writer = new StreamWriter(path, false);
            if (((FileStream)(writer.BaseStream)).Name != path) {
                UnityEngine.Debug.LogWarning(
                    "<color=orange>Warning: </color> " + this.GetType() + ".CreateLogPath(): Could not create StreamWriter at '"
                    + path
                    + ". Writing in '" + ((FileStream)(writer.BaseStream)).Name + "'."
                );
            }
        } catch (IOException e){
            if (!e.Message.StartsWith("Sharing violation on path")){
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> " + this.GetType() + ".CreateLogPath(): Could not create StreamWriter at '"
                    + path
                    + " -- " + e + "."
                );
            }
        }
        return writer;
    }

    public void SetHeaders() {
        if (this.m_keys[0] != "Time")
        {
            this.m_keys.Insert(0, "Time");
        }
        this.m_buffer = this.m_keys;
        this.FlushBuffer();
    }

    // public void LogInitialConditions(InitialConditions initialConditions)
    // {
    //     this.AddTrialConfigEntry(Utilities.InitialConditionsKey, Utilities.PositionKey, initialConditions.Position0);
    //     this.AddTrialConfigEntry(Utilities.InitialConditionsKey, Utilities.AttitudeKey, initialConditions.Attitude0);
    //     this.AddTrialConfigEntry(Utilities.InitialConditionsKey, Utilities.VelocityKey, initialConditions.Velocity0);
    //     this.AddTrialConfigEntry(Utilities.InitialConditionsKey, Utilities.AngularVelocityKey, initialConditions.AngularVelocity0);
    // }

    public void SaveElapsed(TimeSpan elapsed) {
        this.m_elapsed = elapsed;
        this.m_buffer[0] = String.Format("{0:00000}.{1:000}", this.m_elapsed.Seconds + 60 * this.m_elapsed.Minutes, this.m_elapsed.Milliseconds);
    }

    public int GetEntry(string newKey){
        if (!this.m_keys.Contains(newKey)) {
            this.m_keys.Add(newKey);
            return this.m_keys.Count;
        } else {
            UnityEngine.Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".GetEntry(): '" + newKey + "' key already exists.");
            return -1;
        }
    }

    public void AddEntry(int index, float val)
    {
        AddEntry(index, val.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));
    }

    public void AddEntry(int index, string text) {
       try 
        {
            this.m_buffer[index] = text;
        }
        catch (System.Exception e)
        {
            if (e is KeyNotFoundException)
            {
                UnityEngine.Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".AddEntry(): Cannot find " + index + " key in buffer.");
            }
        }
    }

    public void AddTrialConfigEntry(string entryType, string key, float val)
    {
        AddTrialConfigEntry(entryType, key, new System.Collections.Generic.List<string> { val.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) });
    }

    public void AddTrialConfigEntry(string entryType, string key, List<float> val)
    {
        AddTrialConfigEntry(entryType, key, val.Select(d => d.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)).ToList());
    }

    public void AddTrialConfigEntry(string entryType, string key, List<string> val)
    {
        if (!this.m_trialConfigBuffer.ContainsKey(entryType))
        {
            this.m_trialConfigBuffer.Add(entryType, new Dictionary<string, List<string>>());
        }
        try
        {
            this.m_trialConfigBuffer[entryType].Add(key, val);
        }
        catch (System.Exception e)
        {
            if (e is System.ArgumentException)
            {
                UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".AddEntry(): " + key + " key already in buffer under " + entryType + " entry.");
            }
            else
            {
                UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".AddEntry(): Cannot find " + entryType + " key in top-level buffer.");
            }
        }
    }

    public void AddTrialConfigEntry(string entryType, string key, Type val)
    {
        if (!this.m_trialConfigBuffer.ContainsKey(entryType))
        {
            this.m_trialConfigBuffer.Add(entryType, new Dictionary<string, List<string>>());
        }
        try
        {
            this.m_trialConfigBuffer[entryType].Add(key, new List<string> { "\"" + val + "\"" });
        }
        catch (System.Exception e)
        {
            if (e is System.ArgumentException)
            {
                UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".AddEntry(): " + key + " key already in buffer under " + entryType + " entry.");
            }
            else
            {
                UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".AddEntry(): Cannot find " + entryType + " key in top-level buffer.");
            }
        }
    }

    public void AddTrialConfigEntry(string entryType, string key, Vector3 vect)
    {
        AddTrialConfigEntry(entryType, key, "x", vect.x.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));
        AddTrialConfigEntry(entryType, key, "y", vect.y.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));
        AddTrialConfigEntry(entryType, key, "z", vect.z.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));
    }

    public void AddTrialConfigEntry(string entryType, string key, string val, List<float> param)
    {
        AddTrialConfigEntry(entryType, key, val, "[" + String.Join(", ", param.Select(d => d.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)).ToList()) + "]");
    }

    public void AddTrialConfigEntry(string entryType, string key, string val, float param)
    {
        AddTrialConfigEntry(entryType, key, val, param.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));
    }

    public void AddTrialConfigEntry(string entryType, string key, string val, string param)
    {
        if (!this.m_trialConfigBuffer.ContainsKey(entryType))
        {
            this.m_trialConfigBuffer.Add(entryType, new Dictionary<string, List<string>>());
        }
        if (!this.m_trialConfigBuffer[entryType].ContainsKey(key))
        {
            this.m_trialConfigBuffer[entryType].Add(key, new List<string>());
        }
        try
        {
            this.m_trialConfigBuffer[entryType][key].Add("\"" + val + "\": " + param);
        }
        catch (System.Exception e)
        {
            if (e is System.ArgumentException)
            {
                UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".AddEntry(): " + key + " key already in buffer under " + entryType + " entry.");
            }
            else
            {
                UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".AddEntry(): Cannot find " + entryType + " key in trial-level buffer.");
            }
        }
    }

    // public void AddTopLevelEntry(int index, string text)
    // {
    //    try 
    //     {
    //         this.m_topLevelBuffer[index] = text;
    //     }
    //     catch (System.Exception e)
    //     {
    //         if (e is KeyNotFoundException)
    //         {
    //             UnityEngine.Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".AddEntry(): Cannot find " + index + " key in buffer.");
    //         }
    //     }
    // }

    public void FlushBuffer()
    {
        // if (Manager.Instance.DuringTrial()) {
            if (this.m_writer != null) {
                try {
                    if (this.m_buffer[0] != null) {
                        this.m_writer.WriteLine(string.Join(this.m_numSep, this.m_buffer));
                    }
                } catch (ObjectDisposedException e) {
                    FileStream tempWriter = (FileStream) this.m_writer.BaseStream;
                    if (tempWriter != null) {
                        UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".FlushBuffer(): Cannot flush buffer in '" + tempWriter.Name + "', file is already closed -- '" + e + "'.");
                    } else {
                        UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".FlushBuffer(): Cannot flush buffer, file is already closed -- '" + e + "'.");
                    }
                }
            }
            this.ResetBuffer();
        // }
    }

    public void FlushTrialConfigBuffer()
    {
        if (Manager.Instance.DuringTrial()) {
            try {
                File.WriteAllText(this.m_trialConfigPath, this.TrialConfigBufferToJSONString(this.m_trialConfigBuffer));
            } catch {
                UnityEngine.Debug.LogError("<color=red>Error: </color> " + "Could not write in '" + this.m_path + "'. Writing in '" + this.m_fallbackPath + "'.");
                File.WriteAllText(this.m_fallbackPath, this.TrialConfigBufferToJSONString(this.m_trialConfigBuffer));
            }
        }
    }

    // public void FlushTopLevelBuffer()
    // {
    //     if (Manager.Instance.DuringTrial()) {
    //         try {
    //             this.m_writer.WriteLine(string.Join(this.m_numSep, this.m_topLevelBuffer));
    //         } catch (ObjectDisposedException e) {
    //             FileStream tempWriter = (FileStream) this.m_writer.BaseStream;
    //             if (tempWriter != null) {
    //                 UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".FlushBuffer(): Cannot flush top-level buffer in '" + tempWriter.Name + "', file is already closed -- '" + e + "'.");
    //             } else {
    //                 UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> " + this.GetType() + ".FlushBuffer(): Cannot flush top-level buffer, file is already closed -- '" + e + "'.");
    //             }
    //         }
    //         this.ResetTopLevelBuffer();
    //     }
    // }

    private string TrialConfigBufferToJSONString(Dictionary<string, Dictionary<string, List<string>>> buffer)
    {
        string str = "{\n";
        foreach(KeyValuePair<string, Dictionary<string, List<string>>> kv0 in buffer) {
            str += "\t\"" + kv0.Key + "\": {\n";
            foreach(KeyValuePair<string, List<string>> kv1 in kv0.Value) {
                if (kv1.Value.Count == 1)
                {
                    str += "\t\t\"" + kv1.Key + "\": " + string.Join("", kv1.Value) + ",\n";
                    // TODO: remove trailing comma
                } else
                {
                    str += "\t\t\"" + kv1.Key + "\": {\n\t\t\t" + string.Join(",\n\t\t\t", kv1.Value) + "\n\t\t},\n";
                }
            }
            str = str.Remove(str.Length - 2, 1);
            str += "\t},\n";
        }
        str = str.Remove(str.Length - 2, 1);
        str += "}";
        return str;
    }

    private void ResetBuffer()
    {
        this.m_buffer = new List<string>(new string[this.m_keys.Count]);
    }

    // private void ResetTopLevelBuffer()
    // {
    //     this.m_topLevelBuffer = new List<string>();
    // }

    private void PlotInPython()
    {
        if (this.m_runPython)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = this.m_pythonPath;
            start.Arguments = this.m_scriptPath;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;
            start.WorkingDirectory = this.m_path;
            UnityEngine.Debug.Log("<color=blue>Info: </color> " + "Running '" + start.FileName + " " + start.Arguments + "'");
            try {
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        UnityEngine.Debug.Log("<color=blue>Info: </color> " + "Plotting graphs in " + this.m_path + ": " + result);
                    }
                }
            } catch (System.ComponentModel.Win32Exception e) {
                UnityEngine.Debug.LogError("<color=red>Error: </color> " + "Failed to plot results in Python: " + e);
            }
        }
    }

    private void CopyVarjoVideo(){
        if (this.m_copyVarjoVideo) {
            try { 
                foreach (string filePath in Directory.GetFiles(this.m_varjoVideoPath, "*.*", SearchOption.AllDirectories)) {
                    if (filePath.Contains(this.m_timestamp.Year.ToString("D4") + this.GetVarjoVideoFilenameSep()
                        + this.m_timestamp.Month.ToString("D2") + this.GetVarjoVideoFilenameSep()
                        + this.m_timestamp.Day.ToString("D2") + this.GetVarjoVideoFilenameSep2()
                        + this.m_timestamp.Hour.ToString("D2") + this.GetVarjoVideoFilenameSep()
                        + this.m_timestamp.Minute.ToString("D2") + this.GetVarjoVideoFilenameSep()
                        + this.m_timestamp.Second.ToString("D2"))) {
                        File.Copy(filePath, this.GenerateNewVarjoVideoFilePath(filePath), true);
                    }
                }
            } catch
            {
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> " + this.GetType() + ".CopyVarjoVideo(): Could not find Varjo path '"
                    + this.m_varjoVideoPath
                    + "'."
                );
            }
        }
    }

    private string GenerateNewVarjoVideoFilePath(string filename) {
        string bla2 = Path.Combine(Path.GetDirectoryName(filename).Replace(this.m_varjoVideoPath, Path.GetDirectoryName(this.m_trackersPath)),
            Path.GetFileNameWithoutExtension(filename) + this.GenerateSuffixFromTrial() + Path.GetExtension(filename));
        return Path.Combine(Path.GetDirectoryName(filename).Replace(this.m_varjoVideoPath, Path.GetDirectoryName(this.m_trackersPath)),
            Path.GetFileNameWithoutExtension(filename) + this.GenerateSuffixFromTrial() + Path.GetExtension(filename));
    }
    
    public static class Utilities
    {
        public const string ConfigurationKey = "Configuration";
        public const string ForcingFunctionKey = "ForcingFunction";
        public const string DAKey = "dA";
        public const string AKey = "A";
        public const string AverageKey = "Average";
        public const string FKey = "F";
        public const string TauKey = "Tau";
        public const string MappingKey = "Mapping";
        public const string ControlKey = "Control";
        public const string ActuationKey = "Actuation";
        public const string HapticKey = "Haptic";
        public const string BKey = "b";
        public const string KGKey = "kG";
        public const string C1Key = "c1";
        public const string C2Key = "c2";
        public const string KOKey = "kO";
        public const string C3Key = "c3";
        public const string C4Key = "c4";
        public const string TrajectoryKey = "Trajectory";
        public const string XGKey = "xG";
        public const string YGKey = "yG";
        public const string XOKey = "xO";
        public const string YOKey = "yO";
        public const string ErrDKey = "errD";
        public const string SolverKey = "Solver";
        public const string OrderKey = "order";
        public const string TSolverKey = "tSolver";
        public const string DtKey = "dt";
        public const string NStepSolverKey = "nStepSolver";
        public const string ErrorDisplayKey = "ErrorDisplay";

        public const string InitialConditionsKey = "InitialConditions";
        public const string PositionKey = "Position";
        public const string AttitudeKey = "Attitude";
        public const string VelocityKey = "Velocity";
        public const string AngularVelocityKey = "AngularVelocity";
        public const string TrimX0Key = "TrimX0";
        public const string TrimY0Key = "TrimY0";
        public const string SecurityFactorKey = "SecurityFactor";
        public const string ReactionTimeKey = "ReactionTime";
        public const string kXKey = "kX";
        public const string kYKey = "kY";

        public const string DefaultValuesKey = "DefaultValues";
        public const string PositionDesiredKey = "PositionDesired";
        public const string OtherAxisDesiredKey = "OtherAxisDesired";
        public const string AltitudeDesiredKey = "AltitudeDesired";
        public const string AttitudeDesiredKey = "AttitudeDesired";
        public const string VelocityDesiredKey = "VelocityDesired";
        public const string YawDesiredKey = "YawDesired";

        public const string FixedValuesKey = "FixedValues";
    }
}