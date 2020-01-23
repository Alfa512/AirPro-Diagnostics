using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AirPro.Parser.Output.Concrete;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser.AutoEnginuity
{
    internal class JsonParser
    {
        internal static IDiagnosticResultDto GetDiagnosticResult(string text)
        {
            // Remove Formatting.
            text = Regex.Replace(text, @"([\r|\n][\s|\t]+)", string.Empty);

            // Process Text.
            var output = new DiagnosticResultDto
            {
                // Load Shop Info.
                ShopName = GetGroupFieldValue(text, "SHOP_INFO", "SHOP_NAME"),
                ShopAddress = GetGroupFieldValue(text, "SHOP_INFO", "SHOP_ADDRESS"),
                ShopPhone = GetGroupFieldValue(text, "SHOP_INFO", "SHOP_PHONE"),
                ShopFax = GetGroupFieldValue(text, "SHOP_INFO", "SHOP_EMAIL"),
                ShopEmail = GetGroupFieldValue(text, "SHOP_INFO", "SHOP_NAME"),
                ScanDateTime = DateTime.TryParse(GetGroupFieldValue(text, "SHOP_INFO", "DATE_TIME"), out var dt)
                    ? (DateTime?) dt
                    : null,

                // Load Vehicle Info.
                VehicleMake = GetGroupFieldValue(text, "VEHICLE_INFO", "MAKE"),
                VehicleModel = GetGroupFieldValue(text, "VEHICLE_INFO", "MODEL"),
                VehicleYear = GetGroupFieldValue(text, "VEHICLE_INFO", "YEAR"),
                VehicleVin = GetGroupFieldValue(text, "VEHICLE_INFO", "VIN"),

                // Load Customer Info.
                CustomerRo = GetGroupFieldValue(text, "CUSTOMER_INFO", "RO"),
                CustomerFirstName = GetGroupFieldValue(text, "CUSTOMER_INFO", "FIRST_NAME"),
                CustomerLastName = GetGroupFieldValue(text, "CUSTOMER_INFO", "LAST_NAME"),

                // Create Collections.
                TestabilityIssuesList = new List<string>(),
                Controllers = new List<IDiagnosticControllerDto>()
            };

            // Load Testability Issues.
            var testabilityIssues = new Regex(@"(""TESTABILITY_ISSUES"").+?(?!\1)""(\w+?)""");
            foreach (Match match in testabilityIssues.Matches(text))
            {
                if (!match.Success || match.Groups.Count <= 2) continue;
                output.TestabilityIssuesList.Add(match.Groups[2].Value);
            }

            // Load Controllers.
            var index = text.IndexOf("CONTROLLER_NAME", 0, StringComparison.Ordinal);
            while (index > 0 && index < text.Length)
            {
                var next = text.IndexOf("CONTROLLER_NAME", index + 1, StringComparison.Ordinal);
                var controllerText = next > 0
                    ? text.Substring(index - 2, next - index - 1)
                    : text.Substring(index - 2, text.Length - index);
                output.Controllers.Add(GetController(controllerText));
                index = next < 0 ? text.Length : next;
            }

            return output;
        }

        private static IDiagnosticControllerDto GetController(string text)
        {
            // Check Input.
            if (text == null) return null;

            // Populate Controller.
            var controller = new DiagnosticControllerDto
            {
                ControllerName = GetFieldValue(text, "CONTROLLER_NAME"),
                TroubleCodes = new List<IDiagnosticTroubleCodeDto>(),
                FreezeFrames = new List<IDiagnosticFreezeFrameDto>()
            };

            // Process Info.
            var textList = Regex.Split(text, @"(DTC_DESCRIPTION?|FF_INFO?)").Skip(1).ToList();
            for (var i = 0; i < textList.Count; i++)
            {
                switch (textList[i])
                {
                    case "DTC_DESCRIPTION":
                        controller.TroubleCodes.Add(GetDiagnosticTroubleCodeResult(textList[i] + textList[i + 1]));
                        i = i + 1;
                        break;
                    case "FF_INFO":
                        controller.FreezeFrames.Add(GetFreezeFrameInfo(textList[i] + textList[i + 1]));
                        i = i + 1;
                        break;
                }
            }

            // Return.
            return controller;
        }

        private static IDiagnosticTroubleCodeDto GetDiagnosticTroubleCodeResult(string text)
        {
            // Check Input.
            if (text == null) return null;

            // Create Result.
            var result = new DiagnosticTroubleCodeDto { DiagnosticTroubleCodeInformationList = new List<string>() };

            // Split for Code.
            var value = GetFieldValue(text, @"DTC_DESCRIPTION");
            var split = Regex.Split(value, DiagnosticFileParser.DtcRegex);
            if (split.Length > 1 && split[0].Length <= 20)
            {
                // Set Code & Description.
                result.DiagnosticTroubleCode = split[0].ToUpper();
                result.DiagnosticTroubleCodeDescription = string.Join(" ", split.Skip(1));
            }
            else
            {
                // Set Text to Description.
                result.DiagnosticTroubleCodeDescription = result.DiagnosticTroubleCodeDescription ?? value;
            }

            // Load Info Entries.
            var info = new Regex(@"(?:""DTC_INFO_ENTRY"").+?""(.+?)""");
            foreach (Match match in info.Matches(text))
            {
                if (!match.Success || match.Groups.Count < 1) continue;
                result.DiagnosticTroubleCodeInformationList.Add(match.Groups[1].Value);
            }

            return result;
        }

        private static IDiagnosticFreezeFrameDto GetFreezeFrameInfo(string text)
        {
            // Check Input.
            if (text == null) return null;

            // Create Result.
            var result = new DiagnosticFreezeFrameDto
            {
                FreezeFrameDiagnosticTroubleCode = GetFieldValue(text, "FF_DTC"),
                FreezeFrameSensorGroups = new List<IDiagnosticFreezeFrameSensorGroupDto>()
            };

            // Split for Sensor Groups.
            var groups = Regex.Split(text, @"""FF_SENSORS""").Skip(1).ToList();
            foreach (var group in groups)
            {
                // Create Sensor Group.
                var sensorGroup = new DiagnosticFreezeFrameSensorGroupDto
                {
                    FreezeFrameSensors = new List<IDiagnosticFreezeFrameSensorDto>()
                };

                // Load Sensors.
                var sensors = Regex.Split(group, @"""FF_SENSOR""").Skip(1).ToList();
                foreach (var sensor in sensors)
                {
                    // Add Delimiter.
                    var s = @"""FF_SENSOR""" + sensor;

                    // Load Sensor.
                    sensorGroup.FreezeFrameSensors.Add(new DiagnosticFreezeFrameSensorDto
                    {
                        SensorName = GetFieldValue(s, "FF_SENSOR"),
                        SensorValue = GetFieldValue(s, "FF_VALUE"),
                        SensorUnit = GetFieldValue(s, "FF_UNITS")
                    });
                }

                // Add to Result.
                result.FreezeFrameSensorGroups.Add(sensorGroup);
            }

            return result;
        }

        private static string GetFieldValue(string text, string field) => 
            GetRegexValue(text, $@"(?:""?{field}""\s?:\s?)+""(.*?)""");

        private static string GetGroupFieldValue(string text, string group, string field) => 
            GetRegexValue(text, $@"(?:""?{group}"".+?""{field}""\s?:\s?)+""(.*?)""");

        private static string GetRegexValue(string text, string search)
        {
            var m = Regex.Match(text, search);
            if (m.Success && m.Groups.Count > 1) return m.Groups[1].Value;
            return string.Empty;
        }
    }
}