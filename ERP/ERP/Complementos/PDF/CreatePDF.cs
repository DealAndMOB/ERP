using Humanizer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Image = iTextSharp.text.Image;
using Font = iTextSharp.text.Font;
using ERP.Models;
using System.IO;
using System.Xml.Linq;
using ERP.Vistas.Ventas;

namespace ERP.Complementos.PDF
{
    public class CreatePDF
    {
        private string rutaFondoPDF;
        private string rutaMultimedia;
        private CotizacionDTO DatosCotizacion;
        private string TipoDocumento;

        public CreatePDF(CotizacionDTO DatosCotizacion, string rutaFondoPDF, string rutaMultimedia, string tipoDocumento)
        {
            this.rutaFondoPDF = rutaFondoPDF;
            this.DatosCotizacion = DatosCotizacion;
            this.rutaMultimedia = rutaMultimedia;
            this.TipoDocumento = tipoDocumento;
        }

        public Estructura Create()
        {
            //se genera el documento y se establecen los margenes
            Document doc = new Document(PageSize.LETTER, 10f, 10f, 110f, 50f);
            PdfWriter writer = PdfWriter.GetInstance(doc, HttpContext.Current.Response.OutputStream);

            String PathImageM = this.rutaFondoPDF;
            writer.PageEvent = new HeaderFooter(PathImageM);
            writer.PageEvent = new EncabezadosCot();
            writer.PageEvent = new CustomPageEvent1();


            doc.AddTitle($"{TipoDocumento}: {DatosCotizacion.Folio}");
            doc.AddAuthor("Dilan");
            doc.Open();

            //declarar fuentes para usar
            Font font1 = FontFactory.GetFont("Calibri", 8);
            Font fontCantletra = FontFactory.GetFont("Calibri", 10, 1, new BaseColor(255, 255, 255));
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 9, 0, BaseColor.BLACK);
            Font fontCot = FontFactory.GetFont("Verdana", 11, 1, new BaseColor(196, 88, 17));
            Font fontFecha = FontFactory.GetFont("Arial", 11, 1);


            PdfPTable Folio = new PdfPTable(1);
            Folio.WidthPercentage = 100;
            string tituloDocumento = $"{TipoDocumento.ToUpper()}: {DatosCotizacion.Folio}";
            PdfPCell casillaFolio = new PdfPCell(new Paragraph(tituloDocumento, fontCot));
            casillaFolio.Border = 0;
            casillaFolio.HorizontalAlignment = Element.ALIGN_RIGHT;
            casillaFolio.VerticalAlignment = Element.ALIGN_MIDDLE;
            Folio.AddCell(casillaFolio);
            doc.Add(Folio);


            //se crean variables para guardar el cliente y su direccion 
            string nombre = DatosCotizacion.Cliente;
            string direccion = DatosCotizacion.Direccion;
            //se imprimen en una tabla
            PdfPTable cliente = new PdfPTable(1);
            cliente.WidthPercentage = 100;
            tableDatos(cliente, $"Nombre Cliente: {nombre} \n Direccion Cliente: {direccion}");
            doc.Add(cliente);

            //se crean variables para guardar la fecha e imprimirla en el pdf
            string fechaLarga = DatosCotizacion.FechaAlterada.Equals(string.Empty)? 
            (TipoDocumento.Equals("COTIZACIÓN") ? DatosCotizacion.FechaCotizacion.ToLongDateString() : DatosCotizacion.FechaVenta.ToLongDateString())
            : DatosCotizacion.FechaAlterada.ToString();

            //se imprime en una tabla
            PdfPTable Fecha = new PdfPTable(1);
            Fecha.WidthPercentage = 100;
            PdfPCell casillaFecha = new PdfPCell(new Paragraph($"Tecámac, Estado de Mexico a {fechaLarga}", fontFecha));
            casillaFecha.Border = 0;
            casillaFecha.PaddingLeft = 260;
            Fecha.AddCell(casillaFecha);
            doc.Add(Fecha);

            doc.Add(new Paragraph("\n"));

            //tabla encabezado
            PdfPTable enc = new PdfPTable(new float[] { 14f, 70f, 20f, 10f, 17f, 17f });
            enc.WidthPercentage = 100;
            enc.DefaultCell.Border = 0;
            tableEnc(enc, "PARTIDA");
            tableEnc(enc, "DESCRIPCION");
            tableEnc(enc, "IMAGEN");
            tableEnc(enc, "CANT.");
            tableEnc(enc, "P.U");
            tableEnc(enc, "TOTAL");
            doc.Add(enc);

