using System.Globalization;
using System.IO;
using Domain;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Helpers
{
    public class PDFUtil
    {
        public static byte[] CreatePDF(UserInfo userInfo, float amountToBorrow)
        {
            var doc = new Document(PageSize.A4, 50, 50, 50, 50);

            using (var output = new MemoryStream())
            {
                var wri = PdfWriter.GetInstance(doc, output);
                doc.Open();

                var header = new Paragraph("Your Mortgage Application for BuyMyHouse")
                    {Alignment = Element.ALIGN_CENTER};
                var paragraph = new Paragraph($"Dear mr/ mrs {userInfo.LastName},");
                var phrase =
                    new Phrase(
                        $"The maximum mortgage you can claim is: \u20AC {amountToBorrow.ToString(CultureInfo.InvariantCulture)}.");

                doc.Add(header);
                doc.Add(paragraph);
                doc.Add(phrase);

                doc.Close();
                return output.ToArray();
            }
        }
    }
}