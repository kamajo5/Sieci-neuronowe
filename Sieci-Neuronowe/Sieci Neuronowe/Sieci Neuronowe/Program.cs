using System;
using System.IO;

namespace Sieci_Neuronowe
{
  class Program
  {
    static void Main(string[] args)
    {
      int b = 1;
      StreamWriter zapis = new StreamWriter("wynik.txt");
      zapis.WriteLine("Dane");
      zapis.WriteLine("Macierz W:");
      double[] dane = { 0.0, -1.0, -3.0, -1.0, 0.0, 2.0, -3.0, 2.0, 0.0 };
      double[] wejsce = new double[3];
      Matrix macierz = new Matrix(dane, 3, 3);
      zapis.WriteLine(macierz.GetRow(0).ToString("F1"));
      zapis.WriteLine(macierz.GetRow(1).ToString("F1"));
      zapis.WriteLine(macierz.GetRow(2).ToString("F1"));
      Matrix wyjscie = new Matrix(3, 1);
      double energia_obecna = 999999999999999999;
      double energia_wstecz = 999999999999999999;
      zapis.WriteLine();
      zapis.WriteLine("Tryb synchroniczny");
      zapis.WriteLine();
      //Tryb synchroniczny
      for (int i = 0; i < 8; i++)
      {

        energia_obecna = 999999999999999999;
        energia_wstecz = 999999999999999999;
        Char delimiter = ' ';
        string[] pom = wczytaj_dane(i).Split(delimiter);
        for (int k = 0; k < 3; k++)
        {
          wejsce[k] = Convert.ToDouble(pom[k]);
        }
        zapis.WriteLine("Bdany Wektor to:");
        zapis.WriteLine(wejsce[0].ToString("F1"));
        zapis.WriteLine(wejsce[1].ToString("F1"));
        zapis.WriteLine(wejsce[2].ToString("F1"));
        zapis.WriteLine();
        Matrix wektor = new Matrix(wejsce, 3, 1);
        int n = 1;
        while (liczba_krokow(macierz.ColumnCount, n))
        {
          Console.WriteLine("krok " + n + " Badanie " + b);
          zapis.WriteLine("krok " + n + " Badanie " + b);
          zapis.WriteLine();
          Console.WriteLine();
          Console.WriteLine(wektor.GetColumn(0).ToString("F1"));

          wyjscie = Matrix.Multiply(macierz, wektor);
          zapis.WriteLine("Potecjal Wejsciowy U");
          zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
          zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
          zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
          zapis.WriteLine();
          Console.WriteLine(wyjscie.GetColumn(0).ToString("F1"));
          wyjscie = wyjscie.ToBiPolar();
          Console.WriteLine("po funkcji Bi polarnej");
          zapis.WriteLine("Potecjal Wyjsciowy V");
          Console.WriteLine(wyjscie.GetColumn(0).ToString("F1"));
          zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
          zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
          zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
          zapis.WriteLine();
          energia_obecna = energia(macierz, wyjscie, wektor);
          Console.WriteLine(energia_obecna.ToString());
          zapis.WriteLine("E = " + energia_obecna.ToString());
          if (sprawdz_energie(energia_obecna, energia_wstecz) == true && sprawdz_wektory(wektor, wyjscie) == false)
          {
            Console.WriteLine("Oscylacja dwu punktowa");
            zapis.WriteLine("Oscylacja dwu punktowa");
            zapis.WriteLine();
            zapis.WriteLine("Punkt oscylacji v1");
            zapis.WriteLine();
            zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
            zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
            zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
            zapis.WriteLine();
            zapis.WriteLine("Punkt oscylacji v2");
            zapis.WriteLine();
            zapis.WriteLine(wektor.GetElement(0, 0).ToString("F1"));
            zapis.WriteLine(wektor.GetElement(1, 0).ToString("F1"));
            zapis.WriteLine(wektor.GetElement(2, 0).ToString("F1"));
            zapis.WriteLine();
            Console.WriteLine("############################");
            zapis.WriteLine("############################");
            b++;
            Console.WriteLine();
            break;
          }
          if (sprawdz_wektory(wektor, wyjscie) == true)
          {
            Console.WriteLine("siec ustabilzowala");
            zapis.WriteLine("Siec ustabilizowała sie");
            zapis.WriteLine();
            zapis.WriteLine("Punkt v1");
            zapis.WriteLine();
            zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
            zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
            zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
            zapis.WriteLine();
            b++;
            zapis.WriteLine("############################");
            Console.WriteLine("############################");
            Console.WriteLine();
            break;
          }

          if (liczba_krokow(macierz.ColumnCount, n) == false)
          {
            Console.WriteLine("koniec dzialania za duza liczba krokow");
            zapis.WriteLine("Za duzo krokow");
            zapis.WriteLine();
            b++;
            Console.WriteLine();
            break;
          }
          energia_wstecz = energia_obecna;
          wektor = wyjscie;
          n++;
          Console.WriteLine();
        }
      }
      // tryb asynchroniczny
      bool przerwanie = false;
      b = 0;
      int p = 0;
      energia_obecna = 999999999999999999;
      //Matrix wektor_pom = new Matrix(3, 1);
      //wybranie kolejnosci
      int[] kolejnosc = new int[3];
      //standardowo 123
      kolejnosc[0] = 0;
      kolejnosc[1] = 1;
      kolejnosc[2] = 2;
      Console.WriteLine("Czy chcesz zmienić kolenosć T -tak N -nie");
      char key;
      key = Console.ReadKey().KeyChar;
      Console.WriteLine();
      if (key == 't' || key == 'T')
      {
        kolejnosc = zmien_kolejnosc();
      }
      zapis.WriteLine();
      zapis.WriteLine("Tryb asynchroniczny");
      zapis.WriteLine();
      for (int i = 0; i < 8; i++)
      {
        b++;
        Char delimiter = ' ';
        string[] pom = wczytaj_dane(i).Split(delimiter);
        for (int k = 0; k < 3; k++)
        {
          wejsce[k] = Convert.ToDouble(pom[k]);
        }
        zapis.WriteLine("Bdany Wektor to:");
        zapis.WriteLine(wejsce[0].ToString("F1"));
        zapis.WriteLine(wejsce[1].ToString("F1"));
        zapis.WriteLine(wejsce[2].ToString("F1"));
        zapis.WriteLine();
        Matrix wektor = new Matrix(wejsce, 3, 1);
        int n = 0;
        while (przerwanie == false && liczba_krokow_asyn(macierz.ColumnCount, n))
        {
          for (int k = 0; k < 3; k++)
          {
            zapis.WriteLine("krok " + (n + 1) + " Badanie " + b);
            zapis.WriteLine();
            Console.WriteLine("krok " + (n + 1) + " Badanie " + b);
            Console.WriteLine();
            Console.WriteLine(wektor.GetColumn(0).ToString("F1"));
            wyjscie = Matrix.Multiply(macierz, wektor);
            zapis.WriteLine("Potecjal Wejsciowy U");
            Console.WriteLine("Potencjal Wejsciowy U");
            //kolejnos nw
            if (kolejnosc[k] == 0)
            {
              zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
              zapis.WriteLine("NW");
              zapis.WriteLine("NW");
              zapis.WriteLine();
              Console.WriteLine(wyjscie.GetElement(0, 0).ToString("F1") + " NW NW");
              Console.WriteLine("po funkcji Bi polarnej");
              wyjscie = wyjscie.ToBiPolar();
              zapis.WriteLine("Potencjal wyjsciowy");
              zapis.WriteLine();
              zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
              zapis.WriteLine(wektor.GetElement(1, 0).ToString("F1"));
              zapis.WriteLine(wektor.GetElement(2, 0).ToString("F1"));
              Console.WriteLine(wyjscie.GetElement(0, 0).ToString("F1") + " " + wektor.GetElement(1, 0).ToString("F1") + " " + wektor.GetElement(2, 0).ToString("F1"));
            }
            if (kolejnosc[k] == 1)
            {
              zapis.WriteLine("NW");
              zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
              zapis.WriteLine("NW");
              zapis.WriteLine();
              Console.WriteLine("NW " + wyjscie.GetElement(1, 0).ToString("F1") + " NW");
              Console.WriteLine("po funkcji Bi polarnej");
              wyjscie = wyjscie.ToBiPolar();
              zapis.WriteLine("Potencjal wyjsciowy");
              zapis.WriteLine();
              zapis.WriteLine(wektor.GetElement(0, 0).ToString("F1"));
              zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
              zapis.WriteLine(wektor.GetElement(2, 0).ToString("F1"));
              Console.WriteLine(wektor.GetElement(0, 0).ToString("F1") + " " + wyjscie.GetElement(1, 0).ToString("F1") + " " + wektor.GetElement(2, 0).ToString("F1"));
            }
            if (kolejnosc[k] == 2)
            {
              zapis.WriteLine("NW");
              zapis.WriteLine("NW");
              zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
              zapis.WriteLine();
              Console.WriteLine("NW NW " + wyjscie.GetElement(2, 0).ToString("F1"));
              Console.WriteLine("po funkcji Bi polarnej");
              wyjscie = wyjscie.ToBiPolar();
              zapis.WriteLine("Potencjal wyjsciowy");
              zapis.WriteLine();
              zapis.WriteLine(wektor.GetElement(0, 0).ToString("F1"));
              zapis.WriteLine(wektor.GetElement(1, 0).ToString("F1"));
              zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
              Console.WriteLine(wektor.GetElement(0, 0).ToString("F1") + " " + wektor.GetElement(1, 0).ToString("F1") + " " + wyjscie.GetElement(2, 0).ToString("F1"));
            }
            zapis.WriteLine();
            energia_obecna = energia_asyn(macierz, wyjscie);
            Console.WriteLine(energia_obecna.ToString());
            zapis.WriteLine("E = " + energia_obecna.ToString());
            zapis.WriteLine();
            n++;
            if (sprawdz_wektory(wektor, wyjscie) == true)
            {
              p++;
              if (p == 3)
              {
                p++;
                Console.WriteLine("siec ustabilzowala");
                zapis.WriteLine("Siec ustabilizowała sie");
                zapis.WriteLine();
                zapis.WriteLine("Punkt v1");
                zapis.WriteLine();
                zapis.WriteLine(wyjscie.GetElement(0, 0).ToString("F1"));
                zapis.WriteLine(wyjscie.GetElement(1, 0).ToString("F1"));
                zapis.WriteLine(wyjscie.GetElement(2, 0).ToString("F1"));
                zapis.WriteLine();
                zapis.WriteLine("E = " + energia_obecna.ToString());
                zapis.WriteLine();
                przerwanie = true;
                zapis.WriteLine("############################");
                Console.WriteLine("############################");
                Console.WriteLine();
                p = 0;
                break;
              }
            }
            else
            {
              p = 0;
            }
            wektor.SetElement(kolejnosc[k], 0, (wyjscie.GetElement(kolejnosc[k], 0)));
          }
        }
        przerwanie = false;
      }
      zapis.Close();
      Console.ReadKey();
    }
    private static int[] zmien_kolejnosc()
    {
      int[] pom = new int[3];
      Console.WriteLine("Zmiana kolejnosci");
      Console.WriteLine();
      int tempKey1 = 0;
      int tempKey2 = 0;
      for (int i = 0; i < 3; i++)
      {
        Console.WriteLine("Podaj kolejność (zakres 0 < x < 4 oraz x != x)");
        var key = Console.ReadKey().KeyChar;
        var k = int.Parse(key.ToString());
        
        if (k > 0 && k < 4 && tempKey1 != k && tempKey2 !=k)
        {
          if (i!=1)
          {
            tempKey1 = k;
          }
          tempKey2 = k;
          Console.WriteLine();
          pom[i] = ((int)char.GetNumericValue(key)) - 1;
        }
        else
        {
          Console.WriteLine(" Zły zakres lub nastapiło powtórzenie liczby");
          i--;
        }
      }
      return pom;
    }
    public static double energia(Matrix m, Matrix we, Matrix wy)
    {
      double wynik = 0;
      for (int row = 0; row < m.RowCount; row++)
      {

        for (int col = 0; col < m.ColumnCount; col++)
        {
          wynik += -(m.GetElement(row, col) * we.GetElement(row, 0) * wy.GetElement(col, 0));
        }
      }
      return wynik;
    }
    public static double energia_asyn(Matrix m, Matrix we)
    {
      double wynik = 0;
      for (int row = 0; row < m.RowCount; row++)
      {
        for (int col = 0; col < m.ColumnCount; col++)
        {
          wynik += m.GetElement(col, row) * we.GetElement(row, 0) * we.GetElement(col, 0);
        }
      }
      return -(wynik / 2);
    }
    public static bool sprawdz_wektory(Matrix we, Matrix wy)
    {
      if (we.Equals(wy))
        return true;
      else
        return false;
    }
    public static bool sprawdz_energie(double we, double wy)
    {
      if (we == wy)
        return true;
      else
        return false;
    }
    public static bool liczba_krokow(double n, int t)
    {

      double pom = Math.Pow(2, n);
      if (t <= pom)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    public static bool liczba_krokow_asyn(double n, int t)
    {
      double pom = Math.Pow(2, n);
      pom = pom * n;
      if (t <= pom)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    public static string wczytaj_dane(int k)
    {
      string line;
      string[] pom = new string[8];
      int i = 0;
      StreamReader file = new System.IO.StreamReader(@"dane.txt");
      while ((line = file.ReadLine()) != null)
      {
        pom[i] = line;
        i++;
      }
      return pom[k];
    }
  }
}
