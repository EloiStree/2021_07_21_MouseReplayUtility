using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveRecordWithDateFormat : MonoBehaviour
{
    public MouseReplayerThreadMono m_replayer;


    public void SaveCurrentRecord() {

            DateTime dt = DateTime.Now;
  
            m_replayer.SaveAsFileNearExe(
                string.Format(
                    "Record/autosave_{0}_{1}_{2}_{3}_{4}.mousereplay",
                    dt.Year,
                    dt.Month,
                    dt.Day,
                    dt.Hour,
                    dt.Minute
                    ));
        
    
    }
}
