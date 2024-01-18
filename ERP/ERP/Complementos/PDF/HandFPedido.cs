using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.PDF
{
    public class HandFPedido : PdfPageEventHelper
    {

        String PathImageM = null;
        public HandFPedido(String MarcaImagen)
        {
            PathImageM = MarcaImagen;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Image image = Image.GetInstance(PathImageM);
            image.ScaleAbsolute(613, 792);
            image.SetAbsolutePosition(0, 0);
            document.Add(image);
        }
    }
}