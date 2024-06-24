using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
namespace Evaluation.Services
{
    public class PdfService(IRazorViewRenderer razorViewRenderer)
    {

        private readonly IRazorViewRenderer _viewRenderService = razorViewRenderer;

        public async Task<PdfDocument> CreatePdf<T>(string viewName,T objet)
        {
            return await Task.Run(() =>
            {
                ChromePdfRenderer renderer = new();
                PdfDocument pdf = renderer.RenderRazorViewToPdf(_viewRenderService, viewName, objet!);
                return pdf;
            });
        }
    }
}
