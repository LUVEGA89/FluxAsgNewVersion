using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.BardCode
{
    public class BardCodeManager : Catalog<BardCode, int, BardCodeCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override BardCode LoadItem(IDataReader dr)
        {
            BardCode bardCode = new BardCode();
            
                bardCode.Identifier = (int)dr["DocNum"];
            /*
            ItemCode = (string)dr["ItemCode"],
            Descripcion = (string)dr["Dscription"],
            Imagen = (string)dr["PicturName"],
            CodeBars = (string)dr["CodeBars"],
            U_CBInner = (string)dr["Inner"],
            U_CBMaster = (string)dr["Master"],
            Base64 = this.GenerateBardCode((string)dr["CodeBars"], (string)dr["ItemCode"], TYPE.CODE39, "CodeBars"),
            Base64Inner = this.GenerateBardCode((string)dr["Inner"], (string)dr["ItemCode"], TYPE.CODE39, "Inner"),
            Base64Master = this.GenerateBardCode((string)dr["Master"], (string)dr["ItemCode"], TYPE.CODE39, "Master")
            */

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    BardCodeItem item = new BardCodeItem()
                    {
                        ItemCode = (string)dr["ItemCode"],
                        Descripcion = (string)dr["Dscription"],
                        Imagen = DBNull.Value == null ? "" : (string)dr["PicturName"],
                        CodeBars = (string)dr["CodeBars"],
                        U_CBInner = (string)dr["Inner"],
                        U_CBMaster = (string)dr["Master"],
                        Base64 = this.GenerateBardCode((string)dr["CodeBars"], (string)dr["ItemCode"], TYPE.CODE39, "CodeBars"),
                        Base64Inner = this.GenerateBardCode((string)dr["Inner"], (string)dr["ItemCode"], TYPE.CODE39, "Inner"),
                        Base64Master = this.GenerateBardCode((string)dr["Master"], (string)dr["ItemCode"], TYPE.CODE39, "Master")
                    };
                    bardCode.AddItemImagen(item);
                }
            }

            return bardCode;
        }

        protected override DbCommand PrepareAddStatement(BardCode item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBardCode");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(BardCode item)
        {
            throw new NotImplementedException();
        }

        public string GenerateBardCode(string codigo, string sku, TYPE type, string carpeta)
        {
            try
            {
                System.Drawing.Image image = null;

                Barcode b = new Barcode();
                b.BackColor = Color.White;//Color del fondo de la imagen
                b.ForeColor = Color.Black;//Color del codigo de barras
                b.IncludeLabel = true;
                b.Alignment = AlignmentPositions.CENTER;
                b.LabelPosition = LabelPositions.BOTTOMCENTER;//Posision de visualizacion del código
                b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;//Formato de imagen
                Font font = new Font("verdana", 15f);//Fuente
                b.LabelFont = font;
                b.Height = 360;//Altura de la imagen

                switch (carpeta)
                {
                    case "CodeBars":
                        b.Width = 560;//Ancho de la imagen
                        break;
                    case "Inner":
                        b.Width = 610;//Ancho de la imagen
                        break;
                    case "Master":
                        b.Width = 610;//Ancho de la imagen
                        break;
                    default:
                        break;

                }
               
                image = b.Encode(type, codigo);//Genera la imagen

                var directorio = AppDomain.CurrentDomain.BaseDirectory;
                string Imag = $"{directorio}BardCode\\{carpeta}\\{sku}.jpg";
                image.Save(Imag, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] imageArray = File.ReadAllBytes(Imag);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                return base64ImageRepresentation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
