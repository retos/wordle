using System.Text;
using WordleLibrary;

Console.WriteLine("Generates puzzles!");

List<string> list = Solver.ReadInput("words.txt");

for (int i = 0; i < 5; i++)
{
    //shuffle
    List<string> newList = Shuffle(list);
    //write back to disk
    WriteToDisk(newList, $"puzzle_{i}.txt");
}

Console.WriteLine("Programm ended - Puzzles are in bin/Debug folder.");

void WriteToDisk(List<string> newList, string filename)
{    
    StringBuilder stringBuilder = new StringBuilder();
    string output = string.Join(Environment.NewLine, newList);
    File.WriteAllText(filename, output);
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