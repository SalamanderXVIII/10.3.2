using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab_10_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Гребенюк А., 21-ИСП-2, 10-я лабораторная, 2-й вариант, высокий уровень.
        private List<int[,]> list = new List<int[,]>();
        private List<int[,]> Fixedlist = new List<int[,]>();
        private string first, second, third;
        private int k, l, m, n, kl;
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (k < l)
            {
                kl = l - k;
                await ExcessMatrix(second, list, l, kl);
                OverrideMatrix(third, kl, Table3, list);
                await RemoveExcessMatrix(second, Fixedlist, kl);
                OverrideMatrix(second, k, Table2, Fixedlist);
            }
            else if (l < k)
            {
                kl = k - l;
                await ExcessMatrix(first, list, k, kl);
                OverrideMatrix(third, kl, Table3, list);
                await RemoveExcessMatrix(first, Fixedlist, kl);
                OverrideMatrix(first, l, Table2, Fixedlist);
            }
        }
        private async Task<List<int[,]>> ExcessMatrix(string file, List<int[,]> list, int BiggerMatrix, int ExcessCount)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                int listCount = 0;
                string? line;
                int[,] matrix = new int[n, m];
                int a = 0;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (listCount < (BiggerMatrix - ExcessCount) * 5)
                    {
                        listCount++;
                        continue;
                    }
                    else if (!line.Equals(""))
                    {
                        string[] mas = line.Split(" ");
                        for (int i = 0; i < mas.Length - 1; i++)
                        {
                            matrix[a, i] = int.Parse(mas[i]);
                        }
                        a++;
                    }
                    else
                    {
                        list.Add(matrix);
                        a = 0;
                        matrix = new int[n, m];
                    }
                }
            }
            return list;
        }
        private async Task<List<int[,]>> RemoveExcessMatrix(string file, List<int[,]> list, int ExcessCount)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string? line;
                int[,] matrix = new int[n, m];
                int a = 0;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!line.Equals(""))
                    {
                        string[] mas = line.Split(" ");
                        for (int i = 0; i < mas.Length - 1; i++)
                        {
                            matrix[a, i] = int.Parse(mas[i]);
                        }
                        a++;
                    }
                    else
                    {
                        list.Add(matrix);
                        a = 0;
                        matrix = new int[n, m];
                    }
                }
                int listcount = list.Count;
                for (int i = listcount - 1; i > (listcount - ExcessCount) - 1; i--)
                {
                    list.Remove(list[i]);
                }
            }
            return list;
        }
        public MainWindow()
        {
            InitializeComponent();
            first = Environment.CurrentDirectory + "\\first.txt";
            second = Environment.CurrentDirectory + "\\second.txt";
            third = Environment.CurrentDirectory + "\\third.txt";
            FileInfo firstFile = new FileInfo(first);
            FileInfo secondFile = new FileInfo(second);
            FileInfo thirdFile = new FileInfo(third);
            if (firstFile.Exists) firstFile.Delete();
            else firstFile.Create();
            if (secondFile.Exists) secondFile.Delete();
            else secondFile.Create();
            if (thirdFile.Exists) thirdFile.Delete();
            else thirdFile.Create();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            k = int.Parse(K.Text);
            l = int.Parse(L.Text);
            m = int.Parse(M.Text);
            n = int.Parse(N.Text);
            GenMatrix(first, k, Table1);
            GenMatrix(second, l, Table2);
        }
        private async void GenMatrix(string file, int a, TextBlock t)
        {
            Random random = new Random();
            for (int x = 1; x <= a; x++)
            {
                int[,] mas = new int[n, m];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        mas[i, j] = random.Next(10, 100);
                        using (StreamWriter writer = new StreamWriter(file, true))
                        {
                            await writer.WriteAsync(mas[i, j] + " ");
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(file, true))
                    {
                        await writer.WriteLineAsync();
                    }
                }
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    await writer.WriteLineAsync();
                }
            }
            using (StreamReader reader = new StreamReader(file))
            {
                string text = await reader.ReadToEndAsync();
                t.Text = text;
            }
        }
        private async void OverrideMatrix(string file, int MatrixCount, TextBlock t, List<int[,]> list)
        {
            t.Text = "";
            File.WriteAllText(file, string.Empty);
            for (int x = 0; x < MatrixCount; x++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        using (StreamWriter writer = new StreamWriter(file, true))
                        {
                            await writer.WriteAsync(list[x][i, j] + " ");
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(file, true))
                    {
                        await writer.WriteLineAsync();
                    }
                }
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    await writer.WriteLineAsync();
                }
            }
            using (StreamReader reader = new StreamReader(file))
            {
                string text = await reader.ReadToEndAsync();
                t.Text += text;
            }
        }
    }
}