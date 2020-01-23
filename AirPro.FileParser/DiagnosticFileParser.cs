using System.IO;
using AirPro.Common.Enumerations;
using AirPro.Parser.Output.Concrete;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser
{
    public static class DiagnosticFileParser
    {
        private const string DefaultVin = "AP000000000000000";
        internal const string DtcRegex = @"\s{4,5}|\s-\s|(?<=\d:)\s";

        public static IDiagnosticResultDto ParseFile(Stream stream, DiagnosticTool tool, DiagnosticFileType type)
        {
            // Load Stream.
            string text;
            using (var reader = new StreamReader(stream))
            {
                // Read All Text.
                text = reader.ReadToEnd();
            }

            // Process.
            return ParseFile(text, tool, type);
        }

        public static IDiagnosticResultDto ParseFile(string text, DiagnosticTool tool, DiagnosticFileType type)
        {
            // Create Result.
            IDiagnosticResultDto result = null;

            // Process File Text.
            switch (tool)
            {
                case DiagnosticTool.AutoEnginuity:
                    switch (type)
                    {
                        case DiagnosticFileType.XML:
                            result = AutoEnginuity.XmlParser.GetDiagnosticResult(text);
                            break;
                        case DiagnosticFileType.JSON:
                            result = AutoEnginuity.JsonParser.GetDiagnosticResult(text);
                            break;
                    }
                    break;
                /* TODO: Add Honda Parser APD-345.
                case DiagnosticTool.Honda:
                    break;
                */
            }

            // Update Result.
            result = result ?? new DiagnosticResultDto();
            result.DiagnosticTool = tool;
            result.DiagnosticFileType = type;
            result.DiagnosticFileText = text;

            // Set Default VIN on Blank.
            if (string.IsNullOrWhiteSpace(result.VehicleVin))
                result.VehicleVin = DefaultVin;

            return result;
        }
    }
}