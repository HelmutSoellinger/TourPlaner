using System;
using System.Diagnostics;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using log4net.Core;
using log4net;
using TourPlaner.Models;
using Microsoft.Extensions.Logging;
using TourPlaner;
using log4net.Config;

namespace TourPlaner.BL
{
    public class PdfGenerator
    {
        public PdfGenerator()
        {
        }
        public void GeneratePdfForTour(TourModel selectedTour)
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            // Relative Pfad vom Basisordner aus zu Ihrem Zielspezialordner
            string relativePathToTargetFolder = @"..\..\..\PDFs";
            string targetFolderPath = System.IO.Path.Combine(appDir, relativePathToTargetFolder);
            // Stellen Sie sicher, dass der Zielspezialordner existiert, falls er noch nicht existiert
            Directory.CreateDirectory(targetFolderPath);
            // Dynamischer Dateiname basierend auf der Tour-Information
            string fileName = $"{selectedTour.Name}.pdf";
            string filePath = System.IO.Path.Combine(targetFolderPath, fileName);
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    if (selectedTour != null)
                    {
                        document.Add(new Paragraph($"Name: {selectedTour.Name}"));
                        document.Add(new Paragraph($"Description: {selectedTour.Description}"));
                        document.Add(new Paragraph($"Start Location: {selectedTour.StartLocation}"));
                        document.Add(new Paragraph($"End Location: {selectedTour.EndLocation}"));
                        document.Add(new Paragraph($"Route Information: {selectedTour.RouteInformation}"));
                        document.Add(new Paragraph($"Distance: {selectedTour.Distance}"));
                        document.Add(new Paragraph($"File Name: {selectedTour.FileName}"));

                        document.Add(new Paragraph("Logs:"));
                        foreach (var log in selectedTour.Logs)
                        {
                            document.Add(new Paragraph($"ID: {log.Id}, Date: {log.Date}, Total Time: {log.TotalTime}, Total Distance: {log.TotalDistance}"));
                        }
                    }

                    document.Close();
                }
            }

            // Öffnet das PDF im Standard-PDF-Anzeiger
            Process.Start(new ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });

            Console.WriteLine("PDF generiert und geöffnet.");
        }
    }
}
