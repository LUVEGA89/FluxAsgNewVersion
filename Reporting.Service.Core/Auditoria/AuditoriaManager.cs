using Reporting.Service.Core.Tiendas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class AuditoriaManager : DataRepository
    {
        public bool AddTipoAuditoria(string Nombre, string Descripcion, string RegistradoEl)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddTipoAuditoria");
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoEl);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public bool EditTipoAuditoria(int Sequence, string Nombre, string Descripcion)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateTipoAuditoria");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public bool EditSegmento(int Sequence, string Nombre, string Descripcion)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSegmento");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public bool DeleteTipoAuditoria(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteTipoAuditoria");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public bool AddSegmento(int Tipo, string Nombre, string Descripcion, int Orden)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSegmento");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
            this.Database.AddInParameter(cmd, "@Orden", DbType.Int32, Orden);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public bool AddRubro(string Segmento, string Nombre, string Descripcion, string Puntuacion, int Requerido, string Nota)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddRubro");
            this.Database.AddInParameter(cmd, "@Segmento", DbType.Int32, Segmento);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
            this.Database.AddInParameter(cmd, "@Puntuacion", DbType.Int32, Puntuacion);
            this.Database.AddInParameter(cmd, "@Requerido", DbType.Int32, Requerido);
            this.Database.AddInParameter(cmd, "@Nota", DbType.String, Nota);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public bool AddImagen(byte[] Imagen, string Nombre, int Tienda, int Tipo, int Segmento, int Rubro, string Imagen64)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddImagen");
            this.Database.AddInParameter(cmd, "@Imagen", DbType.Binary, Imagen);
            this.Database.AddInParameter(cmd, "@Imagen64", DbType.String, Imagen64);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Segmento", DbType.Int32, Segmento);
            this.Database.AddInParameter(cmd, "@Rubro", DbType.Int32, Rubro);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public bool EditRubro(string sequence, string Nombre, string Descripcion, string Puntuacion, int Requerido, string Nota)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prEditRubro");
            this.Database.AddInParameter(cmd, "@sequence", DbType.Int32, sequence);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
            this.Database.AddInParameter(cmd, "@Puntuacion", DbType.Int32, Puntuacion);
            this.Database.AddInParameter(cmd, "@Requerido", DbType.Int32, Requerido);
            this.Database.AddInParameter(cmd, "@Nota", DbType.String, Nota);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public IList<TipoAuditoria> GetTipoAuditoria()
        {
            List<TipoAuditoria> Detalle = new List<TipoAuditoria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTipoAuditoria");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new TipoAuditoria
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Descripcion = (string)dr["Descripcion"],
                    RegistradoEl = (DateTime)dr["RegistradoEl"],
                    Segmentos = (int)dr["Segmentos"] > 0 ? GetSegmentos((int)dr["Sequence"]) : null
                });
            }
            return Detalle;
        }

        public IList<Segmento> GetSegmentos(int Tipo)
        {
            List<Segmento> Detalle = new List<Segmento>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSegmento");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Segmento
                {
                    Sequence = (int)dr["Sequence"],
                    Tipo = (int)dr["Tipo"],
                    Nombre = (string)dr["Nombre"],
                    Descripcion = (string)dr["Descripcion"],
                    Orden = (int)dr["Orden"],
                    //Activo = (int)dr["Activo"],
                    Rubros = (int)dr["Rubros"] > 0 ? GetRubros((int)dr["Sequence"]) : null
                });
            }
            return Detalle;
        }

        public IList<Rubros> GetRubros(int Segmento)
        {
            List<Rubros> Detalle = new List<Rubros>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetRubro");
            this.Database.AddInParameter(cmd, "@Segmento", DbType.Int32, Segmento);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Rubros
                {
                    Sequence = (int)dr["Sequence"],
                    Segmento = (int)dr["Segmento"],
                    Nombre = (string)dr["Nombre"],
                    Descripcion = (string)dr["Descripcion"],
                    Puntuacion = (int)dr["Puntuacion"],
                    Requerido = (int)dr["Requerido"],
                    Nota = (string)dr["Nota"],
                });
            }
            return Detalle;
        }

        public IList<Tienda> GetTiendasAuditoria(string Email, int Tienda = 0)
        {
            List<Tienda> Detalle = new List<Tienda>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTiendasSIAT");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Tienda
                {
                    Sequence = (int)dr["Id_Tienda"],
                    Nombre = dr["Tienda"].ToString(),
                    Id_Auditoria = (int)dr["Id_Auditoria"],
                    Auditoria = dr["Auditoria"].ToString(),
                    PorcentajeEvaluacion = decimal.Parse(dr["PorcentajeEvaluacion"].ToString())
                    //Auditorias = GetAuditoriasTienda((int)dr["Sequence"])
                });
            }
            return Detalle;
        }
        public IList<SeguimientoAuditoria> GetAuditoriasTienda(int Tienda)
        {
            List<SeguimientoAuditoria> Detalle = new List<SeguimientoAuditoria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("GetTiendaAuditoria");
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new SeguimientoAuditoria
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Cumplimiento = (decimal)dr["Porcentaje"],
                });
            }
            return Detalle;
        }
        public IList<SeguimientoAuditoria> GetFinalizados(int Tienda, string Del, string Al, int Tipo = 0)
        {
            List<SeguimientoAuditoria> Detalle = new List<SeguimientoAuditoria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAuditoriasFinalizadas");
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            if (Del != "")
                this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            if (Al != "")
                this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            if (Tipo > 0)
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new SeguimientoAuditoria
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = dr["Tipo"].ToString(),
                    RegistradoEl = dr["FinalizadoEl"].ToString(),
                    Puntuacion = (int)dr["Puntuacion"],
                    Usuario = dr["Usuario"].ToString()
                });
            }
            return Detalle;
        }
        public IList<SeguimientoAuditoria> GetAllFinalizados(string Del, string Al)
        {
            List<SeguimientoAuditoria> Detalle = new List<SeguimientoAuditoria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAllAuditorias");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new SeguimientoAuditoria
                {
                    Id_Tienda = (int)dr["Id_TIenda"],
                    Tienda = (string)dr["Tienda"],
                    Sequence = (int)dr["Tipo"],
                    Nombre = dr["Auditoria"].ToString(),
                    Aplicados = (int)dr["Aplicadas"]
                });
            }
            return Detalle;
        }
        public Captura GetRubroTipoTienda(int Tipo, int Tienda)
        {
            Captura Detalle = new Captura();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetRubroCaptura");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Id_TipoAuditoria = (int)dr["Id_TipoAuditoria"];
                Detalle.TipoAuditoria = dr["TipoAuditoria"].ToString();
                Detalle.DescripcionAuditoria = dr["DescripcionAuditoria"].ToString();
                Detalle.Id_Segmento = (int)dr["Id_Segmento"];
                Detalle.Segmento = dr["Segmento"].ToString();
                Detalle.SegmentoDescripcion = dr["SegmentoDescripcion"].ToString();
                Detalle.Id_Rubro = (int)dr["Id_Rubro"];
                Detalle.Rubro = dr["Rubro"].ToString();
                Detalle.RubroDescripcion = dr["RubroDescripcion"].ToString();
                Detalle.Puntuacion = (int)dr["Puntuacion"];
                Detalle.Requerido = (int)dr["Requerido"];
                Detalle.Nota = (string)dr["Nota"];
            }
            return Detalle;
        }

        public int UpdateAuditoria(int Tipo, int Tienda)
        {
            int SequenceAuditoria = 0;
            Captura Detalle = new Captura();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateAuditoria");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                SequenceAuditoria = (int)dr["sequence"];
            }
            return SequenceAuditoria;
        }

        public bool AddComentarioAuditoria(int Tipo, int Tienda, string Mensaje, DateTime fecha)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddComentarioAuditoria");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            this.Database.AddInParameter(cmd, "@Mensaje", DbType.String, Mensaje);
            this.Database.AddInParameter(cmd, "@Fecha", DbType.Date, fecha);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public bool AddDetalleAuditoria(int Tienda, int Tipo, int Segmento, int Rubro, int Aplica, string Evidencia, string Observaciones, string RegistradoPor, string Latitud, string Longitud, string Exactitud)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddDetalleAuditoria");
            this.Database.AddInParameter(cmd, "@Tienda", DbType.Int32, Tienda);
            this.Database.AddInParameter(cmd, "@Tipo ", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Segmento", DbType.Int32, Segmento);
            this.Database.AddInParameter(cmd, "@Rubro", DbType.Int32, Rubro);
            this.Database.AddInParameter(cmd, "@Aplica", DbType.Int32, Aplica);
            this.Database.AddInParameter(cmd, "@Evidencia", DbType.String, Evidencia);
            this.Database.AddInParameter(cmd, "@Observaciones", DbType.String, Observaciones);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            this.Database.AddInParameter(cmd, "@Latitud", DbType.String, Latitud);
            this.Database.AddInParameter(cmd, "@Longitud", DbType.String, Longitud);
            this.Database.AddInParameter(cmd, "@Exactitud", DbType.String, Exactitud);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public DataTable GetDetalleAuditoria(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAuditoria");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
        public DataTable GetAuditoriasFinalizadasReporte(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAuditoriasFinalizadasReporte");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
        public DataTable GetComentariosAuditoria(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetComentariosAuditorias");
            this.Database.AddInParameter(cmd, "@Auditoria", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }

        public IList<DetalleAuditoria> GetDetalleAuditoriaView(int Sequence)
        {
            List<DetalleAuditoria> detalle = new List<DetalleAuditoria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAuditoria");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<ImagenDetalle> ListaVacia = new List<ImagenDetalle>();
            while (dr.Read())
            {
                
                detalle.Add(new DetalleAuditoria
                {
                    Sequence = int.Parse(dr["sequence"].ToString()),
                    Nombre = dr["Nombre"].ToString(),
                    Tipo = dr["Tipo"].ToString(),
                    Segmento = dr["Segmento"].ToString(),
                    Rubro = dr["Rubro"].ToString(),
                    DescripcionRubro = dr["DescripcionRubro"].ToString(),
                    Puntuacion = int.Parse(dr["Puntuacion"].ToString()),
                    PuntosAplicados = int.Parse(dr["PuntosAplicados"].ToString()),
                    Aplica = dr["Aplica"].ToString(),
                    Observaciones = dr["Observaciones"].ToString(),
                    RegistradoEl = DBNull.Value.Equals(dr["RegistradoEl"]) ? DateTime.MinValue : (DateTime)dr["RegistradoEl"],
                    FinalizadoEl = DBNull.Value.Equals(dr["FinalizadoEl"]) ? DateTime.MinValue : (DateTime)dr["FinalizadoEl"],
                    TotalPuntuacion = (decimal)dr["TotalPuntuacion"],
                    Latitud = dr["Latitud"].ToString(),
                    Longitud = dr["Longitud"].ToString(),
                    Exactitud = dr["Exactitud"].ToString(),
                    Imagenes = int.Parse(dr["Imagen"].ToString()) > 0 ? GetImagenDetalle(int.Parse(dr["ID"].ToString())) : ListaVacia
                });
                
            }
            return detalle;
        }

        public void EliminarRubro(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteRubro");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.ExecuteReader(cmd);
        }

        public void EliminarSegmento(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteSegmento");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.ExecuteReader(cmd);
        }
        public IList<TiendaSIAT> GetTiendasSIAT(string Email)
        {
            List<TiendaSIAT> tiendas = new List<TiendaSIAT>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTiendas");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                tiendas.Add(new TiendaSIAT
                {
                    Id_tienda = (int)dr["id_tienda"],
                    Nombre = (string)dr["Tienda"],
                    Origen = (string)dr["Origen"]
                });
            }
            return tiendas;
        }
        public IList<ImagenDetalle> GetImagenDetalle(int Sequence)
        {
            List<ImagenDetalle> tiendas = new List<ImagenDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetImagenes");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                tiendas.Add(new ImagenDetalle
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Rubro = (int)dr["Rubro"],
                    //Imagen = Convert.ToBase64String((byte[])dr["Imagen"]),
                    Imagen64 = dr["Imagen64"].ToString()
                });
            }
            return tiendas;
        }
    }
}
