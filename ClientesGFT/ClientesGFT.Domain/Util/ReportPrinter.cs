using ClientesGFT.Domain.Entities;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClientesGFT.Domain.Util
{
    public static class ReportPrinter
    {
        public static string GetFileName() => $"fluxos-aprovacao__{DateTime.Now:yyyy-MM-dd_hh-mm}.xlsx";

        public static MemoryStream PrintAsStream(List<Fluxo> fluxos, FluxoFilter filter)
        {
            var fs = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Fluxos");

                //Titulo
                worksheet.Cell("B2").Value = "Relatório de Aprovações!";
                var range = worksheet.Range("B2:D2");
                range.Merge().Style.Font.SetBold().Font.FontSize = 20;

                //Datas
                worksheet.Cell("E2").Value = $"{filter.StartDate.ToShortDateString()} até {filter.EndDate.ToShortDateString()}";
                range = worksheet.Range("E2:F2");
                range.Merge();
                range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                range.Style.Font.SetFontSize(12);


                //Cabeçalhos
                worksheet.Cell("B3").Value = "Usuário Responsável";
                worksheet.Cell("C3").Value = "CPF do Cliente";
                worksheet.Cell("D3").Value = "Cliente";
                worksheet.Cell("E3").Value = "Status";
                worksheet.Cell("F3").Value = "Data de Criação";

                //Corpo
                var row = 4;

                for (int i = 0; i < fluxos.Count; i++)
                {
                    var fluxo = fluxos[i];

                    worksheet.Cell($"B{row}").Value = fluxo.User.Name;
                    worksheet.Cell($"C{row}").Value = fluxo.Client.CPF;
                    worksheet.Cell($"D{row}").Value = fluxo.Client.Name;
                    worksheet.Cell($"E{row}").Value = fluxo.Status.Description.GetDisplayName();
                    worksheet.Cell($"F{row}").Value = fluxo.CreateDate;

                    row++;
                }

                range = worksheet.Range($"B3:F{--row}");
                range.CreateTable();

                worksheet.Columns("2-6").AdjustToContents();


                workbook.SaveAs(fs);
            }

            fs.Position = 0;
            return fs;
        }
    }
}
