using System.Collections;
using System.Globalization;
using Common.Logging;
using Domain.Settings;
using Microsoft.Playwright;

namespace Common.PdfWriter;

public static class PdfWriter
{
    public static async Task PrintPage(IPage page, string filepath)
    {
        // ensure folders exists
        var directory = Path.GetDirectoryName(filepath);
        Directory.CreateDirectory(directory!);
        
        // get document size in px and convert to inces
        var queryHeight = @"document.documentElement.scrollHeight";
        var queryWidth = @"document.documentElement.scrollWidth";
        var heightPx = await page.EvaluateAsync<float>(queryHeight);
        var widthPx = await page.EvaluateAsync<float>(queryWidth);
        // get document size in inches, 96 DPI
        var heightCm = (heightPx / 96.0 * 2.54).ToString(CultureInfo.InvariantCulture);
        var widthCm = (widthPx / 96.0 * 2.54).ToString(CultureInfo.InvariantCulture);
        
        var options = new PagePdfOptions
        {
            Path = filepath,
            Height = $"{heightCm}cm",
            Width = $"{widthCm}cm",
            PrintBackground = true
        };
        
        await page.PdfAsync(options);
    }

    public static async Task<string> WorkbookWeeklyToFile(IPage page, string week_n, string reportFolder)
    {
        var now = DateTime.Now.ToString("yyyyMMdd_HHmm");
        var fileName = $"{now}-{week_n}.pdf";
        var fileDir = Path.Join(reportFolder, "week", week_n);
        var filePath = Path.Join(fileDir, fileName);
        
        await PrintPage(page, filePath);
        return filePath;
    }
}