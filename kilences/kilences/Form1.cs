using kilences.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kilences
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        List<int> NumberOfWoman = new List<int>();
        List<int> NumberOfMan = new List<int>();
        Random rng = new Random(3930);

        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
        }

        private void Simulation(int maxYear)
        {
            for (int year = 2005; year <= maxYear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }
                int nmbrOfMales = (from x in Population
                                   where x.Gender == Gender.Male && x.IsAlive
                                   select x).Count();
                int nmbrOfFemales = (from x in Population
                                     where x.Gender == Gender.Female && x.IsAlive
                                     select x).Count();
                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nmbrOfMales, nmbrOfFemales));
                NumberOfWoman.Add(nmbrOfFemales);
                NumberOfMan.Add(nmbrOfMales);

            }
            DisplayResults(maxYear);
        }

        void DisplayResults(int maxYear)
        {
            for (int year = 2005; year <= maxYear; year++)
            {
                TextBox1.Text += "Szimulációs év: " + year + "\n\t Fiúk: " + NumberOfMan[year - 2005] + "\n\t Lányok: " + NumberOfWoman[year - 2005] + "\n";
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }

        List<BirthProbability> GetBirthProbabilities(string file)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();
            using (StreamReader sr = new StreamReader(file))
            {

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    BirthProbability p = new BirthProbability { 
                        Kor = int.Parse(line[0]), 
                        Gyermekek = int.Parse(line[1]), 
                        GyValószínűség = double.Parse(line[2]) };
                    birthProbabilities.Add(p);
                }
            }
            return birthProbabilities;
        }

        List<DeathProbability> GetDeathProbabilities(string file)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();
            using (StreamReader sr = new StreamReader(file))
            {

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    DeathProbability p = new DeathProbability {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Kor = int.Parse(line[1]),
                        HaValószínűség = double.Parse(line[2]) };
                    deathProbabilities.Add(p);
                }
            }
            return deathProbabilities;
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;
            byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Kor == age
                             select x.HaValószínűség).FirstOrDefault();

            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;


            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Kor == age
                                 select x.GyValószínűség).FirstOrDefault();

                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NumberOfMan.Clear();
            NumberOfWoman.Clear();
            Population.Clear();
            BirthProbabilities.Clear();
            DeathProbabilities.Clear();
            TextBox1.Text = "";
            if (!File.Exists(TextBox1.Text))
            {
                MessageBox.Show("Nem talalhato a file.");
                return;
            }
            Population = GetPopulation(TextBox1.Text);
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
            Simulation(int.Parse(numericUpDown1.Value.ToString()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Temp";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            TextBox1.Text = ofd.FileName;
        }
    }
}
