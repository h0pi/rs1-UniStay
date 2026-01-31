using QuestPDF.Infrastructure;
using UniStay.API.Data.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;




namespace UniStay.API.Helper.Pdf
{
    public static class InvoicePdfGenerator
    {
        public static byte[] Generate(Invoices invoice)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // ================= HEADER =================
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Text("UniStay")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        row.ConstantItem(150)
                            .AlignRight()
                            .Text($"ID -> #{invoice.InvoiceID}")
                            .FontSize(12)
                            .SemiBold();
                    });

                    // ================= CONTENT =================
                    page.Content().PaddingVertical(30).Column(col =>
                    {
                        col.Item()
                            .PaddingBottom(15)
                            .Text("Invoice Details")
                            .FontSize(16)
                            .Bold();

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Student ID:");
                            r.ConstantItem(200)
                                .AlignRight()
                                .Text(invoice.StudentID.ToString())
                                .SemiBold();
                        });

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Total Amount:");
                            r.ConstantItem(200)
                                .AlignRight()
                                .Text($"{invoice.TotalAmount:0.00} KM")
                                .SemiBold();
                        });

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Status:");
                            r.ConstantItem(200)
                                .AlignRight()
                                .Text(invoice.Paid ? "PAID" : "UNPAID")
                                .FontColor(invoice.Paid ? Colors.Green.Medium : Colors.Red.Medium)
                                .SemiBold();
                        });
                    });

                    // ================= FOOTER =================
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Issued on: ");
                        text.Span(DateTime.Now.ToString("dd.MM.yyyy")).SemiBold();
                    });
                });
            })
            .GeneratePdf();
        }
    }
}
