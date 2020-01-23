using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AirPro.Parser.Output.Concrete;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser.AutoEnginuity
{
    internal static class XmlParser
    {
        internal static IDiagnosticResultDto GetDiagnosticResult(string text)
        {
            // Split Text into Lines.
            var lines = Regex.Split(text.Trim(), @"(\<\/?\w+\>)", RegexOptions.CultureInvariant).Where(s => s.Trim().Length > 0)
                .Select(s => s.Trim()).ToList();

            // Create Result.
            var output = new DiagnosticResultDto
            {
                TestabilityIssuesList = new List<string>(),
                Controllers = new List<IDiagnosticControllerDto>()
            };

            // Process File Lines.
            for (var i = 0; i < lines.Count - 1; ++i)
            {
                // Check Line.
                if (CheckLine(lines[i], lines[i + 1])) continue;

                // Load Values.
                var value = GetValue(lines[i + 1]);
                switch (lines[i])
                {
                    case "<SHOP_NAME>":
                        output.ShopName = value;
                        break;
                    case "<SHOP_ADDRESS>":
                        output.ShopAddress = value;
                        break;
                    case "<SHOP_PHONE>":
                        output.ShopPhone = value;
                        break;
                    case "<SHOP_FAX>":
                        output.ShopFax = value;
                        break;
                    case "<SHOP_EMAIL>":
                        output.ShopEmail = value;
                        break;
                    case "<DATE_TIME>":
                        output.ScanDateTime = DateTime.TryParse(value, out var dt) ? (DateTime?)dt : null;
                        break;
                    case "<MAKE>":
                        output.VehicleMake = value;
                        break;
                    case "<MODEL>":
                        output.VehicleModel = value;
                        break;
                    case "<YEAR>":
                        output.VehicleYear = value;
                        break;
                    case "<VIN>":
                        output.VehicleVin = value;
                        break;
                    case "<RO>":
                        output.CustomerRo = value;
                        break;
                    case "<FIRST_NAME>":
                        output.CustomerFirstName = value;
                        break;
                    case "<LAST_NAME>":
                        output.CustomerLastName = value;
                        break;
                    case "<TESTABILITY_ISSUE>":
                        output.TestabilityIssuesList.Add(value);
                        break;
                    case "<CONTROLLER>":
                        var endIndex = lines.FindIndex(i + 1, e => e.EndsWith("CONTROLLER>")) - 1;
                        output.Controllers.Add(GetController(lines.Skip(i + 1).Take(endIndex - i).ToList()));
                        i = endIndex;
                        break;
                }
            }

            return output;
        }

        private static IDiagnosticControllerDto GetController(List<string> lines)
        {
            // Check Input.
            if (!lines?.Any() ?? true) return null;

            // Create Controller.
            var controller = new DiagnosticControllerDto
            {
                TroubleCodes = new List<IDiagnosticTroubleCodeDto>(),
                FreezeFrames = new List<IDiagnosticFreezeFrameDto>()
            };

            // Populate Controller.
            for (var i = 0; i < lines.Count - 1; ++i)
            {
                // Check Line.
                if (CheckLine(lines[i], lines[i + 1])) continue;

                // Load Values.
                var endIndex = i;
                switch (lines[i])
                {
                    case "<CONTROLLER_NAME>":
                        controller.ControllerName = controller.ControllerName ?? GetValue(lines[i + 1]);
                        break;
                    case "<DTC_RESULTS>":
                        endIndex = lines.FindIndex(i + 1, e => e.EndsWith("DTC_RESULTS>")) - 1;
                        controller.TroubleCodes.Add(GetDiagnosticTroubleCodeResult(lines.Skip(i + 1).Take(endIndex - i).ToList()));
                        i = endIndex;
                        break;
                    case "<FF_INFO>":
                        endIndex = lines.FindIndex(i + 1, e => e.EndsWith("FF_INFO>")) - 1;
                        controller.FreezeFrames.Add(GetFreezeFrameInfo(lines.Skip(i + 1).Take(endIndex - i).ToList()));
                        i = endIndex;
                        break;
                }
            }

            // Return.
            return controller;
        }

        private static IDiagnosticTroubleCodeDto GetDiagnosticTroubleCodeResult(List<string> lines)
        {
            // Check Input.
            if (!lines?.Any() ?? true) return null;

            // Create Trouble Code.
            var result = new DiagnosticTroubleCodeDto
            {
                DiagnosticTroubleCodeInformationList = new List<string>()
            };

            // Populate Result.
            for (var i = 0; i < lines.Count - 1; ++i)
            {
                // Check Line.
                if (CheckLine(lines[i], lines[i + 1])) continue;

                // Load Values.
                var value = GetValue(lines[i + 1]);
                if (lines[i] == "<DTC_DESCRIPTION>")
                {
                    // Split for Code.
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
                }
                else if (lines[i].StartsWith("<DTC_INFO_ENTRY"))
                    result.DiagnosticTroubleCodeInformationList.Add(value);
            }

            // Return.
            return result;
        }

        private static IDiagnosticFreezeFrameDto GetFreezeFrameInfo(List<string> lines)
        {
            // Check Input.
            if (!lines?.Any() ?? true) return null;

            // Create Freeze Frame.
            var result = new DiagnosticFreezeFrameDto
            {
                FreezeFrameSensorGroups = new List<IDiagnosticFreezeFrameSensorGroupDto>()
            };

            // Populate Result.
            for (var i = 0; i < lines.Count - 1; ++i)
            {
                // Check Line.
                if (CheckLine(lines[i], lines[i + 1])) continue;

                // Load Values.
                var value = GetValue(lines[i + 1]);
                switch (lines[i])
                {
                    case "<FF_DTC>":
                        result.FreezeFrameDiagnosticTroubleCode = value;
                        break;
                    case "<FF_SENSORS>":
                        var endIndex = lines.FindIndex(i + 1, e => e.EndsWith("FF_SENSORS>")) - 1;
                        result.FreezeFrameSensorGroups.Add(GetFreezeFrameSensorGroup(lines.Skip(i + 1).Take(endIndex - i).ToList()));
                        i = endIndex;
                        break;
                }
            }

            // Return.
            return result;
        }

        private static IDiagnosticFreezeFrameSensorGroupDto GetFreezeFrameSensorGroup(List<string> lines)
        {
            // Check Input.
            if (!lines?.Any() ?? true) return null;

            // Create Sensor.
            var result = new DiagnosticFreezeFrameSensorGroupDto
            {
                FreezeFrameSensors = new List<IDiagnosticFreezeFrameSensorDto>()
            };

            // Populate Result.
            for (var i = 0; i < lines.Count - 1; ++i)
            {
                // Check Line.
                if (CheckLine(lines[i], lines[i + 1])) continue;

                // Check Sensor.
                if (!lines[i].StartsWith("<FF_SENSOR_")) continue;

                // Load Sensor.
                var endIndex = lines.FindIndex(i + 1, e => e.StartsWith("<FF_SENSOR_")) - 1;
                endIndex = endIndex < 0 ? lines.Count - 1 : endIndex;
                result.FreezeFrameSensors.Add(GetFreezeFrameSensor(lines.Skip(i).Take(endIndex - i + 1).ToList()));
                i = endIndex;
            }

            // Return.
            return result;
        }

        private static IDiagnosticFreezeFrameSensorDto GetFreezeFrameSensor(List<string> lines)
        {
            // Check Input.
            if (!lines?.Any() ?? true) return null;

            // Populate Result.
            var result = new DiagnosticFreezeFrameSensorDto();
            for (var i = 0; i < lines.Count - 1; ++i)
            {
                // Check Line.
                if (CheckLine(lines[i], lines[i + 1])) continue;

                // Load Values.
                var value = GetValue(lines[i + 1]);
                if (lines[i].StartsWith("<FF_SENSOR_"))
                    result.SensorName = value;
                else if (lines[i].StartsWith("<FF_VALUE_"))
                    result.SensorValue = value;
                else if (lines[i].StartsWith("<FF_UNITS_"))
                    result.SensorUnit = value;
            }

            // Return.
            return result;
        }

        /// <summary>
        /// Check Value for Tag, Return if NOT Tag.
        /// </summary>
        /// <param name="val">string</param>
        /// <returns>string</returns>
        private static string GetValue(string val) => (!Regex.IsMatch(val, @"\<\/?\w+\>")) ? val : null;

        /// <summary>
        /// Check for No Opening Tag.
        /// Check for Empty Opening/Closing Tags.
        /// </summary>
        /// <param name="current">string</param>
        /// <param name="next">string</param>
        /// <returns>bool</returns>
        private static bool CheckLine(string current, string next) =>
            (!Regex.IsMatch(current, @"\<\w+\>"))
            || (Regex.IsMatch(current, @"\<\w+\>") && Regex.IsMatch(next, @"\<\/\w+\>"));
    }
}
