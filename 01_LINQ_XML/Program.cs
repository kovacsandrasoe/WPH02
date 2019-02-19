using _01_LINQ_XML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_LINQ_XML
{
    class Program
    {
        static List<Teacher> teachers;
        static void Main(string[] args)
        {
            teachers = Teacher.ImportFromXML("http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/people_clean.xml");


            //csak 1 db tulajdonság az eredménybe [Teacher collection -> int collection]
            //feladat: Gyűjtsük ki az összes szoba számot, ami csak van
            var query1 = from x in teachers
                         select x.Room;

            //csak bizonyos property-k leképezése egy új, névtelen osztályba [Teacher collection -> anonymus collection]
            //feladat: Gyűjtsük ki a tanárok nevét és az emelet/szobaszámot
            var query2 = from x in teachers
                         select new
                         {
                             NameOfTeacher = x.Name,
                             Office = x.Floor + "/" + x.Room
                         };


            //KIVÁLOGATÁS: feltételes szűrés + nincs névtelen osztály
            //feladat: Gyűjtsük ki azokat a tanárokat, akik a 3. emeletnél lejjebb laknak
            var query3 = from x in teachers
                         where x.Floor <= 3
                         select x;

            //KIVÁLOGATÁS: feltételes szűrés + 1 propery marad
            //feladat: Mely szobákba laknak docensek? egyetemi docens, címzetes egyetemi docens, stb.
            var query4 = from x in teachers
                         where x.Rank.Contains("docens")
                         select x.Floor + "/" + x.Room;


            //RENDEZÉS
            //feladat: Kérjük le az adjunktusok nevét Z->A rendezésben
            var query5 = from x in teachers
                         where x.Rank.Contains("adjunktus")
                         orderby x.Name descending
                         select x.Name;

            //ALLEKÉRDEZÉS
            //feladat: azon oktatók neve, akik Csink László emeletétől feljebb laknak

            var query6 = from x in teachers
                         let csink_laci_emelete =
                            from y in teachers
                            where y.Name.Contains("Csink")
                            select y.Floor
                         where x.Floor > csink_laci_emelete.FirstOrDefault()
                         select x.Name;


            //ALLEKÉRDEZÉS
            //feladat: ugyanaz, mint az előbb, de nem SQL-szerű formában, hanem láncolt linq függvényekkel

            var query7 = from x in teachers
                         let csink_laci_emelete = teachers.Where(t => t.Name.Contains("Csink")).Select(z => z.Floor).FirstOrDefault()
                         where x.Floor > csink_laci_emelete
                         select x.Name;


            //AGGREGÁCIÓ  ~   csoportosítás valamilyen opció szerint
            //feladat: Hányan laknak az egyes emeleteken?
            //töréspont: Floor lehetséges értékei: 1,2,3,4

            var query8 = from x in teachers
                         group x by x.Floor into g
                         select new
                         {
                             Floor = g.Key,
                             Count = g.Count()
                         };

            //AGGREGÁCIÓ: bonyolultabb töréspont -> dokik és nem dokik

            var query9 = teachers.GroupBy(t =>
            {
                if (t.Name.Contains("dr") || t.Name.Contains("Dr"))
                {
                    return "phd";
                }
                else
                {
                    return "nophd";
                }
            }).Select(z => new
            {
                Degree = z.Key,
                Number = z.Count()
            });

            //AGGREGÁCIÓ EXPERT: dokik és nem dokik átlagos névhossza :D 

            var query10 = teachers.GroupBy(t =>
            {
                if (t.Name.Contains("dr") || t.Name.Contains("Dr"))
                {
                    return "phd";
                }
                else
                {
                    return "nophd";
                }
            }).Select(z => new
            {
                Degree = z.Key,
                AverageNameLenght = z.Average(r => r.Name.Length)
            });


            //"KERESZTTÁBLÁS" lekérdezés
            //feladat: néhány tantárgy -> mindegyik tartozzon egy intézethet
            List<Course> courses = new List<Course>();
            courses.Add(new Course() { CourseName = "opre", Dept = "Alkalmazott Informatikai Intézet" });
            courses.Add(new Course() { CourseName = "java", Dept = "Alkalmazott Informatikai Intézet" });
            courses.Add(new Course() { CourseName = "analízis", Dept = "Alkalmazott Matematikai Intézet" });
            courses.Add(new Course() { CourseName = "valszám", Dept = "Alkalmazott Matematikai Intézet" });

            //feladat: valszámhoz tartozó intézet phd fokozatú tagjainak listája

            var query11 = from x in courses
                          join y in teachers on x.Dept equals y.Dept
                          where x.CourseName == "valszám" && (y.Name.Contains("dr") || y.Name.Contains("Dr."))
                          select y.Name;

            foreach (var item in query11)
            {
                Console.WriteLine(item);
            }

            ;



        }

        
    }
}
