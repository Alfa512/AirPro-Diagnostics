using AirPro.Parser.Output.Concrete;
using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AirPro.Parser.Volvo
{
    internal class PdfParser
    {
        internal static IDiagnosticResultDto GetDiagnosticResult(string text)
        {

            // Process Text.
            var output = new DiagnosticResultDto
            {
                // Load Vehicle Info.
                VehicleModel = GetFieldValue(text, "Model"),
                VehicleVin = RemoveWhiteSpaces(GetFieldValue(text, "VIN")),
                VehicleYear = GetFieldValue(text, "Year"),

                // Create Collections.
                TestabilityIssuesList = new List<string>(),
                Controllers = new List<IDiagnosticControllerDto>()
            };

            // Split Text to lines
            var controllers = Regex.Split(text.Trim(), @"(.*)[\n\r]*Frozen Values", RegexOptions.CultureInvariant)
                                        .Where(s => s.Trim().Length > 25 && s.Trim().Length < 500)
                                        .Select(s => s.Trim()).ToList();


            foreach (var controllerText in controllers)
            {
                // Process Info.
                output.Controllers.Add(GetController(controllerText));
            }

            return output;
        }

        private struct ControllerPartType
        {
            public string Name { get; set; }
            public List<PartTypeCode> TypeCodes { get; set; }
        }

        private struct PartTypeCode
        {
            public string Name { get; set; }
            public string Regex { get; set; }
            public List<string> Values { get; set; }
            public int MaxValue { get; set; }
            public int MinValue { get; set; }
            public int Groups { get; set; }
            public int? CodeGroupPos { get; set; }
            public int? DescGroupPos { get; set; }
            public int? ValueGroupPos { get; set; }
        }

        private static List<ControllerPartType> ControllerPartTypes { get; set; }

        private static void InitControllerPartTypes()
        {
            ControllerPartTypes = new List<ControllerPartType>();
            ControllerPartTypes.Add(new ControllerPartType
            {
                Name = "Frozen Values",
                TypeCodes = new[]
                {
                    new PartTypeCode
                    {
                        Name = "Global real time",
                        Regex = "(Globalrealtime)(.*)",
                        Values = null,
                        CodeGroupPos = null,
                        DescGroupPos = null,
                        ValueGroupPos = 1
                    },
                    new PartTypeCode
                    {
                        Name = "Total distance",
                        Regex = "(Totaldistance)(.*)",
                        Values = null,
                        CodeGroupPos = null,
                        DescGroupPos = null,
                        ValueGroupPos = 1
                    },
                    new PartTypeCode
                    {
                        Name = "Total distance",
                        Regex = "(Totaldistance)(.*)", //May be determined twice - for miles and kilometers
                        Values = null,
                        CodeGroupPos = null,
                        DescGroupPos = null,
                        ValueGroupPos = 1
                    },
                    new PartTypeCode
                    {
                        Name = "MECU supply voltage",
                        Regex = "(MECUsupplyvoltage)(.*)",
                        Values = null,
                        CodeGroupPos = null,
                        DescGroupPos = null,
                        ValueGroupPos = 1
                    },
                    new PartTypeCode
                    {
                        Name = "Usage mode",
                        Regex = "(Usagemode)(.*)",
                        Values = null,
                        CodeGroupPos = null,
                        DescGroupPos = null,
                        ValueGroupPos = 1
                    }
                }.ToList()
            });
            ControllerPartTypes.Add(new ControllerPartType
            {
                Name = "Status bits",
                TypeCodes = new[]
                {
                    new PartTypeCode
                    {
                        Name = null,
                        Regex = "(SB[0-9]{1,2})(.*)(Yes|No)", //Regex: 3 Groups - Code, Code description, Value
                        Values = new []{"Yes", "No"}.ToList(),
                        Groups = 3,
                        CodeGroupPos = 0,
                        DescGroupPos = 1,
                        ValueGroupPos = 2
                    }
                }.ToList()
            });
            ControllerPartTypes.Add(new ControllerPartType
            {
                Name = "Status indicators",
                TypeCodes = new[]
                {
                    new PartTypeCode
                    {
                        Name = null,
                        Regex = "(SI[0-9]{1,2})(.*)(Yes|No)",
                        Values = new []{"Yes", "No"}.ToList(),
                        Groups = 3,
                        CodeGroupPos = 0,
                        DescGroupPos = 1,
                        ValueGroupPos = 2
                    }
                }.ToList()
            });
            ControllerPartTypes.Add(new ControllerPartType
            {
                Name = "ECU operation cycle counters",
                TypeCodes = new[]
                {
                    new PartTypeCode
                    {
                        Name = null,
                        Regex = "(OCC[0-9]{1,2})(.*[^0-9])([0-9]{1,3})",
                        Values = null,
                        Groups = 3,
                        CodeGroupPos = 0,
                        DescGroupPos = 1,
                        ValueGroupPos = 2,
                        MaxValue = 255,
                        MinValue = 0
                    }
                }.ToList()
            });
            ControllerPartTypes.Add(new ControllerPartType
            {
                Name = "Fault counters",
                TypeCodes = new[]
                {
                    new PartTypeCode
                    {
                        Name = null,
                        Regex = "(FDC[0-9]{1,2}){1}(.*[^0-9-]){1}([-]?[0-9]{1,3}){1}",
                        Values = null,
                        MaxValue = 127,
                        MinValue = -128,
                        Groups = 3,
                        CodeGroupPos = 0,
                        DescGroupPos = 1,
                        ValueGroupPos = 2
                    }
                }.ToList()
            });
            ControllerPartTypes.Add(new ControllerPartType
            {
                Name = "Timestamps",
                TypeCodes = new[]
                {
                    new PartTypeCode
                    {
                        Name = null,
                        Regex = "(TS[0-9]{1,2}){1}(.*[^0-9-.]){1}([0-9.]{1,}){1}",
                        Values = null,
                        Groups = 3,
                        CodeGroupPos = 0,
                        DescGroupPos = 1,
                        ValueGroupPos = 2
                    }
                }.ToList()
            });
        }

        private static string GetFieldValue(string text, string field) =>
        GetRegexValue(text, $@"[\n\r].*{field}:\s*([^\n\r]*)");

        private static string GetRegexValue(string text, string search)
        {
            var m = Regex.Match(text, search);
            if (m.Success && m.Groups.Count > 1) return m.Groups[1].Value;
            return string.Empty;
        }

        private static string RemoveWhiteSpaces(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }

        private static IDiagnosticControllerDto GetController(string text)
        {
            // Check Input.
            if (text == null) return null;

            var split = text.Split(new[] { ' ' }, 2);
            var controllerText = split[1];


            var index = controllerText.IndexOf(".", 0, StringComparison.Ordinal);
            var controllerName = controllerText.Substring(0, index);

            // Populate Controller.
            var controller = new DiagnosticControllerDto
            {
                TroubleCodes = new List<IDiagnosticTroubleCodeDto>(),
                FreezeFrames = new List<IDiagnosticFreezeFrameDto>()
            };

            controller.ControllerName = controllerName;
            controller.TroubleCodes.Add(GetDiagnosticTroubleCodeResult(text));
            // Return.
            return controller;
        }

        private static IDiagnosticTroubleCodeDto GetDiagnosticTroubleCodeResult(string text)
        {
            // Check Input.
            if (text == null) return null;

            // Create Result.
            var result = new DiagnosticTroubleCodeDto { DiagnosticTroubleCodeInformationList = new List<string>() };

            if (text != "No DTC was found")
            {
                // Set Code & Description.
                var split = text.Split(new[] { ' ' }, 3);
                result.DiagnosticTroubleCode = split[0].ToUpper();
                result.DiagnosticTroubleCodeDescription = string.Join(" ", split.Skip(1));
            }
            else
            {
                // Set Text to Description.
                result.DiagnosticTroubleCodeDescription = result.DiagnosticTroubleCodeDescription ?? text;
            }
            return result;
        }

    }
}