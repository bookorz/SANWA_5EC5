using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{

    /// <summary>
    /// CARRIER ID STATUS
    /// </summary>
    public enum E87_CIDS
    {
        CARRIER = 0,
        ID_NOT_READ = 1,
        WAITING_FOR_HOST = 2,
        ID_VERIFICATION_FAILED = 3,
        ID_VERIFICATION_OK = 4
    }
    /// <summary>
    /// CARREER SLOT MAP STATUS
    /// </summary>
    public enum E87_CSMS
    {
        CARRIER = 0,
        SLOT_MAP_NOT_READ = 1,
        WAITING_FOR_HOST = 2,
        SLOT_MAP_VERIFICATION_OK = 3,
        SLOT_MAP_VERIFICATION_FAIL = 4,
    }
    /// <summary>
    /// CARREER ACCESSING STATUS
    /// </summary>
    public enum E87_CAS
    {
        CARRIER = 0,
        NOT_ACCESSED = 1,
        IN_ACCESSED = 2,
        CARRIER_COMPLETE = 3,
        CARRIER_STOPPED = 4,
    }

    public enum eSlotMap
    {
        UNDEFINED = 0,
        EMPTY = 1,
        NOT_EMPTY = 2,
        CORRECTLY_OCCUPIED = 3,
        DOUBLESLOTTED = 4,
        CROSS_SLOTTED = 5 
    }
    public class sContentMap
    {
        public string LotID;
        public string SubstrateID;

    }
    public class SanwaCarrier
    {
        public LoadPort LoadPortObj = null;
        public E87_ASSOCIATED Associated = E87_ASSOCIATED.NOT_ASSOCIATION;
 
        /// <summary>
        /// Maximum number of substrates a carrier can hold.
        /// </summary>
        public byte Capacity;
        
        /// <summary>
        /// Current state of the carrier ID verification. 
        /// </summary>
        public E87_CIDS CarrierIDStatus = E87_CIDS.CARRIER;
        public E87_CIDS preCarrierIDStatus = E87_CIDS.CARRIER;
        /// <summary>
        /// The current accessing state of the carrier by the equipment. The current substate of the CarrierAccessingStatus state model.
        /// </summary>
        public E87_CAS CarrierAccessingStatus = E87_CAS.NOT_ACCESSED;
        public E87_CAS preCarrierAccessingStatus = E87_CAS.CARRIER;
        /// <summary>
        /// Ordered list of lot and substrate identifiers corresponding to slot 1,2,3,…n.
        /// </summary>
        public Dictionary<string, sContentMap> ContentMap = new Dictionary<string, sContentMap>();

        /// <summary>
        /// Ordered list of slot status as provided by the host and 
        /// corresponding to slot 1,2,3…n 
        /// until a successful slot map read, then as read by the equipment
        /// </summary>
        //public Dictionary<string, eSlotMap> SlotMap = new Dictionary<string, eSlotMap>();
        public List<eSlotMap> SlotMap = new List<eSlotMap>();
        /// <summary>
        /// Identifier of current location.
        /// </summary>
        public string LocationID;

        /// <summary>
        /// Object type
        /// </summary>
        public string ObjType;

        /// <summary>
        /// Object identifier.
        /// </summary>
        public string ObjID;

        /// <summary>
        /// Current state of slot map verification. 
        /// </summary>
        public E87_CSMS SlotMapStatus;
        public E87_CSMS preSlotMapStatus;

        /// <summary>
        /// The number of substrates currently in the carrier
        /// </summary>
        public byte SubstrateCount;
        
        /// <summary>
        ///  The type of material contained in the carrier(i.e., TEST, DUMMY, PRODUCT, FILLER, etc.). 
        /// </summary>
        public string Usage;
    }
    public partial class SanwaBaseExec
    {
        public bool CarrierIDStatusChange(SanwaCarrier carrier, E87_CIDS IDStatus)
        {
            bool Return = true;

            switch(carrier.CarrierIDStatus)
            {
                case E87_CIDS.CARRIER:
                    break;

                case E87_CIDS.ID_NOT_READ:
                    if (!(E87_CIDS.CARRIER == IDStatus ||
                        E87_CIDS.ID_VERIFICATION_OK == IDStatus ||
                        E87_CIDS.WAITING_FOR_HOST == IDStatus))
                        Return = false;
                    break;

                case E87_CIDS.ID_VERIFICATION_OK:
                    if (E87_CIDS.CARRIER != IDStatus) 
                        Return = false;
                    break;

                case E87_CIDS.ID_VERIFICATION_FAILED:
                    if (E87_CIDS.CARRIER != IDStatus)
                        Return = false;
                    break;

                case E87_CIDS.WAITING_FOR_HOST:
                    if (!(E87_CIDS.CARRIER == IDStatus ||
                        E87_CIDS.ID_VERIFICATION_FAILED == IDStatus ||
                        E87_CIDS.ID_VERIFICATION_OK == IDStatus))
                        Return = false;
                    break;
            }

            if(Return)
            {
                carrier.preCarrierIDStatus = carrier.CarrierIDStatus;
                carrier.CarrierIDStatus = IDStatus;
            }


            return Return;
        }
        public bool CarrierAccessingStatus(SanwaCarrier carrier, E87_CAS AccessingStatus)
        {
            bool Return = true;

            switch (carrier.CarrierAccessingStatus)
            {
                case E87_CAS.CARRIER:
                    break;
                case E87_CAS.NOT_ACCESSED:
                    if (!(E87_CAS.CARRIER == AccessingStatus ||
                        E87_CAS.IN_ACCESSED == AccessingStatus)) Return = false;
                    break;

                case E87_CAS.IN_ACCESSED:
                    if (!(E87_CAS.CARRIER == AccessingStatus ||
                        E87_CAS.CARRIER_COMPLETE == AccessingStatus ||
                        E87_CAS.CARRIER_STOPPED == AccessingStatus)) Return = false;
                    break;
                case E87_CAS.CARRIER_COMPLETE:
                    if (E87_CAS.CARRIER != AccessingStatus) Return = false;
                    break;

                case E87_CAS.CARRIER_STOPPED:
                    if (E87_CAS.CARRIER != AccessingStatus) Return = false;
                    break;
            }

            if (Return)
            {
                carrier.preCarrierAccessingStatus = carrier.CarrierAccessingStatus;
                carrier.CarrierAccessingStatus = AccessingStatus;
            }


            return Return;
        }
        public bool CarrierSlotMapStatusChange(SanwaCarrier carrier, E87_CSMS slotMapStatus)
        {
            bool Return = true;

            switch (carrier.SlotMapStatus)
            {
                case E87_CSMS.CARRIER:
                    break;

                case E87_CSMS.SLOT_MAP_NOT_READ:
                    if (!(slotMapStatus == E87_CSMS.CARRIER ||
                        slotMapStatus == E87_CSMS.SLOT_MAP_VERIFICATION_OK ||
                        slotMapStatus == E87_CSMS.WAITING_FOR_HOST
                        )) Return = false;
                    break;

                case E87_CSMS.WAITING_FOR_HOST:
                    if(!(slotMapStatus == E87_CSMS.SLOT_MAP_NOT_READ || 
                        slotMapStatus == E87_CSMS.SLOT_MAP_VERIFICATION_OK ||
                        slotMapStatus == E87_CSMS.SLOT_MAP_VERIFICATION_FAIL
                        )) Return = false;
                    break;

                case E87_CSMS.SLOT_MAP_VERIFICATION_OK:
                    if (slotMapStatus != E87_CSMS.SLOT_MAP_NOT_READ) Return = false;
                    break;

                case E87_CSMS.SLOT_MAP_VERIFICATION_FAIL:
                    if (slotMapStatus != E87_CSMS.WAITING_FOR_HOST) Return = false;
                    break;
            }


            if (Return)
            {
                carrier.preSlotMapStatus = carrier.SlotMapStatus;
                carrier.SlotMapStatus = slotMapStatus;
            }


            return Return;
        }
    }
}
