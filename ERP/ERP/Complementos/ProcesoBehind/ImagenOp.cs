using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;

namespace ERP.Complementos.ProcesoBehind
{
    public class ImagenOp
    {
        private string ruta, mensaje = string.Empty;
        private FileUpload File;
        public ImagenOp(FileUpload File, string ruta)
        {
            this.ruta = ruta;
            this.File = File;
        }

        public string ValidarImagen(uint TamañoMax)
        {
            // Recoleccion de datos de la imagen - - - - - - - - - - - - - - - - - - - - - - - -
            string extension = Path.GetExtension(File.FileName); // Extención del archivo
            int tamaño = File.PostedFile.ContentLength; // Peso del archivo

            if (!IsImageExtension(extension))
            {
                return mensaje = "¡El formato de la imagen es incompatible!";
            }

            if (tamaño > TamañoMax)
            {
                return mensaje = "¡La imagen exede los 3MB!";
            }

            return mensaje;
        }
        public void SavePNG(string NombreArchivo)
        {
            // Conversion a JPG -> Cargar la imagen en un objeto de tipo Image
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(File.PostedFile.InputStream))
            {
                // Crear un objeto de tipo Bitmap con el mismo tamaño que la imagen original
                using (Bitmap bmp = new Bitmap(img.Width, img.Height))
                {
                    // Dibujar la imagen original en el objeto Bitmap
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(img, 0, 0, img.Width, img.Height);
                    }

                    //Guardar el objeto Bitmap como JPEG en el servidor
                    bmp.Save(Path.Combine(ruta, $"{NombreArchivo}.jpg"), ImageFormat.Jpeg);
                }
            }
        }

        private bool IsImageExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".bmp":
                case ".png":
                case ".jfif":
                //case ".webp":
                //case ".tiff":
                //case ".ico":
                //case ".svg":
                    return true;
                default:
                    return false;
            }
        }

    }
}