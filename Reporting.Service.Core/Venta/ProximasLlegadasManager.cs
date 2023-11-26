using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class ProximasLlegadasManager : DataRepository
    {
        public ProximasLlegadasManager(string cadena)
            : base(cadena)
        {

        }

        public List<Anexo> GetAnexos(string Sku)
        {
            List<Anexo> anexos = new List<Anexo>();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnexos");
                this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);

                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var path = dr["Path"] == DBNull.Value ? "" : (string)dr["Path"];
                    var filename = dr["FileName"] == DBNull.Value ? "" : (string)dr["FileName"];
                    var fileext = dr["FileExt"] == DBNull.Value ? "" : (string)dr["FileExt"];
                    var subpath = dr["SubPath"] == DBNull.Value ? "" : (string)dr["SubPath"];
                    anexos.Add(new Anexo
                    {
                        Identifier = (string)dr["ItemCode"],
                        Path = path,
                        FileName = filename,
                        FileExt = fileext,
                        SubPath = subpath
                    });
                }
                cmd.Dispose();
                dr.Close();
                dr.Dispose();

                return anexos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Producto> GetProductosEnTransito(int EnProduccion)
        {
            DbCommand storedProcCommand = this.Database.GetStoredProcCommand("prGetProximasLlegadasImportaciones");
            this.Database.AddInParameter(storedProcCommand, "@EnProduccion", DbType.Int32, (object)EnProduccion);
            IDataReader dataReader = this.Database.ExecuteReader(storedProcCommand);
            List<Producto> productoList = new List<Producto>();
            while (dataReader.Read())
                productoList.Add(new Producto()
                {
                    Sku = dataReader["Sku"] == DBNull.Value ? "" : (string)dataReader["Sku"],
                    Nombre = dataReader["Articulo"] == DBNull.Value ? "" : (string)dataReader["Articulo"],
                    Contenedor = dataReader["Contedor"] == DBNull.Value ? "Sin Contenedor" : (string)dataReader["Contedor"],
                    Envio = dataReader["Envio"] == DBNull.Value ? 0 : (int)dataReader["Envio"],
                    FechaLlegada = dataReader["FechaAproxCEDIS"] == DBNull.Value ? "Sin fecha" : (string)dataReader["FechaAproxCEDIS"],
                    Cantidad = dataReader["QtyPendiente"] == DBNull.Value ? 0 : (int)dataReader["QtyPendiente"],                    
                    Nom = dataReader["NOM"] == DBNull.Value ? "" : (string)dataReader["NOM"],
                    VencimientoNom = dataReader["VencimientoNom"] == DBNull.Value ? null : (DateTime?)dataReader["VencimientoNom"],
                    FraccionArancelaria = dataReader["FraccionArancelaria"] == DBNull.Value ? "" : (string)dataReader["FraccionArancelaria"],
                    Certificado = dataReader["Certificado"] == DBNull.Value ? "" : (string)dataReader["Certificado"],
                    EmisionCertificado = dataReader["EmisionCertificado"] == DBNull.Value ? null : (DateTime?)dataReader["EmisionCertificado"],

                });
            return productoList;
        }
    }
}
