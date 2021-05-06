using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;
using KERBERO.BL.Components;
using KERBERO.Util;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace KERBERO.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Kerbero" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Kerbero.svc o Kerbero.svc.cs en el Explorador de soluciones e inicie la depuración.
    public partial class Kerbero : IKerbero
    {
        IAuditoria objAuditoria;

        public RespuestaOperacionServicio BuscarAuditoria(string CodSistema, string Usuario, string FechaInicio, string FechaFin)
        {
            List<Evento> lstLog = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objAuditoria = new Auditoria();
                lstLog = objAuditoria.buscarAuditoria(CodSistema, Usuario, FechaInicio, FechaFin);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = lstLog;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ExportarExcel(string CodSistema, string Usuario, string FechaInicio, string FechaFin)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            List<Evento> lstLog = null;
            try
            {
                objAuditoria = new Auditoria();
                lstLog = objAuditoria.buscarAuditoria(CodSistema, Usuario, FechaInicio, FechaFin);
                MemoryStream ms = new MemoryStream();

                using (ExcelPackage xlPackage = new ExcelPackage(ms))
                {
                    var wb = xlPackage.Workbook;
                    var ws = wb.Worksheets.Add("ConsultaLogCerbero");

                    ws.Cells["A2"].LoadFromCollection(lstLog, false);
                    ws.DeleteColumn(1);

                    ws.Cells["A1"].Value = "Fecha de Registro";
                    ws.Cells["B1"].Value = "Sistema";
                    ws.Cells["C1"].Value = "Usuario";
                    ws.Cells["D1"].Value = "Descripción";
                    ws.Cells["E1"].Value = "Origen";

                    int lastRow = ws.Dimension.End.Row;
                    int lastColumn = ws.Dimension.End.Column;

                    ws.Cells[1, 1, lastRow, 1].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
                    ws.Cells[1, 1, lastRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    BorderCelda(ws.Cells[1, 1, lastRow, lastColumn]);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Column(1).Width = 22;
                    xlPackage.Save();
                    rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                    rpta.data = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        private void BorderCelda(ExcelRange objCell)
        {
            Border border = objCell.Style.Border;
            border.Bottom.Style = ExcelBorderStyle.Thin;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
        }
    }
}
