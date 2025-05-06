namespace Multithreading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TryValidateInput(textBox1,"pierwszym polu",out int liczba1)) { return; }
            if (!TryValidateInput(textBox2, "pierwszym polu", out int liczba2)) { return; }
            if (!TryValidateInput(textBox3, "pierwszym polu", out int liczba3)) { return; }
            if (!TryValidateInput(textBox4, "pierwszym polu", out int liczba4)) { return; }

            int dimension, numThreads, seed1, seed2;
            dimension = liczba1;
            numThreads = liczba2;
            seed1 = liczba3;
            seed2 = liczba4;
            System.Diagnostics.Stopwatch watch;

            Problem problem = new Problem(dimension, numThreads, seed1, seed2);

            
            textBox5.Text = $"Wymiar: {dimension} Liczba w¹tków: {numThreads} Ziarno generatora1: {seed1} Ziarno generatora2: {seed2}";
            textBox5.Text += " I trwa³o to ";

            if (!checkBox2.Checked) {
                watch = System.Diagnostics.Stopwatch.StartNew();
                problem.algorithm_parallel(numThreads);
                watch.Stop();
            }
            else
            {
                Thread[] threads = new Thread[numThreads];
                for (int i = 0; i < numThreads; i++)
                {
                    threads[i] = new Thread(problem.algorithm_thread);
                }
                watch = System.Diagnostics.Stopwatch.StartNew();
                for (int i = 0; i < numThreads; i++)
                {
                    threads[i].Start(i);
                }
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
                watch.Stop();
            }
            textBox5.Text += watch.Elapsed.TotalMilliseconds + "ms";
            if (checkBox1.Checked) { textBox5.Text += Environment.NewLine + problem.display_matrixes(); }

            textBox5.Font = new Font("Consolas", 10);
            textBox5.BackColor = Color.LightGreen;
        }

        private bool TryValidateInput(TextBox textBox, string opis, out int liczba)
        {
            string input = textBox.Text.Trim();
            liczba = 0;

            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out liczba) || input.Contains(" "))
            {
                MessageBox.Show($"Coœ nie tak z {opis}.", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.BackColor = Color.Yellow;
                return false;
            }

            if (liczba < 0)
            {
                MessageBox.Show($"Ujemna liczba w {opis}.", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.BackColor = Color.Yellow;
                return false;
            }

            textBox.BackColor = Color.White;
            return true;
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

    }
}
