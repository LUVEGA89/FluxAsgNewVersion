using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Seguimiento
{
    public class SeguimientoManager : DataRepository
    {
        public List<Llegadas> GetLLegadas(DateTime Del, DateTime Al, int Folio = 0)
        {
            List<Llegadas> Detalle = new List<Llegadas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSeguimiendoLLegadas");
            if (Folio != 0)
            {
                this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
            }
            else
            {
                this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
                this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            }

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Llegadas
                {
                    Identifier = (int)dr["Identifier"],
                    Contenedor = (string)dr["Contenedor"],
                    Proveedor = (string)dr["Proveedor"],
                    Forwarder = (string)dr["Forwarder"],
                    Embarcada = (DateTime)dr["Embarcada"],
                    LlegadaPuerto = (DateTime)dr["LlegadaPuerto"],
                    CostoFlete = (decimal)dr["CostoFlete"],
                    Estado = (EstadoCompra)dr["Estado"],
                    IsOk = (int)dr["IsOk"]
                });
            }
            return Detalle;

        }
        public List<Llegadas> GetLLegadasDetalles(DateTime Del, DateTime Al, int Estado = 0)
        {
            List<Llegadas> Detalle = new List<Llegadas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLlegadasDetalle");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, Estado);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            
            while (dr.Read())
            {
                Detalle.Add(new Llegadas
                {
                    Identifier = (int)dr["Identifier"],
                    Contenedor = (string)dr["Contenedor"],
                    Proveedor = (string)dr["Proveedor"],
                    Forwarder = (string)dr["Forwarder"],
                    Embarcada = DBNull.Value.Equals(dr["Embarcada"]) ? DateTime.MinValue : (DateTime)dr["Embarcada"],
                    LlegadaPuerto = DBNull.Value.Equals(dr["LlegadaPuerto"]) ? DateTime.MinValue : (DateTime)dr["LlegadaPuerto"],
                    SalidaPuerto = DBNull.Value.Equals(dr["SalidaPuerto"]) ? DateTime.MinValue : (DateTime)dr["SalidaPuerto"],
                    LlegadaPatco = DBNull.Value.Equals(dr["LlegaPatco"]) ? DateTime.MinValue : (DateTime)dr["LlegaPatco"],
                    SalidaPatco = DBNull.Value.Equals(dr["SalidaPatco"]) ? DateTime.MinValue : (DateTime)dr["SalidaPatco"],
                    LibTransito = DBNull.Value.Equals(dr["LibTransito"]) ? DateTime.MinValue : (DateTime)dr["LibTransito"],
                    LibDespacho = DBNull.Value.Equals(dr["LibDespacho"]) ? DateTime.MinValue : (DateTime)dr["LibDespacho"],
                    CostoFlete = (decimal)dr["CostoFlete"],
                    Estado = (EstadoCompra)dr["Estado"],
                    Comentarios = DBNull.Value.Equals(dr["Comentarios"]) ? string.Empty : (string)dr["Comentarios"],
                    //(string)dr["Coment"] != 0 ? (): null

                });
            }
            return Detalle;

        }
        public DataTable GetLLegadasDetalles2(DateTime Del, DateTime Al, int Estado = 0, int Tipo = 0)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLlegadasDetalleExcel");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, Estado);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
  
            return dt;

        }
        public object GetLlegadasFechasGastos(int folio)
        {
            Llegadas Detalle = new Llegadas();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLlegadasFechasGastos");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, folio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["Sequence"];
                Detalle.Identifier = (int)dr["Folio"];
                Detalle.CuentaDeGastosEly = DBNull.Value.Equals(dr["CuentaDeGastos"]) ? DateTime.MinValue : (DateTime)dr["CuentaDeGastos"];
                Detalle.CuentaDeGastosFinanzas = DBNull.Value.Equals(dr["CuentaDeGastosFinanzas"]) ? DateTime.MinValue : (DateTime)dr["CuentaDeGastosFinanzas"];
                Detalle.CuentaDeGastosTrafico = DBNull.Value.Equals(dr["CuentaDeGastosTrafico"]) ? DateTime.MinValue : (DateTime)dr["CuentaDeGastosTrafico"];

            }
            return Detalle;
        }

        public object UpdateSeguimientoFechasGastos(int folio, DateTime cuentaDeGastosEly, DateTime cuentaDeGastosFinanzas, DateTime cuentaDeGastosTrafico)
        {
            Llegadas Detalle = new Llegadas();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSeguimientoFechasGastos");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, folio);

            if (cuentaDeGastosEly != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@CuentaDeGastos", DbType.Date, cuentaDeGastosEly);

            if (cuentaDeGastosFinanzas != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@CuentaDeGastosFinanzas", DbType.Date, cuentaDeGastosFinanzas);

            if (cuentaDeGastosTrafico != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@CuentaDeGastosTrafico", DbType.Date, cuentaDeGastosTrafico);
            
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["Sequence"];
                Detalle.Identifier = (int)dr["Folio"];
                Detalle.CuentaDeGastosEly = DBNull.Value.Equals(dr["cuentaDeGastos"]) ? DateTime.MinValue : (DateTime)dr["cuentaDeGastos"];
                Detalle.CuentaDeGastosFinanzas = DBNull.Value.Equals(dr["cuentaDeGastosFinanzas"]) ? DateTime.MinValue : (DateTime)dr["cuentaDeGastosFinanzas"];
                Detalle.CuentaDeGastosTrafico = DBNull.Value.Equals(dr["cuentaDeGastosTrafico"]) ? DateTime.MinValue : (DateTime)dr["cuentaDeGastosTrafico"];

            }
            return Detalle;
        }

        public Llegadas GetLlegadasFechas(int Folio)
        {
            Llegadas Detalle = new Llegadas();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLlegadasFechas");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["Sequence"];
                Detalle.Identifier = (int)dr["Folio"];
                Detalle.Embarcada = DBNull.Value.Equals(dr["Embarcada"]) ? DateTime.MinValue : (DateTime)dr["Embarcada"];
                Detalle.LlegadaPuerto = DBNull.Value.Equals(dr["LlegaPuerto"]) ? DateTime.MinValue : (DateTime)dr["LlegaPuerto"];
                Detalle.SalidaPuerto = DBNull.Value.Equals(dr["SalidaPuerto"]) ? DateTime.MinValue : (DateTime)dr["SalidaPuerto"];
                Detalle.LlegadaPatco = DBNull.Value.Equals(dr["LlegaPatco"]) ? DateTime.MinValue : (DateTime)dr["LlegaPatco"];
                Detalle.SalidaPatco = DBNull.Value.Equals(dr["SalidaPatco"]) ? DateTime.MinValue : (DateTime)dr["SalidaPatco"];
                Detalle.LibTransito = DBNull.Value.Equals(dr["LibTransito"]) ? DateTime.MinValue : (DateTime)dr["LibTransito"];
                Detalle.LibDespacho = DBNull.Value.Equals(dr["LibDespacho"]) ? DateTime.MinValue : (DateTime)dr["LibDespacho"];

            }
            return Detalle;

        }

        public void UpdateLlegadaEstado(int folio, int estado)
        {
            List<Comentario> Detalle = new List<Comentario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateLlegadaEstado");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, folio);
            this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, estado);

            IDataReader dr = this.Database.ExecuteReader(cmd);
        }

        public Llegadas UpdateSeguimientoFechas(int Folio, DateTime embarcada, DateTime llegadaPuerto, DateTime salidaPuerto, DateTime llegadaPantaco, DateTime salidaPantaco, DateTime? LibTransito = null , DateTime? LibDespacho = null)
        {
            Llegadas Detalle = new Llegadas();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSeguimientoFechas");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);

            if (embarcada != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@Embarcada", DbType.Date, embarcada);

            if (llegadaPuerto != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@LlegadaPuerto", DbType.Date, llegadaPuerto);

            if (salidaPuerto != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@SalidaPuerto", DbType.Date, salidaPuerto);

            if (llegadaPantaco != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@LlegadaPantaco", DbType.Date, llegadaPantaco);

            if (salidaPantaco != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@SalidaPantaco", DbType.Date, salidaPantaco);

            if (LibTransito != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@LibTransito", DbType.Date, LibTransito);

            if (LibDespacho != DateTime.Parse("0001/01/01"))
                this.Database.AddInParameter(cmd, "@LibDespacho", DbType.Date, LibDespacho);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["Sequence"];
                Detalle.Identifier = (int)dr["Folio"];
                Detalle.Embarcada = DBNull.Value.Equals(dr["Embarcada"]) ? DateTime.MinValue : (DateTime)dr["Embarcada"];
                Detalle.LlegadaPuerto = DBNull.Value.Equals(dr["LlegaPuerto"]) ? DateTime.MinValue : (DateTime)dr["LlegaPuerto"];
                Detalle.SalidaPuerto = DBNull.Value.Equals(dr["SalidaPuerto"]) ? DateTime.MinValue : (DateTime)dr["SalidaPuerto"];
                Detalle.LlegadaPatco = DBNull.Value.Equals(dr["LlegaPatco"]) ? DateTime.MinValue : (DateTime)dr["LlegaPatco"];
                Detalle.SalidaPatco = DBNull.Value.Equals(dr["SalidaPatco"]) ? DateTime.MinValue : (DateTime)dr["SalidaPatco"];
                Detalle.LibTransito = DBNull.Value.Equals(dr["LibTransito"]) ? DateTime.MinValue : (DateTime)dr["LibTransito"];
                Detalle.LibDespacho = DBNull.Value.Equals(dr["LibDespacho"]) ? DateTime.MinValue : (DateTime)dr["LibDespacho"];

            }
            return Detalle;
        }

        public void AddComentario(int folio, string mensaje)
        {
            List<Comentario> Detalle = new List<Comentario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prLLegadasAddComentario");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, folio);
            this.Database.AddInParameter(cmd, "@Mensaje", DbType.String, mensaje);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }

        public List<Comentario> GetLlegadasComentarios(int Folio)
        {
            List<Comentario> Detalle = new List<Comentario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLLegadasComentarios");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Comentario
                {
                    Folio = (int)dr["Folio"],
                    Sequence = (int)dr["Sequence"],
                    Detalle = (string)dr["Detalle"],
                    RegistradoEl = DBNull.Value.Equals(dr["RegistradoEl"]) ? DateTime.MinValue : (DateTime)dr["RegistradoEl"]
                });
            }

            return Detalle;
        }

    }
}
