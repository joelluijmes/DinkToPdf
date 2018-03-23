﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Document;
using DinkToPdf.Settings;

namespace DinkToPdf.TestThreadSafe
{
    public class Program
    {
        static SynchronizedConverter converter;

        public static void Main(string[] args)
        {
            converter = new SynchronizedConverter(new PdfTools());
            
            var doc = new PdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Grayscale,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10 },
                },
                Objects = {
                    new PdfPage() {
                        Page = "http://www.color-hex.com/"
                    }
                }
            };

            Task.Run(() => Action(doc));
            
            var doc2 = new PdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4Small
                },

                Objects = {
                    new PdfPage()
                    {
                        Page = "http://google.com/",
                    }
                }
            };


            Task.Run(() => Action(doc2));

            Console.ReadKey();
        }

        private static void Action(PdfDocument doc)
        {
            byte[] pdf = converter.Convert(doc);

            if (!Directory.Exists("Files"))
            {
                Directory.CreateDirectory("Files");
            }

            using (FileStream stream = new FileStream(@"Files\" + DateTime.UtcNow.Ticks.ToString() + ".pdf", FileMode.Create))
            {
                stream.Write(pdf, 0, pdf.Length);
            }
        }
    }
}
