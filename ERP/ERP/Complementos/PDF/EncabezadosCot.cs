using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.PDF
{
    //clase para el header y el footer 
    public class EncabezadosCot : PdfPageEventHelper
    {

        public PdfPTable tableEnc(PdfPTable table, String contenido)
        {
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 6, 1, new BaseColor(255, 255, 255));
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BackgroundColor = new BaseColor(2, 136, 150), BorderColor = new BaseColor(255, 255, 255) };
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.VerticalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 5;
            celda.Border = 15;
            table.AddCell(celda);
            return table;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            //tabla encabezado
            PdfPTable enc = new PdfPTable(new float[] { 14f, 70f, 20f, 10f, 17f, 17f });
            enc.TotalWidth = document.PageSize.Width - 17;
            enc.DefaultCell.Border = 0;
            tableEnc(enc, "PARTIDA");
            tableEnc(enc, "DESCRIPCION");
            tableEnc(enc, "IMAGEN");
            tableEnc(enc, "CANT.");
            tableEnc(enc, "P.U");
            tableEnc(enc, "TOTAL");

            if (document.PageNumber >= 2)
            {
                enc.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 15, writer.DirectContent);
            }

        }

    }
}