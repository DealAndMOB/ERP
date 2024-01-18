using Humanizer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Models;
using ERP.Vistas.Catalogos;
using System.Globalization;
using static Humanizer.In;
using System.Xml.Linq;

namespace ERP.Complementos.PDF
{
    public class PedidoPDF
    {
        private string rutaFondoPDF;
        private string rutaMultimedia;
        private PedidoDTO pedidoDTO;

        public PedidoPDF(PedidoDTO pedidoDTO, string rutaFondoPDF, string rutaMultimedia)
        {
            this.rutaFondoPDF = rutaFondoPDF;
            this.pedidoDTO = pedidoDTO;
            this.rutaMultimedia = rutaMultimedia;
        }

        public Estructura CrearPDFpedido()
        {
            //se genera el documento y se establecen los margenes
            Document doc = new Document(PageSize.LETTER, 30f, 30f, 30f, 90f);
            PdfWriter writer = PdfWriter.GetInstance(doc, HttpContext.Current.Response.OutputStream);

            Font font = FontFactory.GetFont("Verdana", 12, 0);
            Font font2 = FontFactory.GetFont("Verdana", 8, 1);
            Font fontTitulo = FontFactory.GetFont("Verdana", 11, 1, new BaseColor(196, 88, 17));
            Font Titulos = FontFactory.GetFont("Verdana", 15, 1, new BaseColor(196, 88, 17));

            String PathImageM = this.rutaFondoPDF;
            writer.PageEvent = new HandFPedido(PathImageM);
            writer.PageEvent = new CustomPageEvent2();
            writer.PageEvent = new EncabezadosPedido();

            doc.AddTitle("Reporte");
            doc.AddAuthor("AGC");
            doc.Open();

            PdfPTable Folio = new PdfPTable(1);
            PdfPCell casillaFolio = new PdfPCell(new Paragraph("ORDEN DE COMPRA: " + pedidoDTO.Folio.ToString(), fontTitulo));
            casillaFolio.Border = 0;
            casillaFolio.HorizontalAlignment = Element.ALIGN_RIGHT;
            casillaFolio.VerticalAlignment = Element.ALIGN_MIDDLE;
            casillaFolio.PaddingRight = 20; // Ajusta el espacio a la derecha
            Folio.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            Folio.AddCell(casillaFolio);
            doc.Add(Folio);

            doc.Add(new Paragraph("\n"));

            PdfPTable tablaTitulo = new PdfPTable(1);
            tablaTitulo.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            PdfPCell celdaTitulo = new PdfPCell(new Paragraph("ORDEN DE COMPRA", Titulos));
            celdaTitulo.Border = 0;
            celdaTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            celdaTitulo.PaddingTop = 10; // Ajusta el espacio superior
            tablaTitulo.AddCell(celdaTitulo);

            doc.Add(tablaTitulo);


            doc.Add(new Paragraph("\n"));

            //se crean variables para guardar la fecha e imprimirla en el pdf
            string fechaLarga = pedidoDTO.FechaAlterada.Equals(string.Empty) ?
                   pedidoDTO.Fecha.ToLongDateString() : pedidoDTO.FechaAlterada.ToString();

            // Crear la tabla de firmas
            PdfPTable tablaDatos = new PdfPTable(3);
            tablaDatos.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento


            // Agregar la primera firma
            PdfPCell dato1 = new PdfPCell(new Paragraph($"FECHA: {fechaLarga}\n\n" +
                                                        $"RFC:{pedidoDTO.RFC}\n\n" +
                                                        $"RAZON SOCIAL:{pedidoDTO.RazonSocial}", font2));
            dato1.HorizontalAlignment = Element.ALIGN_LEFT;
            dato1.Border = 0;
            tablaDatos.AddCell(dato1);

            // Agregar una celda en blanco como espacio entre las firmas
            PdfPCell espacio = new PdfPCell();
            espacio.Border = PdfPCell.NO_BORDER;
            espacio.FixedHeight = 15; // Ajusta el espacio entre las firmas
            tablaDatos.AddCell(espacio);

            // Agregar la segunda firma
            PdfPCell dato2 = new PdfPCell(new Paragraph("DIRECCIÓN: CALLE CANAL DEL NORTE MZ 1.\n\n" +
                                                        "MA. OZUMBILLA, TECAMAC, ESTADO DE\n\n" +
                                                        "MEXICO. C.P 55760.\n\n" +
                                                        "TELEFONO: (55) 43864869\n\n" +
                                                        "E-mail. agc_comercial@hotmail.com\n\n", font2));
            dato2.HorizontalAlignment = Element.ALIGN_LEFT;
            dato2.Border = 0;
            tablaDatos.AddCell(dato2);
            doc.Add(tablaDatos);

            PdfPTable tablaTitulo2 = new PdfPTable(1);
            tablaTitulo2.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            celdaTitulo = new PdfPCell(new Paragraph("PROVEEDORES", Titulos));
            celdaTitulo.Border = 0;
            celdaTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            celdaTitulo.PaddingTop = 10; // Ajusta el espacio superior
            tablaTitulo2.AddCell(celdaTitulo);
            doc.Add(tablaTitulo2);

            // Crear la tabla de firmas
            PdfPTable tablaProveedor = new PdfPTable(3);
            tablaProveedor.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento


            // Agregar la primera firma
            PdfPCell datosProveedor = new PdfPCell(new Paragraph($"NOMBRE: {pedidoDTO.Proveedor}\n\n" +
                                                        $"DIRECCIÓN: {pedidoDTO.Direccion}\n\n" +
                                                        $"TEL: {pedidoDTO.Telefono}\n\n" +
                                                        $"E-mail: {pedidoDTO.Email}", font2));
            datosProveedor.HorizontalAlignment = Element.ALIGN_LEFT;
            datosProveedor.Border = 0;
            tablaProveedor.AddCell(datosProveedor);

            // Agregar una celda en blanco como espacio entre las firmas
            PdfPCell espacioProv = new PdfPCell();
            espacioProv.Border = PdfPCell.NO_BORDER;
            espacioProv.FixedHeight = 15; // Ajusta el espacio entre las firmas
            tablaProveedor.AddCell(espacioProv);

            // Agregar la segunda firma
            PdfPCell logotipo = new PdfPCell();
            tablaProveedor.AddCell(logotipo);
            doc.Add(tablaProveedor);

            doc.Add(new Paragraph("\n"));

            PdfPTable tablaEnc = new PdfPTable(new float[] { 0.5f, 2, 0.5f, 0.5f, 0.5f, 0.7f });

            tableEnc(tablaEnc, "IMAGEN");
            tableEnc(tablaEnc, "DESCRIPCION");
            tableEnc(tablaEnc, "ORDENADO");
            tableEnc(tablaEnc, "U.M");
            tableEnc(tablaEnc, "PRECIO");
            tableEnc(tablaEnc, "IMPORTE");

            // Alinear la tabla al centro
            tablaEnc.HorizontalAlignment = Element.ALIGN_CENTER;

            // Establecer el ancho de la tabla para ocupar todo el margen de la página
            tablaEnc.WidthPercentage = 100;

            doc.Add(tablaEnc);

            PdfPTable Tabla = new PdfPTable(new float[] { 0.5f, 2, 0.5f, 0.5f, 0.5f, 0.7f });

            foreach (var tablaProd in pedidoDTO.Partidas)
            {
                Image image = Image.GetInstance(rutaMultimedia + tablaProd.Imagen.ToString());
                image.ScaleAbsolute(42, 60);
                PdfPCell cell = new PdfPCell(image);
                cell.Padding = 10;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tabla.AddCell(cell);

                table(Tabla, tablaProd.DescripProducto.ToString(), true);
                table(Tabla, "PZAS");
                table(Tabla, tablaProd.Cantidad.ToString());
                table(Tabla, tablaProd.CostoFormato.ToString());
                table(Tabla, tablaProd.TotalFormato.ToString());
            }

            // Alinear la tabla al centro
            Tabla.HorizontalAlignment = Element.ALIGN_CENTER;
            Tabla.WidthPercentage = 100;
            doc.Add(Tabla);




            string[] numerosD = pedidoDTO.Total.ToString("0.00").Split('.');
            int numeroUno = int.Parse(numerosD[0]);
            int numeroDos = int.Parse(numerosD[1]);

            PdfPTable tabla = new PdfPTable(3); // 3 columnas: 2 para cantidad en letra, 1 para cantidad en número

            string cantidadEnLetraTexto = $"{numeroUno.ToWords()} Pesos {numeroDos}/100 MN";
            string cantidadEnLetraCapitalizada = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cantidadEnLetraTexto.ToLower());

