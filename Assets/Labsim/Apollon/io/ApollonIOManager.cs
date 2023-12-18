//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// avoid namespace pollution
namespace Labsim.apollon.io
{
    public sealed class ApollonIOManager
        : ApollonAbstractManager
    {
        #region event handling

        #region global section

        // global event Dictionary 
        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        // the event args class
        public class ApollonFileEventArgs
            : System.EventArgs
        {

            // ctor
            public ApollonFileEventArgs(string filename, string fileext, string filepath)
                : base()
            {
                this.TimeLoaded = System.DateTime.Now;
            }

            // property
            public System.DateTime TimeLoaded { get; protected set; }
            public string Filename { get; protected set; }
            public string Fileext { get; protected set; }
            public string Filepath { get; protected set; }
            public string FullFilename
            {
                get
                {
                    return this.Filepath + "/" + this.Filename + "." + this.Fileext;
                }
            }

        } /* ApollonFileEventArgs() */

        #endregion

        #region input section

        // event List

        private readonly System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>
            _eventInputFileLoadedList   = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>(),
            _eventInputFileSavedList    = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>(),
            _eventInputFileCreatedList  = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>();

        // the actual events

        public event System.EventHandler<ApollonFileEventArgs> InputFileLoadedEvent
        {
            add
            {
                this._eventInputFileLoadedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["InputFileLoaded"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileLoaded"] + value;
                }
            }

            remove
            {
                if (!this._eventInputFileLoadedList.Contains(value))
                {
                    return;
                }
                this._eventInputFileLoadedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["InputFileLoaded"] = null;
                    foreach (var eventInputFileLoaded in this._eventInputFileLoadedList)
                    {
                        this._eventTable["InputFileLoaded"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileLoaded"] + eventInputFileLoaded;
                    }
                }
            }

        } /* InputFileLoadedEvent */

        public event System.EventHandler<ApollonFileEventArgs> InputFileSavedEvent
        {
            add
            {
                this._eventInputFileSavedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["InputFileSaved"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileSaved"] + value;
                }
            }

            remove
            {
                if (!this._eventInputFileSavedList.Contains(value))
                {
                    return;
                }
                this._eventInputFileSavedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["InputFileSaved"] = null;
                    foreach (var eventInputFileSaved in this._eventInputFileSavedList)
                    {
                        this._eventTable["InputFileSaved"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileSaved"] + eventInputFileSaved;
                    }
                }
            }

        } /* InputFileSavedEvent */

        public event System.EventHandler<ApollonFileEventArgs> InputFileCreatedEvent
        {
            add
            {
                this._eventInputFileCreatedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["InputFileCreated"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileCreated"] + value;
                }
            }

            remove
            {
                if (!this._eventInputFileCreatedList.Contains(value))
                {
                    return;
                }
                this._eventInputFileCreatedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["InputFileCreated"] = null;
                    foreach (var eventInputFileCreated in this._eventInputFileCreatedList)
                    {
                        this._eventTable["InputFileCreated"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileCreated"] + eventInputFileCreated;
                    }
                }
            }

        } /* InputFileSavedEvent */

        internal void RaiseInputFileLoadedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileLoaded"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.InputFilename,
                        this.InputFileext,
                        this.ExperimentationPath + this.ExperimentationName
                    )
                );
            }
        }

        internal void RaiseInputFileSavedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileSaved"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.InputFilename,
                        this.InputFileext,
                        this.ExperimentationPath + this.ExperimentationName
                    )
                );
            }
        }

        internal void RaiseInputFileCreatedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["InputFileCreated"];                
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.InputFilename,
                        this.InputFileext,
                        this.ExperimentationPath + this.ExperimentationName
                    )
                );
            }
        }

        #endregion

        #region output section

        private readonly System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>
            _eventOutputFileLoadedList  = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>(),
            _eventOutputFileSavedList   = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>(),
            _eventOutputFileCreatedList = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>();

        public event System.EventHandler<ApollonFileEventArgs> OutputFileLoadedEvent
        {
            add
            {
                this._eventOutputFileLoadedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["OutputFileLoaded"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileLoaded"] + value;
                }
            }

            remove
            {
                if (!this._eventOutputFileLoadedList.Contains(value))
                {
                    return;
                }
                this._eventOutputFileLoadedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["OutputFileLoaded"] = null;
                    foreach (var eventOutputFileLoaded in this._eventOutputFileLoadedList)
                    {
                        this._eventTable["OutputFileLoaded"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileLoaded"] + eventOutputFileLoaded;
                    }
                }
            }

        } /* OutputFileLoadedEvent */

        public event System.EventHandler<ApollonFileEventArgs> OutputFileSavedEvent
        {
            add
            {
                this._eventOutputFileSavedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["OutputFileSaved"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileSaved"] + value;
                }
            }

            remove
            {
                if (!this._eventOutputFileSavedList.Contains(value))
                {
                    return;
                }
                this._eventOutputFileSavedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["OutputFileSaved"] = null;
                    foreach (var eventOutputFileSaved in this._eventOutputFileSavedList)
                    {
                        this._eventTable["OutputFileSaved"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileSaved"] + eventOutputFileSaved;
                    }
                }
            }

        } /* OutputFileSavedEvent */

        public event System.EventHandler<ApollonFileEventArgs> OutputFileCreatedEvent
        {
            add
            {
                this._eventOutputFileCreatedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["OutputFileCreated"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileCreated"] + value;
                }
            }

            remove
            {
                if (!this._eventOutputFileCreatedList.Contains(value))
                {
                    return;
                }
                this._eventOutputFileCreatedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["OutputFileCreated"] = null;
                    foreach (var eventOutputFileCreated in this._eventOutputFileCreatedList)
                    {
                        this._eventTable["OutputFileCreated"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileCreated"] + eventOutputFileCreated;
                    }
                }
            }

        } /* OutputFileSavedEvent */

        internal void RaiseOutputFileLoadedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileLoaded"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.OutputFilename,
                        this.OutputFileext,
                        this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName
                    )
                );
            }
        }

        internal void RaiseOutputFileSavedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileSaved"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.OutputFilename,
                        this.OutputFileext,
                        this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName
                    )
                );
            }
        }

        internal void RaiseOutputFileCreatedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["OutputFileCreated"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.OutputFilename,
                        this.OutputFileext,
                        this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName
                    )
                );
            }
        }

        #endregion

        #region record section

        private readonly System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>
            _eventRecordFileLoadedList  = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>(),
            _eventRecordFileSavedList   = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>(),
            _eventRecordFileCreatedList = new System.Collections.Generic.List<System.EventHandler<ApollonFileEventArgs>>();

        public event System.EventHandler<ApollonFileEventArgs> RecordFileLoadedEvent
        {
            add
            {
                this._eventRecordFileLoadedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RecordFileLoaded"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileLoaded"] + value;
                }
            }

            remove
            {
                if (!this._eventRecordFileLoadedList.Contains(value))
                {
                    return;
                }
                this._eventRecordFileLoadedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RecordFileLoaded"] = null;
                    foreach (var eventRecordFileLoaded in this._eventRecordFileLoadedList)
                    {
                        this._eventTable["RecordFileLoaded"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileLoaded"] + eventRecordFileLoaded;
                    }
                }
            }

        } /* RecordFileLoadedEvent */

        public event System.EventHandler<ApollonFileEventArgs> RecordFileSavedEvent
        {
            add
            {
                this._eventRecordFileSavedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RecordFileSaved"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileSaved"] + value;
                }
            }

            remove
            {
                if (!this._eventRecordFileSavedList.Contains(value))
                {
                    return;
                }
                this._eventRecordFileSavedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RecordFileSaved"] = null;
                    foreach (var eventRecordFileSaved in this._eventRecordFileSavedList)
                    {
                        this._eventTable["RecordFileSaved"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileSaved"] + eventRecordFileSaved;
                    }
                }
            }

        } /* RecordFileSavedEvent */

        public event System.EventHandler<ApollonFileEventArgs> RecordFileCreatedEvent
        {
            add
            {
                this._eventRecordFileCreatedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RecordFileCreated"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileCreated"] + value;
                }
            }

            remove
            {
                if (!this._eventRecordFileCreatedList.Contains(value))
                {
                    return;
                }
                this._eventRecordFileCreatedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RecordFileCreated"] = null;
                    foreach (var eventRecordFileCreated in this._eventRecordFileCreatedList)
                    {
                        this._eventTable["RecordFileCreated"] = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileCreated"] + eventRecordFileCreated;
                    }
                }
            }

        } /* RecordFileSavedEvent */

        internal void RaiseRecordFileLoadedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileLoaded"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.RecordFilename,
                        this.RecordFileext,
                        this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName
                    )
                );
            }
        }

        internal void RaiseRecordFileSavedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileSaved"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.RecordFilename,
                        this.RecordFileext,
                        this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName
                    )
                );
            }
        }

        internal void RaiseRecordFileCreatedEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<ApollonFileEventArgs>)this._eventTable["RecordFileCreated"];
                callback?.Invoke(
                    this, 
                    new ApollonFileEventArgs(
                        this.RecordFilename,
                        this.RecordFileext,
                        this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName
                    )
                );
            }
        }

        #endregion

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<ApollonIOManager> _lazyInstance
            = new System.Lazy<ApollonIOManager>(() => new ApollonIOManager());

        // Instance  property
        public static ApollonIOManager Instance { get { return _lazyInstance.Value; } }

        // private ctor
        private ApollonIOManager()
        {
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "InputFileLoaded", null },
                { "InputFileSaved", null },
                { "InputFileCreated", null }
            };

        } /* ApollonIOManager() */

        #endregion

        #region private member / public property

        private file.ApollonInputFileFacade m_input = null;
        public file.ApollonInputFileFacade Input
        {
            get
            {
                if (this.m_input != null)
                {
                    return this.m_input;
                }
                else
                {
                    UnityEngine.Debug.LogError("<color=red>Error: </color> ApollonIOManager::Input -> property get : failed, settings are empty ! data are not loaded...");
                    return null;
                }
            }
            private set
            {
                // warn
                if (this.m_input != null)
                {
                    UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> ApollonIOManager::Input -> property set : override, settings are not empty ! ...");
                }

                // assign
                this.m_input = value;

            }
        }

        private readonly string m_inputFilename = "ApollonInput";
        public string InputFilename
        {
            get
            {
                return this.m_inputFilename;
            }
        }

        private readonly string m_inputFileext = "xml";
        public string InputFileext
        {
            get
            {
                return this.m_inputFileext;
            }
        }

        public string InputFullFilename
        {
            get
            {
                return (
                    this.ExperimentationPath 
                    + this.ExperimentationName 
                    + "/" 
                    + this.InputFilename 
                    + "." 
                    + this.InputFileext
                );
            }
        }

        private file.ApollonOutputFileFacade m_output = null;
        public file.ApollonOutputFileFacade Output
        {
            get
            {
                if (this.m_output != null)
                {
                    return this.m_output;
                }
                else
                {
                    UnityEngine.Debug.LogError("<color=red>Error: </color> ApollonIOManager::Output -> property get : failed, settings are empty ! data are not loaded...");
                    return null;
                }
            }
            private set
            {
                // warn
                if (this.m_output != null)
                {
                    UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> ApollonIOManager::Output -> property set : override, settings are not empty ! ...");
                }

                // assign
                this.m_output = value;

            }
        }

        private readonly string m_outputFilename = "ApollonOutput";
        public string OutputFilename
        {
            get
            {
                return this.m_outputFilename;
            }
        }

        private readonly string m_outputFileext = "xml";
        public string OutputFileext
        {
            get
            {
                return this.m_outputFileext;
            }
        }

        public string OutputFullFilename
        {
            get
            {
                return (
                    this.ExperimentationPath 
                    + this.ExperimentationName 
                    + "/" 
                    + this.RunName
                    + "/"
                    + this.OutputFilename 
                    + "." 
                    + this.OutputFileext
                );
            }
        }

        private file.ApollonRecordFileFacade m_record = null;
        public file.ApollonRecordFileFacade Record
        {
            get
            {
                if (this.m_record != null)
                {
                    return this.m_record;
                }
                else
                {
                    UnityEngine.Debug.LogError("<color=red>Error: </color> ApollonIOManager::Record -> property get : failed, settings are empty ! data are not loaded...");
                    return null;
                }
            }
            private set
            {
                // warn
                if (this.m_record != null)
                {
                    UnityEngine.Debug.LogWarning("<color=orange>Warning: </color> ApollonIOManager::Record -> property set : override, settings are not empty ! ...");
                }

                // assign
                this.m_record = value;

            }
        }

        private readonly string m_recordFilename = "ApollonRecord";
        public string RecordFilename
        {
            get
            {
                return this.m_recordFilename;
            }
        }

        private readonly string m_recordFileext = "xml";
        public string RecordFileext
        {
            get
            {
                return this.m_recordFileext;
            }
        }

        public string RecordFullFilename
        {
            get
            {
                return (
                    this.ExperimentationPath
                    + this.ExperimentationName
                    + "/"
                    + this.RunName
                    + "/"
                    + this.RecordFilename
                    + "."
                    + this.RecordFileext
                );
            }
        }

        private readonly string m_experimentationPath = System.IO.Path.GetFullPath(UnityEngine.Application.dataPath + "/Experimentation/");
        public string ExperimentationPath
        {
            get
            {
                return this.m_experimentationPath;
            }
        }

        private string m_experimentationName = "";
        public string ExperimentationName
        {
            get
            {
                return this.m_experimentationName;
            }
            private set
            {
                this.m_experimentationName = value;
            }
        }

        private string m_runName = "";
        public string RunName
        {
            get
            {
                return this.m_runName;
            }
            private set
            {
                this.m_runName = value;
            }
        }

        #endregion

        #region public method

        public void LoadInput(string experimentName)
        {

            // set
            this.ExperimentationName = experimentName;

            // load 
            this.Input = file.ApollonInputFileController.Load(this.InputFullFilename);

            // raise event
            this.RaiseInputFileLoadedEvent();

        } /* LoadInput() */

        public void SaveInput()
        {

            // check
            if (!System.IO.Directory.Exists(this.ExperimentationPath + this.ExperimentationName))
            {
                System.IO.Directory.CreateDirectory(this.ExperimentationPath + this.ExperimentationName);
            }

            // save 
            file.ApollonInputFileController.Save(this.InputFullFilename, this.Input);

            // raise event
            this.RaiseInputFileSavedEvent();

        } /* SaveInput() */

        public void CreateInput()
        {

            // get a sub experiment dir name
            this.ExperimentationName = "new_experiment_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss");

            // instantiate
            this.Input = file.ApollonInputFileController.Create();

            // raise signal
            this.RaiseInputFileCreatedEvent();

        } /* CreateInput() */

        public void LoadOutput(string experimentName, string runName)
        {

            // set
            this.ExperimentationName = experimentName;
            this.RunName = runName;

            // load 
            this.Output = file.ApollonOutputFileController.Load(this.OutputFullFilename);

            // raise event
            this.RaiseOutputFileLoadedEvent();

        } /* LoadOutput() */

        public void SaveOutput()
        {

            // check
            if (!System.IO.Directory.Exists(this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName))
            {
                System.IO.Directory.CreateDirectory(this.ExperimentationPath + this.ExperimentationName + "/" + this.RunName);
            }

            // save 
            file.ApollonOutputFileController.Save(this.OutputFullFilename, this.Output);

            // raise event
            this.RaiseInputFileSavedEvent();

        } /* SaveOutput() */

        public void CreateOutput()
        {

            // get a sub run dir name
            this.RunName = "new_run_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss");

            // instantiate
            this.Output = file.ApollonOutputFileController.Create();

            // raise signal
            this.RaiseInputFileCreatedEvent();

        } /* CreateOutput() */  

        public System.Collections.Generic.List<string> ListAvailableExperiment()
        {
            return file.ApollonExperimentExplorer.ListAvailable(this.ExperimentationPath);
        }

        #endregion

        #region abstract manager implementation

        public override void onStart(object sender, ApollonEngine.EngineEventArgs arg)
        {

            // check if directories exist
            if (!System.IO.Directory.Exists(this.ExperimentationPath))
            {
                System.IO.Directory.CreateDirectory(this.ExperimentationPath);
            }

        } /* onStart() */

        #endregion

    } /* ApollonIOManager Singleton */

} /* namespace Labsim.apollon.io */