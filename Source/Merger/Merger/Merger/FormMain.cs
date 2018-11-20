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

namespace Merger
{
		public partial class FormMain : Form
		{

				List<string> LoadAllImages;
				string folderPath;
				public FormMain()
				{
						InitializeComponent();
				}

				private void button1_Click(object sender, EventArgs e)
				{
						using (FolderBrowserDialog fbd = new FolderBrowserDialog())
						{
								fbd.RootFolder = Environment.SpecialFolder.MyComputer;
								fbd.Description = "выберите папку";
								fbd.ShowNewFolderButton = false;
								if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
								{
										folderPath = fbd.SelectedPath;
										string searchPattern = "*.png";
										LoadAllImages = Directory.GetFiles(folderPath, searchPattern, SearchOption.TopDirectoryOnly).ToList();
										Image test = Bitmap.FromFile(LoadAllImages[0]);

										int height = test.Height;
										int width = test.Width;

										Bitmap res = new Bitmap(width * 6, height * ((LoadAllImages.Count / 6) + 1));
										Graphics g = Graphics.FromImage(res);
										int j = 0;
										int i = 0;
										foreach (var path in LoadAllImages)
										{
												if (LoadAllImages.IndexOf(path) % 6 == 0 && LoadAllImages.IndexOf(path) != 0)
												{
														i = 0;
														j++;
												}

												test = Bitmap.FromFile(path);
												g.DrawImage(test, width * i, height * j);
												i++;
										}

										using (SaveFileDialog saveFileDialog = new SaveFileDialog())
										{
												saveFileDialog.Filter = "PNG(*.png) | *.png";
												if (saveFileDialog.ShowDialog() == DialogResult.OK)
												{
														res.Save(saveFileDialog.FileName);
														MessageBox.Show("Картинка собрана.");
												}
										}
								}
						}
				}
		}
}
