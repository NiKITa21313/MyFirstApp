using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;

namespace telecommunication_company
{
    internal class FileDogovor
    {
       static string nomerDogovora, den, mesaz, god, nazvanieKompanii, imyaDirectora, ispitatelnySrok,
            zarplata, zarplataSlovami, inyeViplaty, inn, adres, imyaRabotnika, seriya, nomer, kemVydan;

        Dictionary<string, string> items = new Dictionary<string, string>()
            {
                {"<nomerDogovora>", nomerDogovora},
                {"<den>", den},
                {"<mesaz>",mesaz },
                {"<god>", god},
                {"<nazvanieKompanii>", nazvanieKompanii},
                {"<imyaDirectora>", imyaDirectora},
                {"<ispitatelnySrok>", ispitatelnySrok},
                {"<zarplata>", zarplata},
                {"<zarplataSlovami>", zarplataSlovami},
                {"<inyeViplaty>", inyeViplaty},
                {"<inn>", inn},
                {"<adres>", adres},
                {"<imyaRabotnika>", imyaRabotnika},
                {"<seriya>", seriya},
                {"<nomer>", nomer},
                {"<kemVydan>", kemVydan}
            };
        Microsoft.Office.Interop.Word.Application wordApp = null;
        Document wordDoc;
    }
}
