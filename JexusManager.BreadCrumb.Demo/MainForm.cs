using System;
using System.Drawing;
using System.Windows.Forms;
using JexusManager.Breadcrumb;

namespace JexusManager.BreadCrumb.Demo
{
    public partial class MainForm : Form
    {
        private readonly BreadcrumbControl _breadcrumb;
        private readonly ToolStripBreadcrumbItem _toolStripBreadcrumb;
        private readonly Panel _contentPanel;
        private readonly ListBox _navigationHistoryListBox; public MainForm()
        {
            Text = "BreadCrumb Control Demo";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            // Create the standard breadcrumb control
            _breadcrumb = new BreadcrumbControl
            {
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(10),
                Font = new Font("Segoe UI", 9F),
                HighlightColor = Color.FromArgb(0, 120, 215)
            };
            _breadcrumb.ItemClicked += Breadcrumb_ItemClicked;

            // Create a ToolStrip with breadcrumb
            var toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
                RenderMode = ToolStripRenderMode.System
            };

            // Add some standard ToolStrip items
            toolStrip.Items.Add(new ToolStripButton("Home", null, (s, e) => InitializeNavigation())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = SystemIcons.Application.ToBitmap()
            });

            toolStrip.Items.Add(new ToolStripSeparator());

            // Create the ToolStripBreadcrumbItem
            _toolStripBreadcrumb = new ToolStripBreadcrumbItem
            {
                Width = 500,
                Font = new Font("Segoe UI", 9F),
                HighlightColor = Color.FromArgb(0, 120, 215)
            };
            _toolStripBreadcrumb.ItemClicked += ToolStripBreadcrumb_ItemClicked;

            toolStrip.Items.Add(_toolStripBreadcrumb);