            PdfPTable cot = new PdfPTable(new float[] { 14f, 70f, 20f, 10f, 17f, 17f });
            cot.WidthPercentage = 100;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            for (int x = 0; x < DatosCotizacion.Partidas.Count; x++)
            {

                table(cot, DatosCotizacion.Partidas[x].Partida.ToString());
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                String descripcionProducto = DatosCotizacion.Partidas[x].DescripProducto.ToString();

                PdfPCell cell = new PdfPCell(new Paragraph($"{descripcionProducto}", font1)) { BorderColor = new BaseColor(2, 204, 226) };
                cell.Padding = 10;
                cell.SetLeading(0, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cot.AddCell(cell);

                Image image = Image.GetInstance(rutaMultimedia + DatosCotizacion.Partidas[x].Imagen.ToString());
                image.ScaleAbsolute(42, 60);
                cell = new PdfPCell(image) { BorderColor = new BaseColor(2, 204, 226) };
                cell.Padding = 10;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cot.AddCell(cell);

                table(cot, DatosCotizacion.Partidas[x].Cantidad.ToString());
                table(cot, $"{DatosCotizacion.Partidas[x].PrecioFormato}");

                table(cot, $"{Convert.ToSingle(DatosCotizacion.Partidas[x].TotalFormato.Substring(1)).ToString("C", new CultureInfo("es-MX"))}");

            }
            doc.Add(cot);


            string[] numerosD = DatosCotizacion.Total.ToString("0.00").Split('.');
            int numeroUno = int.Parse(numerosD[0]);
            int numeroDos = int.Parse(numerosD[1]);

            PdfPTable Total = new PdfPTable(new float[] { 84f, 47f, 17f });
            Total.WidthPercentage = 100;
            // the cell object
            string cantidadEnLetra = $"{numeroUno.ToWords()} Pesos {numeroDos}/100 MN";
            string cantidadEnLetraCapitalizada = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cantidadEnLetra.ToLower());

            PdfPCell Totales = new PdfPCell(new Paragraph($"CANTIDAD CON LETRA \n ({cantidadEnLetraCapitalizada})", fontCantletra)) { BackgroundColor = new BaseColor(2, 136, 150), BorderColor = new BaseColor(255, 255, 255) };
            Totales.Rowspan = 3;
            Totales.HorizontalAlignment = Element.ALIGN_CENTER;
            Totales.VerticalAlignment = Element.ALIGN_MIDDLE;
            Total.AddCell(Totales);

            table2(Total, "SUBTOTAL");
            table2(Total, $"{DatosCotizacion.SubTotal.ToString("C", new CultureInfo("es-MX"))}");
            table2(Total, "IVA");
            table2(Total, $"{DatosCotizacion.DiferenciaIVA.ToString("C", new CultureInfo("es-MX"))}"); table2(Total, "TOTAL");
            table2(Total, $"{DatosCotizacion.Total.ToString("C", new CultureInfo("es-MX"))}");

            doc.Add(Total);
            PdfPTable Condiciones = new PdfPTable(1);
            Condiciones.WidthPercentage = 100;
            PdfPCell cond = new PdfPCell(new Paragraph($"\n CONDICIONES DE VENTA: \n \n{DatosCotizacion.Condiciones} \n \n \n \n \n \n \n \n \n \n", font)) { BorderColor = new BaseColor(13, 146, 173), BorderWidth = 2f };
            Condiciones.AddCell(cond);
            doc.Add(Condiciones);

            writer.PageEvent = null;
            writer.PageEvent = new HeaderFooter(PathImageM);
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
            celda.Padding = 10;
            table.AddCell(celda);
            return table;
        }

        public PdfPTable table2(PdfPTable table, String contenido)
        {
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 9, 1, new BaseColor(255, 255, 255));
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BackgroundColor = new BaseColor(2, 136, 150), BorderColor = new BaseColor(255, 255, 255) };
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 1;
            celda.VerticalAlignment = Element.ALIGN_MIDDLE;
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
            celda.Padding = 5;
            table.AddCell(celda);
            return table;
        }

        public class Estructura
        {
            public Document doc { get; set; }
            public PdfWriter writer { get; set; }
        };

    }

    public class CustomPageEvent1 : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            int pageNumber = document.PageNumber;

            if (pageNumber >= 1)
            {
                document.SetMargins(10f, 10f, 130f, 50f);
            }
            base.OnStartPage(writer, document);
        }
    }
}