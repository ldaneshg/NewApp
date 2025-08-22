using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace VariableStatusTracker
{
    public class StatusForm : Form
    {
        private Label[] statusLabels = new Label[4];
        private Button[] updateButtons = new Button[4];
        private Panel[] statusLights = new Panel[4];
        private string[] variableNames = {
            "Emergency Electronic Brake Lights (EEBL)",
            "Reduced Speed Zone Warning (RSZW)",
            "Var3",
            "Var4"
        };

        public StatusForm()
        {
            Text = "V2X and I2X Message Status Monitor";
            Width = 700;
            Height = 300;

            for (int i = 0; i < 4; i++)
            {
                // Create status label
                statusLabels[i] = new Label()
                {
                    Text = $"{variableNames[i]}: Not Received",
                    //Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Top = 30 + i * 50,
                    Left = 20,
                    Width = 300,
                    Height = 40,             // Taller to fit multiple lines
                    AutoSize = false,        // Prevent shrinking to one line
                    TextAlign = ContentAlignment.TopLeft

                };

                // Create green light panel
                statusLights[i] = new Panel()
                {
                    Size = new Size(20, 20),
                    Top = 30 + i * 50,
                    Left = 350,
                    BackColor = Color.Red,
                    BorderStyle = BorderStyle.None
                };

                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, statusLights[i].Width, statusLights[i].Height);
                statusLights[i].Region = new Region(path);

                // Create button
                updateButtons[i] = new Button()
                {
                    Text = "Mark as Received",
                    Top = 30 + i * 50,
                    Left = 450,
                    Width = 150
                };

                // Capture index for event handler
                int index = i;
                updateButtons[i].Click += (sender, e) =>
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    statusLabels[index].Text = $"{variableNames[index]}: \nReceived at {timestamp}";
                    statusLights[index].BackColor = Color.LimeGreen;

                    // Optional: turn off after 2 seconds
                    var timer = new Timer();
                    timer.Interval = 2000;
                    timer.Tick += (s, args) =>
                    {
                        statusLights[index].BackColor = Color.Red;
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                };

                Controls.Add(statusLabels[i]);
                Controls.Add(statusLights[i]);
                Controls.Add(updateButtons[i]);
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new StatusForm());
        }
    }
}