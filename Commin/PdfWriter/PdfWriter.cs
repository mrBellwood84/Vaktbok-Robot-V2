using Microsoft.Playwright;

namespace Common.PdfWriter;

/// <summary>
/// Provides static methods for generating PDF files from web pages, including support for exporting individual pages
/// and creating weekly report PDFs.
/// </summary>
/// <remarks>The PdfWriter class offers utility methods to export web page content as PDF documents using
/// predefined formatting and file organization conventions. All methods are asynchronous and overwrite existing files
/// at the target location if present. This class is not intended to be instantiated.</remarks>
public static class PdfWriter
{
    /// <summary>
    /// Generates a PDF of the specified page and saves it to the given file path using screen media styles.
    /// </summary>
    /// <remarks>The PDF is generated in A2 landscape format with background graphics included. All elements
    /// on the page are adjusted to ensure content is fully visible in the output. Existing files at the specified path
    /// will be overwritten.</remarks>
    /// <param name="page">The page to render and export as a PDF. Must not be null.</param>
    /// <param name="filepath">The full file path where the generated PDF will be saved. The directory will be created if it does not exist.</param>
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

    /// <summary>
    /// Generates a weekly PDF report from the specified page and saves it to a file in the designated report folder.
    /// </summary>
    /// <remarks>The generated PDF file is saved in a subdirectory structure under the specified report
    /// folder, organized by week. The file name includes the current date and the week identifier.</remarks>
    /// <param name="page">The page to be rendered and saved as a PDF file.</param>
    /// <param name="week_n">The identifier for the week, used to organize and name the report file.</param>
    /// <param name="reportFolder">The root directory where the weekly report file will be saved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the full file path of the generated
    /// PDF report.</returns>
    public static async Task<string> WorkbookWeeklyToFile(IPage page, string week_n, string reportFolder)
    {
        var week_n_string = week_n.PadLeft(2, '0');
        var now = DateTime.Now.ToString("yyyyMMdd");
        var fileName = $"{now}_{week_n}.pdf";
        var fileDir = Path.Join(reportFolder, "Faksimile", week_n_string);
        var filePath = Path.Join(fileDir, fileName);
        
        await PrintPage(page, filePath);
        return filePath;
    }
}