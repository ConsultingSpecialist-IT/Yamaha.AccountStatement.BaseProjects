using iTextSharp.text;
using iTextSharp.text.pdf;
using Yamaha.AccountStatement.Core.Models;

namespace Yamaha.AccountStatement.Infrastructure.Services
{
    internal sealed class PdfClientStatement
    {
        private readonly DealerBalance _header;
        private readonly List<DealerAccountStatement> _details;
        private readonly decimal _balanceDue;
        private readonly List<BankAccounts> _bankAccounts;
        private readonly string _logoImagePath;

        private static Font Black8 => new(Font.FontFamily.HELVETICA, 8f, Font.NORMAL, BaseColor.BLACK);
        private static Font Black9 => new(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK);
        private static Font Black10 => new(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK);
        private static Font Black11 => new(Font.FontFamily.HELVETICA, 11f, Font.NORMAL, BaseColor.BLACK);
        private static Font Black14 => new(Font.FontFamily.HELVETICA, 14f, Font.NORMAL, BaseColor.BLACK);
        private static Font Bold8 => new(Font.FontFamily.HELVETICA, 8f, Font.BOLD, BaseColor.BLACK);
        private static Font Bold9 => new(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.BLACK);
        private static Font Bold10 => new(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.BLACK);
        private static Font Courier9 => new(Font.FontFamily.COURIER, 9f, Font.NORMAL, BaseColor.BLACK);
        private static Font Courier10 => new(Font.FontFamily.COURIER, 10f, Font.NORMAL, BaseColor.BLACK);

        public PdfClientStatement(
            DealerBalance header,
            List<DealerAccountStatement> details,
            decimal balanceDue,
            List<BankAccounts> bankAccounts)
        {
            _header = header ?? throw new ArgumentNullException(nameof(header));
            _details = details ?? [];
            _balanceDue = balanceDue;
            _bankAccounts = bankAccounts ?? [];
            _logoImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images", "logo_yamaha.gif");
        }

        public byte[] Process()
        {
            using var memoryStream = new MemoryStream();
            using var document = new Document(PageSize.LETTER, 20, 20, 10, 10);

            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            var pdfDest = new PdfDestination(PdfDestination.XYZ, 0, document.PageSize.Height, 1f);

            document.Open();
            document.AddAuthor(string.Empty);
            document.AddTitle("Estado de Cuenta");

            writer.SetOpenAction(PdfAction.GotoLocalPage(1, pdfDest, writer));

            AddLogo(document);

            document.Add(BuildStatementHeader());
            document.Add(BuildStatementCreditData(1));
            document.Add(BuildStatementDetails());
            document.Add(BuildStatementFooter());

            document.Close();
            writer.Close();

            return memoryStream.ToArray();
        }

        private void AddLogo(Document document)
        {
            if (!File.Exists(_logoImagePath))
                return;

            var image = Image.GetInstance(_logoImagePath);
            image.ScalePercent(40f);
            image.SetAbsolutePosition(30, document.PageSize.Height - 37f);
            document.Add(image);
        }

        private PdfPTable BuildStatementHeader()
        {
            var table = new PdfPTable(20)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_CENTER,
                SpacingBefore = 1f
            };

            table.SetWidths(Enumerable.Repeat(1f, 20).ToArray());

            table.AddCell(new PdfPCell(new Phrase("Yamaha de México S.A. de C.V.", Black14))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BorderWidth = 0,
                PaddingBottom = 0,
                Colspan = 20
            });

            table.AddCell(NoBorderCell("", Black11, 8, Element.ALIGN_LEFT));
            table.AddCell(NoBorderCell("\nEstado de Cuenta", Black11, 11, Element.ALIGN_RIGHT));
            table.AddCell(NoBorderCell("", Black11, 1, Element.ALIGN_LEFT));

