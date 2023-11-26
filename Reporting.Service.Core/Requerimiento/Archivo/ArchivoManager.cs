using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.Archivo
{
    public class ArchivoManager : Catalog<Archivo, int, ArchivoCriteria>
    {
        public ArchivoManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Archivo LoadItem(IDataReader dr)
        {
            return new Archivo()
            {
                Identifier = (int)dr["Sequence"],
                UserName = (string)dr["UsuarioName"],
                ArchivoBase64 = (string)dr["Archivo"],
                Estatus = (bool)dr["Estatus"],
                RegistradoEl = (DateTime)dr["RegistradoEl"],
                Tipo = (EvidenciaKind)dr["Tipo"],
                Extension = (string)dr["Extension"],
                FileType = (string)dr["Extension"],
                Modulo = (int)dr["Modulo"]
            };
        }

        protected override DbCommand PrepareAddStatement(Archivo item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Archivo item)
        {
            throw new NotImplementedException();
        }

        public List<Archivo> GetArchivosByFolio(int FolioGeneral)
        {
            var lista = new List<Archivo>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetArchivoAdjunto");
            this.Database.AddInParameter(command, "@FolioGeneral", DbType.Int32, FolioGeneral);

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                lista.Add(this.LoadItem(dr));
            }

            return lista;
        }

        public bool AddArchivo(Archivo item)
        {
            try
            {

                DbCommand command = this.Database.GetStoredProcCommand("dbo.prAddArchivoAdjunto");
                this.Database.AddInParameter(command, "@FolioGeneral", DbType.Int32, item.FolioGeneral);
                this.Database.AddInParameter(command, "@Modulo", DbType.Int32, 1);
                this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.UserName);
                this.Database.AddInParameter(command, "@ArchivoBase64", DbType.String, item.ArchivoBase64);
                this.Database.AddInParameter(command, "@Tipo", DbType.Int32, item.Tipo);
                this.Database.AddInParameter(command, "@Extension", DbType.String, item.Extension);
                this.Database.AddInParameter(command, "@FileType", DbType.String, item.FileType);
                this.Database.ExecuteNonQuery(command);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Ha fallado al guardar la imagen.", ex);
            }
        }
    }
}
