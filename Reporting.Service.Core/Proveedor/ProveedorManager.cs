using Resporting.Service.Core.Proveedor;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;
using Resporting.Service.Core.ProveedorCarrier;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Resporting.Service.Core.ProveedorAeropuertos;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace Resporting.Service.Core.Proveedor
{

    public class ProveedorManager : Catalog<Proveedor, int, ProveedorFilter>
    {
        protected override string FindPagedItemsProcedure => "prFindProveedor";

        protected override Proveedor LoadItem(IDataReader dr)
        {
            Proveedor item = new Proveedor();
            item.Identifier = (int)dr["Sequence"];
            item.TipoProveedor = dr["TipoDeProveedor"] == DBNull.Value ? 0 : (int)dr["TipoDeProveedor"];
            item.Rfc = dr["Rfc"] == DBNull.Value ? "" : (string)dr["Rfc"];
            item.Nombre = dr["Nombre"] == DBNull.Value ? "" : (string)dr["Nombre"];
            item.ApellidoPaterno = dr["ApellidoPaterno"] == DBNull.Value ? "" : (string)dr["ApellidoPaterno"];
            item.ApellidoMaterno = dr["ApellidoMaterno"] == DBNull.Value ? "" : (string)dr["ApellidoMaterno"];
            item.IdNacionalidad = dr["IdNacionalidad"] == null ? 0 : (int)dr["IdNacionalidad"];
            item.FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? DateTime.Now : (DateTime)dr["FechaNacimiento"];
            item.NumeroTelefono = dr["NumeroTelefono"] == DBNull.Value ? "" : (string)dr["NumeroTelefono"];
            item.TieneVisa = dr["TieneVisa"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TieneVisa"]);
            item.EmailProveedor = dr["EmailProveedor"] == DBNull.Value ? "" : (string)dr["EmailProveedor"];
            //item.DireccionFacturacion = new DireccionFacturacion();
            //item.DireccionFacturacion.Calle = (string)dr["Calle"];
            //item.DireccionFacturacion.NumeroCalle = (string)dr["Numero"];
            //item.DireccionFacturacion.Cp = (string)dr["CodigoPostal"];
            //item.DireccionFacturacion.Colonia = (string)dr["Colonia"];
            //item.DireccionFacturacion.CiudadMunicpio = (string)dr["CiudadMunicipio"];
            //item.DireccionFacturacion.Estado = (string)dr["Estado"];
            //item.DireccionFacturacion.IdPais = (int)dr["IdPais"];
            //item.DireccionFacturacion.NombrePais = (string)dr["NombrePais"];
            //item.DireccionFacturacion.Estado = (string)dr["Estado"];
            //item.DireccionFacturacion.EmailProveedor = (string)dr["EmailProveedor"];
            item.DatosBancarios = new DatosBancarios();
            item.DatosBancarios.IdBanco = dr["IdBanco"] == DBNull.Value ? "" : (string)dr["IdBanco"];
            item.DatosBancarios.TitularCuenta = dr["TitularCuenta"] == DBNull.Value ? "" : (string)dr["TitularCuenta"];
            item.DatosBancarios.CuentaBanco = dr["CuentaBanco"] == DBNull.Value ? "" : (string)dr["CuentaBanco"];
            item.DatosBancarios.ClabeInterbancaria = dr["ClabeInterbancaria"] == DBNull.Value ? "" : (string)dr["ClabeInterbancaria"];
            item.DatosBancarios.DireccionCuentaBancaria = dr["DireccionCuentaBancaria"] == DBNull.Value ? "" : (string)dr["DireccionCuentaBancaria"];
            //item.RegistradoPor = Convert.ToString(dr["RegistradoPor"]);
            item.Activo = (bool)dr["Activo"];
            item.Servicio = dr["Servicio"] == DBNull.Value ? "" : (string)dr["Servicio"];

            return item;

        }

        /*protected override DbCommand PrepareFindPagedItemsStatement(ClienteFilter criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Activo", DbType.Int32, criteria.Activo);            
            return cmd;
        }*/

        protected override DbCommand PrepareAddStatement(Proveedor item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddProveedor");
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre.ToUpper());
            this.Database.AddInParameter(command, "@ApellidoPaterno", DbType.String, item.ApellidoPaterno.ToUpper());
            this.Database.AddInParameter(command, "@ApellidoMaterno", DbType.String, item.ApellidoMaterno.ToUpper());
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.Guid, Guid.Parse(item.RegistradoPor));
            this.Database.AddOutParameter(command, "@IdProveedor", DbType.Int32, 4);
            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prDeleteProveedor");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand dbCommand = this.Database.GetStoredProcCommand("prGetProveedor");
            this.Database.AddInParameter(dbCommand, "@Id", DbType.String, id);
            return dbCommand;
        }

        protected override void BeforeAddExecuted(DataContext<Proveedor, int, ProveedorFilter> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        // metodos para seguimiento a cliente
        protected override void CommandAddComplete(DataContext<Proveedor, int, ProveedorFilter> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdProveedor"].Value;

            context.Command.Parameters.Clear();
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override DbCommand PrepareUpdateStatement(Proveedor item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateProveedor");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@Rfc", DbType.String, item.Rfc.ToUpper());
            this.Database.AddInParameter(command, "@TipoProveedor", DbType.Int32, item.TipoProveedor);
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre.ToUpper());
            this.Database.AddInParameter(command, "@ApellidoPaterno", DbType.String, item.ApellidoPaterno.ToUpper());
            this.Database.AddInParameter(command, "@ApellidoMaterno", DbType.String, item.ApellidoMaterno.ToUpper());
            this.Database.AddInParameter(command, "@FechaNacimiento", DbType.DateTime, item.FechaNacimiento.Date);
            this.Database.AddInParameter(command, "@IdNacionalidad", DbType.Int32, item.IdNacionalidad);
            this.Database.AddInParameter(command, "@EntradaGlobal", DbType.Int32, item.EntradaGlobal);
            this.Database.AddInParameter(command, "@TieneVisa", DbType.Int32, item.TieneVisa);
            this.Database.AddInParameter(command, "@NumeroTelefono", DbType.String, item.NumeroTelefono);
            this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));

            return command;
        }

        public List<Proveedor> GetProveedoresMision(ProveedorFilter criteria)
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetProveedoresMision");
            this.Database.AddInParameter(command, "@MisionId", DbType.Int32, criteria.Cotizacion);//aqui la cotizacion trae la misionId
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                proveedores.Add(this.LoadItem(dr));
            }

            return proveedores;
        }

        public List<Proveedor> GetProveedoresAvailable(ProveedorFilter criteria)
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetProveedoresDisponibles");
            this.Database.AddInParameter(command, "@CotizacionId", DbType.Int32, criteria.Cotizacion);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                proveedores.Add(this.LoadItem(dr));
            }

            return proveedores;
        }

        public void AgregarProveedoresAMision(int proveedorId, int misionId, int servicioId)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddProveedorMision");
            this.Database.AddInParameter(command, "@MisionId", DbType.Int32, misionId);
            this.Database.AddInParameter(command, "@ProveedorId", DbType.Int32, proveedorId);
            this.Database.AddInParameter(command, "@ServicioId", DbType.Int32, servicioId);
            this.Database.ExecuteNonQuery(command);
        }
        public void EliminarProveedoresAMision(int proveedorId, int cotizacionId, string servicio)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prDeleteProveedorMision");
            this.Database.AddInParameter(command, "@CotizacionId", DbType.Int32, cotizacionId);
            this.Database.AddInParameter(command, "@ProveedorId", DbType.Int32, proveedorId);
            this.Database.AddInParameter(command, "@Servicio", DbType.String, servicio);
            this.Database.ExecuteNonQuery(command);
        }
        public bool AddProveedorUsuario(int IdProveedor, string userId)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prAddProvedorUsuario");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, IdProveedor);
            this.Database.AddInParameter(command, "@UserId", DbType.Guid, Guid.Parse(userId));

            int res = this.Database.ExecuteNonQuery(command);
            if (res > 0)
            {
                bandera = true;
            }
            return bandera;
        }


        public Proveedor GetProveedorByUsuario(string userName)
        {
            Proveedor item = null;
            DbCommand command = this.Database.GetStoredProcCommand("prGetIdProveedorUsuario");
            this.Database.AddInParameter(command, "@Email", DbType.String, userName);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                item = this.LoadItem(dr);
            }
            return item;
        }

        public bool Update2(Proveedor item)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateProveedor2");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@IdBanco", DbType.String, item.DatosBancarios.IdBanco);
            this.Database.AddInParameter(command, "@TitularCuenta", DbType.String, item.DatosBancarios.TitularCuenta.ToUpper());
            this.Database.AddInParameter(command, "@CuentaBanco", DbType.String, item.DatosBancarios.CuentaBanco);
            this.Database.AddInParameter(command, "@ClabeInterbancaria", DbType.String, item.DatosBancarios.ClabeInterbancaria);
            this.Database.AddInParameter(command, "@DireccionCuentaBanco", DbType.String, item.DatosBancarios.DireccionCuentaBancaria.ToUpper());
            this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));

            int res = this.Database.ExecuteNonQuery(command);
            if (res > 0)
            {
                bandera = true;
            }
            return bandera;
        }


        public bool Update3(Proveedor item)
        {
            bool bandera = false;
            foreach (var item1 in item.Aeropuertos)
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateProveedor3");
                this.Database.AddInParameter(command, "@Identifier", DbType.Int32, item.Identifier);
                this.Database.AddInParameter(command, "@ClabeAeropuerto", DbType.String, item1.Clabe);
                //this.Database.AddInParameter(command, "@EstatusAeropuerto", DbType.String, item1.EstatusAerolinea);
                this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));

                int res = this.Database.ExecuteNonQuery(command);
                if (res > 0)
                {
                    bandera = true;
                }
            }
            return bandera;
        }


        public bool Update4(int idProveedor, string rutaArchivo, string nombreArchivo, string numeroPasaporte, DateTime expiredPasaporte, int tipoArchivo, int tipoDocumento, int idNacionalidadPas)
        {
            bool bandera = false;

            DbCommand command = this.Database.GetStoredProcCommand("prUpdateProveedor4");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, idProveedor);
            this.Database.AddInParameter(command, "@RutaAchivo", DbType.String, rutaArchivo);
            this.Database.AddInParameter(command, "@NombreAchivo", DbType.String, nombreArchivo);
            this.Database.AddInParameter(command, "@TipoArchivo", DbType.Int32, tipoArchivo);
            this.Database.AddInParameter(command, "@TipoDocumento", DbType.Int32, tipoDocumento);
            this.Database.AddInParameter(command, "@IdNacionalidadPass", DbType.Int32, idNacionalidadPas);
            this.Database.AddInParameter(command, "@PassporNumber", DbType.String, numeroPasaporte);
            this.Database.AddInParameter(command, "@FechaExpiracion", DbType.DateTime, expiredPasaporte);
            this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));
            int res = this.Database.ExecuteNonQuery(command);
            if (res > 0)
            {
                bandera = true;
            }

            return bandera;
        }

        public bool Update41(int idProveedor, string rutaArchivo, string nombreArchivo, string numeroPasaporte, DateTime expiredPasaporte, int tipoArchivo, int tipoDocumento, int idNacionalidadPas)
        {
            bool bandera = false;

            DbCommand command = this.Database.GetStoredProcCommand("prUpdateProveedor4");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, idProveedor);
            this.Database.AddInParameter(command, "@RutaAchivo", DbType.String, rutaArchivo);
            this.Database.AddInParameter(command, "@NombreAchivo", DbType.String, nombreArchivo);
            this.Database.AddInParameter(command, "@TipoArchivo", DbType.Int32, tipoArchivo);
            this.Database.AddInParameter(command, "@TipoDocumento", DbType.Int32, tipoDocumento);
            this.Database.AddInParameter(command, "@IdNacionalidadPass", DbType.Int32, idNacionalidadPas);
            this.Database.AddInParameter(command, "@PassporNumber", DbType.String, numeroPasaporte);
            this.Database.AddInParameter(command, "@FechaExpiracion", DbType.DateTime, expiredPasaporte);
            this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));
            int res = this.Database.ExecuteNonQuery(command);
            if (res > 0)
            {
                bandera = true;
            }

            return bandera;
        }

        #region Aeropuertos
        public List<ProveedorAeropuertos.ProveedorAeropuertos> getAeropuertos(int idProveedor)
        {
            List<ProveedorAeropuertos.ProveedorAeropuertos> datos = new List<ProveedorAeropuertos.ProveedorAeropuertos>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetAeropuertos");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProveedor);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                ProveedorAeropuertos.ProveedorAeropuertos item = new ProveedorAeropuertos.ProveedorAeropuertos();
                item.Identifier = (int)dr["Sequence"];
                item.Clabe = (string)dr["ClabeAeropuerto"];
                datos.Add(item);
            }
            return datos;
        }
        #endregion

        #region Aerolineas
        public List<ProveedorAeropuertos.ProveedorAeropuertoAirline> getAerolineas(int idProveedor)
        {
            List<ProveedorAeropuertos.ProveedorAeropuertoAirline> datos = new List<ProveedorAeropuertos.ProveedorAeropuertoAirline>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetAirlines");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProveedor);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                ProveedorAeropuertoAirline item = new ProveedorAeropuertoAirline();
                item.Identifier = (int)dr["Sequence"];
                item.Aerolinea = (string)dr["Aerolinea"];
                item.NumerViajeroFrecuente = (string)dr["NumeroViajeroFrecuente"];
                item.EstatusAerolinea = (string)dr["EstatusAerolinea"];
                datos.Add(item);
            }
            return datos;
        }

        #endregion


        public string GetRoleByUsuario(string email)
        {
            string name = "";
            DbCommand command = this.Database.GetStoredProcCommand("prGetRolebyEmail");
            this.Database.AddInParameter(command, "@Email", DbType.String, email);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                name = (string)dr["Name"];
            }
            return name;
        }

        public List<Archivo> getPasaportes(int idProveedor, int tipoDocumento)
        {
            List<Archivo> datos = new List<Archivo>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetDocumentosProveedor");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProveedor);
            this.Database.AddInParameter(command, "@IdTipoDocumento", DbType.Int32, tipoDocumento);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Archivo item = new Archivo();
                item.Identifier = (int)dr["Sequence"];
                item.NombreArchivo = (string)dr["NombreArchivo"];
                item.RutaArchivo = (string)dr["RutaArchivo"];
                item.TipoArchivo = (EvidenciaKind)dr["TipoArchivo"];
                item.IdNacionalidad = (int)dr["IdNacionalidadPass"];
                item.TipoDocumento = (int)dr["TipoDocumento"];
                item.NombrePais = (string)dr["PaisNombre"];
                item.PasaporteNumber = (string)dr["CodigoDocumento"];
                item.FechaExpiracion = (DateTime)dr["FechaExpiracionDocumento"];
                datos.Add(item);
            }
            return datos;
        }

        public Archivo getDocumento(int sequence, int tipoDocumento)
        {
            Archivo item = new Archivo();
            DbCommand command = this.Database.GetStoredProcCommand("prGetDocumentosProveedorSequence");
            this.Database.AddInParameter(command, "@Sequence", DbType.Int32, sequence);
            this.Database.AddInParameter(command, "@TipoDocumento", DbType.Int32, tipoDocumento);

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {

                item.Identifier = (int)dr["Sequence"];
                item.NombreArchivo = (string)dr["NombreArchivo"];
                item.TipoArchivo = (EvidenciaKind)dr["TipoArchivo"];

                /*La ruta del archivo se pasa completa sin necesidad de convertir a base64 ya que
                el servidor de imágenes, guarda todos los archivos como una imagen, por lo tanto el src del tag img
                puede tomar directamente la url de la imagen como se guarda en la bd sobre la tabla ProveedorArchivo
                */
                item.RutaArchivo = (string)dr["RutaArchivo"];
                /*
                if (item.TipoArchivo == EvidenciaKind.Imagen)
                {
                    item.RutaArchivo = DocumentoImagenBase64((string)dr["RutaArchivo"]);
                }
                else if(item.TipoArchivo == EvidenciaKind.PDF)
                {
                    item.RutaArchivo = DocumentoPdfBase64((string)dr["RutaArchivo"]);
                }
                */
                item.IdNacionalidad = (int)dr["IdNacionalidadPass"];
                item.TipoDocumento = (int)dr["TipoDocumento"];
                item.NombrePais = (string)dr["PaisNombre"];
                item.PasaporteNumber = (string)dr["CodigoDocumento"];
                item.FechaExpiracion = (DateTime)dr["FechaExpiracionDocumento"];
            }
            return item;
        }

        public string DocumentoImagenBase64(string fileName)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(fileName))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, ImageFormat.Jpeg);
                    byte[] imageBytes = m.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return $"data:image/png;base64,{base64String}";
                }
            }
        }

        public string DocumentoPdfBase64(string fileName)
        {
            Byte[] fileBytes = File.ReadAllBytes(fileName);
            var content = Convert.ToBase64String(fileBytes);
            return content;
        }
    }
}