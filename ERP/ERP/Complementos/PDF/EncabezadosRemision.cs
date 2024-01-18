using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.PDF
{
    public class EncabezadosRemision: PdfPageEventHelper
    {


        public PdfPTable tableEnc(PdfPTable table, String contenido)
        {
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 6, 1, new BaseColor(255, 255, 255));
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BackgroundColor = new BaseColor(2, 136, 150), BorderColor = new BaseColor(255, 255, 255) };
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.VerticalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 10;
            celda.Border = 15;
            table.AddCell(celda);
            return table;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            PdfPTable productosEnc = new PdfPTable(5);
            productosEnc.TotalWidth = document.PageSize.Width - 40;
            productosEnc.SetWidths(new float[] { 0.5F, 2.5F, 0.5F, 0.4F, 1F }); // Ejemplo de valores de ancho relativo
            
            tableEnc(productosEnc, "PARTIDA");
            tableEnc(productosEnc, "DESCRIPCION");
            tableEnc(productosEnc, "IMAGEN");
            tableEnc(productosEnc, "CANT.");
            tableEnc(productosEnc, "ENTREGO");
         

            if (document.PageNumber >= 2)
            {
                productosEnc.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 21, writer.DirectContent);
            }

        }

    }
}