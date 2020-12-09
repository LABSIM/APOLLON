using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;


namespace UXF
{
	/// <summary>
	/// Component that handles collecting all Debug.Log calls
	/// </summary>
	public class SessionLogger : MonoBehaviour
	{	
		private Session session;
		private FileIOManager fileIOManager;
		private DataTable table;

        void Awake()
		{
			AttachReferences(
				newFileIOManager: GetComponent<FileIOManager>(),
				newSession: GetComponent<Session>()
			);
			Initialise();
		}

        /// <summary>
        /// Provide references to other components 
        /// </summary>
        /// <param name="newFileIOManager"></param>
        /// <param name="newSession"></param>
        public void AttachReferences(FileIOManager newFileIOManager = null, Session newSession = null)
        {
            if (newFileIOManager != null) fileIOManager = newFileIOManager;
            if (newSession != null) session = newSession;
        }

		/// <summary>
		/// Initialises the session logger, creating the internal data structures, and attaching its logging method to handle Debug.Log messages 
		/// </summary>
		public void Initialise()
		{
			table = new DataTable();
			table.Columns.Add(
				new DataColumn("timestamp", typeof(string))
			);
            table.Columns.Add(
                new DataColumn("log_type", typeof(string))
            );
            table.Columns.Add(
                new DataColumn("message", typeof(string))
            );

            Application.logMessageReceivedThreaded += HandleLog;
			session.cleanUp += Finalise; // finalise logger when cleaning up the session

		}
        
        void HandleLog(string logString, string stackTrace, LogType type)
		{
            // instantiate a row
			DataRow row = table.NewRow();

            row["timestamp"] = System.DateTime.Now.ToString("HH:mm:ss.ffffff");
            row["log_type"] = type.ToString();
            row["message"] = logString.Replace(",", string.Empty);
            
            // finally
			table.Rows.Add(row);

        } /* HandleLog() */

        /// <summary>
        /// Finalises the session logger, saving the data and detaching its logging method from handling Debug.Log messages  
        /// </summary>
        public void Finalise()
		{

            WriteFileInfo fileInfo = new WriteFileInfo(
                WriteFileType.Log,
                session.BasePath,
                session.experimentName,
                session.ppid,
                session.FolderName,
                "log.csv"
                );

			fileIOManager.ManageInWorker(() => fileIOManager.WriteCSV(table, fileInfo));
            Application.logMessageReceivedThreaded -= HandleLog;
			session.cleanUp -= Finalise;

        } /* Finalise() */

    }

}