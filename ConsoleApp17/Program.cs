using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace ConsoleApp17
{
    class Program
    {
        static void Main(string[] args)
        {

            // XMl Dosya islemleri************************
            XmlDocument document = new XmlDocument();
            XmlElement rehber;
            if (!File.Exists("Rehber.xml"))
            {
                rehber = document.CreateElement("rehber");
                document.AppendChild(rehber);
            }
            else
            {
                document.Load("Rehber.xml");
                rehber = (XmlElement)document.SelectSingleNode("rehber");
            }




            //**************************************************************************
            bool cs = true;

            while (cs)
            {


                Console.WriteLine("Lütfen yapmak istediğiniz işlemi seçiniz :) \n*******************************************\n(1) Yeni Numara Kaydetmek\n(2) Varolan Numarayı Silmek\n(3) Varolan Numarayı Güncelleme\n(4) Rehberi Listelemek\n(5) Rehberde Arama Yapmak\n(6) CIKIS﻿");

                int islem = Convert.ToInt32(Console.ReadLine());
                PhoneNumber person = new PhoneNumber();


                switch (islem)
                {
                    case 1:

                        Console.Write("Lütfen isim giriniz             :");
                        person.name = Console.ReadLine();
                        Console.Write("Lütfen soyisim giriniz          :");
                        person.surname = Console.ReadLine();
                        Console.Write("Lütfen telefon numarası giriniz :");
                        person.number = Console.ReadLine();
                        NumaraKaydet(person, document);
                        Console.WriteLine("*****Telefon Numarasi Basariyla Kaydedildi*****");

                        break;

                    case 2:
                        bool cntrl = true;
                        char chr = ' ';
                        while (cntrl)
                        {


                            Console.WriteLine("Lütfen numarasını silmek istediğiniz kişinin adını ya da soyadını giriniz:");
                            string name12 = Console.ReadLine();
                            if (AdSoyad_BilgiGetirList(document, name12).Count != 0)
                            {
                                Console.Write("{0} isimli kişi rehberden silinmek üzere, onaylıyor musunuz ?(y/n)", name12);
                                chr = Convert.ToChar(Console.ReadLine());
                                if (chr == 'y')
                                {
                                    KisiSil(name12, document);
                                    Console.WriteLine("Silme Islemi basarili sekilde gerceklesti");
                                    cntrl = false;

                                }
                                else if (chr == 'n')
                                {
                                    Console.WriteLine("Kisi Silme Islemi Iptal Edildi");

                                }

                            }

                            else
                            {
                                Console.WriteLine("Aradığınız krtiterlere uygun veri rehberde bulunamadı. Lütfen bir seçim yapınız.\n\n* Silmeyi sonlandırmak için : (1)\n* Yeniden denemek için      : (2)");
                                int num = Convert.ToInt32(Console.ReadLine());
                                if (num == 1)
                                {
                                    cntrl = false;
                                }
                                else if (num == 2)
                                {
                                    cntrl = true;
                                }

                            }

                        }

                        break;

                    case 3:
                        Console.Write("Guncelemek istediginiz kisinin  adini veya soyadini giriniz:");
                        string str1 = Console.ReadLine();
                        if (AdSoyad_BilgiGetirList(document, str1).Count > 0)
                        {
                            PhoneNumber pers = new PhoneNumber();
                            Console.Write("Yeni isim giriniz:");
                            pers.name = Console.ReadLine();
                            Console.Write("Yeni soyisim giriniz:");
                            pers.surname = Console.ReadLine();
                            Console.Write("Yeni numara giriniz :");
                            pers.number = Console.ReadLine();

                            KisiGuncelle(document, str1, pers);
                            Console.WriteLine("Guncelleme Tamamlandi");
                        }

                        break;

                    case 4:

                        Console.WriteLine("Telefon Rehberi\n**********************************************");

                        foreach (PhoneNumber perso in TumRehberGetirList(document))
                        {
                            PersonEkranaYaz(perso);
                        }


                        break;

                    case 5:
                        Console.Write("Arama yapmak istediğiniz tipi seçiniz.\n**********************************************\nİsim veya soyisime göre arama yapmak için: (1)\nTelefon numarasına göre arama yapmak için: (2)");
                        int number = Convert.ToInt32(Console.ReadLine());

                        if (number == 1)
                        {
                            Console.Write("Arama yapmak  istediginiz Isim veya  Soyisim Giriniz:");
                            string str = Console.ReadLine();
                            Console.WriteLine("Arama Sonuçlarınız:\n**********************************************\n");
                            foreach (PhoneNumber perso in AdSoyad_BilgiGetirList(document, str))
                            {
                                PersonEkranaYaz(perso);
                            }
                        }
                        else if (number == 2)
                        {
                            Console.Write("Arama yapmak  istediginiz Telefon Numarasini Giriniz:");
                            string str = Console.ReadLine();
                            Console.WriteLine("Arama Sonuçlarınız:\n**********************************************\n");
                            foreach (PhoneNumber perso in Numara_BilgileriGetir(document, str))
                            {
                                PersonEkranaYaz(perso);
                            }
                        }

                        break;

                    case 6:
                        cs = false;
                        break;

                    default:
                        Console.WriteLine("Hatali Islem Sectiniz!");
                        break;
                }

            }


            Console.ReadKey();



        }


        static void NumaraKaydet(PhoneNumber person1, XmlDocument document)
        {
            XmlElement rehber = (XmlElement)document.SelectSingleNode("rehber");
            XmlElement person = document.CreateElement("person");

            XmlElement name1 = document.CreateElement("name");
            name1.InnerText = person1.name;
            person.AppendChild(name1);

            XmlElement surname1 = document.CreateElement("surname");
            surname1.InnerText = person1.surname;
            person.AppendChild(surname1);

            XmlElement number1 = document.CreateElement("number");
            number1.InnerText = person1.number;
            person.AppendChild(number1);

            rehber.AppendChild(person);
            document.Save("Rehber.xml");

        }

        static List<PhoneNumber> AdSoyad_BilgiGetirList(XmlDocument document, string name)
        {

            XmlNode rehber = document.SelectSingleNode("rehber");
            XmlNodeList personList = rehber.SelectNodes("person");
            List<PhoneNumber> phoneList = new List<PhoneNumber>();


            foreach (XmlNode person in personList)

            {
                if (person.SelectSingleNode("name").InnerText == name || person.SelectSingleNode("name").InnerText == name)
                {
                    PhoneNumber p = new PhoneNumber();
                    p.name = person.SelectSingleNode("name").InnerText;
                    p.surname = person.SelectSingleNode("surname").InnerText;
                    p.number = person.SelectSingleNode("number").InnerText;
                    phoneList.Add(p);
                }
            }

            return phoneList;
        }

        static List<PhoneNumber> Numara_BilgileriGetir(XmlDocument document, string number)
        {

            XmlNode rehber = document.SelectSingleNode("rehber");
            XmlNodeList personList = rehber.SelectNodes("person");
            List<PhoneNumber> phoneList = new List<PhoneNumber>();
            foreach (XmlNode person in personList)
            {
                if (person.SelectSingleNode("number").InnerText == number)
                {
                    PhoneNumber s = new PhoneNumber();
                    s.name = person.SelectSingleNode("name").InnerText;
                    s.surname = person.SelectSingleNode("surname").InnerText;
                    s.number = person.SelectSingleNode("number").InnerText;
                    phoneList.Add(s);
                }
            }

            return phoneList;
        }

        static void KisiSil(string name, XmlDocument document)
        {
            XmlNode rehber = document.SelectSingleNode("rehber");
            XmlNodeList personList = rehber.SelectNodes("person");
            foreach (XmlNode person in personList)
            {
                if (name == person.SelectSingleNode("name").InnerText || name == person.SelectSingleNode("surname").InnerText)
                {
                    rehber.RemoveChild(person);
                    document.Save("Rehber.xml");
                    break;
                }
            }

        }

        static void KisiGuncelle(XmlDocument document, string name, PhoneNumber guncel)
        {
            foreach (PhoneNumber person in AdSoyad_BilgiGetirList(document, name))
            {
                if (name == person.name)
                {
                    KisiSil(name, document);
                    NumaraKaydet(guncel, document);
                    break;
                }
            }

        }

        static List<PhoneNumber> TumRehberGetirList(XmlDocument document)
        {
            XmlNode rehber = document.SelectSingleNode("rehber");
            XmlNodeList personList = rehber.SelectNodes("person");
            List<PhoneNumber> rehberlist = new List<PhoneNumber>();


            foreach (XmlNode person in personList)
            {
                PhoneNumber pr = new PhoneNumber();
                pr.name = person.SelectSingleNode("name").InnerText;
                pr.surname = person.SelectSingleNode("surname").InnerText;
                pr.number = person.SelectSingleNode("number").InnerText;
                rehberlist.Add(pr);
            }

            return rehberlist;

        }

        static void PersonEkranaYaz(PhoneNumber ph)
        {
            Console.WriteLine("Isim: {0}", ph.name);
            Console.WriteLine("Soyisim: {0}", ph.surname);
            Console.WriteLine("Telefon Numarasi: {0}", ph.number);
            Console.WriteLine("-");
        }

    }
}
