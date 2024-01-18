using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.PDF
{
    //clase para el header y el footer 
    public class HeaderFooter : PdfPageEventHelper
    {

        String PathImageM = null;
        public HeaderFooter(String MarcaImagen)
        {
            PathImageM = MarcaImagen;
        }


        public override void OnEndPage(PdfWriter writer, Document document)
        {
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 12, 1, BaseColor.BLACK);

            BaseFont fuente1 = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, BaseFont.EMBEDDED);
            Font font1 = new Font(fuente1, 7, 0, BaseColor.BLACK);
            //imagen
            Image image = Image.GetInstance(PathImageM);
            image.ScaleAbsolute(613, 792);
            image.SetAbsolutePosition(0, 0);
            document.Add(image);

            //Tabla para el Header
            PdfPTable header = new PdfPTable(3);
            header.TotalWidth = document.PageSize.Width + 2;
            header.DefaultCell.Border = 0;

            //primera celda
            PdfPCell celda = new PdfPCell(new Paragraph("ANTONIO AGUILAR CRUZ", font));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Border = 0;
            header.AddCell(celda);


            header.AddCell(new Paragraph());

            PdfPCell Rfc = new PdfPCell(new Paragraph("AUCA830428VE3", font));
            Rfc.HorizontalAlignment = Element.ALIGN_CENTER;
            Rfc.Border = 0;
            header.AddCell(Rfc);




            if (document.PageNumber >= 2)
            {
                header.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 37, writer.DirectContent);
            }
            else
            {
                header.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 17, writer.DirectContent);
            }



            //int paginas = document.PageNumber;
            //document.Add(new Phrase(paginas.ToString()));
            PdfContentByte pos = writer.DirectContent;
            pos.MoveTo(50, 50);
            pos.LineTo(544, 50);
            pos.SetColorStroke(BaseColor.BLACK);
            pos.SetLineWidth(2.5);
            pos.ClosePathStroke();


            //footer primera tabla
            PdfPTable footer1 = new PdfPTable(1);
            footer1.TotalWidth = document.PageSize.Width;
            footer1.DefaultCell.Border = 0;


            celda = new PdfPCell(new Paragraph("C. Canal del Norte Mz1. Lte.5 Col. Sta. Ma. Ozumbilla, Tecámac, Edo. Mex C.P. 55760", font1));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Border = 0;
            footer1.AddCell(celda);
            footer1.WriteSelectedRows(0, -1, document.RightMargin, writer.PageSize.GetBottom(document.BottomMargin), writer.DirectContent);

            //footer segunda tabla
            PdfPTable footer = new PdfPTable(2);
            footer.TotalWidth = document.PageSize.Width;
            footer.DefaultCell.Border = 0;

            celda = new PdfPCell(new Paragraph("Teléfono: 554 386 4869", font1));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Border = 0;

            footer.AddCell(celda);


            //celda = new PdfPCell(new Paragraph("Pagina" + " " + writer.PageNumber));
            PdfPCell celda2 = new PdfPCell(new Paragraph("Mail: agc_comercial@hotmail.com", font1));
            celda2.HorizontalAlignment = Element.ALIGN_CENTER;
            celda2.Border = 0;

            footer.AddCell(celda2);
            footer.WriteSelectedRows(0, -1, document.RightMargin, writer.PageSize.GetBottom(document.BottomMargin) - 15, writer.DirectContent);
        }
    }
}