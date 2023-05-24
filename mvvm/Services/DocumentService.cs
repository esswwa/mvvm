using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;

namespace mvvm.Services
{
    public class DocumentService
    {
        public async Task GetCheck(Point PickupPoint, int OrderCode, int OrderNumber, List<DbProduct> Products)
        {
            Func<float> GetFullPrice = () =>
            {
                float ammount = 0;
                foreach (var item in Products)
                {
                    ammount += (float)item.DisplayedPrice * Global.CurrentCart.First(c => c.ArticleName.Equals(item.Article)).Count;
                }
                return ammount;
            };

            PdfWriter writer = new($"Товарный чек.pdf");
            PdfDocument pdf = new(writer);
            Document document = new(pdf);

            PdfFont comic = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\Arial.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);

            var content = new Paragraph($"PishiStiray")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                .SetFont(comic)
                .SetFontSize(12);

            document.Add(content);

            content = new Paragraph($"Товарный чек № {string.Format("{0}", OrderNumber)} от {DateOnly.FromDateTime(DateTime.Now).ToString("D")}")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16);

            document.Add(content);

            var table = new Table(7)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetHeight(UnitValue.CreatePercentValue(100))
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            table.AddCell(new Paragraph("№ п/п")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Артикул")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Наименование")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Описание")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Количество")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Цена")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Сумма")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            for (int i = 0; i < Products.Count; i++)
            {
                table.AddCell(new Paragraph(i.ToString())
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(comic)
                   .SetFontSize(14));

                table.AddCell(new Paragraph(Products[i].Article)
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(comic)
                   .SetFontSize(14));
                table.AddCell(new Paragraph(Products[i].Title)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));
                table.AddCell(new Paragraph(Products[i].Description)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));
                table.AddCell(new Paragraph(Global.CurrentCart.First(c => c.ArticleName.Equals(Products[i].Article)).Count.ToString())
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));

                table.AddCell(new Paragraph(string.Format("{0:C2}", Products[i].DisplayedPrice))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));

                table.AddCell(new Paragraph(string.Format("{0:C2}", Products[i].DisplayedPrice * Global.CurrentCart.First(c => c.ArticleName.Equals(Products[i].Article)).Count))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));
            }

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Итого")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(string.Format("{0:C2}", GetFullPrice()))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            document.Add(table);

            content = new Paragraph(string.Format("Пункт выдачи: {0}, г. {1}, ул. {2}, д. {3}",
                PickupPoint.Index, PickupPoint.Country, PickupPoint.Street, PickupPoint.House))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                .SetFont(comic)
                .SetFontSize(14);

            document.Add(content);

            content = new Paragraph(string.Format("Код получения:\n {0}", OrderCode))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16);

            document.Add(content);

            document.Close();

            await Task.CompletedTask;
        }


    }
}
