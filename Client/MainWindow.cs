using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Services;
using Model;

namespace Client.GUI
{
    public partial class MainWindow : Form, IObserver
    {
        public IServices service;
        public String oficiuUsername;
        
        private Dictionary<long, (string Style, int Distance, int Count)> probaTable = new();
        private List<Participant> allParticipants = new();

        public MainWindow(IServices service)
        {
            this.service = service;
            
            InitializeComponent();
            //LoadData();
        }
        
        public void LoadData()
        {
            try
            {
                // Fetch all probes from the service
                var probes = service.getAllProbe();

                // Clear the probaTable and populate it with new data
                probaTable.Clear();
                foreach (var entry in probes)
                {
                    probaTable[entry.Key.Id] = (entry.Key.Stil, entry.Key.Distanta, entry.Value);
                }

                // Clear and populate dataGridView2
                dataGridView2.Rows.Clear();
                foreach (var kvp in probaTable)
                {
                    var (style, distance, count) = kvp.Value;
                    dataGridView2.Rows.Add(style, distance, count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SetOficiuUsername(string username)
        {
            oficiuUsername = username;
        }
        
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn nameColumn;
        private DataGridViewTextBoxColumn ageColumn;
        private DataGridViewTextBoxColumn probeColumn;
        private DataGridViewTextBoxColumn styleColumn;
        private DataGridViewTextBoxColumn distanceColumn;
        private DataGridViewTextBoxColumn participantiColumn;
        private TextBox nameField;
        private TextBox ageField;
        private ComboBox distanceCombo;
        private ComboBox styleCombo;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button inscriereBtn;
        private Button searchBtn;
        private Button logoutBtn;
        private DataGridView dataGridView2;

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            nameColumn = new DataGridViewTextBoxColumn();
            ageColumn = new DataGridViewTextBoxColumn();
            probeColumn = new DataGridViewTextBoxColumn();
            dataGridView2 = new DataGridView();
            styleColumn = new DataGridViewTextBoxColumn();
            distanceColumn = new DataGridViewTextBoxColumn();
            participantiColumn = new DataGridViewTextBoxColumn();
            nameField = new TextBox();
            ageField = new TextBox();
            distanceCombo = new ComboBox();
            styleCombo = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            inscriereBtn = new Button();
            searchBtn = new Button();
            logoutBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { nameColumn, ageColumn, probeColumn });
            dataGridView1.Location = new Point(521, 24);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(418, 192);
            dataGridView1.TabIndex = 0;
            // 
            // nameColumn
            // 
            nameColumn.HeaderText = "Nume";
            nameColumn.MinimumWidth = 6;
            nameColumn.Name = "nameColumn";
            nameColumn.ReadOnly = true;
            nameColumn.Width = 125;
            // 
            // ageColumn
            // 
            ageColumn.HeaderText = "Varsta";
            ageColumn.MinimumWidth = 6;
            ageColumn.Name = "ageColumn";
            ageColumn.ReadOnly = true;
            ageColumn.Width = 125;
            // 
            // probeColumn
            // 
            probeColumn.HeaderText = "Nr. Probe";
            probeColumn.MinimumWidth = 6;
            probeColumn.Name = "probeColumn";
            probeColumn.ReadOnly = true;
            probeColumn.Width = 125;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { styleColumn, distanceColumn, participantiColumn });
            dataGridView2.Location = new Point(34, 24);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(409, 192);
            dataGridView2.TabIndex = 1;
            // 
            // styleColumn
            // 
            styleColumn.HeaderText = "Stil";
            styleColumn.MinimumWidth = 6;
            styleColumn.Name = "styleColumn";
            styleColumn.ReadOnly = true;
            styleColumn.Width = 125;
            // 
            // distanceColumn
            // 
            distanceColumn.HeaderText = "Distanta";
            distanceColumn.MinimumWidth = 6;
            distanceColumn.Name = "distanceColumn";
            distanceColumn.ReadOnly = true;
            distanceColumn.Width = 125;
            // 
            // participantiColumn
            // 
            participantiColumn.HeaderText = "Nr. Participanti";
            participantiColumn.MinimumWidth = 6;
            participantiColumn.Name = "participantiColumn";
            participantiColumn.ReadOnly = true;
            participantiColumn.Width = 125;
            // 
            // nameField
            // 
            nameField.Location = new Point(34, 258);
            nameField.Name = "nameField";
            nameField.Size = new Size(236, 27);
            nameField.TabIndex = 2;
            // 
            // ageField
            // 
            ageField.Location = new Point(34, 327);
            ageField.Name = "ageField";
            ageField.Size = new Size(236, 27);
            ageField.TabIndex = 3;
            // 
            // distanceCombo
            // 
            distanceCombo.FormattingEnabled = true;
            distanceCombo.Items.AddRange(new object[] { "50", "200", "800", "1500" });
            distanceCombo.Location = new Point(521, 258);
            distanceCombo.Name = "distanceCombo";
            distanceCombo.Size = new Size(151, 28);
            distanceCombo.TabIndex = 4;
            // 
            // styleCombo
            // 
            styleCombo.FormattingEnabled = true;
            styleCombo.Items.AddRange(new object[] { "liber", "spate", "fluture", "mixt" });
            styleCombo.Location = new Point(521, 327);
            styleCombo.Name = "styleCombo";
            styleCombo.Size = new Size(151, 28);
            styleCombo.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 234);
            label1.Name = "label1";
            label1.Size = new Size(128, 20);
            label1.TabIndex = 6;
            label1.Text = "Nume participant:";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 304);
            label2.Name = "label2";
            label2.Size = new Size(52, 20);
            label2.TabIndex = 7;
            label2.Text = "Varsta:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(521, 234);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 8;
            label3.Text = "Distanta:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(521, 304);
            label4.Name = "label4";
            label4.Size = new Size(33, 20);
            label4.TabIndex = 9;
            label4.Text = "Stil:";
            // 
            // inscriereBtn
            // 
            inscriereBtn.Location = new Point(312, 295);
            inscriereBtn.Name = "inscriereBtn";
            inscriereBtn.Size = new Size(94, 29);
            inscriereBtn.TabIndex = 10;
            inscriereBtn.Text = "Inscriere";
            inscriereBtn.UseVisualStyleBackColor = true;
            inscriereBtn.Click += new EventHandler(inscriereBtn_Click);
            // 
            // searchBtn
            // 
            searchBtn.Location = new Point(750, 285);
            searchBtn.Name = "searchBtn";
            searchBtn.Size = new Size(94, 59);
            searchBtn.TabIndex = 11;
            searchBtn.Text = "Cauta proba";
            searchBtn.UseVisualStyleBackColor = true;
            searchBtn.Click += new EventHandler(searchBtn_Click);
            // 
            // logoutBtn
            // 
            logoutBtn.Location = new Point(870, 387);
            logoutBtn.Name = "logoutBtn";
            logoutBtn.Size = new Size(94, 29);
            logoutBtn.TabIndex = 12;
            logoutBtn.Text = "Log Out";
            logoutBtn.UseVisualStyleBackColor = true;
            logoutBtn.Click += new EventHandler(logoutBtn_Click);
            // 
            // MainWindow
            // 
            ClientSize = new Size(965, 418);
            Controls.Add(logoutBtn);
            Controls.Add(searchBtn);
            Controls.Add(inscriereBtn);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(styleCombo);
            Controls.Add(distanceCombo);
            Controls.Add(ageField);
            Controls.Add(nameField);
            Controls.Add(dataGridView2);
            Controls.Add(dataGridView1);
            Name = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

 

       
        private void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(styleCombo.Text) && !string.IsNullOrEmpty(distanceCombo.Text))
                {
                    string style = styleCombo.Text;
                    int distance = int.Parse(distanceCombo.Text);

                    Dictionary<Participant, Int32> result = service.getParticipantsByProba(distance, style);                    
                   
                    if (result == null)
                    {
                        MessageBox.Show("Nu s-au găsit participanți pentru această probă sau metoda returnează null.", "Informație", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataGridView1.Rows.Clear();
                    
                    foreach (var participant in result.Keys)
                    {
                        dataGridView1.Rows.Add(participant.Name, participant.Age, result[participant]);
                    }
                   
                }
                else
                {
                    MessageBox.Show("Introduceți stilul și distanța pentru căutare!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            try
            {
                service.logout(oficiuUsername, this);
                MessageBox.Show("Logout successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }
        }

        public void Update(Participant participant, long[] probaIds)
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (var probaId in probaIds)
                    {
                        if (probaTable.ContainsKey(probaId))
                        {
                            var (style, distance, count) = probaTable[probaId];
                            probaTable[probaId] = (style, distance, count + 1);
                        }
                    }

                    Invoke(new Action(() =>
                    {
                        dataGridView2.Rows.Clear();
                        foreach (var kvp in probaTable)
                        {
                            var (style, distance, count) = kvp.Value;
                            dataGridView2.Rows.Add(style, distance, count);
                        }

                        if (!string.IsNullOrEmpty(styleCombo.Text) && !string.IsNullOrEmpty(distanceCombo.Text))
                        {
                            string selectedStyle = styleCombo.Text;
                            int selectedDistance = int.Parse(distanceCombo.Text);

                            foreach (var probaId in probaIds)
                            {
                                if (probaTable.TryGetValue(probaId, out var probeInfo))
                                {
                                    if (probeInfo.Style == selectedStyle && probeInfo.Distance == selectedDistance)
                                    {
                                        int probeCount = probaIds.Length;
                                        dataGridView1.Rows.Add(participant.Name, participant.Age, probeCount);
                                        allParticipants.Add(participant);
                                        break;
                                    }
                                }
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        
        private void inscriereBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameField.Text;
                int age = int.Parse(ageField.Text);

                if (dataGridView2.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selectați o probă!");
                    return;
                }

                List<long> selectedProbeIds = new();
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    long id = probaTable.FirstOrDefault(p => p.Value.Style == row.Cells[0].Value.ToString() &&
                                                             p.Value.Distance == int.Parse(row.Cells[1].Value.ToString())).Key;
                    selectedProbeIds.Add(id);
                }

                if (selectedProbeIds.Count == 0)
                {
                    MessageBox.Show("Nu s-a putut identifica proba selectată!");
                    return;
                }

                service.inscriereParticipant(new Participant(0,name,age), selectedProbeIds.ToArray());

                nameField.Clear();
                ageField.Clear();
                MessageBox.Show("Participantul a fost înscris cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}