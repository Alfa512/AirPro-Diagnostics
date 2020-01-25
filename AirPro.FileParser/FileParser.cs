using IronOcr;

namespace AirPro.Parser
{
    public static class FileParser
    {
        public static string ParsePdf(string pdfFilePath)
        {
            var Ocr = new AdvancedOcr()
            {
                CleanBackgroundNoise = false,
                ColorDepth = 4,
                ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                EnhanceContrast = false,
                DetectWhiteTextOnDarkBackgrounds = false,
                RotateAndStraighten = false,
                Language = IronOcr.Languages.English.OcrLanguagePack,
                EnhanceResolution = false,
                InputImageType = AdvancedOcr.InputTypes.Document,
                ReadBarCodes = false,
                Strategy = AdvancedOcr.OcrStrategy.Fast
            };
            var Results = Ocr.ReadPdf(pdfFilePath);
            var Pages = Results.Pages;
            var Barcodes = Results.Barcodes;
            var FullPdfText = Results.Text;

            return FullPdfText;
        }

        public static string ParseFile(string anyFilePath)
        {
            var Ocr = new AutoOcr();
            var Result = Ocr.Read(anyFilePath);
            return Result.Text;
        }
    }
}