            return table;
        }

        private PdfPTable BuildStatementCreditData(int pageNumber)
        {
            var table = new PdfPTable(42) { WidthPercentage = 100f };
            table.DefaultCell.BorderWidth = Rectangle.NO_BORDER;

            AddTopFrame(table);

            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell(GetClientName(), "", 15, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(LeftMarginCell("Saldo Balance: ", "", 5, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(GetHeaderBalance().ToString("N2"), "", 5, Courier10, 1, 1, 10f, Element.ALIGN_RIGHT));

            table.AddCell(WithoutMarginCell("", "", 2, Courier9, 2, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("Sus pagos deberán ser transferidos a:", "", 12, Courier9, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("Corte:      ", "", 5, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(DateTime.Now.ToString("yyyy-MM"), "", 10, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(LeftMarginCell("Saldo Vencido: ", "", 5, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(_balanceDue.ToString("N2"), "", 5, Courier10, 1, 1, 10f, Element.ALIGN_RIGHT));

            table.AddCell(WithoutMarginCell("", "", 2, Courier9, 2, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(GetBankAccountText(0), "", 12, Courier9, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("Impresión:  ", "", 5, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(DateTime.Now.ToString("dd/MM/yyyy"), "", 10, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(LeftMarginCell("No. Referencia: ", "", 5, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(GetClientKey(), "", 5, Courier10, 1, 1, 10f, Element.ALIGN_RIGHT));

            table.AddCell(WithoutMarginCell("", "", 2, Courier9, 2, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell(GetBankAccountText(1), "", 12, Courier9, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 15, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 24, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(MarginTop("", "", 15, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(MarginTop("Página " + pageNumber + " / 1", "", 24, Courier9, 1, 1, 10f, Element.ALIGN_RIGHT));
            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            return table;
        }

        private PdfPTable BuildStatementDetails()
        {
            var table = new PdfPTable(42) { WidthPercentage = 100f, HorizontalAlignment = Element.ALIGN_CENTER };
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Documento", "", 25, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Pagos", "", 10, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(new PdfPCell(new Phrase("Saldo", Bold8))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 2,
                Rowspan = 2,
                Colspan = 5
            });
            table.AddCell(LeftMarginCell("", "", 1, Black10, 2, 1, 20f, Element.ALIGN_CENTER));

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Tipo", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Número", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Fecha", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Vencimiento", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Importe", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Importe", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Fecha", "", 5, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 2, 1, 20f, Element.ALIGN_CENTER));

            int rows = 0;
            bool paymentHeaderAdded = false;
            decimal finalBalance = 0m;

            foreach (DealerAccountStatement detail in _details)
            {
                finalBalance = detail.Balance;

                if (!IsPayment(detail.DocumentType))
                {
                    AddRegularDetailRow(table, detail);
                }
                else
                {
                    if (!paymentHeaderAdded)
                    {
                        paymentHeaderAdded = true;
                        AddPaymentSectionHeader(table);
                    }

                    AddPaymentRows(table, detail);
                }

                rows++;
            }

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("************************************************************************ Gracias por sus pagos realizados ************************************************************************", "", 40, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));

            if (rows < 48)
            {
                for (int i = 0; i < 48 - rows; i++)
                {
                    AddEmptyDetailRow(table);
                }
            }

            AddTotals(table, finalBalance);

            return table;
        }

        private static bool IsPayment(string? documentType)
        {
            return string.Equals(documentType, "PAGO", StringComparison.OrdinalIgnoreCase)
                || string.Equals(documentType, "P", StringComparison.OrdinalIgnoreCase);
        }

        private void AddRegularDetailRow(PdfPTable table, DealerAccountStatement detail)
        {
            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(detail.DocumentType, "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell(detail.DocumentNumber.ToString(), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(FormatDate(detail.DocumentDate), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(FormatDate(detail.ExpirationDate), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(detail.TransactionAmount.ToString("N2"), "", 5, Black10, 1, 1, 10f, Element.ALIGN_RIGHT));
            table.AddCell(LeftMarginCell(detail.PaymentAmount.ToString("N2"), "", 5, Black10, 1, 1, 10f, Element.ALIGN_RIGHT));
            table.AddCell(LeftMarginCell(FormatDate(detail.PaymentDate), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(detail.Balance.ToString("N2"), "", 5, Black10, 1, 1, 10f, Element.ALIGN_RIGHT));
            table.AddCell(LeftMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
        }

        private void AddPaymentSectionHeader(PdfPTable table)
        {
            AddEmptyDetailRow(table);

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("Pagos realizados", "", 10, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
        }

        private void AddPaymentRows(PdfPTable table, DealerAccountStatement detail)
        {
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(detail.DocumentType, "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell(detail.DocumentNumber.ToString(), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell(FormatDate(detail.DocumentDate), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell($"-{detail.TransactionAmount:N2}", "", 5, Black10, 1, 1, 10f, Element.ALIGN_RIGHT));
            table.AddCell(LeftMarginCell(FormatDate(detail.PaymentDate), "", 5, Black10, 1, 1, 10f, Element.ALIGN_CENTER));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell($"Ref number: {detail.DocumentId}", "", 10, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
        }

        private void AddTotals(PdfPTable table, decimal finalBalance)
        {
            AddEmptyDetailRow(table);
            AddEmptyDetailRow(table);

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(MarginTop("", "", 40, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
            table.AddCell(PMinCell("Saldo Vencido:", "", 15, Black10, 2, 1, 20f, Element.ALIGN_RIGHT));
            table.AddCell(PMinCell(_balanceDue.ToString("N2"), "", 5, Black10, 2, 1, 20f, Element.ALIGN_RIGHT));
            table.AddCell(PMinCell("Saldo Total al Corte:", "", 15, Black10, 2, 1, 20f, Element.ALIGN_RIGHT));
            table.AddCell(PMinCell(finalBalance.ToString("N2"), "", 5, Black10, 2, 1, 20f, Element.ALIGN_RIGHT));
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 2, 1, 20f, Element.ALIGN_CENTER));
        }

        private static void AddTopFrame(PdfPTable table)
        {
            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(MarginBottom("", "", 15, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(MarginBottom("", "", 24, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 15, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 24, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Courier10, 1, 1, 10f, Element.ALIGN_LEFT));
        }

        private static void AddEmptyDetailRow(PdfPTable table)
        {
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 5, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(LeftMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
        }

        private PdfPTable BuildStatementFooter()
        {
            var table = new PdfPTable(42) { WidthPercentage = 100f, HorizontalAlignment = Element.ALIGN_CENTER };
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("Cualquier discrepancia con sus registros contables, favor de comunicarse al departamento de crédito y cobranza al 58040600", "", 40, Bold10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));

            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("Nota: Los atrasos en sus pagos causarán el 2% de interés mensual.", "", 40, Black10, 1, 1, 10f, Element.ALIGN_LEFT));
            table.AddCell(WithoutMarginCell("", "", 1, Black10, 1, 1, 10f, Element.ALIGN_LEFT));

            return table;
        }

        private string GetClientName()
        {
            return _header.DealerName ?? string.Empty;
        }

        private string GetClientKey()
        {
            return _header.DealerKey ?? string.Empty;
        }

        private decimal GetHeaderBalance()
        {
            return _header.Balance;
        }

        private string GetBankAccountText(int index)
        {
            if (_bankAccounts.Count <= index)
                return string.Empty;

            return $"{_bankAccounts[index].AccountName}: {_bankAccounts[index].AccountNumber}";
        }

        private static string FormatDate(DateTime? date)
        {
            return date?.ToString("dd/MM/yyyy") ?? string.Empty;
        }

        private static PdfPCell PMinCell(string text, string description, int colSpan, Font font, int padTop, int padBottom, float height, int align)
        {
            return new PdfPCell
            {
                Phrase = new Phrase
                {
                    new Chunk(text, Bold8),
                    new Chunk(description, font)
                },
                Colspan = colSpan,
                PaddingBottom = padBottom,
                PaddingTop = padTop,
                HorizontalAlignment = align,
                VerticalAlignment = align,
                FixedHeight = height
            };
        }

        private static PdfPCell MarginTop(string text, string description, int colSpan, Font font, int padTop, int padBottom, float height, int align)
        {
            return new PdfPCell
            {
                Phrase = new Phrase
                {
                    new Chunk(text, Bold8),
                    new Chunk(description, font)
                },
                Colspan = colSpan,
                PaddingBottom = padBottom,
                PaddingTop = padTop,
                HorizontalAlignment = align,
                FixedHeight = height,
                Border = PdfPCell.TOP_BORDER
            };
        }

        private static PdfPCell MarginBottom(string text, string description, int colSpan, Font font, int padTop, int padBottom, float height, int align)
        {
            return new PdfPCell
            {
                Phrase = new Phrase
                {
                    new Chunk(text, Bold8),
                    new Chunk(description, font)
                },
                Colspan = colSpan,
                PaddingBottom = padBottom,
                PaddingTop = padTop,
                HorizontalAlignment = align,
                FixedHeight = height,
                Border = PdfPCell.BOTTOM_BORDER
            };
        }

        private static PdfPCell LeftMarginCell(string text, string description, int colSpan, Font font, int padTop, int padBottom, float height, int align)
        {
            return new PdfPCell
            {
                Phrase = new Phrase
                {
                    new Chunk(text, Bold8),
                    new Chunk(description, font)
                },
                Colspan = colSpan,
                PaddingBottom = padBottom,
                PaddingTop = padTop,
                HorizontalAlignment = align,
                FixedHeight = height,
                Border = PdfPCell.LEFT_BORDER
            };
        }

        private static PdfPCell WithoutMarginCell(string text, string description, int colSpan, Font font, int padTop, int padBottom, float height, int align)
        {
            return new PdfPCell
            {
                Phrase = new Phrase
                {
                    new Chunk(text, Bold8),
                    new Chunk(description, font)
                },
                Colspan = colSpan,
                PaddingBottom = padBottom,
                PaddingTop = padTop,
                HorizontalAlignment = align,
                FixedHeight = height,
                Border = PdfPCell.NO_BORDER
            };
        }

        private static PdfPCell NoBorderCell(string text, Font font, int colSpan, int align)
        {
            return new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = align,
                BorderWidth = 0,
                Colspan = colSpan
            };
        }
    }
}
