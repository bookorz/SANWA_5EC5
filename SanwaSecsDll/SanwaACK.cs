using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public static class SanwaACK
    {
        public const byte COMMACK_ACK = 0x00;
        public const byte COMMACK_DENIED = 0x01;

        //Receive/Reply by S1F15/16
        public const byte OFLACK_ACK = 0x00;

        //Receive/Reply by S1F17/18
        public const byte ONLACK_ACCEPTED = 0x00;
        public const byte ONLACK_NOT_ACCEPTED = 0x01;
        public const byte ONLACK_ALREADY_ON_LINE = 0x02;

        //Receive/Reply by S2F15/16
        public const byte EAC_ACK = 0x00;
        public const byte EAC_DENIED_EC_NOT_EXIST = 0x01;
        public const byte EAC_DENIED_BUSY = 0x02;
        public const byte EAC_DENIED_EC_OUT_RANGE = 0x03;
        public const byte EAC_DENIED_BY_SANWA_DEFINED = 0x04;

        //Receive/Reply by S2F23/24
        public const byte TIAACK_ACK = 0x00;
        public const byte TIAACK_TO_MANY_SVIDS = 0x01;
        public const byte TIAACK_NO_MORE_TRACES_ALLOWED= 0x02;
        public const byte TIAACK_INVALID_DSPER = 0x03;
        public const byte TIAACK_INVALID_SVID = 0x04;
        public const byte TIAACK_INVALID_REPGSZ = 0x05;

        //Receive/Reply by S2F31/32
        public const byte TIACK_ACK = 0x00;
        public const byte TIACK_ERROR = 0x01;

        //Reply by S2F34
        public const byte DRACK_ACK = 0x00;
        public const byte DRACK_INSUFFICIENT_SPACE = 0x01;
        public const byte DRACK_INVALID_FORMAT = 0x02;
        public const byte DRACK_RPTID_DEFINED = 0x03;
        public const byte DRACK_VID_NOT_EXIST = 0x04;

        //Reply by S2F36
        public const byte LRACK_ACK = 0x00;
        public const byte LRACK_INSUFFICIENT_SPACE = 0x01;
        public const byte LRACK_INVALID_FORMAT = 0x02;
        public const byte LRACK_CEID_DEFINED = 0x03;
        public const byte LRACK_RPTID_NOT_EXIST = 0x04;

        //Reply by S2F38
        public const byte ERACK_ACK = 0x00;
        public const byte ERACK_CEID_NOT_EXIS = 0x01;

        //Receive by S5F3
        public const byte ALED_ENABLED = 0x01;
        public const byte ALED_DISABLED = 0x00;

        //Reply by S5F4
        public const byte ACKC5_ACK = 0x00;
        public const byte ACKC5_ERROR = 0x01;

        //Reply by S10F4
        public const byte ACKC10_ACK = 0x00;
        public const byte ACKC10_NOT_DISPLAYED = 0x01;
        public const byte ACKC10_TERMINAL_NOT_AVAILABLE = 0x02;

        //Reply by S2F42
        public const byte HCACK_ACK = 0x00;
        public const byte HCACK_CMD_ERROR = 0x01;
        public const byte HCACK_CANNOT_PERFORM = 0x02;
        public const byte HCACK_PARAMETER_INVALID = 0x03;
        public const byte HCACK_ACK_FINISH_BY_EVENT = 0x04;
        public const byte HCACK_REJECT_ALREADY_IN_DESIRED_CONDITION = 0x05;
        public const byte HCACK_OBJECT_NOT_EXIST = 0x06;

        //Reply by S3F18/S3F22/S3F28
        public const byte CAACK_ACK = 0x00;                                     //Acknowledge, command has been performed.
        public const byte CAACK_INVALID_CMD = 0x01;                             //Invalid command.
        public const byte CAACK_CANT_PERFORM_NOW = 0x02;                        //Can not perform now.
        public const byte CAACK_INVALID_DATA = 0x03;                            //Invalid data or argument.
        public const byte CAACK_ACK_FINISH_BY_EVENT = 0x04;                     //Acknowledge, request will be performed with completion signaled later by an event. 
        public const byte CAACK_REJECTED = 0x05;                                //Rejected. Invalid state.
        public const byte CAACK_CMD_PERFOEM_WITH_ERR = 0x06;                    //Command performed with errors. 

        //ERRORCODE
        public const ulong ERRCODE_NO_ERROR = 0;                                //No error
        public const ulong ERRCODE_UNKNOWN_OBJECT = 1;                          //Unknown object in Object Specifier 
        public const ulong ERRCODE_UNKNOWN_TARGET_OBJECT_TYPE = 2;              //Unknown target object type
        public const ulong ERRCODE_UNKNOWN_OBJECT_INSTANCE = 3;                 //Unknown object instance
        public const ulong ERRCODE_UNKNOWN_ATTRIBUTE_NAME = 4;                  //Unknown attribute name
        public const ulong ERRCODE_READ_ONLY_ATTRIBUTE = 5;                     //Read-only attribute - access denied 
        public const ulong ERRCODE_UNKNOWN_OBJECT_TYPE = 6;                     //Unknown object type
        public const ulong ERRCODE_INVALID_ATTRIBUTE_VALUE = 7;                 //Invalid attribute value
        public const ulong ERRCODE_SYNTAX_ERROR = 8;                            //Syntax error 
        public const ulong ERRCODE_VERIFICATION_ERROR = 9;                      //Verification error
        public const ulong ERRCODE_VALIDATION_ERROR = 10;                       //Validation error
        public const ulong ERRCODE_OBJECT_IDENTIFIER_IN_USE = 11;               //Object identifier in use
        public const ulong ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED = 12;        //Parameters improperly specified
        public const ulong ERRCODE_INSUFFICIENT_PARAMETERS_SPECIFIED = 13;      //Insufficient parameters specified
        public const ulong ERRCODE_UNSUPPORTED_OPTION_REQUESTED = 14;           //unsupported option requested 
        public const ulong ERRCODE_BUSY = 15;                                   //Busy
        public const ulong ERRCODE_NOT_AVAILABLE_FOR_PROCESSING = 16;           //Not available for processing
        public const ulong ERRCODE_COMMAND_NOT_VALID_FOR_CURRENT_STATE = 17;    //Command not valid for current state
        public const ulong ERRCODE_NO_MATERIAL_ALTERED = 18;                    //No material altered 
        public const ulong ERRCODE_MATERIAL_PARTIALLY_PROCESSED = 19;           //Material partially processed 
        public const ulong ERRCODE_ALL_MATERIAL_PROCESSED = 20;                 //All material processed
        public const ulong ERRCODE_RECIPE_SPECIFICATION_RELATED_ERROR = 21;     //Recipe specification related error 
        public const ulong ERRCODE_FAILED_DURING_PROCESSING = 22;               //Failed during processing  
        public const ulong ERRCODE_FAILED_WHILE_NOT_PROCESSING = 23;            //Failed while not processing 
        public const ulong ERRCODE_FAILED_DUE_TO_LACK_OF_MATERIAL = 24;         //Failed due to lack of material 
        public const ulong ERRCODE_JOB_ABORTED = 25;                            //Job aborted 
        public const ulong ERRCODE_JOB_STOPPED = 26;                            //Job stopped 
        public const ulong ERRCODE_JOB_CANCELLED = 27;                          //Job Cancelled 
        public const ulong ERRCODE_CANNOT_CHANGE_SELECTED_RECIPE = 28;          //Cannot Change Selected Recipe 
        public const ulong ERRCODE_UNKNOWN_EVENT= 29;                           //Unknown Event
        public const ulong ERRCODE_DUPLICATE_REPORT_ID = 30;                    //Duplicate report ID
        public const ulong ERRCODE_UNKNOWN_DATA_REPORT = 31;                    //Unknown data report
        public const ulong ERRCODE_DATA_REPORT_NOT_LINKED = 32;                 //Data report not linked
        public const ulong ERRCODE_UNKNOWN_TRACE_REPORT = 33;                   //Unknown trace report
        public const ulong ERRCODE_DUPLICATE_TRACE_ID = 34;                     //Duplicate trace ID 
        public const ulong ERRCODE_TOO_MANY_DATA_REPORTS  = 35;                 //Too many data reports 
        public const ulong ERRCODE_SAMPLE_PERIOD_OUT_OF_RANGE = 36;             //Sample period out of range   
        public const ulong ERRCODE_GROUP_SIZE_TO_LARGE  = 37;                   //Group size to large   
        public const ulong ERRCODE_RECOVERY_ACTION_CURRENTLY_INVALID = 38;      //Recovery action currently invalid 
        //Busy with another recovery currently unable to perform the recovery 
        public const ulong ERRCODE_BUSY_WITH_ANOTHER_RECOVERY_CURRENTLY_UNABLE_TO_PERFORM_THE_RECOVERY = 39;          
        public const ulong ERRCODE_NO_ACTIVE_RECOVERY_ACTION = 40;              //No active recovery action
        public const ulong ERRCODE_EXCEPTION_RECOVERY_FAILED = 41;              //Exception recovery failed 
        public const ulong ERRCODE_EXCEPTION_RECOVERY_ABORTED = 42;             //Exception recovery aborted
        public const ulong ERRCODE_INVALID_TABLE_ELEMENT = 43;                  //Invalid table element
        public const ulong ERRCODE_UNKNOWN_TABLE_ELEMENT = 44;                  //Unknown table element
        public const ulong ERRCODE_CANNOT_DELETE_PREDEFINED = 45;               //Cannot delete predefined
        public const ulong ERRCODE_INVALID_TOKEN  = 46;                         //Invalid token 
        public const ulong ERRCODE_INVALID_PARAMETER = 47;                      // Invalid parameter 
        public const ulong ERRCODE_LOAD_PORT_DOES_NOT_EXIST = 48;               //Load port does not exist 
        public const ulong ERRCODE_LOAD_PORT_ALREADY_IN_USE = 49;               //Load port already in use 
        public const ulong ERRCODE_MISSING_CARRIER  = 50;                       //Missing Carrier 
        //Action will be performed at earliest opportunity
        public const ulong ERRCODE_ACTION_WILL_BE_PERFORMED_AT_EARLIEST_OPPORTUNITY = 32768;        
        public const ulong ERRCODE_ACTION_CAN_NOT_BE_PERFORMED_NOW = 32769;     //Action can not be performed now   
        public const ulong ERRCODE_ACTION_FAILED_DUE_TO_ERRORS = 32770;         //Action failed due to errors    
        public const ulong ERRCODE_INVALID_COMMAND  = 32771;                    //Invalid command    
        public const ulong ERRCODE_CLIENT_ALR = 32772;                          // Client Alr   
        public const ulong ERRCODE_DUPLICATE_CLIENTID = 32773;                  // Duplicate ClientID  
        public const ulong ERRCODE_INVALID_CLIENTTYPE = 32774;                  // Invalid Clienttype 
        public const ulong ERRCODE_INCOMPATIBLEVERSIONS = 32775;                // IncompatibleVersions
        public const ulong ERRCODE_UNRECOGNIZED_CLIENTID = 32776;               // Unrecognized ClientID
        public const ulong ERRCODE_COMPLETED_UNSUCCESSFULLY = 32777;            //Failed(Completed Unsuccess-fully)
        public const ulong ERRCODE_EXTERNAL_INTERVENTION_REQUIRED = 32778;      //Failed(Unsafe) — External intervention required 
        public const ulong ERRCODE_SENSOR_DETECTED_OBSTACLE = 32779;            //Sensor-Detected Obstacle
        public const ulong ERRCODE_MATERIAL_NOT_SENT = 32780;                   //Material Not Sent
        public const ulong ERRCODE_MATERIAL_NOT_RECEIVED = 32781;               //Material Not Received
        public const ulong ERRCODE_MATERIAL_LOST = 32782;                       //Material Lost  
        public const ulong ERRCODE_HARDWARE_FAILURE  = 32783;                   //Hardware Failure   
        public const ulong ERRCODE_TRANSFER_CANCELLED = 32784;                  //Transfer Cancelled















        }
}
