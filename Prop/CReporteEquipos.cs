using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Helper;
using PdfRpt.ColumnsItemsTemplates;


namespace Prop
{


    public class CReporteEquipos
    {
        public IPdfReportData CreatePdfReport(int iIdTorneo,string sNombreEquipo, List<tablaEquiposjugadores> listEquipos)
        {
            var sArchivo = string.Format("\\Pdf\\ReporteEquipo_{0}.pdf", sNombreEquipo);

            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata
                {
                    Author = "Victor Ignacio",
                    Application = "Prop",
                    Keywords = "Liga",
                    Subject = "Reporte de Goleo",
                    Title = "Liga"
                });
            })
            .DefaultFonts(fonts =>
            {
                fonts.Path(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\arial.ttf",
                                  Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\verdana.ttf");
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            })
            .PagesHeader(header =>
            {
                header.XHtmlHeader(rptHeader =>
                {
                    header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                    rptHeader.PageHeaderProperties(new XHeaderBasicProperties
                    {
                        RunDirection = PdfRunDirection.LeftToRight,
                        ShowBorder = true
                    });
                    rptHeader.AddPageHeader(pageHeader =>
                    {
                        string temp = System.IO.Path.GetTempPath();
                        string urlImg = temp + "imglogo" + iIdTorneo + ".jpg";
                        string urlImg2 = temp + "imglogo2" + iIdTorneo + ".jpg";

                        var message = string.Format("Reporte de Equipo {0}<hr size='0' width='100%' align='center' />", sNombreEquipo);                        
                        var image = string.Format("<img src='{0}' />", urlImg);
                        var image2 = string.Format("<img src='{0}' />", urlImg2);
                        return string.Format(@"<table style='width: 100%;font-size:25pt;font-family:tahoma;'>
										            <tr>
											            <td style='width: 25%;' align='center'>{0}</td>
                                                        <td style='width: 50%;' align='center'>{1}</td>
                                                        <td style='width: 25%;' align='center'>{2}</td>
										            </tr>										            
								                </table>", image, message, image2);
                    });

                    rptHeader.GroupHeaderProperties(new XHeaderBasicProperties
                    {
                        RunDirection = PdfRunDirection.LeftToRight,
                        ShowBorder = true,
                        SpacingBeforeTable = 10f
                    });

                });
            }).MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.ClassicTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.FitToContent);                
            })
            .MainTableDataSource(dataSource =>
            {
                dataSource.StronglyTypedList(listEquipos);
            })
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName("sNombre");                    
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(1);
                    column.HeaderCell("Nombre Jugador");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iGoles");
                    //column.IsRowNumber(true);
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Goles");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iAmarillas");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    //column.IsRowNumber(true);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Tarjetas Amarillas");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iRojas");
                    //column.IsRowNumber(true);
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Tarjetas Rojas");
                });

            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "No hay datos disponibles para mostrar");
            })
            .Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            })
            .Generate(                
                data => data.AsPdfFile(System.AppDomain.CurrentDomain.BaseDirectory + sArchivo)
                );
        }
    }

        

}

