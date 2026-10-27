using System;
using System.Buffers;
using System.IO;
using System.Net;

class Program
{
    static string server = "[SERVER IP]";
    static string user = "Sahib";
    static string pass = "4678";
    
    static FtpWebRequest Sorgu(string fayl, string metod)
    {
        var req = (FtpWebRequest)WebRequest.Create(
            server + fayl);

        req.Method = metod;
        req.Credentials = new NetworkCredential(user, pass);

        return req;
    }

    
    static void Siyahi()
    {
        var req = Sorgu(
            "",
            WebRequestMethods.Ftp.ListDirectory);

        using var res = (FtpWebResponse)req.GetResponse();
        using var reader = new StreamReader(
            res.GetResponseStream());

        Console.WriteLine("--- Fayllar ---");
        Console.WriteLine(reader.ReadToEnd());
    }

    
    static void Yukle()
    {
        Console.Write("Yuklenecek faylin yolu: ");
        string yol = Console.ReadLine();

        if (!File.Exists(yol))
        {
            Console.WriteLine("Fayl tapilmadi!");
            return;
        }

        string ad = Path.GetFileName(yol);

        var req = Sorgu(
            ad,
            WebRequestMethods.Ftp.UploadFile);

        byte[] data = File.ReadAllBytes(yol);

        using (var stream = req.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        using var res = (FtpWebResponse)req.GetResponse();

        Console.WriteLine("Yuklendi: " + res.StatusDescription);
    }

    
    static void Endir()
    {
        Console.Write("Endirilecek faylin adi: ");
        string ad = Console.ReadLine() ?? "";

        var req = Sorgu(
            ad,
            WebRequestMethods.Ftp.DownloadFile);

        using var res = (FtpWebResponse)req.GetResponse();
        using var stream = res.GetResponseStream();
        using var file = File.Create(ad);

        stream.CopyTo(file);

        Console.WriteLine(
            "Endirildi: " + Path.GetFullPath(ad));
    }

    
    static void Sil()
    {
        Console.Write("Silinecek faylin adi: ");
        string ad = Console.ReadLine() ?? "";

        var req = Sorgu(
            ad,
            WebRequestMethods.Ftp.DeleteFile);

        using var res = (FtpWebResponse)req.GetResponse();

        Console.WriteLine("Silindi: " + res.StatusDescription);
    }

    // 5. Serverdeki fayli yenile (PUT kimi)
    static void Put()
    {
        Console.Write("Yenilenecek faylin adi: ");
        string ad = Console.ReadLine() ?? "";

        Console.Write("Yeni faylin yolu: ");
        string yol = Console.ReadLine() ?? "";

        if (!File.Exists(yol))
        {
            Console.WriteLine("Fayl tapilmadi!");
            return;
        }

        var req = Sorgu(
            ad,
            WebRequestMethods.Ftp.UploadFile);

        byte[] data = File.ReadAllBytes(yol);

        using (var stream = req.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        using var res = (FtpWebResponse)req.GetResponse();

        Console.WriteLine(
            "Fayl yenilendi: " + res.StatusDescription);
    }
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Fayllarin siyahisi");
            Console.WriteLine("2 - Fayl yukle (UPLOAD)");
            Console.WriteLine("3 - Fayl endir (DOWNLOAD)");
            Console.WriteLine("4 - Fayl sil (DELETE)");
            Console.WriteLine("5 - Fayli yenile (PUT)");
            Console.WriteLine("0 - Cixis");
            Console.Write("Secim: ");

            string secim = Console.ReadLine();

            try
            {
                if (secim == "1")
                    Siyahi();
                else if (secim == "2")
                    Yukle();
                else if (secim == "3")
                    Endir();
                else if (secim == "4")
                    Sil();
                else if (secim == "5")
                    Put();
                else if (secim == "0")
                    break;
                else
                    Console.WriteLine("Yanlis secim!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xeta: " + ex.Message);
            }
        }
    }


}