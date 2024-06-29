using IronPdf.Extensions.Mvc.Core;
namespace Evaluation.Services.Utile
{
    public class PdfService(IRazorViewRenderer razorViewRenderer)
    {

        private readonly IRazorViewRenderer _viewRenderService = razorViewRenderer;

        #region CreatePdf
        public async Task<PdfDocument> CreatePdf<T>(string viewName, T objet)
        {
            return await Task.Run(() =>
            {
                ChromePdfRenderer renderer = new();
                PdfDocument pdf = renderer.RenderRazorViewToPdf(_viewRenderService, viewName, objet!);
                return pdf;
            });
        }
        #endregion
    }
}
