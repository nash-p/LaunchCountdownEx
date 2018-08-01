using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace NASA_CountDown
{
    public class ModuleNASACountdown : PartModule
    {
        [KSPField(guiActive = false, guiName = "craftName", isPersistant = true)]
        public string craftName;

        public static string CraftName(Vessel v)
        {

            foreach (var p in v.Parts)
            {
                if (p.Modules.Contains<ModuleCommand>())
                {
                    ModuleNASACountdown m = p.Modules.GetModule<ModuleNASACountdown>();
                    return m.craftName;
                }
            }

            return v.vesselName;
        }

        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH && craftName == "")
            {
                craftName = FlightGlobals.ActiveVessel.vesselName;
                Log.Info("ModuleNASACountdown.OnAwake, part: " + this.part.partInfo.title + ", craftName: " + craftName);
            }
            else
            {
                if (HighLogic.LoadedSceneIsFlight)
                    Log.Info("ModuleNASACountdown.OnAwake, LoadSceneIsFlight, but no ActiveVessel");
            }
        }
    }    
}
