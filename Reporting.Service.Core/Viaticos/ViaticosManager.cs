using Reporting.Service.Core.Viaticos.Viaticos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class ViaticosManager : DataRepository
    {
        public List<Estados> GetEstados()
        {
            List<Estados> Detalle = new List<Estados>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEstados");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Estados
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Nombre = dr["Nombre"].ToString()
                });
            }
            return Detalle;
        }
        public List<Actividades> GetActividades()
        {
            List<Actividades> Detalle = new List<Actividades>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetActividades");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Actividades
                {
                    Id = int.Parse(dr["Id"].ToString()),
                    Nombre = dr["Nombre"].ToString()
                });
            }
            return Detalle;
        }

        public List<Tiendas.Tienda> GetTiendasSAP()
        {
            List<Tiendas.Tienda> Detalle = new List<Tiendas.Tienda>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTiendasSAP");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Tiendas.Tienda
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Nombre = dr["Nombre"].ToString()
                });
            }
            return Detalle;
        }

        public List<Producto> GetProductos(int Departamento = 0)
        {
            List<Producto> Detalle = new List<Producto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductosViaticos");
            this.Database.AddInParameter(cmd, "@Departamento", DbType.Int32, Departamento);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Producto
                {
                    //Sequence = int.Parse(dr["Sequence"].ToString()),
                    Sku = dr["Sku"].ToString(),
                    Descripcion = dr["Descripcion"].ToString()
                });
            }
            return Detalle;
        }
        public List<CentroCosto> GetCentroCostos(int Departamento = 0)
        {
            List<CentroCosto> Detalle = new List<CentroCosto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCentroCostosViaticos");
            this.Database.AddInParameter(cmd, "@Departamento", DbType.Int32, Departamento);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CentroCosto
                {
                    Codigo = dr["Codigo"].ToString(),
                    Nombre = dr["Nombre"].ToString()
                });
            }
            return Detalle;
        }

        public List<Solicitudes> GetSolicitudes(string RegistradoPor)
        {
            List<Solicitudes> Detalle = new List<Solicitudes>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSolicitudes");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Solicitudes
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    RegistradoEl = (DateTime)dr["RegistradoEl"],
                    FechaRequisicion = (DateTime)dr["FechaRequisicion"],
                    FolioSAP = (int)dr["FolioSAP"],
                    Cheque = (int)dr["Cheque"],
                    Total = (decimal)dr["Total"],
                    Sucursal = (string)dr["Sucursal"],
                    Estado = (string)dr["Estado"]
                });
            }
            return Detalle;
        }
        public List<Usuario> GetUsuariosSAP()
        {
            List<Usuario> Detalle = new List<Usuario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUsuariosSAP");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Usuario
                {
                    Id = int.Parse(dr["Id"].ToString()),
                    Nombre = (string)dr["Nombre"],
                    ApellidoPaterno = (string)dr["ApellidoPaterno"],
                    ApellidoMaterno = (string)dr["ApellidoMaterno"],
                    Email = (string)dr["Email"]
                });
            }
            return Detalle;
        }
        public List<Presupuesto> GetPresupuestos()
        {
            List<Presupuesto> Detalle = new List<Presupuesto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPresupuestos");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Presupuesto
                {
                    Usuario = (string)dr["Usuario"],
                    Fecha = (string)dr["Fecha"],
                    Monto = (decimal)dr["Presupuesto"]
                });
            }
            return Detalle;
        }

        public void UpdateSolicitud(int Folio, int Estado, string ModificadoPor = "")
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSolicitud");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
            this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, Estado);
            if (ModificadoPor != "")
                this.Database.AddInParameter(cmd, "@ModificadoPor", DbType.String, ModificadoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }

        public void EliminarDetalleSolicitud(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteDetalleSolicitud");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }

        public void EliminarDetalleActividadSolicitud(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteActividadSolicitud");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }


        public List<DetalleSolicitud> GetUltimaSolicitud(string RegistradoPor)
        {
            List<DetalleSolicitud> Detalle = new List<DetalleSolicitud>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUltimaSolicitud");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleSolicitud
                {
                    Sequence = (int)dr["Sequence"],
                    SequenceItem = (int)dr["SequenceItem"],
                    Id_Departamento = (int)dr["Id_Departamento"],
                    Departamento = (string)dr["Departamento"],
                    Sku_Producto = (string)dr["Sku_Producto"],
                    Producto = (string)dr["Producto"],
                    CentroCosto = (string)dr["CentroCosto"],
                    Id_Sucursal = (int)dr["Id_Sucursal"],
                    Sucursal = (string)dr["Sucursal"],
                    FechaSolicitud = (DateTime)dr["FechaSolicitud"],
                    FechaInicio = (DateTime)dr["FechaInicio"],
                    FechaFin = (DateTime)dr["FechaFin"],
                    Monto = (decimal)dr["Monto"],
                    Presupuesto = (decimal)dr["Presupuesto"],
                    MontoActual = (decimal)dr["MontoActual"],
                    Disponible = (decimal)dr["Disponible"]
                });
            }
            return Detalle;
        }
        public List<Departamento> GetDepartamento(int Sequence = 0)
        {
            List<Departamento> Detalle = new List<Departamento>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDepartamento");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Departamento
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Descripcion = (string)dr["Descripcion"],
                    Codigo = (string)dr["Codigo"]
                });
            }
            return Detalle;
        }


        public Cabecera AddSolicitud(int Sequence, DateTime FechaSolicitud, DateTime FechaInicio, DateTime FechaFin, int Departamento, int Sucursal, string Producto, string CentroCosto, decimal Monto, string RegistradoPor)
        {
            Cabecera datos = new Cabecera();
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSolicitud");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.AddInParameter(cmd, "@FechaSolicitud", DbType.Date, FechaSolicitud);
            this.Database.AddInParameter(cmd, "@FechaInicio", DbType.DateTime, FechaInicio);
            this.Database.AddInParameter(cmd, "@FechaFin", DbType.DateTime, FechaFin);
            this.Database.AddInParameter(cmd, "@Departamento", DbType.Int32, Departamento);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.Int32, Sucursal);
            this.Database.AddInParameter(cmd, "@Producto", DbType.String, Producto);
            this.Database.AddInParameter(cmd, "@CentroCosto", DbType.String, CentroCosto);
            this.Database.AddInParameter(cmd, "@Monto", DbType.Decimal, Monto);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                datos.Folio = (int)dr["Sequence"];
                datos.Item = (int)dr["Item"];
                datos.Presupuesto = (decimal)dr["Presupuesto"];
                datos.MontoActual = (decimal)dr["MontoActual"];
                datos.Disponible = (decimal)dr["Disponible"];
            }

            return datos;
        }

        public void AddActividadSolicitud(int Solicitud, DateTime Fecha, string HoraInicial, string HoraFinal, int Actividad)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddActividadSolicitud");
                this.Database.AddInParameter(cmd, "@Solicitud", DbType.Int32, Solicitud);
                this.Database.AddInParameter(cmd, "@Fecha", DbType.Date, Fecha);
                this.Database.AddInParameter(cmd, "@HoraInicial", DbType.String, HoraInicial);
                this.Database.AddInParameter(cmd, "@HoraFinal", DbType.String, HoraFinal);
                this.Database.AddInParameter(cmd, "@Actividad", DbType.Int32, Actividad);
                this.Database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddPresupuestoUsuario(int Usuario, int Mes, int Año, decimal Monto)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddPresupuestoUsuario");
            this.Database.AddInParameter(cmd, "@Usuario", DbType.Int32, Usuario);
            this.Database.AddInParameter(cmd, "@Mes", DbType.Int32, Mes);
            this.Database.AddInParameter(cmd, "@Año", DbType.Int32, Año);
            this.Database.AddInParameter(cmd, "@Presupuesto", DbType.Decimal, Monto);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }

        public void AddCodigosSAT(string Producto, string CodigosSAT, int Viaticos, int CajaChica)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddCodigosPermitidosSAT");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Producto);
            this.Database.AddInParameter(cmd, "@CodigoSAT", DbType.String, CodigosSAT);
            this.Database.AddInParameter(cmd, "@Viaticos", DbType.Int32, Viaticos);
            this.Database.AddInParameter(cmd, "@CajaChica", DbType.Int32, CajaChica);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }

        public List<DetalleActividad> GetActividadesSolicitud(int Folio)
        {
            List<DetalleActividad> Detalle = new List<DetalleActividad>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleActividades");
            this.Database.AddInParameter(cmd, "@Folio", DbType.String, Folio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleActividad
                {
                    Sequence = (int)dr["Sequence"],
                    Fecha = (DateTime)dr["Fecha"],
                    HoraInicial = (string)dr["HoraInicial"],
                    HoraFinal = (string)dr["HoraFinal"],
                    Id_Actividad = (int)dr["Id_Actividad"],
                    Actividad = (string)dr["Actividad"]
                });
            }
            return Detalle;
        }

        public List<CodigoSAT> GetListaCodigosSAT()
        {
            List<CodigoSAT> Detalle = new List<CodigoSAT>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetListaCodigosSAT");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CodigoSAT
                {
                    Sequence = (int)dr["Sequence"],
                    Sku = (string)dr["Sku"],
                    Codigo = (string)dr["CodigoSAT"],
                    Viaticos = (string)dr["Viaticos"],
                    CajaChica = (string)dr["CajaChica"],
                    Descripcion = (string)dr["Descripcion"]
                });
            }
            return Detalle;
        }

        public List<DetalleSolicitud> GetSolicitudBySequence(int Folio)
        {
            List<DetalleSolicitud> Detalle = new List<DetalleSolicitud>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSolicitudBySequence");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleSolicitud
                {
                    Sequence = (int)dr["Sequence"],
                    SequenceItem = (int)dr["SequenceItem"],
                    Id_Departamento = (int)dr["Id_Departamento"],
                    Departamento = (string)dr["Departamento"],
                    Sku_Producto = (string)dr["Sku_Producto"],
                    Producto = (string)dr["Producto"],
                    CentroCosto = (string)dr["CentroCosto"],
                    Id_Sucursal = (int)dr["Id_Sucursal"],
                    Sucursal = (string)dr["Sucursal"],
                    FechaSolicitud = (DateTime)dr["FechaSolicitud"],
                    FechaInicio = (DateTime)dr["FechaInicio"],
                    FechaFin = (DateTime)dr["FechaFin"],
                    Estado = (int)dr["Estado"],
                    Comentarios = (string)dr["Comentarios"],
                    Monto = (decimal)dr["Monto"],
                    Nombre = (string)dr["Nombre"] + ' ' + (string)dr["ApellidoPaterno"] + ' ' + (string)dr["ApellidoMaterno"],
                    Email = (string)dr["Email"]

                });
            }
            return Detalle;
        }

        public List<Facturas> GetFacturasSolicitudBySequence(int DetalleSolicitud)
        {
            List<Facturas> Detalle = new List<Facturas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFacturasDetalleSolicitud");
            this.Database.AddInParameter(cmd, "@DetalleSolicitud", DbType.Int32, DetalleSolicitud);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Facturas
                {
                    Sequence = (int)dr["Sequence"],
                    XMLString = (string)dr["XMLString"],
                    FechaTimbrado = (DateTime)dr["FechaTimbrado"],
                    UUID = (string)dr["UUID"],
                    Monto = (decimal)dr["Monto"],
                    UsoCFDI = (string)dr["UsoCFDI"],
                    FormaPago = (string)dr["FormaPago"]
                });
            }
            return Detalle;
        }

        public List<DetalleActividad> GetActividadesSolicitudBySequence(int Folio)
        {
            List<DetalleActividad> Detalle = new List<DetalleActividad>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetActividadesSolicitudBySequence");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleActividad
                {
                    Sequence = (int)dr["Sequence"],
                    Fecha = (DateTime)dr["Fecha"],
                    HoraInicial = (string)dr["HoraInicial"],
                    HoraFinal = (string)dr["HoraFinal"],
                    Id_Actividad = (int)dr["Id_Actividad"],
                    Actividad = (string)dr["Actividad"]
                });
            }
            return Detalle;
        }

        public List<DetalleItemSolicitud> GetDetalleItemSolicitud(int Item)
        {
            List<DetalleItemSolicitud> Detalle = new List<DetalleItemSolicitud>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleItemSolicitud");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Item);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleItemSolicitud
                {
                    Sequence = (int)dr["Sequence"],
                    Producto = (string)dr["Producto"],
                    Monto = (decimal)dr["Monto"],
                    CodigoSat = (string)dr["CodigoSAT"]
                });
            }
            return Detalle;
        }

        public List<Facturas> GetFacturasSolicitud(int SequenceItem = 0, string UUID = "")
        {
            List<Facturas> Detalle = new List<Facturas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFacturasDetalleSolicitud");

            if (SequenceItem != 0)
                this.Database.AddInParameter(cmd, "@DetalleSolicitud", DbType.Int32, SequenceItem);
            if (UUID != "")
                this.Database.AddInParameter(cmd, "@UUID", DbType.String, UUID);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Facturas
                {
                    Sequence = (int)dr["Sequence"],
                    XMLString = (string)dr["XMLString"],
                    Monto = (decimal)dr["Monto"],
                    FechaTimbrado = (DateTime)dr["FechaTimbrado"],
                    UUID = (string)dr["UUID"],
                    UsoCFDI = (string)dr["UsoCFDI"],
                    FormaPago = (string)dr["FormaPago"]

                });
            }
            return Detalle;
        }

        public bool AddFacturaSolicitud(int DetalleSolicitud, string XMlString, DateTime FechaTimbrado, string UUID, decimal Monto, string FormaPago, string UsoCFDI, int? ClaveProdServ = null)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddFacturaSolicitud");
            this.Database.AddInParameter(cmd, "@DetalleSolicitud", DbType.Int32, DetalleSolicitud);
            this.Database.AddInParameter(cmd, "@XMlString", DbType.String, XMlString);
            this.Database.AddInParameter(cmd, "@FechaTimbrado", DbType.DateTime, FechaTimbrado);
            this.Database.AddInParameter(cmd, "@UUID", DbType.String, UUID);
            this.Database.AddInParameter(cmd, "@Monto", DbType.Decimal, Monto);
            this.Database.AddInParameter(cmd, "@FormaPago", DbType.String, FormaPago);
            this.Database.AddInParameter(cmd, "@UsoCFDI", DbType.String, UsoCFDI);



            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected >= 0)
                return true;
            else
                return false;
        }

        public bool EliminarFactura(int Folio)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteFacturaSolicitud");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected >= 0)
                return true;
            else
                return false;
        }

        public List<Solicitudes> GetViaticos(string UserId)
        {
            List<Solicitudes> Detalle = new List<Solicitudes>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetViaticos");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, UserId);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Solicitudes item = new Solicitudes();

                item.Sequence = (int)dr["Sequence"];
                item.RegistradoEl = (DateTime)dr["RegistradoEl"];
                item.FechaRequisicion = (DateTime)dr["FechaRequisicion"];
                item.FolioSAP = (int)dr["FolioSAP"];
                item.Cheque = (int)dr["Cheque"];
                item.Total = (decimal)dr["Total"];
                item.Sucursal = (string)dr["Sucursal"];
                item.Estado = (string)dr["Estado"];

                item.Usuario = new Usuario();
                item.Usuario.UserId = (string)dr["UserId"];
                item.Usuario.Email = (string)dr["UserName"];

                Detalle.Add(item);
            }
            cmd.Dispose();
            dr.Close();
            dr.Dispose();
            return Detalle;
        }

        #region Daniel Viaticos
        public List<DetalleSolicitud> GetSolicitud(int Sequence)
        {
            List<DetalleSolicitud> Detalle = new List<DetalleSolicitud>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSolicitud");
            this.Database.AddInParameter(cmd, "@Folio", DbType.String, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                DetalleSolicitud item = new DetalleSolicitud();

                item.SequenceItem = (int)dr["SequenceItem"];
                item.Sequence = (int)dr["Sequence"];
                item.Producto = (string)dr["Producto"];
                item.CentroCosto = (string)dr["CentroCosto"];
                item.Id_Sucursal = (int)dr["Id_Sucursal"];
                item.Sucursal = (string)dr["Sucursal"];
                item.FechaInicio = (DateTime)dr["Fecha"];
                item.Monto = (decimal)dr["Monto"];
                item.Estado = (int)dr["Estado"];
                item.MontoSubido = (decimal)dr["MontoSubido"];
                item.FolioSAP = (int)dr["FolioSAP"];
                item.Cheque = (int)dr["Cheque"];

                item.Usuario = new Usuario();
                item.Usuario.UserId = (string)dr["UserId"];
                item.Usuario.Email = (string)dr["UserName"];

                item.Facturas = GetFacturasSolicitudBySequence((int)dr["SequenceItem"]);

                Detalle.Add(item);
                //Detalle.Add(new DetalleSolicitud
                //{
                //    SequenceItem = (int)dr["SequenceItem"],
                //    Sequence = (int)dr["Sequence"],
                //    Producto = (string)dr["Producto"],
                //    CentroCosto = (string)dr["CentroCosto"],
                //    Id_Sucursal = (int)dr["Id_Sucursal"],
                //    Sucursal = (string)dr["Sucursal"],
                //    FechaInicio = (DateTime)dr["Fecha"],
                //    Monto = (decimal)dr["Monto"],
                //    Estado = (int)dr["Estado"],
                //    MontoSubido = (decimal)dr["MontoSubido"],
                //    FolioSAP = (int)dr["FolioSAP"],
                //    Cheque = (int)dr["Cheque"],
                //    Facturas = GetFacturasSolicitudBySequence((int)dr["SequenceItem"])
                //});                
            }
            return Detalle;
        }
        public List<Facturas> GetFacturas(int ViaticoDetalle)
        {
            List<Facturas> lista = new List<Facturas>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetViaticoFacturas");
            this.Database.AddInParameter(command, "@ViaticoDetalle", DbType.Int32, ViaticoDetalle);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Facturas item = new Facturas();
                item.Sequence = (int)dr["Sequence"];
                item.XMLString = (string)dr["XMLString"];
                item.FechaTimbrado = (DateTime)dr["FechaTimbrado"];
                item.UUID = (string)dr["UUID"];
                item.Monto = (decimal)dr["Monto"];
                //item.FormaPago = (string)dr["FormaPago"];

                item.FormaPagoSAT = new FormaPagoSAT();
                item.FormaPagoSAT.Codigo = (string)dr["FormaPago"];
                item.FormaPagoSAT.Descripcion = (string)dr["Descripcion"];

                //item.UsoCFDI = (string)dr["UsoCFDI"];   
                item.FileExistPDF = (bool)dr["FileExistPDF"];

                item.UsoCFDISAT = new UsoCFDI();
                item.UsoCFDISAT.Codigo = (string)dr["UsoCFDI"];
                item.UsoCFDISAT.Descripcion = (string)dr["UsoCFDIDescripcion"];

                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public FormaPagoSAT GetFormaPagoSAT(int FormaPago)
        {
            FormaPagoSAT item = null;
            DbCommand command = this.Database.GetStoredProcCommand("prGetFormaPagoSAT");
            this.Database.AddInParameter(command, "@FormaPago", DbType.String, FormaPago);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                item = new FormaPagoSAT();
                item.Identifier = (int)dr["Sequence"];
                item.Codigo = (string)dr["Codigo"];
                item.Descripcion = (string)dr["Descripcion"];
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return item;
        }


        public bool UpdateViaticoDetalleFactura(int ViaticoDetalle, string FilePath)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateViaticoDetalleFactura");
                this.Database.AddInParameter(command, "@ViaticoDetalle", DbType.Int32, ViaticoDetalle);
                this.Database.ExecuteNonQuery(command);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Mensaje ocurrio un error Store Procedure prUpdateViaticoDetalleFactura: " + ex.Message);
            }
        }

        public FacturaConcepto getValidaConceptoFactura(ViaticosCriteria Criteria)
        {
            FacturaConcepto item = null;
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetViaticoFacturaConceptoValida");
            //SequenceItem
            this.Database.AddInParameter(command, "@Viatico", DbType.Int32, Criteria.Viatico);
            this.Database.AddInParameter(command, "@ViaticoDetalle", DbType.Int32, Criteria.ViaticoDetalle);
            this.Database.AddInParameter(command, "@UUID", DbType.String, Criteria.UUID);
            this.Database.AddInParameter(command, "@Descripcion", DbType.String, Criteria.Descripcion);
            this.Database.AddInParameter(command, "@ClaveProdServ", DbType.Int32, Criteria.ClaveProServ);
            this.Database.AddInParameter(command, "@Importe", DbType.Decimal, Criteria.Importe);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                item = new FacturaConcepto();
                item.IsActivoSAT = (bool)dr["ActivoSAT"];
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return item;
        }


        public bool GetViaticoFacturaConceptoClaveProdServValida(int ClaveProdServ)
        {
            bool? resul = null;
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetViaticoFacturaConceptoClaveProdServ");
            this.Database.AddInParameter(command, "@ClaveProdServ", DbType.Int32, ClaveProdServ);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                resul = (bool)dr["IsActivoSAT"];
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            return resul.Value;
        }


        public bool AddViaticoFactura(Facturas item)
        {

            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            DbTransaction dbTransaction = connection.BeginTransaction();

            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("[prAddFacturaSolicitud]");
                this.Database.AddInParameter(cmd, "@DetalleSolicitud", DbType.Int32, item.DetalleSolicitud);
                this.Database.AddInParameter(cmd, "@XMlString", DbType.String, item.XMLString);
                this.Database.AddInParameter(cmd, "@FechaTimbrado", DbType.DateTime, item.FechaTimbrado);
                this.Database.AddInParameter(cmd, "@UUID", DbType.String, item.UUID);
                this.Database.AddInParameter(cmd, "@Monto", DbType.Decimal, item.Monto);
                this.Database.AddInParameter(cmd, "@FormaPago", DbType.String, item.FormaPago);
                this.Database.AddInParameter(cmd, "@UsoCFDI", DbType.String, item.UsoCFDI);
                this.Database.AddOutParameter(cmd, "@Identifier", DbType.Int32, 4);

                this.Database.ExecuteNonQuery(cmd, dbTransaction);

                int? x = Convert.ToInt32(this.Database.GetParameterValue(cmd, "@Identifier"));

                if (x.HasValue && x.Value != 0)
                {
                    item.Sequence = x.Value;
                }
                else
                {
                    throw new ArgumentException("Ocurrio un error al agregar la cabecera del XML ");
                }

                foreach (var concepto in item.Conceptos)
                {
                    cmd.Parameters.Clear();

                    cmd = this.Database.GetStoredProcCommand("prAddViaticoFacturaConcepto");
                    this.Database.AddInParameter(cmd, "@ViaticoFactura", DbType.Int32, item.Sequence);
                    this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, concepto.Descripcion);
                    this.Database.AddInParameter(cmd, "@ClaveProdServ", DbType.Int32, concepto.ClaveProdServ);
                    this.Database.AddInParameter(cmd, "@Importe", DbType.Decimal, concepto.Importe);
                    this.Database.AddInParameter(cmd, "@Cantidad", DbType.String, concepto.Cantidad);
                    this.Database.ExecuteNonQuery(cmd, dbTransaction);

                }
                dbTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                Debug.WriteLine("Error generado Store Procedure prAddFacturaSolicitud, mensaje:" + ex.Message);
                item.Sequence = 0;
                return false;
            }
            finally
            {
                if (dbTransaction.Connection != null && dbTransaction.Connection.State == ConnectionState.Open)
                {
                    dbTransaction.Connection.Close();
                }
            }
        }


        public List<ProductoServicioSAT> GetProductoServicioSAT()
        {
            List<ProductoServicioSAT> lista = new List<ProductoServicioSAT>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetViaticoProductoServicioSAT");
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                ProductoServicioSAT item = new ProductoServicioSAT();
                item.Identifier = (int)dr["Sequence"];
                item.Codigo = dr["Codigo"].ToString();
                item.Descripcion = (string)dr["Descripcion"];
                item.IsOcupado = (bool)dr["IsOcupado"];
                lista.Add(item);
            }
            dr.Close();
            dr.Dispose();

            return lista;
        }

        // validar disponibilidad de XML 
        // conceptos utilizados en otro comprobacion
        public Facturas GetFacturasConceptosUtilizado(ViaticosCriteria Criteria)
        {
            Facturas item = null;
            DbCommand command = this.Database.GetStoredProcCommand("[dbo].[prGetViaticoFacturaConceptoUUIDValida]");
            this.Database.AddInParameter(command, "@UUID", DbType.String, Criteria.UUID);
            this.Database.AddInParameter(command, "@Monto", DbType.Decimal, Criteria.Total);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                item = new Facturas();
                item.Monto = (decimal)dr["Monto"];
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    FacturaConcepto itemConcepto = new FacturaConcepto();

                    itemConcepto.Identifier = (int)dr["Sequence"];
                    itemConcepto.Descripcion = (string)dr["Descripcion"];
                    itemConcepto.ClaveProdServ = (int)dr["ProductoServicioSAT"];
                    itemConcepto.Importe = (decimal)dr["Importe"];
                    itemConcepto.Cantidad = (string)dr["Cantidad"];

                    if (item.Conceptos == null)
                        item.Conceptos = new List<FacturaConcepto>();

                    item.Conceptos.Add(itemConcepto);
                }
            }

            return item;
        }

        public bool GetProductoServicioSATValida(ViaticosCriteria Criteria)
        {
            bool result = false;
            DbCommand command = this.Database.GetStoredProcCommand("prGetProductosCodigosSATValida");
            this.Database.AddInParameter(command, "@SKU", DbType.String, Criteria.Sku);
            this.Database.AddInParameter(command, "@CodigoSAT", DbType.String, Criteria.ClaveProServ);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                result = (bool)dr["Code"];
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return result;
        }

        public bool AddProductoCodigosSAT(string Producto, string CodigosSAT, int Viaticos, int CajaChica)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddCodigosPermitidosSAT");
                this.Database.AddInParameter(cmd, "@Sku", DbType.String, Producto);
                this.Database.AddInParameter(cmd, "@CodigoSAT", DbType.String, CodigosSAT);
                this.Database.AddInParameter(cmd, "@Viaticos", DbType.Int32, Viaticos);
                this.Database.AddInParameter(cmd, "@CajaChica", DbType.Int32, CajaChica);
                this.Database.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Solicitudes> GetViaticosAprobacionFianzas(string UserId)
        {
            List<Solicitudes> Detalle = new List<Solicitudes>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetViaticos");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, UserId);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Solicitudes item = new Solicitudes();

                item.Sequence = (int)dr["Sequence"];
                item.RegistradoEl = (DateTime)dr["RegistradoEl"];
                item.FechaRequisicion = (DateTime)dr["FechaRequisicion"];
                item.FolioSAP = (int)dr["FolioSAP"];
                item.Cheque = (int)dr["Cheque"];
                item.Total = (decimal)dr["Total"];
                item.Sucursal = (string)dr["Sucursal"];
                item.Estado = (string)dr["Estado"];

                item.Usuario = new Usuario();
                item.Usuario.UserId = (string)dr["UserId"];
                item.Usuario.Email = (string)dr["UserName"];

                Detalle.Add(item);
            }
            cmd.Dispose();
            dr.Close();
            dr.Dispose();
            return Detalle;
        }

        // //public bool AddViaticoAprobacion()
        public bool UpdateViaticoAprobacion(string Usuario, string Comentario, string SequenceForeignKey, int Departamento)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddAprobacion");
                this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
                this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
                this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.String, SequenceForeignKey);
                this.Database.AddInParameter(cmd, "@Departamento", DbType.Int16, Departamento);
                this.Database.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool AddViaticoAprobacionComentario(string Usuario, string Comentario, string SequenceForeignKey, int Departamento)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddComentarios");
                this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
                this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
                this.Database.AddInParameter(cmd, "@FolioSIE", DbType.String, SequenceForeignKey);
                this.Database.AddInParameter(cmd, "@Departamento", DbType.Int16, Departamento);
                this.Database.AddInParameter(cmd, "@Module", DbType.Int32, 2);// 2 Modulo de Viaticos 
                this.Database.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        //historico de viaticos de cada usuario
        public List<DetalleSolicitud> GetViaticosHistoricoUser(string RegistradoPor)
        {
            List<DetalleSolicitud> Detalle = new List<DetalleSolicitud>();
            DbCommand cmd = this.Database.GetStoredProcCommand("dbo.prGetViaticoHistoricoUser");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                DetalleSolicitud item = new DetalleSolicitud();

                item.Sequence = (int)dr["Sequence"];
                item.SequenceItem = (int)dr["SequenceItem"];
                item.Id_Departamento = (int)dr["Id_Departamento"];
                item.Departamento = (string)dr["Departamento"];
                item.Sku_Producto = (string)dr["Sku_Producto"];
                item.Producto = (string)dr["Producto"];
                item.CentroCosto = (string)dr["CentroCosto"];
                
                item.Id_Sucursal = (int)dr["Id_Sucursal"];
                item.Sucursal = (string)dr["Sucursal"];
                item.FechaSolicitud = (DateTime)dr["FechaSolicitud"];
                item.FechaInicio = (DateTime)dr["FechaInicio"];
                item.FechaFin = (DateTime)dr["FechaFin"];
                item.Monto = (decimal)dr["Monto"];

                item.Usuario = new Usuario();
                item.Usuario.UserId = (string)dr["UserId"];
                item.Usuario.Email = (string)dr["UserEmail"];

                Detalle.Add(item);
            }
            return Detalle;
        }

        public bool ViaticoUpdateConceptoMonto(int Concepto, decimal MontoNuevo)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateViaticoConceptoMonto");
                this.Database.AddInParameter(command, "@ViaticoConcepto", DbType.Int32, Concepto);
                this.Database.AddInParameter(command, "@MontoNuevo", DbType.Decimal, MontoNuevo);
                this.Database.ExecuteNonQuery(command);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
