using ERP.Models;
using Humanizer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace ERP.Complementos.PDF
{
    public class PDFReporte
    {
        private string rutaFondoPDF;
        private List<DatosReportePartidas> Partidas;

        public PDFReporte(List<DatosReportePartidas> Partidas, string rutaFondoPDF)
        {
            this.rutaFondoPDF = rutaFondoPDF;
            this.Partidas = Partidas;
        }

        public Estructura CrearPDF()
        {
            // Se genera el documento y se establecen los márgenes
            Document doc = new Document(PageSize.LETTER, 20f, 40f, 110f, 80f);

            PdfWriter writer = PdfWriter.GetInstance(doc, HttpContext.Current.Response.OutputStream);
            BaseFont fuente = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            Font font = new Font(fuente, 11, 1, BaseColor.BLACK);
            Font fontReporte = FontFactory.GetFont("Verdana", 11, 1, new BaseColor(196, 88, 17));

            BaseFont fuente2 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            Font font2 = new Font(fuente2, 12, 0, BaseColor.BLACK);

            String PathImageM = this.rutaFondoPDF;
            writer.PageEvent = new HandF(PathImageM);

            doc.AddTitle("Reporte");
            doc.AddAuthor("AGC");
            doc.Open();

            doc.Add(new Paragraph("\n"));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            DatosReportePartidas ObtenerFolio = Partidas.FirstOrDefault();
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            // Agregar título del reporte
            PdfPTable tablaTitulo = new PdfPTable(1);
            PdfPCell casillaFolio = new PdfPCell(new Paragraph("ESTE REPORTE PERTENECE A LA VENTA CON EL FOLIO DE: " + ObtenerFolio.Folio, fontReporte));
            casillaFolio.Border = 0;
            tablaTitulo.AddCell(casillaFolio);
            doc.Add(tablaTitulo);
            doc.Add(new Paragraph("\n"));

            // Se crean variables para guardar la fecha e imprimirla en el PDF
            DateTime fechaCot = DateTime.Now;
            String fechaLarga = fechaCot.ToLongDateString();

            // Fecha del informe
            PdfPTable Fecha = new PdfPTable(1);
            PdfPCell casillaFecha = new PdfPCell(new Paragraph($"Fecha del informe: Tecámac, Estado de México a {fechaLarga}", font));
            casillaFecha.Border = 0;
            casillaFecha.PaddingLeft = 15;
            Fecha.AddCell(casillaFecha);
            doc.Add(Fecha);

            doc.Add(new Paragraph("\n"));

            // I. Unidades Vendidas por Partida
            doc.Add(new Paragraph("I. Unidades Vendidas por Partida:\n\n", font));

            // Explicación de Unidades Vendidas por Partida
            string explicacionUnidadesVendidas = "En la siguiente tabla se muestra el detalle de las unidades vendidas por partida. Cada partida representa una categoría con su tipo de producto comercializado. \n\n";
            doc.Add(new Paragraph(explicacionUnidadesVendidas, font2));

            PdfPTable tablaUnidadesVendidasEnc = new PdfPTable(3);
            tableEnc(tablaUnidadesVendidasEnc, "Partida");
            tableEnc(tablaUnidadesVendidasEnc, "Producto");
            tableEnc(tablaUnidadesVendidasEnc, "Unidades Vendidas");
            doc.Add(tablaUnidadesVendidasEnc);

            PdfPTable tablaUnidadesVendidas = new PdfPTable(3);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            foreach (var partida in Partidas)
            {
                table(tablaUnidadesVendidas, partida.Partida.ToString());
                table(tablaUnidadesVendidas, partida.NombreProducto.ToString());
                table(tablaUnidadesVendidas, partida.Unidades.ToString());
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            doc.Add(tablaUnidadesVendidas);

            doc.Add(new Paragraph("\n"));

            // II. Costo Total por Partida
            doc.Add(new Paragraph("II. Costo Total por Partida:\n\n", font));

            // Explicación de Costo Total por Partida
            string explicacionCostoTotal = "La siguiente tabla se muestra el costo total por partida, el cual refleja el costo total incurrido para producir o adquirir las unidades vendidas en cada partida. \n\n";
            doc.Add(new Paragraph(explicacionCostoTotal, font2));

            PdfPTable tablaCostoTotalEnc = new PdfPTable(2);
            tableEnc(tablaCostoTotalEnc, "Partida");
            tableEnc(tablaCostoTotalEnc, "Costo Total");
            doc.Add(tablaCostoTotalEnc);

            PdfPTable tablaCostoTotal = new PdfPTable(2);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            foreach (var partida in Partidas)
            {
                table(tablaCostoTotal, partida.Partida.ToString());
                table(tablaCostoTotal, partida.TotalPartidaCostoFormato.ToString());
            }
            doc.Add(tablaCostoTotal);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


            doc.Add(new Paragraph("\n"));

            // III. Ventas Totales por Partida
            doc.Add(new Paragraph("III. Ventas Totales por Partida:\n\n", font));

            // Explicación de Ventas Totales por Partida
            string explicacionVentasTotales = "La siguiente tabla se muestra las ventas totales generadas por cada partida en la venta analizada. Estos valores representan los ingresos obtenidos por la venta de las unidades correspondientes. Además, se presenta el margen bruto de cada partida, que refleja la rentabilidad de las ventas después de considerar los costos directos asociados con la producción o adquisición de los productos vendidos.\n\n";
            doc.Add(new Paragraph(explicacionVentasTotales, font2));
            PdfPTable tablaVentasTotalesEnc = new PdfPTable(3);
            tableEnc(tablaVentasTotalesEnc, "Partida");
            tableEnc(tablaVentasTotalesEnc, "Ventas Totaless");
            tableEnc(tablaVentasTotalesEnc, "Utilidad Bruta");
            doc.Add(tablaVentasTotalesEnc);
            PdfPTable tablaVentasTotales = new PdfPTable(3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            foreach (var partida in Partidas)
            {
                table(tablaVentasTotales, partida.Partida.ToString());
                table(tablaVentasTotales, partida.TotalPartidaVentaFormato.ToString());
                table(tablaVentasTotales, partida.MargenBruto.ToString());
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            doc.Add(tablaVentasTotales);

            doc.Add(new Paragraph("\n"));

            // IV. Análisis e Interpretación
            doc.Add(new Paragraph("IV. Análisis e Interpretación:\n\n", font));

            // Texto de análisis e interpretación
            string textoAnalisis = "En el ejercicio analizado, se vendieron diferentes partidas de productos, como se muestra en la sección \"Unidades Vendidas por Partida\". Estas partidas representan las diversas categorías o tipos de productos comercializados.\n\n" +
                                   "En cuanto al costo total por partida, se detalla en la sección correspondiente. Este valor refleja el costo total incurrido para producir o adquirir las unidades vendidas en cada partida.\n\n" +
                                   "Por último, en la sección \"Ventas Totales por Partida\", se muestran las ventas totales generadas por cada partida durante el período analizado. Estos valores representan los ingresos obtenidos por la venta de las unidades correspondientes.\n\n" +
                                   "Es importante destacar que estos datos son informativos y pueden ser utilizados para evaluar el rendimiento financiero de cada partida y tomar decisiones estratégicas adecuadas. Además, es recomendable realizar un análisis más detallado considerando factores adicionales como los márgenes de beneficio, los costos indirectos y otros gastos asociados.";

            var paragraph1 = new Paragraph(textoAnalisis, font2);
            paragraph1.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(paragraph1);

            doc.Add(new Paragraph("\n"));

            // Texto de análisis e interpretación
            string textoPA = "En el reporte financiero, se detallan las diferentes partidas y productos vendidos durante el período analizado. Cada partida representa una categoría o tipo específico de producto comercializado.\n\n" +
                               "Para cada partida y producto, se incluyen los siguientes datos:\n\n" +
                               "- Partida: se indica el número o identificador de la partida correspondiente.\n\n" +
                               "- Nombre: se muestra el nombre o descripción del producto.\n\n" +
                               "- Precio Base: se especifica el precio inicial o base del producto antes de cualquier ajuste o descuento.\n\n" +
                               "- Porcentaje Aumentado (PA): se indica el porcentaje de aumento aplicado al precio base. Este porcentaje representa el monto adicional que se agrega al precio base para obtener el precio final.\n\n" +
                               "- Monto Agregado: se calcula aplicando el porcentaje aumentado al precio base. Este monto representa el valor numérico correspondiente al porcentaje de aumento.\n\n" +
                               "- Precio Final: se obtiene sumando el monto agregado al precio base, lo que resulta en el precio final de venta del producto.\n\n" +
                               "Estos datos son importantes para evaluar la rentabilidad de cada partida y producto, así como para determinar los márgenes de beneficio. Además, permiten realizar análisis comparativos y tomar decisiones estratégicas en relación con la fijación de precios y la rentabilidad de cada producto.\n\n" +
                               "A continuación, se presenta información adicional para cada partida y producto:\n\n";

            var paragraph2 = new Paragraph(textoPA, font2);
            paragraph2.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(paragraph2);

            doc.Add(new Paragraph("\n"));

            PdfPTable tablaPartidasEnc = new PdfPTable(6);
            tableEnc(tablaPartidasEnc, "Partida");
            tableEnc(tablaPartidasEnc, "Nombre");
            tableEnc(tablaPartidasEnc, "Precio Base");
            tableEnc(tablaPartidasEnc, "Porcentaje Aumentado (PA)");
            tableEnc(tablaPartidasEnc, "Monto Agregado");
            tableEnc(tablaPartidasEnc, "Precio Final");
            doc.Add(tablaPartidasEnc);

            // Tabla de partidas y productos
            PdfPTable tablaPartidas = new PdfPTable(6); // Número de columnas

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            foreach (var partida in Partidas)
            {
                table(tablaPartidas, partida.Partida.ToString());
                table(tablaPartidas, partida.NombreProducto);
                table(tablaPartidas, partida.BaseFormato);
                table(tablaPartidas, partida.Porcentaje);
                table(tablaPartidas, partida.MontoAumento);
                table(tablaPartidas, partida.PrecioFormato);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            doc.Add(tablaPartidas);

            doc.Add(new Paragraph("\n"));

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            float sumaCosto = Partidas.Sum(p => p.TotalPartidaCosto);
            float C_IVA = Convert.ToSingle(sumaCosto * 0.16);
            float C_Total = sumaCosto + C_IVA;

            float sumaPrecio = Partidas.Sum(p => float.Parse(p.TotalPartidaVenta.ToString()));
            float P_IVA = Convert.ToSingle(sumaPrecio * 0.16);
            float P_Total = sumaPrecio + P_IVA;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            // Tabla de Gastos y Venta Total
            PdfPTable tablaGastos = new PdfPTable(3);
            PdfPTable tablaGastosEnc = new PdfPTable(3);
            string textoGastos = "A continuación se muestra la tabla de Gastos, que incluye el subtotal, el IVA y los gastos totales.\n\n";
            doc.Add(new Paragraph(textoGastos));

            tableEnc(tablaGastosEnc, "Subtotal");
            tableEnc(tablaGastosEnc, "IVA");
            tableEnc(tablaGastosEnc, "Gastos");
            doc.Add(tablaGastosEnc);

            table(tablaGastos, sumaCosto.ToString("C2", new CultureInfo("es-MX")));
            table(tablaGastos, C_IVA.ToString("C2", new CultureInfo("es-MX")));
            table(tablaGastos, C_Total.ToString("C2", new CultureInfo("es-MX")));

            doc.Add(tablaGastos);

            doc.Add(new Paragraph("\n"));

            // Tabla de Gastos y Venta Total
            PdfPTable tablaVentas = new PdfPTable(3);
            PdfPTable tablaVentasEnc = new PdfPTable(3);
            string textoVentas = "A continuación se muestra la tabla de Ventas, que incluye el subtotal, el IVA y los gastos totales.\n\n";
            doc.Add(new Paragraph(textoVentas));

            tableEnc(tablaVentasEnc, "Subtotal");
            tableEnc(tablaVentasEnc, "IVA");
            tableEnc(tablaVentasEnc, "Ventas");
            doc.Add(tablaVentasEnc);

            table(tablaVentas, sumaPrecio.ToString("C2", new CultureInfo("es-MX")));
            table(tablaVentas, P_IVA.ToString("C2", new CultureInfo("es-MX")));
            table(tablaVentas, P_Total.ToString("C2", new CultureInfo("es-MX")));

            doc.Add(tablaVentas);

            doc.Add(new Paragraph("\n"));

            // Tabla de Margen Bruto sin IVA
            PdfPTable tablaMargenSinIVA = new PdfPTable(1);
            PdfPTable tablaMargenSinIVAEnc = new PdfPTable(1);
            string textoMargenSinIVA = "A continuación se muestra la tabla de Ventas, que incluye la Utilidad bruta sin IVA.\n\n";
            doc.Add(new Paragraph(textoMargenSinIVA));
            tableEnc(tablaMargenSinIVAEnc, "Utilidad bruta Total sin IVA");
            doc.Add(tablaMargenSinIVAEnc);
            table(tablaMargenSinIVA, (sumaPrecio-sumaCosto).ToString("C2", new CultureInfo("es-MX")));
            doc.Add(tablaMargenSinIVA);

            // Tabla de Margen Bruto con IVA
            PdfPTable tablaMargenIVA = new PdfPTable(1);
            PdfPTable tablaMargenIVAEnc = new PdfPTable(1);
            string textoMargenIVA = "A continuación se muestra la tabla de Ventas, que incluye la Utilidad bruta con IVA.\n\n";
            doc.Add(new Paragraph(textoMargenIVA));
            tableEnc(tablaMargenIVAEnc, "Utilidad bruta Total con IVA");
            doc.Add(tablaMargenIVAEnc);
            table(tablaMargenIVA, (P_Total - C_Total).ToString("C2", new CultureInfo("es-MX")));
            doc.Add(tablaMargenIVA);

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
            celda.Padding = 3;
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
            Font font = new Font(fuente, 9, 1, new BaseColor(255, 255, 255));
            PdfPCell celda = new PdfPCell(new Paragraph(contenido, font)) { BackgroundColor = new BaseColor(2, 136, 150), BorderColor = new BaseColor(255, 255, 255) };
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.VerticalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 2;
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
}