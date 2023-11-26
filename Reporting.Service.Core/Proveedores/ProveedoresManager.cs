using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Proveedores
{
    //Se refiere al pago de provverdores pero por comodidad las clases se llaman Proveedores
    public class ProveedoresManager : Catalog<Proveedor, int, ProveedorCriteria>
    {
        private string Empresa;
        private int Tipo;

        public ProveedoresManager()
        {
        }

        public ProveedoresManager(string empresa, int tipo)
        {
            Empresa = empresa;
            Tipo = tipo;
        }

        public ProveedoresManager(string empresa)
        {
            Empresa = empresa;
        }

        protected override string FindPagedItemsProcedure => "prFindPagoProveedores";

        protected override Proveedor LoadItem(IDataReader dr)
        {
            return new Proveedor
            {
                Identifier = (int)dr["DocNum"],
                CardName = (string)dr["CardName"],
                DocDate = dr["DocDate"] == DBNull.Value ? null : (DateTime?)dr["DocDate"],
                DocTotal = (decimal)dr["DocTotal"],
                FechaPago = dr["FechaPago"] == DBNull.Value ? null : (DateTime?)dr["FechaPago"],
                Referencia = (string)dr["Referencia"],
                MontoPagado = (decimal)dr["MontoPagado"],
                TotalPagar = (decimal)dr["TotalPagar"],
                Archivo = (string)dr["Archivo"],
                Banco = (string)dr["Banco"],
                Cuenta = (string)dr["Cuenta"],
                Clave = (string)dr["Clave"],
                Uuid = (string)dr["Uuid"],
                Descripcion = (string)dr["Descripcion"],
                Sucursal = (string)dr["Sucursal"],
                MetodoPago = (string)dr["MetodoPago"],
                LineaCaptura = (string)dr["LineaCaptura"],
                Contenedor = (string)dr["Contenedor"],
                Moneda = (string)dr["Moneda"],
                Rfc = (string)dr["Rfc"]
            };
        }

        protected override DbCommand PrepareAddStatement(Proveedor item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPagoProveedor");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, this.Empresa);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, this.Tipo);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Proveedor item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ProveedorCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Status", DbType.Int32, criteria.Status);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, criteria.Empresa);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, criteria.Tipo);
            this.Database.AddInParameter(cmd, "@EstatusPagos", DbType.Int32, criteria.EstatusPagos);

            return cmd;
        }

        public ResponsePagos AddPagosIniciaProceso(Pagos pagos, string empresa, int tipo)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prAddPagosProveedoresIniciaProceso");
                this.Database.AddInParameter(command, "@RegistradoPor", DbType.String);
                this.Database.AddInParameter(command, "@DocNum", DbType.Int32);
                this.Database.AddInParameter(command, "@FechaPago", DbType.DateTime);
                this.Database.AddInParameter(command, "@Empresa", DbType.String);
                this.Database.AddInParameter(command, "@Tipo", DbType.Int32);

                DbParameter RegistradoPor = command.Parameters["@RegistradoPor"];
                DbParameter DocNum = command.Parameters["@DocNum"];
                DbParameter FechaPago = command.Parameters["@FechaPago"];
                DbParameter Empresa = command.Parameters["@Empresa"];
                DbParameter Tipo = command.Parameters["@Tipo"];

                foreach (Pedido item in pagos.Pedidos)
                {
                    RegistradoPor.Value = pagos.RegistradoPor;
                    DocNum.Value = item.DocNum;
                    FechaPago.Value = item.FechaPago;
                    Empresa.Value = empresa;
                    Tipo.Value = tipo;

                    this.Database.ExecuteNonQuery(command, transaction);
                }

                transaction.Commit();

                return new ResponsePagos { Correct = true, Folio = 0 };
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    DbConnection connection1 = transaction.Connection;
                    transaction.Rollback();

                    if (connection1 != null && connection1.State == ConnectionState.Open)
                    {
                        connection1.Close();
                    }
                }

                return new ResponsePagos { Correct = false, Folio = 0 };
            }
        }
        
        public void ActualizarPago(int Identifier, EstatusPagos estatusPagos, DbTransaction transaction = null)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateEstatusPagos");
                this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, Identifier);
                this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, estatusPagos);

                if (transaction == null)
                {
                    this.Database.ExecuteNonQuery(cmd);
                }
                else
                {
                    this.Database.ExecuteNonQuery(cmd, transaction);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ResponsePagos AddPagosPrimerAutorizacion(Pagos pagos, string empresa, int tipo)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                foreach (Pedido item in pagos.Pedidos)
                {
                    this.ActualizarPago(item.DocNum, EstatusPagos.AprobadoContadorSr, transaction);
                }

                transaction.Commit();

                return new ResponsePagos { Correct = true, Folio = 0 };
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    DbConnection connection1 = transaction.Connection;
                    transaction.Rollback();

                    if (connection1 != null && connection1.State == ConnectionState.Open)
                    {
                        connection1.Close();
                    }
                }

                return new ResponsePagos { Correct = false, Folio = 0 };
            }
        }

        public ResponsePagos AddPagos(Pagos pagos, string empresa, int tipo)
        {
            var FolioDB = 1;
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetLastFolio");
                this.Database.AddInParameter(cmd, "@Empresa", DbType.String, empresa);
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, tipo);

                IDataReader dr = this.Database.ExecuteReader(cmd, transaction);
                while (dr.Read())
                {
                    FolioDB = (int)dr["MaxFolio"];
                }
                cmd.Dispose();
                dr.Close();
                dr.Dispose();

                DbCommand command = this.Database.GetStoredProcCommand("prAddPagosProveedores");
                this.Database.AddInParameter(command, "@RegistradoPor", DbType.String);
                this.Database.AddInParameter(command, "@DocNum", DbType.Int32);
                this.Database.AddInParameter(command, "@FechaPago", DbType.DateTime);
                this.Database.AddInParameter(command, "@Folio", DbType.Int32);
                this.Database.AddInParameter(command, "@Empresa", DbType.String);
                this.Database.AddInParameter(command, "@Tipo", DbType.Int32);

                DbParameter RegistradoPor = command.Parameters["@RegistradoPor"];
                DbParameter DocNum = command.Parameters["@DocNum"];
                DbParameter FechaPago = command.Parameters["@FechaPago"];
                DbParameter Folio = command.Parameters["@Folio"];
                DbParameter Empresa = command.Parameters["@Empresa"];
                DbParameter Tipo = command.Parameters["@Tipo"];

                foreach (Pedido item in pagos.Pedidos)
                {
                    RegistradoPor.Value = pagos.RegistradoPor;
                    DocNum.Value = item.DocNum;
                    FechaPago.Value = item.FechaPago;
                    Folio.Value = FolioDB;
                    Empresa.Value = empresa;
                    Tipo.Value = tipo;

                    this.Database.ExecuteNonQuery(command, transaction);
                }

                transaction.Commit();

                return new ResponsePagos { Correct = true, Folio = FolioDB };
            }
            catch(Exception ex)
            {
                if (transaction != null)
                {
                    DbConnection connection1 = transaction.Connection;
                    transaction.Rollback();

                    if (connection1 != null && connection1.State == ConnectionState.Open)
                    {
                        connection1.Close();
                    }
                }

                return new ResponsePagos { Correct = false, Folio = 0};
            }
        }

        public List<Proveedor> GetPedidos(int Folio)
        {
            List<Proveedor> pedidos = new List<Proveedor>();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetPagoProveedorReenviarCorreo");
                this.Database.AddInParameter(cmd, "@Empresa", DbType.String, this.Empresa);
                this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {

                    pedidos.Add(new Proveedor
                    {
                        Identifier = (int)dr["DocNum"],
                        CardName = (string)dr["CardName"],
                        DocDate = dr["DocDate"] == DBNull.Value ? null : (DateTime?)dr["DocDate"],
                        DocTotal = (decimal)dr["DocTotal"],
                        FechaPago = dr["FechaPago"] == DBNull.Value ? null : (DateTime?)dr["FechaPago"],
                        Referencia = (string)dr["Referencia"],
                        MontoPagado = (decimal)dr["MontoPagado"],
                        TotalPagar = (decimal)dr["TotalPagar"],
                        Archivo = (string)dr["Archivo"],
                        Banco = (string)dr["Banco"],
                        Cuenta = (string)dr["Cuenta"],
                        Clave = (string)dr["Clave"],
                        Uuid = (string)dr["Uuid"],
                        Descripcion = (string)dr["Descripcion"],
                        Sucursal = (string)dr["Sucursal"],
                        MetodoPago = (string)dr["MetodoPago"],
                        LineaCaptura = (string)dr["LineaCaptura"],
                        Contenedor = (string)dr["Contenedor"],
                        Moneda = (string)dr["Moneda"],
                        Rfc = (string)dr["Rfc"]
                    });
                }
                cmd.Dispose();
                dr.Close();
                dr.Dispose();

                return pedidos;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Anexo> GetAnexos(int DocNum, int Tipo)
        {
            List<Anexo> anexos = new List<Anexo>();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnexosProveedores");
                this.Database.AddInParameter(cmd, "@Empresa", DbType.String, this.Empresa);
                this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var path = dr["Path"] == DBNull.Value ? "" : (string)dr["Path"];
                    var filename = dr["FileName"] == DBNull.Value ? "" : (string)dr["FileName"];
                    var fileext = dr["FileExt"] == DBNull.Value ? "" : (string)dr["FileExt"];
                    var subpath = dr["SubPath"] == DBNull.Value ? "" : (string)dr["SubPath"];
                    anexos.Add(new Anexo
                    {
                        Identifier = (int)dr["DocNum"],
                        Path = path,
                        FileName = filename,
                        FileExt = fileext,
                        SubPath = subpath,
                        Base64 = this.ToBase64(path, filename, fileext, subpath)

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

        private string ToBase64(string path, string filename, string fileext, string subpath)
        {
            byte[] AsBytes = File.ReadAllBytes($"{path}\\{subpath}\\{filename}.{fileext}");
            return Convert.ToBase64String(AsBytes);
        }
    }
}
