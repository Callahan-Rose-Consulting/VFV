﻿
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersonalityInfo : MonoBehaviour
{
    public void OutputPersonalityType(byte[] pngBytes) {
        string path = createFile();

        using (FileStream fs = File.Create(path)) { };

        File.WriteAllBytes(path, pngBytes);

    }

    private string createFile() {
        string Dir = "Personality Info";

        if (!Directory.Exists(Dir))
        {
            Directory.CreateDirectory(Dir);
        }

        // The DateTime is added at the end of players name. This makes each playthrough have a unique file name
        string fileName = "Personality Results on: " +
            DateTime.Now.Month.ToString() + "-" +
            DateTime.Now.Day.ToString() + "-" +
            DateTime.Now.Year.ToString() +

            " H" + DateTime.Now.Hour.ToString() +
            "M" + DateTime.Now.Minute.ToString() +
            "S" + DateTime.Now.Second.ToString() +
            ".png";

        return @"Personality Info" + "/" + fileName;
    }
}
