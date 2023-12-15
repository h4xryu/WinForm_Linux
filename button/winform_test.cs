using System;
using System.Drawing;
using System.Windows.Forms;

public class Test : Form
{
    static public void Main()
    {
        Application.Run(new Test());
    }

    public Test()
    {
        Button btn = new Button();
        btn.Text = "test";
        btn.Click += new EventHandler(Btn_Clicked);
        Controls.Add(btn);
        //Controls.add(tb);
    }

    private void Btn_Clicked(object sender, EventArgs e)
    {
        MessageBox.Show("success");
        
    }
}
