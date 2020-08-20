// using directives 

// avoid namespace pollution
namespace Labsim.apollon.backend.handle
{

    public class ApollonActiveSeatHandle
        : ApollonAbstractCANHandle
    {
        #region CAN message transmission/reception implementation

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
        
        protected override void AsynCANReaderCallback()
        {

            // buffer
            Ixxat.Vci4.Bal.Can.ICanMessage[] msgArray;

            // loop
            do
            {

                // Wait 100 msec for a message reception
                if (this.m_RxEvent.WaitOne(100, false))
                { 

                    // take all messages
                    if (this.m_CANMessageReader.ReadMessages(out msgArray) > 0)
                    {

                        // flush FIFO
                        foreach (Ixxat.Vci4.Bal.Can.ICanMessage entry in msgArray)
                        {

                            // do

                        } /* foreach() */

                    } /* if() */

                } /* if() */

            } while (0 == this.m_RxEnd);

        } /* AsynCANReaderCallback() */

        #endregion

        // ctor
        public ApollonActiveSeatHandle()
            : base()
        {
            this.m_handleID = ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle;
        }

    } /* class ApollonActiveSeatHandle */

} /* namespace Labsim.apollon.backend */
