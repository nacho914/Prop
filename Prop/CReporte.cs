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
    public class User
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string LastName { set; get; }
        public long Balance { set; get; }
        public DateTime RegisterDate { set; get; }
    }

    public class IListPdfReport
    {

        public IPdfReportData CreatePdfReport(int iIdTorneo,List<TablaEquipos> itemsTabla)
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Landscape);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata
                {
                    Author = "Victor Ignacio",
                    Application = "Prop",
                    Keywords = "Liga",
                    Subject = "Reporte de liga",
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

                        var message = "Puntuación General de la Liga <hr size='0' width='100%' align='center' />";
                        //var photo = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "pepsi.png");
                        //var photo2 = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "pepsi2.png");
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
                //table.NumberOfDataRowsPerPage(20);
            })
            .MainTableDataSource(dataSource =>
            {
                dataSource.StronglyTypedList(itemsTabla);
            })
            /*.MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Summary");
                summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                summarySettings.PageSummarySettings("Page Summary");
            })*/
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName("iLugar");
                    column.IsRowNumber(true);
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(1);
                    column.HeaderCell("Lugar");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("sEquipo");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Equipo");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iJJ");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("JJ");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iJG");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("JG");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iJE");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("JE");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iJP");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("JP");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iGF");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("GF");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iGC");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("GC");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iDif");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Dif");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("iPuntos");
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Puntos");
                });

                /*
                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.LastName);
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(3);
                    column.HeaderCell("Last Name");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Balance);
                    column.CellsHorizontalAlignment(PdfRpt.Core.Contracts.HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("Balance");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                    });
                    column.AggregateFunction(aggregateFunction =>
                    {
                        aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                        aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                    });
                });*/

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
            .Generate(data => data.AsPdfFile(System.AppDomain.CurrentDomain.BaseDirectory + "\\Pdf\\ReporteTabla.pdf"));
        }
    }

        

}

