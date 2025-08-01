using PatPilot.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace PatPilot.Helper
{
    public static class PdfHelper
    {
        public static byte[] GenererFacturePdf(Commande commande, Client client, List<CommandeDetail> details)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Content()
                        .Column(col =>
                        {
                            col.Item().Text($"Commande #{commande.Id}").FontSize(18).Bold();
                            col.Item().Text($"Client : {client.Prenom} {client.Nom}");
                            col.Item().Text($"Adresse : {client.Adresse}");
                            col.Item().Text($"Email : {client.Email}");
                            col.Item().Text($"Téléphone : {client.Telephone}");
                            col.Item().PaddingVertical(10).Text("Détails de la commande :").Bold();

                            foreach (var item in details)
                            {
                                col.Item().Text($"{item.Gateau.Name} - {item.Quantite} x {item.Gateau.Price}€");
                            }
                        });
                });
            }).GeneratePdf();
        }
    }

}
