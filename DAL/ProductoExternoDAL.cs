using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace DAL
{
    public class ProductoExternoDAL
    {
        public static Entidades.ProductoExterno InsertarProductoExterno(Entidades.ProductoExterno productoExterno)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_PROD_PRODUCTO_INS");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, productoExterno.Id);
            db.AddInParameter(dbCommand, "ID_EXTERNO", DbType.Int32, productoExterno.Id_Externo != 0 ? productoExterno.Id_Externo : (object)null);
            db.AddInParameter(dbCommand, "EST_ID", DbType.Int32, productoExterno.Est_Id != 0 ? productoExterno.Est_Id : (object)null);
            db.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, productoExterno.Descripcion != null ? productoExterno.Descripcion : (object)null);
            db.AddInParameter(dbCommand, "FACTOR_SEGURIDAD", DbType.Decimal, productoExterno.FactorSeguridad != 0 ? productoExterno.FactorSeguridad : (object)null);
            db.AddInParameter(dbCommand, "UNID_ID", DbType.Int32, productoExterno.Unid_Id != 0 ? productoExterno.Unid_Id : (object)null);
            db.AddInParameter(dbCommand, "ELIMINADO", DbType.Byte, productoExterno.Eliminado == true ? 1 : 0);

            productoExterno.Id = int.Parse(db.ExecuteScalar(dbCommand).ToString());

            return productoExterno;
        }
        public static List<Entidades.ProductoExterno> ObtenerProductoExterno(Entidades.Filtro filtro)
        {
            List<Entidades.ProductoExterno> lista = new List<Entidades.ProductoExterno>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_PROD_PRODUCTO_LEER");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, filtro.Id != 0 ? filtro.Id : (object)null);
            db.AddInParameter(dbCommand, "EST_ID", DbType.Int32, filtro.Est_Id != 0 ? filtro.Est_Id : (object)null);
            db.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, filtro.Descripcion != "" ? filtro.Descripcion : (object)null);

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int ID_EXTERNO = reader.GetOrdinal("ID_EXTERNO");
                int EST_ID = reader.GetOrdinal("EST_ID");
                int ESTABLECIMIENTO = reader.GetOrdinal("ESTABLECIMIENTO");
                int DESCRIPCION = reader.GetOrdinal("DESCRIPCION");
                int UNID_ID = reader.GetOrdinal("UNID_ID");
                int UNIDAD = reader.GetOrdinal("UNIDAD");

                while (reader.Read())
                {
                    Entidades.ProductoExterno OBJ = new Entidades.ProductoExterno();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Id_Externo = (int)(!reader.IsDBNull(ID_EXTERNO) ? reader.GetValue(ID_EXTERNO) : 0);
                    OBJ.Est_Id = (int)(!reader.IsDBNull(EST_ID) ? reader.GetValue(EST_ID) : 0);
                    OBJ.Establecimiento = (String)(!reader.IsDBNull(ESTABLECIMIENTO) ? reader.GetValue(ESTABLECIMIENTO) : string.Empty);
                    OBJ.Descripcion = (String)(!reader.IsDBNull(DESCRIPCION) ? reader.GetValue(DESCRIPCION) : string.Empty);
                    OBJ.Unid_Id = (int)(!reader.IsDBNull(UNID_ID) ? reader.GetValue(UNID_ID) : 0);
                    OBJ.Unidad = (String)(!reader.IsDBNull(UNIDAD) ? reader.GetValue(UNIDAD) : string.Empty);
                    //EndFields

                    lista.Add(OBJ);
                }
            }
            catch (Exception ex)
            {
                //GlobalesDAO.InsertErrores(ex);
                throw ex;
            }
            finally
            {
                reader.Close();
            }



            return lista;

        }
    }
}
