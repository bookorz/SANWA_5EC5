using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public enum E40_PJSTATE
    {
        QUEUED = 0,
        SETTING_UP,
        WAITING_FOR_START,
        PROCESSING,
        PROCESS_COMPLETE,
        Resserved,
        PAUSING,
        PAUSED,
        STOPPING,
        ABORTING,
        STOPPED,
        ABORTED
    };
    public enum E40_PR_RECIPE_METHOD
    {
        RECIPE_ONLY = 1,
        RECIPE_WITH_VARIABLE_TUNING,
    };

    public class RecipeVariable
    {
        public string RecipeVarName;

        /// <summary>
        /// RecipeVarValue 暫不支援List型式
        /// </summary>
        public object RecipeVarValue;
    }

    public class E40_HostCommand
    {
        public string Name = null;
        public string MessageName = null;
    }

    public class MaterialInCarrier
    {
        public List<string> SlotID = new List<string>();
    }

    public class SanwaPJ
    {
        public string ObjID;
        public string ObjType = "PROCESSJOB";
        public List<string> PauseEvent = new List<string>();
        public E40_PJSTATE PRJobState = E40_PJSTATE.QUEUED;

        /// <summary>
        /// IN_CARRIERS(13) = Quantities in carriers 
        /// IN_SUBSTRATES(14) = Quantities in substrates
        /// </summary>
        public byte PRMtlType = 0x00;
        /// <summary>
        /// if IN_CARRIERS
        /// object = Disctionary<'CarrierID', List<string> 'SlotID'>
        /// if IN_SUBSTRATES
        /// object = List<'SlotID'>
        /// </summary>
        //public Dictionary<string, Object> PRMtlNameList = new Dictionary<string, Object>();
        public object PRMtlNameList;
        /// <summary>
        /// TRUE:Automatic
        /// FALSE:Manual start
        /// </summary>
        public bool PRProcessStart;

        public E40_PR_RECIPE_METHOD PRRecipeMethod;

        public string RecID;

        public Dictionary<string, RecipeVariable> RecVariableList = new Dictionary<string, RecipeVariable>();

    }
}