            PdfPCell cantidadEnLetra = new PdfPCell(new Paragraph($"CANTIDAD CON LETRA\n({cantidadEnLetraCapitalizada})"));
            cantidadEnLetra.Colspan = 2; // Ocupa las 2 primeras columnas
            cantidadEnLetra.HorizontalAlignment = Element.ALIGN_CENTER;
            cantidadEnLetra.VerticalAlignment = Element.ALIGN_MIDDLE;

            PdfPCell cantidadEnNumero = new PdfPCell(new Paragraph(pedidoDTO.Total.ToString("C", new CultureInfo("es-MX"))));
            cantidadEnNumero.HorizontalAlignment = Element.ALIGN_CENTER;
            cantidadEnNumero.VerticalAlignment = Element.ALIGN_MIDDLE;

            tabla.AddCell(cantidadEnLetra);
            tabla.AddCell(cantidadEnNumero);

            tabla.HorizontalAlignment = Element.ALIGN_CENTER;

            tabla.WidthPercentage = 100;

            doc.Add(tabla);


            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));


            string texto = "SE REQUIERE A LA FIRMA DE ESTA ORDEN DE COMPRA DE AMBAS PARTES PARA LA VALIDEZ DE LA MISMA RESPETANDO" +
                            " LAS CLAUSULAS ACORDADAS Y MENCIONADAS EN LA COTIZACIÓN.";


            Paragraph paragraph = new Paragraph(texto, font2);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            doc.Add(new Paragraph("\n"));

            PdfPTable Condiciones = new PdfPTable(1);
            PdfPCell cond = new PdfPCell(new Paragraph($"{pedidoDTO.Condiciones}", font));
            cond.Border = 0;
            Condiciones.AddCell(cond);

            Condiciones.HorizontalAlignment = Element.ALIGN_CENTER;
            Condiciones.WidthPercentage = 100;
            doc.Add(Condiciones);

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));


            // Crear la tabla de firmas
            PdfPTable tablaFirmas = new PdfPTable(3);
            tablaFirmas.WidthPercentage = 100; // Ajusta el ancho de la tabla al 100% del documento
            tablaFirmas.DefaultCell.Border = PdfPCell.NO_BORDER; // Elimina los bordes de las celdas por defecto

            // Agregar la primera firma
            PdfPCell firma1 = new PdfPCell(new Paragraph($"{pedidoDTO.Contacto} REPRESENTANTE DE {pedidoDTO.RazonSocial}", font));
            firma1.HorizontalAlignment = Element.ALIGN_CENTER;
            firma1.VerticalAlignment = Element.ALIGN_MIDDLE;
            firma1.Border = PdfPCell.TOP_BORDER; // Agrega solo el borde superior a la celda de firma
            firma1.PaddingTop = 10; // Ajusta el espacio superior de la celda de firma
            tablaFirmas.AddCell(firma1);

            // Agregar una celda en blanco como espacio entre las firmas
            espacio = new PdfPCell();
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
            writer.PageEvent = new HandFPedido(PathImageM);
            writer.PageEvent = new CustomPageEvent2();

            doc.Close();

            Estructura estructura = new Estructura()
            {
                doc = doc,
                writer = writer,
            };

            return estructura;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public (PdfPTable, float) table(PdfPTable table, String contenido, bool alinear = false)
        {
            Font font = FontFactory.GetFont("Verdana", 10, 0);
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BorderColor = new BaseColor(0, 0, 0) };

            // Obtener la altura estimada de la celda como la altura de la fila que la contiene
            float alturaEstimada = celda.SpaceCharRatio;

            if (alinear)
            {
                celda.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            }
            else
            {
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            celda.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda.Padding = 3;
            table.AddCell(celda);
            return (table, alturaEstimada);
        }


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

        public class Estructura
        {
            public Document doc { get; set; }
            public PdfWriter writer { get; set; }
        };
    }

    public class CustomPageEvent2 : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            int pageNumber = document.PageNumber;

            if (pageNumber >= 1)
            {
                document.SetMargins(30f, 30f, 120f, 90f);
            }

            base.OnStartPage(writer, document);
        }
    }

}