            // Create the content panel
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Window,
                Padding = new Padding(10)
            };

            // Create navigation history list
            _navigationHistoryListBox = new ListBox
            {
                Dock = DockStyle.Right,
                Width = 250,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Create a splitter between panels
            var splitter = new Splitter
            {
                Dock = DockStyle.Right,
                Width = 3,
                BackColor = SystemColors.ControlDark
            };

            // Create buttons for demonstration
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(5),
                FlowDirection = FlowDirection.RightToLeft
            };

            var resetButton = new Button
            {
                Text = "Reset Navigation",
                Height = 30,
                Width = 150,
                Margin = new Padding(5, 0, 0, 0)
            };
            resetButton.Click += ResetButton_Click;

            var navigateButton = new Button
            {
                Text = "Navigate Deeper",
                Height = 30,
                Width = 150,
                Margin = new Padding(5, 0, 0, 0)
            };
            navigateButton.Click += NavigateButton_Click;            // Add a label for each breadcrumb implementation
            var standardLabel = new Label
            {
                Text = "Standard Breadcrumb Control:",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8F),
                ForeColor = SystemColors.GrayText,
                Padding = new Padding(10, 3, 0, 0)
            };

            var toolstripLabel = new Label
            {
                Text = "ToolStrip Breadcrumb Control:",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8F),
                ForeColor = SystemColors.GrayText,
                Padding = new Padding(10, 3, 0, 0)
            };

            // Add controls to the form
            buttonPanel.Controls.Add(resetButton);
            buttonPanel.Controls.Add(navigateButton);

            Controls.Add(_contentPanel);
            Controls.Add(splitter);
            Controls.Add(_navigationHistoryListBox);
            Controls.Add(buttonPanel);
            Controls.Add(_breadcrumb);
            Controls.Add(standardLabel);
            Controls.Add(toolStrip);
            Controls.Add(toolstripLabel);

            // Initialize with Home
            InitializeNavigation();
        }
        private void InitializeNavigation()
        {
            // Initialize standard breadcrumb
            _breadcrumb.Clear();
            _breadcrumb.AddItem("Home", "home");

            // Initialize toolstrip breadcrumb
            _toolStripBreadcrumb.Clear();
            _toolStripBreadcrumb.AddItem("Home", "home");

            UpdateContentPanel("Home Page");
            AddToNavigationHistory("Navigated to Home");
        }

        private void ToolStripBreadcrumb_ItemClicked(object sender, BreadcrumbItemClickedEventArgs e)
        {
            // If we click on an item, remove all items after it
            while (_toolStripBreadcrumb.Items.Count > e.Index + 1)
            {
                _toolStripBreadcrumb.RemoveLastItem();
            }

            // Also sync the standard breadcrumb to match
            _breadcrumb.Clear();
            foreach (var item in _toolStripBreadcrumb.Items)
            {
                _breadcrumb.AddItem(item.Text, item.Tag);
            }

            // Update the content panel based on the clicked item
            UpdateContentPanel($"{e.Item.Text} Page (ToolStrip)");
            AddToNavigationHistory($"Navigated to {e.Item.Text} via ToolStrip breadcrumb");
        }

        private void Breadcrumb_ItemClicked(object sender, BreadcrumbItemClickedEventArgs e)
        {
            // If we click on an item, remove all items after it
            while (_breadcrumb.Items.Count > e.Index + 1)
            {
                _breadcrumb.RemoveLastItem();
            }

            // Update the content panel based on the clicked item
            UpdateContentPanel($"{e.Item.Text} Page");
            AddToNavigationHistory($"Navigated to {e.Item.Text} via breadcrumb");
        }
        private void NavigateButton_Click(object sender, EventArgs e)
        {
            // Generate a new level based on the current path
            string[] sections = { "Settings", "Users", "Products", "Reports", "Services" };
            string[] subsections = { "List", "Details", "Edit", "Create", "Delete", "Properties" };

            Random random = new Random();

            string newSection;

            // If we're at the beginning, pick a main section
            if (_breadcrumb.Items.Count <= 1)
            {
                newSection = sections[random.Next(sections.Length)];
            }
            // Otherwise, add a subsection
            else
            {
                newSection = subsections[random.Next(subsections.Length)];
            }

            // Add the new item to both breadcrumb controls
            string tagValue = $"level_{_breadcrumb.Items.Count}";
            _breadcrumb.AddItem(newSection, tagValue);
            _toolStripBreadcrumb.AddItem(newSection, tagValue);

            // Update the content panel
            UpdateContentPanel($"{newSection} Page");

            // Add to navigation history
            AddToNavigationHistory($"Navigated to {newSection}");
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            InitializeNavigation();
        }

        private void UpdateContentPanel(string content)
        {
            // Clear existing controls
            _contentPanel.Controls.Clear();

            // Create a label to display the current "page"
            var contentLabel = new Label
            {
                Text = content,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Create a label to show current breadcrumb path
            var fullPathLabel = new Label
            {
                Text = "Current Path: " + GetFullPath(),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(20, 60)
            };

            // Add a description about what to do
            var descriptionLabel = new Label
            {
                Text = "Use the 'Navigate Deeper' button to navigate to a new section.\n" +
                      "Click on any item in the breadcrumb trail to navigate back to that level.\n" +
                      "The 'Reset Navigation' button will clear the path and return to Home.",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(20, 100)
            };

            _contentPanel.Controls.Add(contentLabel);
            _contentPanel.Controls.Add(fullPathLabel);
            _contentPanel.Controls.Add(descriptionLabel);
        }

        private string GetFullPath()
        {
            string path = string.Empty;

            for (int i = 0; i < _breadcrumb.Items.Count; i++)
            {
                path += _breadcrumb.Items[i].Text;

                if (i < _breadcrumb.Items.Count - 1)
                {
                    path += " > ";
                }
            }

            return path;
        }

        private void AddToNavigationHistory(string action)
        {
            _navigationHistoryListBox.Items.Insert(0, $"[{DateTime.Now.ToLongTimeString()}] {action}");
        }
    }
}
