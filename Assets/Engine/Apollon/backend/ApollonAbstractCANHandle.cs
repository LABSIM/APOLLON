// using directives 
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractCANHandle
        : ApollonAbstractHandle
    {

        #region CAN members
        
        // Reference to the used VCI device.
        private Ixxat.Vci4.IVciDevice m_VCIDevice;
        
        // Reference to the CAN controller.
        private Ixxat.Vci4.Bal.Can.ICanControl m_CANController;
        
        // Reference to the CAN message communication channel.
        private Ixxat.Vci4.Bal.Can.ICanChannel m_CANChannel;
        
        // Reference to the CAN message scheduler.
        private Ixxat.Vci4.Bal.Can.ICanScheduler m_CANScheduler;
        
        // Reference to the message writer of the CAN message channel.
        private Ixxat.Vci4.Bal.Can.ICanMessageWriter m_CANMessageWriter;
        
        // Reference to the message reader of the CAN message channel.
        private Ixxat.Vci4.Bal.Can.ICanMessageReader m_CANMessageReader;
        
        // Thread that handles the message reception.
        private System.Threading.Thread m_RxThread;
        
        // Quit flag for the receive thread.
        private long m_RxEnd = 0;
        
        // Event that's set if at least one message was received.
        private System.Threading.AutoResetEvent m_RxEvent;

        #endregion

        #region CAN device selection
        
        // Select the first CAN adapter.
        private void SelectDevice()
        {

            // temporary
            Ixxat.Vci4.IVciDeviceManager deviceManager = null;
            Ixxat.Vci4.IVciDeviceList deviceList = null;
            System.Collections.IEnumerator deviceEnum = null;

            // encapsulate
            try {
                
                // Get device manager from VCI server
                deviceManager = Ixxat.Vci4.VciServer.Instance().DeviceManager;

                // Get the list of installed VCI devices
                deviceList = deviceManager.GetDeviceList();
                
                // Get enumerator for the list of devices
                deviceEnum = deviceList.GetEnumerator();
                
                // Get first CAN device
                // #TODO be more specific to select the apropriate device in list
                deviceEnum.MoveNext();
                this.m_VCIDevice = deviceEnum.Current as Ixxat.Vci4.IVciDevice;
                
                // print the bus type, controller type, device name and serial number of first found controller
                Ixxat.Vci4.IVciCtrlInfo info = this.m_VCIDevice.Equipment[0];
                object serialNumberGuid = this.m_VCIDevice.UniqueHardwareId;
                string serialNumberText = ApollonAbstractCANHandle.GetSerialNumberText(ref serialNumberGuid);
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractCANHandle.SelectDevice() : found CAN device [{ BusType: "
                    + info.BusType
                    + " },{ ControllerType: "
                    + info.ControllerType
                    + " },{ Interface: "
                    + this.m_VCIDevice.Description
                    + " },{ Serial_Number: "
                    + serialNumberText
                    + " }]"
                );
                
            } catch (System.Exception ex) {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractCANHandle.SelectDevice() : failed with error ["
                    + ex.Message
                    + "] => "
                    + ex.StackTrace
                );

            } finally {

                // Dispose device manager ; it's no longer needed.
                ApollonAbstractCANHandle.DisposeVciObject(deviceManager);

                // Dispose device list ; it's no longer needed.
                ApollonAbstractCANHandle.DisposeVciObject(deviceList);

                // Dispose device list ; it's no longer needed.
                ApollonAbstractCANHandle.DisposeVciObject(deviceEnum);

            } /* try */

        } /* SelectDevice() */

        #endregion

        #region CAN socket init & setup

        // Opens the specified socket, creates a message channel, initializes
        // and starts the CAN controller.
        private bool InitSocket(System.Byte canNo)
        {

            // temp
            Ixxat.Vci4.Bal.IBalObject bal = null;
            bool bRslt = false;

            // encapsulate
            try
            {

                // Open bus access layer
                bal = this.m_VCIDevice.OpenBusAccessLayer();
                
                // Open a message channel for the CAN controller
                this.m_CANChannel = bal.OpenSocket(canNo, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
                
                // Open the scheduler of the CAN controller
                this.m_CANScheduler = bal.OpenSocket(canNo, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler)) as Ixxat.Vci4.Bal.Can.ICanScheduler;

                // Initialize the message channel
                this.m_CANChannel.Initialize(
                    receiveFifoSize:
                        1024,
                    transmitFifoSize: 
                        128,
                    exclusive:
                        false
                );

                // Get a message reader object
                this.m_CANMessageReader = this.m_CANChannel.GetMessageReader();

                // Initialize message reader
                this.m_CANMessageReader.Threshold = 1;

                // Create and assign the event that's set if at least one message was received.
                this.m_RxEvent = new System.Threading.AutoResetEvent(false);
                this.m_CANMessageReader.AssignEvent(this.m_RxEvent);

                // Get a message wrtier object
                this.m_CANMessageWriter = this.m_CANChannel.GetMessageWriter();

                // Initialize message writer
                this.m_CANMessageWriter.Threshold = 1;

                // Activate the message channel
                this.m_CANChannel.Activate();

                // Open the CAN controller
                this.m_CANController = bal.OpenSocket(canNo, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;

                // Initialize the CAN controller
                this.m_CANController.InitLine(
                    operatingMode:
                        Ixxat.Vci4.Bal.Can.CanOperatingModes.Standard 
                        | Ixxat.Vci4.Bal.Can.CanOperatingModes.Extended
                        | Ixxat.Vci4.Bal.Can.CanOperatingModes.ErrFrame,
                    bitrate:
                        Ixxat.Vci4.Bal.Can.CanBitrate.Cia125KBit
                );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractCANHandle.InitSocket() : setup CAN socket [{ LineStatus: "
                    + this.m_CANController.LineStatus
                    + " }]"
                );

                // Set the acceptance filter for std identifiers
                this.m_CANController.SetAccFilter(
                    select:
                        Ixxat.Vci4.Bal.Can.CanFilter.Std,
                    code:
                        (uint)Ixxat.Vci4.Bal.Can.CanAccCode.All,
                    mask:
                        (uint)Ixxat.Vci4.Bal.Can.CanAccMask.All
                );

                // Set the acceptance filter for ext identifiers
                this.m_CANController.SetAccFilter(
                    select:
                        Ixxat.Vci4.Bal.Can.CanFilter.Ext,
                    code:
                        (uint)Ixxat.Vci4.Bal.Can.CanAccCode.All,
                    mask:
                        (uint)Ixxat.Vci4.Bal.Can.CanAccMask.All
                );

                // Start the CAN controller
                this.m_CANController.StartLine();

                // success
                bRslt = true;

            }
            catch (System.Exception ex)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractCANHandle.InitSocket() : failed with error ["
                    + ex.Message
                    + "] => "
                    + ex.StackTrace
                );
                
                // fail
                bRslt = false;

            }
            finally
            {

                // Dispose bus access layer
                ApollonAbstractCANHandle.DisposeVciObject(bal);

            } /* try */

            // return
            return bRslt;

        } /* InitSocket() */

        #endregion

        #region CAN message transmission/reception

        ///// <summary>
        //// Transmits a CAN message with ID 0x100.
        ///// </summary>
        //static void TransmitData()
        //{
        //    IMessageFactory factory = VciServer.Instance().MsgFactory;
        //    ICanMessage canMsg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));

        //    canMsg.TimeStamp = 0;
        //    canMsg.Identifier = 0x100;
        //    canMsg.FrameType = CanMsgFrameType.Data;
        //    canMsg.DataLength = 8;
        //    canMsg.SelfReceptionRequest = true;  // show this message in the console window

        //    for (Byte i = 0; i < canMsg.DataLength; i++)
        //    {
        //        canMsg[i] = i;
        //    }

        //    // Write the CAN message into the transmit FIFO
        //    mWriter.SendMessage(canMsg);
        //}


        ////************************************************************************
        ///// <summary>
        ///// Print a CAN message
        ///// </summary>
        ///// <param name="canMessage"></param>
        ////************************************************************************
        //static void PrintMessage(ICanMessage canMessage)
        //{
        //    switch (canMessage.FrameType)
        //    {
        //        //
        //        // show data frames
        //        //
        //        case CanMsgFrameType.Data:
        //            {
        //                if (!canMessage.RemoteTransmissionRequest)
        //                {
        //                    Console.Write("\nTime: {0,10}  ID: {1,3:X}  DLC: {2,1}  Data:",
        //                                  canMessage.TimeStamp,
        //                                  canMessage.Identifier,
        //                                  canMessage.DataLength);

        //                    for (int index = 0; index < canMessage.DataLength; index++)
        //                    {
        //                        Console.Write(" {0,2:X}", canMessage[index]);
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Write("\nTime: {0,10}  ID: {1,3:X}  DLC: {2,1}  Remote Frame",
        //                                  canMessage.TimeStamp,
        //                                  canMessage.Identifier,
        //                                  canMessage.DataLength);
        //                }
        //                break;
        //            }

        //        //
        //        // show informational frames
        //        //
        //        case CanMsgFrameType.Info:
        //            {
        //                switch ((CanMsgInfoValue)canMessage[0])
        //                {
        //                    case CanMsgInfoValue.Start:
        //                        Console.Write("\nCAN started...");
        //                        break;
        //                    case CanMsgInfoValue.Stop:
        //                        Console.Write("\nCAN stopped...");
        //                        break;
        //                    case CanMsgInfoValue.Reset:
        //                        Console.Write("\nCAN reseted...");
        //                        break;
        //                }
        //                break;
        //            }

        //        //
        //        // show error frames
        //        //
        //        case CanMsgFrameType.Error:
        //            {
        //                switch ((CanMsgError)canMessage[0])
        //                {
        //                    case CanMsgError.Stuff:
        //                        Console.Write("\nstuff error...");
        //                        break;
        //                    case CanMsgError.Form:
        //                        Console.Write("\nform error...");
        //                        break;
        //                    case CanMsgError.Acknowledge:
        //                        Console.Write("\nacknowledgment error...");
        //                        break;
        //                    case CanMsgError.Bit:
        //                        Console.Write("\nbit error...");
        //                        break;
        //                    case CanMsgError.Fdb:
        //                        Console.Write("\nfast data bit error...");
        //                        break;
        //                    case CanMsgError.Crc:
        //                        Console.Write("\nCRC error...");
        //                        break;
        //                    case CanMsgError.Dlc:
        //                        Console.Write("\nData length error...");
        //                        break;
        //                    case CanMsgError.Other:
        //                        Console.Write("\nother error...");
        //                        break;
        //                }
        //                break;
        //            }
        //    }
        //}

        ////************************************************************************
        ///// <summary>
        ///// Demonstrate reading messages via MsgReader::ReadMessages() function
        ///// </summary>
        ////************************************************************************
        //static void ReadMultipleMsgsViaReadMessages()
        //{
        //    ICanMessage[] msgArray;

        //    do
        //    {
        //        // Wait 100 msec for a message reception
        //        if (mRxEvent.WaitOne(100, false))
        //        {
        //            if (mReader.ReadMessages(out msgArray) > 0)
        //            {
        //                foreach (ICanMessage entry in msgArray)
        //                {
        //                    PrintMessage(entry);
        //                }
        //            }
        //        }
        //    } while (0 == mMustQuit);
        //}

        ////************************************************************************
        ///// <summary>
        ///// Demonstrate reading messages via MsgReader::ReadMessage() function
        ///// </summary>
        ////************************************************************************
        //static void ReadMsgsViaReadMessage()
        //{
        //    ICanMessage canMessage;

        //    do
        //    {
        //        // Wait 100 msec for a message reception
        //        if (mRxEvent.WaitOne(100, false))
        //        {
        //            // read a CAN message from the receive FIFO
        //            while (mReader.ReadMessage(out canMessage))
        //            {
        //                PrintMessage(canMessage);
        //            }
        //        }
        //    } while (0 == mMustQuit);
        //}

        ////************************************************************************
        ///// <summary>
        //// This method is the works as receive thread.
        ///// </summary>
        ////************************************************************************
        //static void ReceiveThreadFunc()
        //{
        //    ReadMsgsViaReadMessage();
        //    //
        //    // alternative: use ReadMultipleMsgsViaReadMessages();
        //    //
        //}


        #endregion

        #region CAN utility methods

        // Returns the UniqueHardwareID GUID number as string which
        // shows the serial number.
        // Note: This function will be obsolete in later version of the VCI.
        // Until VCI Version 3.1.4.1784 there is a bug in the .NET API which
        // returns always the GUID of the interface. In later versions there
        // the serial number itself will be returned by the UniqueHardwareID property.
        private static string GetSerialNumberText(ref object serialNumberGuid)
        {
            // temp
            string resultText;

            // check if the object is really a GUID type
            if (serialNumberGuid.GetType() == typeof(System.Guid))
            {
                // convert the object type to a GUID
                System.Guid tempGuid = (System.Guid)serialNumberGuid;

                // copy the data into a byte array
                byte[] byteArray = tempGuid.ToByteArray();

                // serial numbers starts always with "HW"
                if (((char)byteArray[0] == 'H') && ((char)byteArray[1] == 'W'))
                {

                    // run a loop and add the byte data as char to the result string
                    resultText = "";
                    int i = 0;
                    while (true)
                    {

                        // the string stops with a zero
                        if (byteArray[i] != 0)
                        {

                            resultText += (char)byteArray[i];

                        }
                        else
                        {

                            break;

                        } /* if() */

                        // increment
                        i++;

                        // stop also when all bytes are converted to the string
                        // but this should never happen
                        if (i == byteArray.Length)
                        {

                            break;

                        } /* if() */

                    } /* while() */

                }
                else
                {

                    // if the data did not start with "HW" convert only the GUID to a string
                    resultText = serialNumberGuid.ToString();

                } /* if() */

            }
            else
            {

                // if the data is not a GUID convert it to a string
                string tempString = (string)(string)serialNumberGuid;
                resultText = "";
                for (int i = 0; i < tempString.Length; i++)
                {
                    if (tempString[i] != 0)
                        resultText += tempString[i];
                    else
                        break;
                }

            } /* if() */

            // result
            return resultText;

        } /* GetSerialNumberText() */
       
        // This method tries to dispose the specified object.
        // The VCI interfaces provide access to native driver resources. 
        // Because the .NET garbage collector is only designed to manage memory, 
        // but not native OS and driver resources the application itself is 
        // responsible to release these resources via calling 
        // IDisposable.Dispose() for the obects obtained from the VCI API 
        // when these are no longer needed. 
        // Otherwise native memory and resource leaks may occure.
        private static void DisposeVciObject(object obj)
        {

            if (null != obj)
            {

                System.IDisposable dispose = obj as System.IDisposable;
                if (null != dispose)
                {

                    dispose.Dispose();
                    obj = null;

                } /* if() */

            } /* if() */

        } /* DisposeVciObject() */

        #endregion

        // ctor     
        public ApollonAbstractCANHandle()
            : base()
        { }
        
        protected override void Dispose(bool bDisposing = true)
        {

            //
            // Dispose all hold VCI objects.
            //

            // Dispose message reader
            ApollonAbstractCANHandle.DisposeVciObject(this.m_CANMessageReader);

            // Dispose message writer 
            ApollonAbstractCANHandle.DisposeVciObject(this.m_CANMessageWriter);

            // Dispose CAN channel
            ApollonAbstractCANHandle.DisposeVciObject(this.m_CANChannel);

            // Dispose CAN controller
            ApollonAbstractCANHandle.DisposeVciObject(this.m_CANController);

            // Dispose VCI device
            ApollonAbstractCANHandle.DisposeVciObject(this.m_VCIDevice);

        } /* Dispose(bool) */

        #region event handling 

        public override void onHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {
                

                // finally register
                ApollonBackendManager.Instance.RegisterHandle(this.ID, this);

            } /* if() */

        } /* onHandleActivationRequested() */

        // unregistration
        public override void onHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // unplug it
                this.Dispose();

                // unregister
                ApollonBackendManager.Instance.UnregisterHandle(this.ID, this);

            } /* if() */

        } /* onHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractCANHandle */

} /* } namespace */