using ERP.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ERP.Complementos.PDF
{
    public class PDFremision
    {
        private string rutaFondoPDF;
        private string rutaMultimedia;
        private RemisionDTO Remision;

        public PDFremision(RemisionDTO Remision, string rutaFondoPDF, string rutaMultimedia)
        {
            this.rutaFondoPDF = rutaFondoPDF;
            this.Remision = Remision;
            this.rutaMultimedia = rutaMultimedia;
        }

        public Estructura CrearPDFremision()
        {
            // Se genera el documento y se establecen los márgenes
            Document doc = new Document(PageSize.LETTER, 20f, 20f, 110f, 80f);
            PdfWriter writer = PdfWriter.GetInstance(doc, HttpContext.Current.Response.OutputStream);

            Font font = FontFactory.GetFont("Verdana", 12, 0);
            Font font2 = FontFactory.GetFont("Verdana", 11, 1);
            Font fontRemision = FontFactory.GetFont("Verdana", 11, 1, new BaseColor(196, 88, 17));
            Font fontTabla = FontFactory.GetFont("Calibri", 8);

            String PathImageM = this.rutaFondoPDF;
            writer.PageEvent = new HandF(PathImageM);
            writer.PageEvent = new EncabezadosRemision();
            writer.PageEvent = new CustomPageEvent();
            doc.AddTitle("Reporte");
            doc.AddAuthor("AGC");
            doc.Open();


            PdfPTable Folio = new PdfPTable(1);
            PdfPCell casillaFolio = new PdfPCell(new Paragraph("REMISION: " + Remision.Folio.ToString(), fontRemision));
            casillaFolio.Border = 0;
            casillaFolio.HorizontalAlignment = Element.ALIGN_RIGHT;
            casillaFolio.VerticalAlignment = Element.ALIGN_MIDDLE;
            casillaFolio.PaddingRight = 20; // Ajusta el espacio a la derecha
            Folio.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            Folio.AddCell(casillaFolio);
            doc.Add(Folio);


            //se crean variables para guardar el cliente y su direccion 
            string nombre = Remision.Cliente;
            string direccion = Remision.Direccion;
            PdfPTable cliente = new PdfPTable(1);
            cliente.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            tableDatos(cliente, $"Nombre Cliente: {nombre} \n Direccion Cliente: {direccion}");
            doc.Add(cliente);

            //se crean variables para guardar la fecha e imprimirla en el pdf
            string fechaLarga = Remision.FechaAlterada.Equals(string.Empty) ?
                   Remision.FechaEntrega.ToLongDateString() : Remision.FechaAlterada.ToString();

            //String fechaLarga = fecha.ToLongDateString();
            PdfPTable Fecha = new PdfPTable(1);
            PdfPCell casillaFecha = new PdfPCell(new Paragraph($"Tecámac, Estado de Mexico a {fechaLarga}", font2));
            casillaFecha.Border = 0;
            casillaFecha.HorizontalAlignment = Element.ALIGN_RIGHT;
            casillaFecha.PaddingRight = 20; // Ajusta el espacio a la derecha
            Fecha.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            Fecha.AddCell(casillaFecha);
            doc.Add(Fecha);

            doc.Add(new Paragraph("\n"));

            string instruccion = "Por medio de la presente hago entrega de los siguientes artículos:";
            Paragraph inst = new Paragraph(instruccion, font);
            inst.Alignment = Element.ALIGN_CENTER;
            doc.Add(inst);

            doc.Add(new Paragraph("\n"));


            PdfPTable productosEnc = new PdfPTable(5);
            productosEnc.WidthPercentage = 100;
            productosEnc.SetWidths(new float[] { 0.5F, 2.5F, 0.5F, 0.4F, 1F }); // Ejemplo de valores de ancho relativo

            tableEnc(productosEnc, "PARTIDA");
            tableEnc(productosEnc, "DESCRIPCION");
            tableEnc(productosEnc, "IMAGEN");
            tableEnc(productosEnc, "CANT.");
            tableEnc(productosEnc, "ENTREGO");
            doc.Add(productosEnc);

            PdfPTable productos = new PdfPTable(5);
            productos.WidthPercentage = 100;
            productos.SetWidths(new float[] { 0.5F, 2.5F, 0.5F, 0.4F, 1F }); // Ejemplo de valores de ancho relativo

            foreach (var tabla in Remision.partidasRemision)
            {
                table(productos, tabla.Partida.ToString());

                PdfPCell cell = new PdfPCell();
                Paragraph descripcion = new Paragraph(tabla.Descripcion.ToString(),fontTabla);
                descripcion.Alignment = Element.ALIGN_JUSTIFIED;
                cell.AddElement(descripcion);
                productos.AddCell(cell);

                Image image = Image.GetInstance(rutaMultimedia + tabla.Imagen.ToString());
                image.ScaleAbsolute(30, 48);
                cell = new PdfPCell(image) { BorderColor = new BaseColor(2, 204, 226) };
                cell.Padding = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                productos.AddCell(cell);
                table(productos, tabla.Cantidad.ToString());
                table(productos, " ");
            }

            doc.Add(productos);

            doc.Add(new Paragraph("\n"));

            string texto = "Por lo cual, me hago responsable de recibir los artículos y/o mobiliarios nuevos y en buenas condiciones,\n" +
                "entregados por 'AGC COMERCIAL', anexo en este documento, ha sido revisado y aprobado por mí,\n" +
                "el cual cuenta con mi firma.";

            Paragraph paragraph = new Paragraph(texto, font);
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(paragraph);

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));

            // Crear la tabla de firmas
            PdfPTable tablaFirmas = new PdfPTable(3);
            tablaFirmas.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            tablaFirmas.DefaultCell.Border = PdfPCell.NO_BORDER; // Elimina los bordes de las celdas por defecto

            // Agregar la primera firma
            PdfPCell firma1 = new PdfPCell(new Paragraph("RECIBIO \n NOMBRE Y FIRMA", font));
            firma1.HorizontalAlignment = Element.ALIGN_CENTER;
            firma1.VerticalAlignment = Element.ALIGN_MIDDLE;
            firma1.Border = PdfPCell.TOP_BORDER; // Agrega solo el borde superior a la celda de firma
            firma1.PaddingTop = 10; // Ajusta el espacio superior de la celda de firma
            tablaFirmas.AddCell(firma1);

            // Agregar una celda en blanco como espacio entre las firmas
            PdfPCell espacio = new PdfPCell();
            espacio.Border = PdfPCell.NO_BORDER;
            espacio.FixedHeight = 20; // Ajusta el espacio entre las firmas
            tablaFirmas.AddCell(espacio);

            // Agregar la segunda firma
            PdfPCell firma2 = new PdfPCell(new Paragraph("ING. ANTONIO AGUILAR CRUZ \n DIRECTOR GENERAL", font));
            firma2.HorizontalAlignment = Element.ALIGN_CENTER;
            firma2.VerticalAlignment = Element.ALIGN_MIDDLE;
            firma2.Border = PdfPCell.TOP_BORDER; // Agrega solo el borde superior a la celda de firma
            firma2.PaddingTop = 10; // Ajusta el espacio superior de la celda de firma
            tablaFirmas.AddCell(firma2);

            doc.Add(tablaFirmas);

            writer.PageEvent = null;
            writer.PageEvent = new HandF(PathImageM);
            writer.PageEvent = new CustomPageEvent();

            doc.Close();

            Estructura estructura = new Estructura()
            {
                doc = doc,
                writer = writer,
            };

            return estructura;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public PdfPTable table(PdfPTable table, String contenido)
        {
            Font font = FontFactory.GetFont("Calibri", 8);
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BorderColor = new BaseColor(2, 204, 226) };
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda.Padding = 0;
            table.AddCell(celda);
            return table;
        }

        public PdfPTable tableDatos(PdfPTable table, String contenido)
        {
            Font fontDireccion = FontFactory.GetFont("Arial", 10, 1);
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, fontDireccion)) { BorderColor = new BaseColor(13, 146, 173), BorderWidth = 2f };
            celda.FixedHeight = 50;
            celda.PaddingLeft = 5;
            celda.SetLeading(0, 1.5f);
            celda.HorizontalAlignment = Element.ALIGN_LEFT;
            celda.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(celda);
            return table;
        }

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

        public class Estructura
        {
            public Document doc { get; set; }
            public PdfWriter writer { get; set; }
        };
    }


    public class CustomPageEvent : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            int pageNumber = document.PageNumber;

            if (pageNumber >= 1)
            {
                document.SetMargins(20f, 20f, 150f, 80f);
            }
            base.OnStartPage(writer, document);
        }
    }
}