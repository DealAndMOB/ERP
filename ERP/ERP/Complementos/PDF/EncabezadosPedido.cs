using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.PDF
{
    public class EncabezadosPedido: PdfPageEventHelper
    {
        public PdfPTable tableEnc(PdfPTable table, String contenido)
        {
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 7, 0, new BaseColor(0, 0, 0));
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BackgroundColor = new BaseColor(217, 217, 217), BorderColor = new BaseColor(0, 0, 0) };
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.VerticalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 5;
            celda.Border = 15;
            table.AddCell(celda);
            return table;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            PdfPTable tablaEnc = new PdfPTable(new float[] { 0.5f, 2, 0.5f, 0.5f, 0.5f, 0.7f });
            tablaEnc.TotalWidth = document.PageSize.Width -60;
            tablaEnc.DefaultCell.Border = 0;
            tableEnc(tablaEnc, "IMAGEN");
            tableEnc(tablaEnc, "DESCRIPCION");
            tableEnc(tablaEnc, "ORDENADO");
            tableEnc(tablaEnc, "U.M");
            tableEnc(tablaEnc, "PRECIO");
            tableEnc(tablaEnc, "IMPORTE");

            if (document.PageNumber >= 2)
            {
                tablaEnc.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 15, writer.DirectContent);
            }

        }

    }
}