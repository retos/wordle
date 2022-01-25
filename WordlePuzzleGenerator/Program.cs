// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Generates puzzles!");

string nameOfBaseWordlist = "words_alpha.txt";
List<string> list = ReadInput(nameOfBaseWordlist);
list = list.Where(e => e.Count() == 5).ToList();

for (int i = 0; i < 5; i++)
{
    //shuffle
    List<string> newList = Shuffle(list);
    //write back to disk
    WriteToDisk(newList, $"puzzzle_{i}.txt");
}

Console.WriteLine("Programm ended - Puzzles are in bin/Debug folder.");

void WriteToDisk(List<string> newList, string filename)
{
    
    StringBuilder stringBuilder = new StringBuilder();
    string output = string.Join(Environment.NewLine, newList);
    File.WriteAllText(filename, output);
}

List<string> ReadInput(string filename)
{
    StreamReader reader = new StreamReader(filename);

    List<string> inputList = new List<string>();
    while (!reader.EndOfStream)
    {
        inputList.Add(reader.ReadLine());
    }

    return inputList;
}


List<string> Shuffle(List<string> input)
{
    Random rng = new Random();
    int n = input.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);
        string value = input[k];
        input[k] = input[n];
        input[n] = value;
    }
    return input;
}