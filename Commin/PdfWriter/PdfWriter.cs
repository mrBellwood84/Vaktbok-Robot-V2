using Microsoft.Playwright;

namespace Common.PdfWriter;

public static class PdfWriter
{
    public static async Task PrintPage(IPage page, string filepath)
    {
        // ensure folders exists
        var directory = Path.GetDirectoryName(filepath);
        Directory.CreateDirectory(directory!);
        
        // set css to screen view
        await page.EmulateMediaAsync(new PageEmulateMediaOptions
        {
            Media = Media.Screen,
        });
        
        await page.EvaluateAsync(@"
            (() => {
              const els = document.querySelectorAll('*');
              for (const el of els) {
                el.style.overflow = 'visible';
                el.style.maxHeight = 'none';
                if (el.style.height === '100%' || el.style.height === '100vh') {
                  el.style.height = 'auto';
                }
              }
            })();
            ");

        var options = new PagePdfOptions
        {
            Path = filepath,
            PrintBackground = true,
            Format = "A2",
            Landscape = true
        };
        
        await page.PdfAsync(options);
    }

    public static async Task<string> WorkbookWeeklyToFile(IPage page, string week_n, string reportFolder)
    {
        var now = DateTime.Now.ToString("yyyyMMdd_HHmm");
        var fileName = $"{now}__{week_n}.pdf";
        var fileDir = Path.Join(reportFolder, "week", week_n);
        var filePath = Path.Join(fileDir, fileName);
        
        await PrintPage(page, filePath);
        return filePath;
    }
}